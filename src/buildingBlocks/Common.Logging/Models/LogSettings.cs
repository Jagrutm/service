namespace Common.Logging.Modles
{
    /// <summary>
    /// Model to populate the log settings
    /// </summary>
    public class LogSettings
    {
        public string LogType { get; set; }
        public LogFileConfiguration LogFileConfiguration { get; set; }
        public ElasticConfiguration ElasticConfiguration { get; set; }
    }

    public class ElasticConfiguration
    {
        public string Uri { get; set; }
        public int? NumberOfShards { get; set; }
        public int? NumberOfReplicas { get; set; }
        public string IndexWildCard { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AutoRegisterTemplateVersion { get; set; }
        public bool UseEncryptedElasticSearchCredentials { get; set; }
        public string CertificatePath { get; set; }
        public string CertificatePassword { get; set; }
    }

    public class LogFileConfiguration
    {
        public string LogFilePath { get; set; }
        public string SelfLogPath { get; set; }
    }
}
