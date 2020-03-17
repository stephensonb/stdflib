using System.IO;
using System.Linq;

namespace STDFLib.STDF
{
    public class ByteArray 
    {
        public byte[] Values;

        public GetItemCount ItemCount { get; set; } = null;

        public void Serialize(BinaryWriter bw, StringPaddingOptions options)
        {
            int count = (ItemCount == null) ? Values.Length : ItemCount();
            bw.Write(Values, 0, count);
        }

        public void Deserialize(BinaryReader br)
        {
            int count = (ItemCount == null) ? Values.Length : ItemCount();
            Values = br.ReadBytes(count);
        }

        public int GetSerializedObjectSize()
        {
            return Values.Length;
        }

        public byte[] GetBytes()
        {
            return Values.ToArray();
        }
    }
}
