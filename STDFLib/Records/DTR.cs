namespace STDFLib
{

    /// <summary>
    /// Datalog Text Record
    /// </summary>
    public class DTR : STDFRecord
    {
        public DTR() : base(RecordTypes.DTR, "Datalog Text Record") { }

        [STDF] public string TEXT_DAT { get; set; } = "";

    }
}
