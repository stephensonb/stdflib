using System;
using System.Reflection;

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

        public string ItemCountProperty { get; set; } = null;

        // If set > 0 and applied to a field of type string or string[], then each string
        // written will be written with a fixed length.  If the string length is greater than
        // StringFixedLength then it will be truncated.  If the string length is less than
        // StringFixedLength, then it will be padded with zeroes.
        public int DataLength { get; set; } = -1;

        public STDFAttribute() : base() { }

        public STDFAttribute(string itemCountProperty) : base()
        {
            ItemCountProperty = itemCountProperty;
        }
    }
}
