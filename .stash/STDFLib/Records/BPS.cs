namespace STDFLib2
{
    /// <summary>
    /// Begin Program Segment record
    /// </summary>

    public interface IBeginProgramSequence
    {
        string SequenceName { get; set; }
    }

    public class BPS : STDFRecord
    {
        public BPS() : base(RecordTypes.BPS, "Begin Program Section") { }

        [STDFOptional] public string SEQ_NAME { get; set; } = "";

        public override string ToString()
        {
            return "***** Begin Sequence " + SEQ_NAME + " *****";
        }
    }
}
