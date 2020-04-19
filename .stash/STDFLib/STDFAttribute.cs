using System;

namespace STDFLib2
{
    /// <summary>
    /// Data member attribute to mark a property as able to be serialized by the STDF serialization facility.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class STDFAttribute : Attribute
    {
        public string ItemCountProperty { get; } = null;

        public STDFAttribute() { }

        public STDFAttribute(string itemCountProperty)
        {
            ItemCountProperty = itemCountProperty;
        }
    }

    /// <summary>
    /// Data member attribute to mark a property as able to be serialized by the STDF serialization facility.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class STDFOptionalAttribute : STDFAttribute
    {
    }
}