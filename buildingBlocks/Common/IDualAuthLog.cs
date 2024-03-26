using System;

namespace CredECard.Common.BusinessService
{
    public interface IDualAuthLog
    {
        string GetDualAuthLog( int dualAuthAction );
    }
}
