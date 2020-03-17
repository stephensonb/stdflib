using System.IO;
using System.Reflection;
using STDFLib.Serialization;

namespace STDFLib.STDF
{
    public class BitFieldSerializer : DefaultSerializer
    {
        public override object DeserializeObject(BinaryReader br, SerializationParameters parms)
        {
            BitField b = new BitField();

            b.Length = br.ReadByte();  // First byte is the length if the bit array in bytes;

            if (b.Length == 0) return null;  // If the bit array length is zero, then return.

            b.BitArray = br.ReadBytes(b.Length);

            return b;
        }

        public override void SerializeObject(BinaryWriter bw, object propertyValue, SerializationParameters parms)
        {
            // If the bitfield is null, then it is not set in the record.
            if (propertyValue == null)
            {
                bw.Write((byte)0);   // Write zero length for the bitfield and then return
                return;
            }

            var pVal = propertyValue as BitField;

            // Write the length byte first.  Length is the length of the bit array in bytes
            bw.Write((byte)pVal.Length);

            // If the length is zero, then return;
            if (pVal.Length == 0) return;

            // Write the bitfield bytes next
            bw.Write(pVal.BitArray, 0, pVal.Length);
        }
    }
}
