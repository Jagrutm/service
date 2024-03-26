using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CredECard.Common.Enums.WebMethod
{
    public enum EnumWebMethod
    {
        CreateUser = 1,
        CreateUserWithVoucherCode = 2,
        CreateContact = 3,
        CreateAccount = 4,
        ProposeAgreement = 5,
        SignAgreement = 6,
        GetAddressInfoList = 7,
        GetAgreementInfo = 8,
        AddCardRequest = 9,
        QueueKYC = 10,
        CardPayment = 11,
        GetBarCodeURL = 12,
        CreateUserWithSMSCode = 13,
        SignAgreementForCardProgram = 14
    }

}
