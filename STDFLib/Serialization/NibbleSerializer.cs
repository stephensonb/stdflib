using System;
using System.IO;
using System.Reflection;

namespace STDFLib
{
    public class NibbleSerializer : DefaultSerializer
    {
        public override object DeserializeObject(BinaryReader br, SerializationParameters parms)
        {
            Nibbles nibbleArray;

            try
            {
                if (parms.ArrayItemCount != null)
                {
                    nibbleArray = new Nibbles((int)parms.ArrayItemCount);
                    nibbleArray.SetNibbleBytes(br.ReadBytes(((int)parms.ArrayItemCount / 2)+1));
                    return nibbleArray;
                }

                // If there was no item count, then this is a single nibble.
                nibbleArray = new Nibbles(1);
                nibbleArray.SetNibbleBytes(br.ReadBytes(1));
                return nibbleArray;
            }
            catch
            {
                return null;
            }
        }

        public override void SerializeObject(BinaryWriter bw, object objValue, SerializationParameters parms)
        {

            if (objValue == null) return;

            var pVal = objValue as Nibbles;

            if (pVal != null)
            {
                if (parms.ArrayItemCount != null)
                {
                    bw.Write(pVal.GetNibbleBytes((int)parms.ArrayItemCount));
                }
                else
                {
                    bw.Write(pVal[0]);  // Just write out the first nibble byte;
                }
                return;
            }
        }
    }
}
