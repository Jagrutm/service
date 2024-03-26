using CredECard.Common.BusinessService;
using System;
using System.Collections.Generic;
using System.Text;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public abstract class DataItem : IPersistable
    {
        //private readonly IConfiguration Configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //   .AddJsonFile("appsettings.json")
        //   .Build();

       // //
        private DataController _controller = null;

        /// <summary>
        /// Save 
        /// </summary>
        public virtual void Save()
        {
            if (this is IPersistableV2)
            {
                _controller = new DataController();
                _controller.StartDatabase(CredECard.Common.BusinessService.CredECardConfig.WriteConnectionString);
                try
                {
                    ((IPersistableV2)this).Save(_controller);
                }
                finally
                {
                    if (_controller.IsDBStarted) _controller.EndDatabase();
                }
            }
        }

        /// <summary>
        /// Validate before save
        /// </summary>
        /// <returns></returns>
		public virtual ValidateResult Validate()
        {
            return new ValidateResult(null);
        }
    }
}
