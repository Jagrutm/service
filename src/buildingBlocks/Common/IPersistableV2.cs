using System;
using System.Collections.Generic;
using System.Text;

namespace CredECard.Common.BusinessService
{
    public interface IPersistableV2
    {
        void Save(DataController conn);
    }
}
