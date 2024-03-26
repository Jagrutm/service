using CredECard.BugReporting.BusinessService;
using CredECard.CommonSetting.BusinessService;
using CredEncryption.DataObject;
using System;

namespace CredEncryption.BusinessService
{
    public class SignatureHelper
    {
        /// <author>Sapan</author>
        /// <created>24-Jan-2018</created>
        /// <summary>
        /// Get Signature for clearbank base on given string. 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public static string GeneratePKCS10Signature(string requestBody)
        {
            //signature will be generated here
            
            long primaryHSMLongIP = 0;
            long secondaryHSMLongIP = 0;
            int port = 0;
            short signKeyIndex = 0;
            
            GeneralSettingList settingList = GeneralSettingList.LoadGeneralSettings();
            if (settingList != null)
            {
                GeneralSetting objSettingHSMPrimaryIP = settingList[(int)EnumGeneralSettings.PRIMARY_HSM_LONG_IP];
                GeneralSetting objSettingHSMSecondaryIP = settingList[(int)EnumGeneralSettings.SECONDARY_HSM_LONG_IP];
                GeneralSetting objSettingHSMPort = settingList[(int)EnumGeneralSettings.HSM_EXCRYPT_PORT];
                GeneralSetting objSettingSignKeyIndex = settingList[(int)EnumGeneralSettings.ContisDebitPlatform_SignKeyIndex];

                if (objSettingHSMPrimaryIP != null) long.TryParse(objSettingHSMPrimaryIP.SettingValue, out primaryHSMLongIP);
                if (objSettingHSMSecondaryIP != null) long.TryParse(objSettingHSMSecondaryIP.SettingValue, out secondaryHSMLongIP);
                if (objSettingHSMPort != null) Int32.TryParse(objSettingHSMPort.SettingValue, out port);
                if (objSettingSignKeyIndex != null) short.TryParse(objSettingSignKeyIndex.SettingValue, out signKeyIndex);
            }

            var signatureRequestBody = new SignatureRequest();

            signatureRequestBody.SigningBody = requestBody;
            signatureRequestBody.PrimaryHSMLongIP = primaryHSMLongIP;
            signatureRequestBody.SecondaryHSMLongIP = secondaryHSMLongIP;
            signatureRequestBody.Port = port;
            signatureRequestBody.SigningKeyIndex = signKeyIndex;

            return GeneratePKCS10Signature(signatureRequestBody);
        }

        public static string GeneratePKCS10Signature(SignatureRequest signatureRequest)
        {
            string signature = string.Empty;

            signature = PKCS10SigningUtil.GenerateDegitalSignature(signatureRequest.PrimaryHSMLongIP
                , signatureRequest.SecondaryHSMLongIP
                , signatureRequest.Port
                , signatureRequest.SigningBody, 
                signatureRequest.SigningKeyIndex);

            return signature;
        }

        public static bool VerifyPKCS10DigitalSignature(string bodyText, string ClientDigitalSign)
        {
            bool isVerifyDigitalSignature = false;
            try
            {
                string ClearBankClientPublicKey = GeneralSetting.GetSettingValue(EnumGeneralSettings.ContisDebitPlatform_ClientPublicKey);
                isVerifyDigitalSignature = PKCS10SigningUtil.VerifyDigitalSignature(ClientDigitalSign, bodyText.ToString(), ClearBankClientPublicKey);
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex, "Error while verifying digital signature", "", "");
                isVerifyDigitalSignature = false;
            }
            return isVerifyDigitalSignature;
        }
    }
}
