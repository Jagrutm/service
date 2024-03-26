using System;
using System.Collections;
using System.Text;
using System.IO;

using CredECard.Common.BusinessService;

using CredECard.Common.Enums.Transaction;
using ContisGroup.MessageParser.ISO8586Parser;
using ContisGroup.MessageParser.ISO8586Parser.Interface;


namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Prashant Soni</author>
    /// <created>26-Sep-2006</created>
    /// <summary>
    /// </summary>
    /// <exception cref="ValidateException">
    /// </exception>
    [Serializable()]
    public class ISO85831987MessageParser :ISO8583MessageParser
    {

        public ISO85831987MessageParser()
        {
            _iso8583Version = EnumISOVesion.ISO8583_1987;
        }
    }

 
}

