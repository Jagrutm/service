namespace AgencyService.Domain.Enums
{
    public enum QualifiedAcceptanceCode
    {
        UnSpecifiedDay = 0080,
        SameDay = 0081,
        NextCalendarDay = 0082,
        NextWorkingDay = 0083,
        AfterTheNextWorkingDay = 0084,
        WithinTwoHours = 0085
    }
}
