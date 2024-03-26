using Common.Logging.Modles;
using CredECard.Common.BusinessService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Common.Logging
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (builder, loggerConfiguration) =>
           {
               try
               {
                   var logSetting = builder.Configuration.Get<LogSettings>();
                   string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                   string logPath = string.Empty, logFilePath = string.Empty, elasticUri = string.Empty;

                   if (string.IsNullOrEmpty(logSetting.LogType)) return;

                   #region Self-log
                   var selfLogPath = logSetting.LogFileConfiguration.SelfLogPath;
                   if (!string.IsNullOrEmpty(selfLogPath))
                   {
                       if (!Directory.Exists(selfLogPath))
                       {
                           Directory.CreateDirectory(selfLogPath);
                       }

                       var selfLogPathFile = Path.Combine(selfLogPath, string.Format("LogData_{0}.log", DateTime.Today.ToString("yyyyMMdd")));
                       SelfLog.Enable(msg => File.AppendAllText(selfLogPathFile, msg));
                   }
                   else
                   {
                       SelfLog.Enable(Console.Error);
                   }
                   #endregion

                   loggerConfiguration
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .WriteTo.Debug()
                        .WriteTo.Console()
                        .Enrich.WithProperty("Environment", builder.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProperty("Application", builder.HostingEnvironment.ApplicationName)
                        //.MinimumLevel.Override("Microsoft", LogEventLevel.Error) //for namespace Microsoft.*, create logs for Information level or above
                        //.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information) //for namespace Microsoft.*, create logs for Information level or above
                        //.MinimumLevel.Override("System", LogEventLevel.Error) //for namespace System.*, create logs for Error level or above
                        .ReadFrom.Configuration(builder.Configuration);

                   #region File Sink
                   //log to file configuration
                   if (logSetting.LogType.ToUpper().Contains("FILE"))
                   {
                       logPath = logSetting.LogFileConfiguration.LogFilePath;
                       if (!string.IsNullOrEmpty(logPath))
                       {
                           logFilePath = Path.Combine(logPath, string.Format("LogData_{0}.log", DateTime.Today.ToString("yyyyMMdd")));

                           loggerConfiguration.WriteTo.
                                File(path: logFilePath, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                        rollOnFileSizeLimit: true,
                                        retainedFileCountLimit: 20,
                                        rollingInterval: RollingInterval.Day,
                                        fileSizeLimitBytes: 10000000); //10MB
                       }
                   }
                   #endregion

                   #region ElasticSearch Sink
                   //log to elasticsearch configuration
                   if (logSetting.LogType.ToUpper().Contains("ELASTIC"))
                   {
                       elasticUri = logSetting.ElasticConfiguration.Uri;

                       //validate URL
                       bool isValidUri = Uri.TryCreate(elasticUri, UriKind.Absolute, out Uri uriResult)
                           && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                       if (isValidUri)
                       {
                           try
                           {
                               var indexFormatForError = $"applogs-error"
                                      + $"-{builder.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-") + logSetting.ElasticConfiguration.IndexWildCard}"
                                      + $"-{builder.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}"
                                      + $"-{DateTime.UtcNow:yyyy-MM-dd}";

                               var indexFormatForInfo = $"applogs-info"
                                        + $"-{builder.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-") + logSetting.ElasticConfiguration.IndexWildCard}"
                                        + $"-{builder.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}"
                                        + $"-{DateTime.UtcNow:yyyy-MM-dd}";

                               X509Certificate2Collection elasticSearchCertificate = new();

                               //string FileBeatCertificateValues = SystemSetting.GetSystemSettingValue(EnumSystemSettings.FilebeatCertificate);
                               string FileBeatCertificateValues = String.Empty;

                               if (!string.IsNullOrEmpty(FileBeatCertificateValues))
                               {
                                   byte[] FileBeatCertificateBytes = Encoding.ASCII.GetBytes(FileBeatCertificateValues);
                                   elasticSearchCertificate = new X509Certificate2Collection { new X509Certificate2(FileBeatCertificateBytes, logSetting.ElasticConfiguration.CertificatePassword, X509KeyStorageFlags.MachineKeySet) };
                               }
                               else if (File.Exists(logSetting.ElasticConfiguration.CertificatePath))
                               {
                                   elasticSearchCertificate = new X509Certificate2Collection { new X509Certificate2(logSetting.ElasticConfiguration.CertificatePath, logSetting.ElasticConfiguration.CertificatePassword, X509KeyStorageFlags.MachineKeySet) };
                               }

                               ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                               loggerConfiguration
                               .WriteTo.Logger(lc
                                    => lc.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error || e.Level == LogEventLevel.Fatal)
                                        .WriteTo.Elasticsearch(getElasticSinkObject(elasticUri, logSetting, indexFormatForError, elasticSearchCertificate)))
                               .WriteTo.Logger(lc
                                    => lc.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information || e.Level == LogEventLevel.Debug)
                                        .WriteTo.Elasticsearch(getElasticSinkObject(elasticUri, logSetting, indexFormatForInfo, elasticSearchCertificate)));
                           }
                           catch (Exception ex)
                           {
                               throw;
                           }
                       }
                       else
                       {
                           //Do nothing
                       }
                   }
                   #endregion
               }
               catch (Exception ex)
               {
                   throw;
               }
           };


        private static ElasticsearchSinkOptions getElasticSinkObject(string elasticUri, LogSettings logSetting, string indexFormat, X509Certificate2Collection elasticSearchCertificate)
        {
            return new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                ModifyConnectionSettings = configuration => configuration.BasicAuthentication(
                                                                             GetDecryptedValue(logSetting.ElasticConfiguration.UseEncryptedElasticSearchCredentials, logSetting.ElasticConfiguration.Username),
                                                                             GetDecryptedValue(logSetting.ElasticConfiguration.UseEncryptedElasticSearchCredentials, logSetting.ElasticConfiguration.Password))
                                                                         .ClientCertificates(elasticSearchCertificate)
                                                                         .IncludeServerStackTraceOnError(true),
                IndexFormat = indexFormat,
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = GetAutoRegisterTemplateVersion(logSetting.ElasticConfiguration.AutoRegisterTemplateVersion),
                NumberOfShards = logSetting.ElasticConfiguration.NumberOfShards ?? 2,
                NumberOfReplicas = logSetting.ElasticConfiguration.NumberOfReplicas ?? 1
            };
        }

        /// <summary>
        /// Get AutoRegisterTemplateVersion
        /// </summary>
        /// <param name="templateVersion"></param>
        /// <returns></returns>
        private static AutoRegisterTemplateVersion GetAutoRegisterTemplateVersion(string templateVersion)
        {
            AutoRegisterTemplateVersion version = AutoRegisterTemplateVersion.ESv7;

            switch (templateVersion)
            {
                case ("ESv7"): version = AutoRegisterTemplateVersion.ESv7; break;
                case ("ESv6"): version = AutoRegisterTemplateVersion.ESv6; break;
                case ("ESv5"): version = AutoRegisterTemplateVersion.ESv5; break;
                case ("ESv2"): version = AutoRegisterTemplateVersion.ESv2; break;
                default: version = AutoRegisterTemplateVersion.ESv7; break;
            };

            return version;
        }

        /// <summary>
        /// <author>Raxit Chauhan</author>
        /// <CreatedOn>03-Aug-2022</CreatedOn>
        /// Get Decrypted Value
        /// </summary>
        /// <param name="useEncryptedElasticSearchCredentials"></param>
        /// <param name="encryptedValue"></param>
        /// <returns>Returns string</returns>
        private static string GetDecryptedValue(bool useEncryptedElasticSearchCredentials, string encryptedValue) => useEncryptedElasticSearchCredentials ? new SymmCrypto().Decrypting(encryptedValue) : encryptedValue;
    }
}
