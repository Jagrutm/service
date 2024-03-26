namespace Common.CBGEnums
{
    public enum EnumBACSDataRecordType : byte
    {
        NONE = 0
        , VOL1 = 1	//VOLUME HEADER LABEL ONE
        , HDR1 = 2	//HEADER LABEL ONE
        , HDR2 = 3	//HEADER LABEL TWO
        , UHL1 = 4	//USER HEADER LABEL ONE
        , DATA = 5	//DATA RECORD
        , EOF1 = 6  //END OF FILE LABEL ONE
        , EOF2 = 7  //END OF FILE LABEL TWO
        , UTL1 = 8  //USER TRAILER LABEL ONE
    }
}
