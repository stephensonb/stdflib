namespace STDFLib
{
    /// <summary>
    /// End Program Section Record
    /// </summary>
    public class EPS : STDFRecord
    {
        protected override RecordType TypeCode => 0x2020;
        public override string Description => "End Program Section";
    }
}
