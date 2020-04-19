namespace STDFLib2
{
    /// <summary>
    /// End Program Section Record
    /// </summary>
    public class EPS : STDFRecord
    {
        public EPS() : base((ushort)RecordTypes.EPS)
        {
        }
        public override string ToString()
        {
            return "***** End Sequence *****";
        }
    }
}
