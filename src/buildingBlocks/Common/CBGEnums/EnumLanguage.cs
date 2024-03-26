using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Common.CBGEnums
{
    public enum EnumLanguage
    {
        None = 0,
        [XmlEnum("1")]
        English = 1,
        [XmlEnum("2")]
        Russian = 2,
        [XmlEnum("3")]
        Latvian = 3,
        [XmlEnum("4")]
        EnglishLatvian = 4,
        [XmlEnum("5")]
        Polish = 5,
        [XmlEnum("6")]
        Lithuanian = 6,
        [XmlEnum("8")]
        Chinese = 8,
        [XmlEnum("9")]
        Albanian = 9

    }

    public enum EnumCulture
    {
        [Description("en-GB")]
        en_GB = 1,
        [Description("ru-RU")]
        ru_RU = 2,
        [Description("lv-LV")]
        lv_LV = 3,
        [Description("eng-LV")]
        eng_LV = 4,
        [Description("en-FFREES")]
        en_FFREES = 5,
        [Description("en-MMona")]
        en_MMona = 6,
        [Description("en-SEASONS")]
        en_SEASONS = 7,
        [Description("en-SUITME")]
        en_SUITME = 8,
        [Description("en-SPROUT")]
        en_SPROUT = 9,
        [Description("en-Paze")]
        en_Paze = 10,
        [Description("en-MORSES")]
        en_MORSES = 11,
        [Description("pl")]
        pl = 12,
        [Description("lt")]
        lt = 13,
        [Description("en-ENGAGE")]
        en_ENGAGE = 14,
        [Description("en-OESFUS")]
        en_OESFUS = 15,
        [Description("en-ESFBLK")]
        en_ESFBLK = 16,
        [Description("zh-Hans")]
        zh_Hans = 19
    }

}
