namespace STDFLib
{

    /// <summary>
    /// Datalog Text Record
    /// </summary>
    public class DTR : STDFRecord
    {
        public DTR() : base(RecordTypes.DTR) { }

        [STDF] public string TEXT_DAT = "";

        public override string ToString()
        {
            return TEXT_DAT;
        }
    }
}
