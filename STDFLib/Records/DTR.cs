namespace STDFLib
{

    /// <summary>
    /// Datalog Text Record
    /// </summary>
    public class DTR : STDFRecord
    {
        public override RecordType TypeCode => 0x321E;

        [STDF(Order = 1)]
        public string TEXT_DAT { get; set; } = "";

        public override string Description => "Datalog Text Record";

    }
}
