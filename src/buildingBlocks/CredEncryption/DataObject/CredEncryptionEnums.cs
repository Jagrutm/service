using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CredEcard.CredEncryption.DataObject
{
    public enum EnumEncryptFileFormat
    {
        None = 0,   //Dharati Metra: Case 22541
        Binary = 1,
        ASCII = 2
    }

    public enum EnumFileEncryptionMethod
    {
        None = 0,
        PGP = 1,
    }

    public enum EnumPGPSignature
    {
        BinaryDocument = 0,
        CanonicalTextDocument = 1,
        CasualCertification = 18,
        CertificationRevocation = 48,
        DefaultCertification = 16,
        DirectKey = 31,
        KeyRevocation = 32,
        NoCertification = 17,
        PositiveCertification = 19,
        PrimaryKeyBinding = 25,
        StandAlone = 2,
        SubkeyBinding = 24,
        SubkeyRevocation = 40,
        Timestamp = 64,
    }
}
