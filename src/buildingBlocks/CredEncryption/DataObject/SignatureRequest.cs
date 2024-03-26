namespace CredEncryption.DataObject
{
    public class SignatureRequest
    {
        public long PrimaryHSMLongIP { get; set; }
        public long SecondaryHSMLongIP { get; set; }
        public int Port { get; set; }
        public string SigningBody { get; set; }
        public short SigningKeyIndex { get; set; }
    }
}
