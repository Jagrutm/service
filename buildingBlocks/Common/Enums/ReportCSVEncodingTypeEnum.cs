
namespace CredECard.Common.Enums
{
    /// <author>Nidhi Thakrar</author>
    /// <created>31-Jul-2013</created>
    /// <summary>Encoding types for exporting reports in CSV format</summary>
    public enum EnumReportCSVEncodingType
    {
        None = 0,
        UTF7 = 1,
        UTF8_With_BOM = 2,
        UTF8_Without_BOM = 3,
        Unicode_With_BOM = 4,
        Unicode_Without_BOM = 5,
        ASCII = 6
    }

    /// <summary>
    /// Enum for Separator types
    /// </summary>
    /// <Author>Dharati Metra</Author>
    /// <CreatedDate>24-Jul-2014</CreatedDate>
    public enum EnumSeparatorTypes
    {
        Comma = 1,
        Tab = 2
    }
}
