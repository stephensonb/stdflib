using System.IO;
using System.Reflection;

namespace STDFLib
{
    public class BitField2Serializer : DefaultSerializer
    {
        public override object DeserializeObject(BinaryReader br, SerializationParameters parms)
        {
            BitField2 b = new BitField2();

            b.Length = br.ReadUInt16();  // First 2 bytes is the bitfield length in bits.  NOTE: this must be converted to bytes 

            if (b.Length == 0) return null; // If zero, then return, nothing else to read.

            var slen = BitsToBytes(b.Length);

            b.BitArray = br.ReadBytes(slen);

            return b;
        }

        public override void SerializeObject(BinaryWriter bw, object propertyValue, SerializationParameters parms)
        {
            // If the value is null (not set in the record)
            if (propertyValue == null)
            {
                bw.Write((ushort)0);  // Write a zero length and then return
                return;
            }

            var pVal = propertyValue as BitField2;

            // Write the length byte first.  Note this is the lenght in bits of the bit field, not the number of bytes to write/read
            bw.Write((ushort)pVal.Length);

            var slen = BitsToBytes(pVal.Length);

            // If the length is zero then return
            if (slen == 0) return;

            // Write the bitfield bytes next
            bw.Write(pVal.BitArray, 0, slen); // MUST CONVERT THE BITS TO BYTES!!!!
        }
    }
}