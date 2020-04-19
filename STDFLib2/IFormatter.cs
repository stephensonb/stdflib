using System.IO;

namespace STDFLib2
{
    public interface IFormatter
    {
        object Deserialize(Stream stream);
        void Serialize(Stream stream, object obj);
    }
}
