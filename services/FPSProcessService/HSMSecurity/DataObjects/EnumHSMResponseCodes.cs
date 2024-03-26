using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContisGroup.CardUtil.DataObjects
{
    public enum EnumPinResponseCodes
    {
        Success = 0,
        //PINTriesExceeded = 2,
        //Inactive = 3,
        //CardExpired = 4,
        //Lost = 5,
        //Stolen = 6,
        //Issuercancelled = 8,

        //CVV2Success_CardInactive = 994,
        //ICVVSuccess_PINTriesExceeded = 995,
        InvalidPIN = 996,
        UnsafePIN = 997, //Unsafe PIN
        UnableToVerifyPIN = 998, //In case of HSM Exception
        Failed = 999, //InvalidCVV2, Invalid PIN, invalid Pin Change
    }


}
