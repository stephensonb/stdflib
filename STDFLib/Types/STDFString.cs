using System;
using System.IO;
namespace STDFLib
{
    public class STDFString : STDFType
    {
        public STDFString(object val) : base(val)
        {
        }

        public char this[int index]
        {
            get
            {
                return ((string)Value)[index];
            }
        }

        public override int SerializedLength => ((string)Value).Length + 1;

        public override BinaryReader Deserialize(BinaryReader inStream)
        {
            try
            {
                int length = inStream.ReadByte();
                if(length > 0)
                {
                    Value = new string(inStream.ReadChars(length));
                } else
                {
                    Value = "";
                }
            } catch (EndOfStreamException)
            {
            }

            return inStream;
        }
        
        public override BinaryWriter Serialize(BinaryWriter outStream)
        {
            byte length = (byte)((string)Value).Length;
            outStream.Write(length);
            if (length > 0)
            {
                outStream.Write((string)Value);
            }
            return outStream;
        }

        public static implicit operator string(STDFString fld)
        {
            return (string)fld.Value;
        }

        public static implicit operator STDFString(string val)
        {
            return new STDFString(val);
        }
    }
}
