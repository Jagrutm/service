using System;
using System.Collections.Generic;
using System.Text;

namespace UserSecurity.Common
{
    public interface IAPISecurable
    {
        string Hash { get; set; }

        string Token { get; set; }

        bool ValidateHash();
    }

    public interface IAPIDataContract
    {
        string GetHashDataString();

        string GetRequestHashString();
    }

    public interface IAPIGenerateHash
    {
        string HashKey { get; set; }
        string HashDataString { get; set; }
        void GenerateHash();
    }
}
