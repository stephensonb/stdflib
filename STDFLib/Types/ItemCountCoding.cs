namespace STDFLib
{
    public enum ItemCountCoding
    {
        FirstByte = 1,     // Item count contained in first byte
        First2Bytes = 2,   // Item count contained in first 2 bytes (UInt32)
        First4Bytes = 3,   // Item count contained in first 4 bytes (UInt64)
        None = 0,          // No item count prefix
        OtherProperty = 5  // Get item count from another property
    }
}

