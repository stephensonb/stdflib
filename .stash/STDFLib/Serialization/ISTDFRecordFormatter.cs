using System;
using System.IO;
using System.Reflection;

// Formatter - serializes an object to or from a binary stream
// Converter - converts custom objects to a value of a specific type
// Surrogate - provides custom serialization for an object
// SurrogateSelector - determines which surrogate to use for serialization of custom types
// ByteConverter - converts value types to/from binary representation, honoring endianness

namespace STDFLib2.Serialization
{
    public interface ISTDFRecordFormatter : ISTDFBinaryFormatter, IDisposable
    {
        ISTDFRecord Deserialize(ISTDFBinaryReader reader, ISTDFRecord record);
        void Deserialize(ISTDFBinaryReader reader, ISTDFRecord record, PropertyInfo prop);
        void Serialize(ISTDFBinaryWriter writer, ISTDFRecord record);
        void Serialize(ISTDFBinaryWriter writer, ISTDFRecord record, PropertyInfo prop, out bool stopSerializing);
    }
}
