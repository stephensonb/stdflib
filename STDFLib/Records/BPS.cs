﻿namespace STDFLib
{
    /// <summary>
    /// Begin Program Segment record
    /// </summary>
    public partial class BPS : STDFRecord
    {
        public BPS() : base(RecordTypes.BPS) { }

        [STDF] public string SEQ_NAME = "";

        public override string ToString()
        {
            return "***** Begin Sequence " + SEQ_NAME + " *****";
        }
    }
}
