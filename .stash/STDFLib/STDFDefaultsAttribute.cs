using System;

namespace STDFLib2
{
    /// <summary>
    /// Marks a class implementing the ISTDFRecord interface as being able to provide default values
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class STDFDefaultsAttribute : Attribute
    {
        public STDFDefaultsAttribute() : base() { }
    }

}