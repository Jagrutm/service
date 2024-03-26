using System;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public class StandardDataService
    {
        private DataController _controller = null;

        public StandardDataService (DataController dataController)
        {
            _controller = dataController;
        }

        public DataController dataController
        {
            get { return _controller; }
        }

        public DataController Controller
        {
            get { return dataController; }
        }
    }
}
