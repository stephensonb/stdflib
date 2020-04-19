namespace STDFLib2
{

    /// <summary>
    /// Datalog Text Record
    /// </summary>
    public class DTR : STDFRecord
    {
        public DTR() : base((ushort)RecordTypes.DTR) { }

        [STDF] public string TEXT_DAT = "";

        public override string ToString()
        {
            return TEXT_DAT;
        }
    }
}
