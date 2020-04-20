using System;

namespace STDFLib
{
    /// <summary>
    /// Data member attribute to mark a property as able to be serialized by the STDF serialization facility.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class STDFAttribute : Attribute
    {
        public string ItemCountProperty { get; } = null;

        public STDFAttribute() { }

        public STDFAttribute(string itemCountProperty)
        {
            ItemCountProperty = itemCountProperty;
        }
    }
}