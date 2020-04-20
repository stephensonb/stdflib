namespace STDFLib
{
    /// <summary>
    /// End Program Section Record
    /// </summary>
    public class EPS : STDFRecord
    {
        public EPS() : base(RecordTypes.EPS)
        {
        }
        public override string ToString()
        {
            return "***** End Sequence *****";
        }
    }
}
