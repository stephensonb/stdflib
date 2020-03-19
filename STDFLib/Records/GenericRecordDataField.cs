namespace STDFLib
{
    /// <summary>
    /// Defines field used in the Generic Data Record (GDR) format.
    /// </summary>
    public class GenericRecordDataField
    {
        public byte FieldType { get; set; } = 0;
        public object Value { get; set; } = null;

        public override string ToString()
        {
            return string.Format("{0,25}:{1,-30}", FieldType, Value.ToString());
        }
    }
}
