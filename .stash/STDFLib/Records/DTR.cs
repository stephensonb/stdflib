namespace STDFLib2
{

    /// <summary>
    /// Datalog Text Record
    /// </summary>
    public class DTR : STDFRecord
    {
        public DTR() : base(RecordTypes.DTR, "Datalog Text Record") { }

        [STDF] public string TEXT_DAT { get; set; } = "";

        public override string ToString()
        {
            return TEXT_DAT;
        }
    }
}
