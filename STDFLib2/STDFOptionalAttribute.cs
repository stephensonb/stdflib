using System;

namespace STDFLib2
{
    /// <summary>
    /// Data member attribute to mark a property as able to be serialized by the STDF serialization facility.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class STDFOptionalAttribute : STDFAttribute
    {
        public STDFOptionalAttribute() : base() { }

        public STDFOptionalAttribute(string itemCountProperty) : base(itemCountProperty) { }
    }
}