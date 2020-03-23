using System;
using System.IO;
using System.Text;

namespace STDFLib
{
    public class STDFBinaryReader : BinaryReader
    {
        ByteConverter Converter = new ByteConverter();
        public STDFBinaryReader(string path) : this(path, Encoding.ASCII, true) { }
        public STDFBinaryReader(string path, bool leaveOpen) : this(path, Encoding.ASCII, true) { }
        public STDFBinaryReader(string path, Encoding encoding) : this(path, encoding, true) { }
        public STDFBinaryReader(string path, Encoding encoding, bool leaveOpen) : this(File.Open(path, FileMode.Open, FileAccess.Read), encoding, leaveOpen) { }
        public STDFBinaryReader(string path, Endianness byteOrder) : this(path, byteOrder, Encoding.ASCII, true) { }
        public STDFBinaryReader(string path, Endianness byteOrder, bool leaveOpen) : this(path, byteOrder, Encoding.ASCII, leaveOpen) { }
        public STDFBinaryReader(string path, Endianness byteOrder, Encoding encoding) : this(path, byteOrder, encoding, true) { }
        public STDFBinaryReader(string path, Endianness byteOrder, Encoding encoding, bool leaveOpen) : this(path, encoding, leaveOpen)
        {
            Converter.SetEndianness(byteOrder);
        }
        public STDFBinaryReader(Stream input) : this(input, Encoding.ASCII, true) { }
        public STDFBinaryReader(Stream input, bool leaveOpen) : this(input, Encoding.ASCII, true) { }
        public STDFBinaryReader(Stream input, Encoding encoding) : this(input, encoding, true) { }
        public STDFBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) 
        {
            // Determine the endianness of the file
            input.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[sizeof(ushort)];
            input.Read(buffer, 0, 2);
            ushort recLength = Converter.ToUInt16(buffer);
            // recLength should be 2.  If it is 512, then we need to switch endianness
            if(recLength == 512)
            {
                // switch endianness
                Converter.SetEndianness(Converter.Endianness == Endianness.LittleEndian ? Endianness.BigEndian : Endianness.BigEndian);
            }
            // move back to beginning of the file
            input.Seek(0, SeekOrigin.Begin);
        }
        public STDFBinaryReader(Stream input, Endianness byteOrder) : this(input, byteOrder, Encoding.ASCII, true) { }
        public STDFBinaryReader(Stream input, Endianness byteOrder, bool leaveOpen) : this(input, byteOrder, Encoding.ASCII, leaveOpen) { }
        public STDFBinaryReader(Stream input, Endianness byteOrder, Encoding encoding) : this(input, byteOrder, encoding, true) { }
        public STDFBinaryReader(Stream input, Endianness byteOrder, Encoding encoding, bool leaveOpen) : this(input, encoding,leaveOpen)
        {
            Converter.SetEndianness(byteOrder);
        }
        public STDFBinaryReader(byte[] buffer) : this(buffer, Encoding.ASCII, true) { }
        public STDFBinaryReader(byte[] buffer, bool leaveOpen) : this(buffer, Encoding.ASCII, leaveOpen) { }
        public STDFBinaryReader(byte[] buffer, Encoding encoding) : this(buffer, encoding, true) { }
        public STDFBinaryReader(byte[] buffer, Encoding encoding, bool leaveOpen) : this(new MemoryStream(buffer, false), encoding, leaveOpen) { }
        public STDFBinaryReader(byte[] buffer, Endianness byteOrder) : this(buffer, byteOrder, Encoding.ASCII, true) { }
        public STDFBinaryReader(byte[] buffer, Endianness byteOrder, bool leaveOpen) : this(buffer, byteOrder, Encoding.ASCII, leaveOpen) { }
        public STDFBinaryReader(byte[] buffer, Endianness byteOrder, Encoding encoding) : this(buffer, byteOrder, encoding, true) { }
        public STDFBinaryReader(byte[] buffer, Endianness byteOrder, Encoding encoding, bool leaveOpen) : this(buffer, Encoding.ASCII, leaveOpen) 
        {
            Converter.SetEndianness(byteOrder);
        }

        public object ReadArray(Type elementType, int elementCount)
        {
            Array objArray = Array.CreateInstance(elementType, elementCount);
            if(elementCount > 0)
            {
                for(int i=0;i<elementCount;i++)
                {
                    objArray.SetValue(Read(elementType),i);
                }
            }
            return objArray;
        }

        public object[] ReadVarDataArray(int elementCount)
        {
            object[] objArray = new object[elementCount];
            if(elementCount > 0)
            {
                for(int i=0;i<elementCount;i++)
                {
                    objArray[i] = ReadVarDataField();
                }
            }
            return objArray;
        }

        // override default read string.  First byte has length of string (max 255 characters)
        public override string ReadString()
        {
            int length = BaseStream.ReadByte();
            if (length == 0)
            {
                return "";
            }
            return new string(ReadChars(length));
        }
        
        public override double ReadDouble() => Converter.ToDouble(ReadBytes(sizeof(double)));
        public override float ReadSingle() => Converter.ToFloat(ReadBytes(sizeof(float)));
        public override short ReadInt16() => Converter.ToInt16(ReadBytes(sizeof(short)));
        public override ushort ReadUInt16() => Converter.ToUInt16(ReadBytes(sizeof(ushort)));
        public override int ReadInt32() => Converter.ToInt32(ReadBytes(sizeof(int)));
        public override uint ReadUInt32() => Converter.ToUInt32(ReadBytes(sizeof(uint)));
        public override long ReadInt64() => Converter.ToInt64(ReadBytes(sizeof(long)));
        public override ulong ReadUInt64() => Converter.ToUInt64(ReadBytes(sizeof(ulong)));

        public BitField ReadBitField()
        {
            int length = ReadByte();
            if (length == 0)
            {
                return new BitField();
            }
            return new BitField(ReadBytes(length));
        }

        public BitField2 ReadBitField2()
        {
            int bitCount = ReadUInt16();
            if(bitCount == 0)
            {
                return new BitField2();
            }
            return new BitField2(ReadBytes((bitCount / 8) + ((bitCount % 8) > 0 ? 1 : 0)));
        }

        public Nibbles ReadNibbles(int nibbleCount)
        {
            if (nibbleCount <= 0)
            {
                return new Nibbles();
            }
            return new Nibbles(ReadBytes(nibbleCount / 2 + (nibbleCount % 2)));
        }

        public object Read(Type dataType, int itemCount=0)
        {
            if (dataType.IsArray)
            {
                if (itemCount < 0)
                {
                    itemCount = ReadByte();
                }

                if (itemCount > 0)
                {
                    return ReadArray(dataType.GetElementType(), itemCount);
                } else
                {
                    // return an empty array of the necessary data type
                    return Array.CreateInstance(dataType.GetElementType(), 0);
                }
            }

            // write the field
            return dataType.Name switch
            {
                "Bool" => ReadBoolean(),
                "DateTime" => DateTime.UnixEpoch.AddSeconds(ReadUInt32()),
                "Byte" => ReadByte(),
                "SByte" => ReadSByte(),
                "Char" => ReadChar(),
                "Int16" => ReadInt16(),
                "Int32" => ReadInt32(),
                "UInt16" => ReadUInt16(),
                "UInt32" => ReadUInt32(),
                "Single" => ReadSingle(),
                "Double" => ReadDouble(),
                "String" => ReadString(),
                "Nibbles" => ReadNibbles(itemCount),
                "BitField" => ReadBitField(),
                "BitField2" => ReadBitField2(),
                "VarData" => ReadVarDataArray(itemCount),
                _ => null,
            };
        }

        public object ReadVarDataField()
        {
            // read the type code for the next field in the stream
            byte fieldTypeCode = ReadByte();

            // if the field started on an even byte, check for a padding byte (code = 0)
            if (fieldTypeCode == 0)
            {
                // padding byte was found, so read the next byte for the actual field type.
                fieldTypeCode = ReadByte();
            }

            switch(fieldTypeCode)
            {
                case 1: return ReadByte();
                case 2: return ReadUInt16();
                case 3: return ReadUInt32();
                case 4: return ReadSByte();
                case 5: return ReadInt16();
                case 6: return ReadInt32();
                case 7: return ReadSingle();
                case 8: return ReadDouble();
                case 10: return ReadString();
                case 11: return ReadBitField();
                case 12: return ReadBitField2();
                case 13: return ReadNibbles(1);
                default:
                    throw new STDFFormatException(string.Format("Invalid file format or unsupported Generic Record field data type found in data stream.  Field data type code '{0}' found at position {1}", fieldTypeCode, BaseStream.Position));
            }
        }
    }
}
