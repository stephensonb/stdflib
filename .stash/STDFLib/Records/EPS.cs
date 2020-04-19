namespace STDFLib2
{
    /// <summary>
    /// End Program Section Record
    /// </summary>
    public class EPS : STDFRecord
    {
        public EPS() : base(RecordTypes.EPS, "End Program Section")
        {
        }
        public override string ToString()
        {
            return "***** End Sequence *****";
        }
    }
}
