using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Common.CBGEnums
{
    public enum UserTypes
    {
        Customer = 1,
        Merchant = 2,
        Justcontact = 3,
        Staff = 4,
        APIUser = 5, //case 16246
        WebCRM = 6,
        SchemeClient = 7,//case 16245
        Business = 8, //C:25461 - HC
        ResourceManagement = 9, //Mahesh Vala : 46454
        Dashboard = 10
    }

    public enum enumUserTypes
    {
        None = 0,
        [XmlEnum("1"), Description("Consumer")]
        Customer = 1,
        [XmlEnum("2")]
        Merchant = 2,
        JustContact = 3,
        Staff = 4,
        ApiUser = 5,
        WebCRM = 6,
        SchemeClient = 7,//case 16245
        Business = 8, // C:25461 - HC
        ResourceManagement = 9,//Mahesh Vala : 46454
        Dashboard = 10
    }

    public enum UserStatusType
    {
        Unknown = 0,
        [XmlEnum("1"), Description("Active - Live")]
        Active = 1,
        [XmlEnum("2"), Description("Inactive - Prior to setup being completed, no transfers or transactions allowed")]
        Inactive = 2,
        [XmlEnum("3"), Description("Incomplete - Prior to setup being completed, no transfers or transactions allowed")]
        Incomplete = 3,
        [XmlEnum("4"), Description("Checked - Additional information required for setup to be completed, no transfers or transactions allowed")]
        Checked = 4,
        Dead = 5,
        [XmlEnum("6"), Description("PartSignup - Information missing to complete setup, no transfers or transactions allowed")]
        PartSignup = 6,
        [XmlEnum("7"), Description("Declined - No transfers or transactions allowed")]
        Declined = 7,
        [XmlEnum("8"), Description("Bogus - No transfers or transactions allowed")]
        Bogus = 8,
        [XmlEnum("9"), Description("Limited - Prior to setup being completed, no transfers or transactions allowed")]
        Limited = 9,
        [XmlEnum("10"), Description("BankCheck - No transfers or transactions allowed")]
        BankCheck = 10,
        WifiLimited = 11,
        [XmlEnum("12"), Description("LockedOut - Close All a/c s and close the user permanently.")]
        LockedOut = 12,
        [XmlEnum("13"), Description("Suspended - All the user activity will be blocked. User status can be changed to LockedOut or Active")]
        Suspended = 13
    }

    public enum EnumBusinessUserType
    {
        NormalUser = 0,
        Director = 1,
        DepartmentHead = 2,
        BusinessCardHolder = 3
    }

    public enum UserDetailsType
    {
        All,
        [XmlEnum("1")]
        Primary,
        [XmlEnum("2")]
        Secondary,
        [XmlEnum("4")]
        Authoriser = 4
    }

    public enum detailType
    {
        Primary = 1,
        Alternative = 2,
        Other = 3,
        Authoriser = 4
    }

    public enum EnumApiLogType
    {
        LOG_IN_FILE = 1,
        LOG_IN_DB = 2
    }

    public enum EnumContentType
    {
        None = 0,
        [XmlEnum("1")]
        JSON = 1,
        [XmlEnum("2")]
        Image = 2
    }

    public enum EnumPromoCodeCriteria
    {
        NONE = 0,
        ONLY_WORK_ONCE = 1,
        LIMITED_USE_WITH_EXPIRE = 2,
        UNLIMITED_USE_WITH_EXPIRE = 3,
        UNLIMITED_USE_WILL_NOT_EXPIRED = 4
    }

    public enum EnumSchemeStatus
    {
        Normal = 1,
        Closed = 2,
        Archived = 3
    }

    public enum enumSystemUserTypes
    {
        None = 0,
        [XmlEnum("1"), Description("Consumer")]
        Customer = 1,
        [XmlEnum("5"), Description("Api User")]
        ApiUser = 5,
        [XmlEnum("10"), Description("Dashboard")]
        Dashboard = 10
    }
}
