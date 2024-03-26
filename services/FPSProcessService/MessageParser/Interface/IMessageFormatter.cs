using System;
using CredECard.Common.BusinessService;

namespace ContisGroup.MessageParser.ISO8586Parser.Interface
{
    public interface IMessageFormatter
    {
        byte[] BinaryDataMessage{ get;set;}
        void ParseMessage(string reportSubGroupNo = "");
    }
}
