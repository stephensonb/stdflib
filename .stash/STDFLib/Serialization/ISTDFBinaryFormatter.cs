using System.IO;

namespace STDFLib2.Serialization
{
    public interface ISTDFBinaryFormatter
    {
        ISTDFSurrogateSelector SurrogateSelector { get; set; }
        object Deserialize(Stream stream);
        void Serialize(Stream stream, object obj);
    }
}
