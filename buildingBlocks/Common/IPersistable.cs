using System;
using System.Collections.Generic;
using System.Text;

namespace CredECard.Common.BusinessService
{
    public interface IPersistable
    {
        void Save();

        ValidateResult Validate();
    }
}
