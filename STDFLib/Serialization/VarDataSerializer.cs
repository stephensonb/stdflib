using System.Linq;
using System.IO;
using System.Reflection;
using STDFLib.Serialization;

namespace STDFLib.STDF
{
    public class VarDataSerializer : DefaultSerializer
    {
        public override object DeserializeObject(BinaryReader br, SerializationParameters parms)
        {
            object pVal = null;

            var fTyp = br.ReadByte();  // Read the data type byte

            if (fTyp != 0)
            {
                pVal = new VarData(ReadFieldType(br, fTyp, parms));
            }

            return pVal;
        }

        public override void SerializeObject(BinaryWriter bw, object objValue, SerializationParameters parms)
        {
            if (objValue == null) return;

            var pVal = objValue as VarData;

            if (pVal != null)
            {
                bw.Write(pVal.DataType);
                Serialize(bw, pVal.Data, parms);
            }
        }

        public object ReadFieldType(BinaryReader br, byte fieldType, SerializationParameters parms)
        {
            switch (fieldType)
            {
                case 0:
                    return null;
                case 1:
                    return br.ReadByte();
                case 2:
                    return br.ReadUInt16();
                case 3:
                    return br.ReadUInt32();
                case 4:
                    return br.ReadSByte();
                case 5:
                    return br.ReadInt16();
                case 6:
                    return br.ReadInt32();
                case 7:
                    return br.ReadSingle();
                case 8:
                    return br.ReadDouble();
                case 10:
                    return Deserialize(br, typeof(string), parms);
                case 11:
                    return Deserialize(br, typeof(BitField), parms);
                case 12:
                    return Deserialize(br, typeof(BitField2), parms);
                case 13:
                    return Deserialize(br, typeof(Nibbles), parms);
            }
            return null;
        }
    }
}
