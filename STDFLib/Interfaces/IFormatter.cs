using System.IO;

namespace STDFLib
{
    /// <summary>
    /// Interface for objects that support serialization and deserialization.
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Deserializes and returns an object from an input data stream.
        /// </summary>
        /// <param name="stream">Stream to read serialized data from.</param>
        /// <returns></returns>
        object Deserialize(Stream stream);
        /// <summary>
        /// Serializes an object to an output data stream.
        /// </summary>
        /// <param name="stream">Stream to write serialized data to.</param>
        /// <param name="obj">Object to serialize.</param>
        void Serialize(Stream stream, object obj);
    }
}
