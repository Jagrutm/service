using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CredECard.Common.Enums.RecordType
{
    public enum EnumRecordType
    {
        FILE_HEADER	 =1
        ,DATA_HEADER=2
        ,FILE_TRAILER=99
        ,Card_Request=11
        ,Card_Issued=12
        ,Card_Exception=13
		,AUTH_ADV=21
        ,AUTH_REV=22
        ,FIN_ADV=23
        ,FIN_REV=24
        ,CHARGEBACK=25
        ,REPRESENTMENT_ADV=26
        ,REPRESENTMENT_REV=27
        ,RETURN=28

    }
}
