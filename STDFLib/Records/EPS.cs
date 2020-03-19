namespace STDFLib
{
    /// <summary>
    /// End Program Section Record
    /// </summary>
    public class EPS : STDFRecord
    {
        public override RecordType TypeCode => 0x1414;
        public override string Description => "End Program Section";
    }
}
