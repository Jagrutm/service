using System;
using System.Collections.Generic;
using System.Text;

namespace CredECard.Common.BusinessService
{
    public interface IDeleteable
    {
        void Delete(DataController con);

        bool Deleted
        {
            get;
        }
    }
}
