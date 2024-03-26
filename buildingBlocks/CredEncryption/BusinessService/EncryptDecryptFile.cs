using CredECard.Common.BusinessService;

namespace CredEcard.CredEncryption.BusinessService
{
    public class EncryptDecryptFile
    {
        public static void EncryptFile(string inFileNameWithPath, string outFileNameWithPath, Encryptionkey  setup)
        {
            switch (setup.EncryptionMethodID)
            {
                case 1:
                    Encryptionkey signKey = setup.SignkeyTypeDetail;

                    if(signKey  == null)
                        PGPEncryptDecrypt.Encrypt(inFileNameWithPath, outFileNameWithPath, setup.DecryptdKeyValue, setup.EncryptFileFormat, setup.SymmetricKeyAlgorithm);
                    else
                        PGPEncryptDecrypt.SignAndEncrypt(inFileNameWithPath, outFileNameWithPath, setup.DecryptdKeyValue, setup.EncryptFileFormat, 
                            signKey.DecryptdKeyValue, signKey.Phrase, signKey.SecretKey,signKey.CheckIntegrity,signKey.SymmetricKeyAlgorithm);
                    break;
                default:
                    throw new PersistException("Encryption Method not implemented");
            }
        }

        public static void DecryptFile(string inFileNameWithPath, string outFileNameWithPath, Encryptionkey setup)
        {
            switch (setup.EncryptionMethodID)
            {
                case 1:
                    PGPEncryptDecrypt.Decrypt(inFileNameWithPath, outFileNameWithPath, setup.DecryptdKeyValue, setup.Phrase);
                    break;
                default:
                    throw new PersistException("Decrypt Method not implemented");
            }
        }
    }
}
