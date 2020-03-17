using System;

namespace STDFLib
{
    /// <summary>
    /// Data member attribute to mark a property as able to be serialized by the STDF serialization facility.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class STDFAttribute: Attribute
    {
        // data members are written/read in index order, from smallest to largest.
        public int Order { get; set; } = int.MinValue;

        // Array length coding - specifies how to indicate the array length in the file for properties that are
        // array types.  Default is byte count coding, which means number of items is in first byte
        // If type is 'ItemCountCoding.Raw' then the value of DataLength is used to determine the number of items 
        // to read/write from the array
        public ItemCountCoding ItemCountCoding { get; set; } = ItemCountCoding.FirstByte;

        // If ItemCountCoding is 'ItemCountCoding.Property' then the property named here will be used to get/set
        // the number of items to be read/written from/to the stream.
        public string ItemCountProvider { get; set; } = null;

        // If set > 0 and applied to a field of type string or string[], then each string
        // written will be written with a fixed length.  If the string length is greater than
        // StringFixedLength then it will be truncated.  If the string length is less than
        // StringFixedLength, then it will be padded with zeroes.
        public int DataLength { get; set; } = -1;

        public STDFAttribute()
        {
        }
    }
}
