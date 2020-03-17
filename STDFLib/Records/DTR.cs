namespace STDFLib
{

    /// <summary>
    /// Datalog Text Record
    /// </summary>
    public class DTR : STDFRecord
    {
        protected override RecordType TypeCode => 0x5030;

        [STDF(Order = 1)]
        public string TEXT_DAT { get; set; } = "";

        public override string Description => "Datalog Text Record";

    }
}
