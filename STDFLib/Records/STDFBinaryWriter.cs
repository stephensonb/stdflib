using System;
using System.IO;
using System.Text;

namespace STDFLib
{
    public class STDFBinaryWriter : BinaryWriter
    {
        private ByteConverter Converter = new ByteConverter();

        public STDFBinaryWriter(string path) : this(path, Encoding.ASCII, true) { }
        public STDFBinaryWriter(string path, bool leaveOpen) : this(path, Encoding.ASCII, leaveOpen) { }
        public STDFBinaryWriter(string path, Encoding encoding) : this(path, encoding,true) { }
        public STDFBinaryWriter(string path, Encoding encoding, bool leaveOpen) : this(File.Open(path, FileMode.Create, FileAccess.ReadWrite), encoding, leaveOpen) { }
        public STDFBinaryWriter(string path, Endianness byteOrder) : this(path, byteOrder, Encoding.ASCII, true) { }
        public STDFBinaryWriter(string path, Endianness byteOrder, Encoding encoding) : this(path,byteOrder,encoding, true) { }
        public STDFBinaryWriter(string path, Endianness byteOrder, Encoding encoding, bool leaveOpen) : this(path, encoding, leaveOpen)
        {
            Converter.SetEndianness(byteOrder);
        }
        public STDFBinaryWriter(Stream output) : this(output, Encoding.ASCII, true) { }
        public STDFBinaryWriter(Stream output, bool leaveOpen) : this(output, Encoding.ASCII, leaveOpen) { }
        public STDFBinaryWriter(Stream output, Encoding encoding) : this(output, encoding, true) { }
        public STDFBinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) { }
        public STDFBinaryWriter(Stream output, Endianness byteOrder) : this(output, byteOrder, Encoding.ASCII, true) { }
        public STDFBinaryWriter(Stream output, Endianness byteOrder, bool leaveOpen) : this(output, byteOrder, Encoding.ASCII, leaveOpen) { }
        public STDFBinaryWriter(Stream output, Endianness byteOrder, Encoding encoding) : this(output, byteOrder, encoding, true) { }
        public STDFBinaryWriter(Stream output, Endianness byteOrder, Encoding encoding, bool leaveOpen) : this(output, encoding, leaveOpen)
        {
            Converter.SetEndianness(byteOrder);
        }
        public STDFBinaryWriter(byte[] buffer) : this(buffer, Encoding.ASCII, true) { }
        public STDFBinaryWriter(byte[] buffer, bool leaveOpen) : this(buffer, Encoding.ASCII, leaveOpen) { }
        public STDFBinaryWriter(byte[] buffer, Encoding encoding) : this(buffer, encoding, true) { }
        public STDFBinaryWriter(byte[] buffer, Encoding encoding, bool leaveOpen) : this(new MemoryStream(buffer, true), encoding, leaveOpen) { }
        public STDFBinaryWriter(byte[] buffer, Endianness byteOrder) : this(buffer, byteOrder, Encoding.ASCII, true) { }
        public STDFBinaryWriter(byte[] buffer, Endianness byteOrder, bool leaveOpen) : this(buffer, byteOrder, Encoding.ASCII, leaveOpen) { }
        public STDFBinaryWriter(byte[] buffer, Endianness byteOrder, Encoding encoding) : this(buffer, byteOrder, encoding, true) { }
        public STDFBinaryWriter(byte[] buffer, Endianness byteOrder, Encoding encoding, bool leaveOpen) : this(buffer, Encoding.ASCII, leaveOpen)
        {
            Converter.SetEndianness(byteOrder);
        }

        public void WriteArray(object[] values, int itemCount=-1)
        {
            if (itemCount < 0)
            {
                itemCount = values.Length;
            }

            for(int i=0;i<itemCount;i++)
            {
                Write(values[i]);
            }
        }

        public void WriteVarDataArray(object[] values, int itemCount=-1)
        {
            if(itemCount < 0)
            {
                itemCount = values.Length;
            }

            for(int i=0;i<itemCount;i++)
            {
                WriteVarData(values[i]);
            }
        }

        public override void Write(double value)
        {
            Write(Converter.GetBytes(value));
        }

        public override void Write(float value)
        {
            Write(Converter.GetBytes(value));
        }

        public override void Write(int value)
        {
            Write(Converter.GetBytes(value));
        }

        public override void Write(long value)
        {
            Write(Converter.GetBytes(value));
        }

        public override void Write(short value)
        {
            Write(Converter.GetBytes(value));
        }

        public override void Write(string value)
        {
            Write(Converter.GetBytes(value));
        }

        public override void Write(uint value)
        {
            Write(Converter.GetBytes(value));
        }

        public override void Write(ulong value)
        {
            Write(Converter.GetBytes(value));
        }

        public override void Write(ushort value)
        {
            Write(Converter.GetBytes(value));
        }

        public void Write(Nibbles value)
        {
            Write(Converter.GetBytes(value));
        }

        public void Write(BitField value)
        {
            Write(Converter.GetBytes(value));
        }

        public void Write(BitField2 value)
        {
            Write(Converter.GetBytes(value));
        }

        public void Write(object value)
        {
            // write the field
            switch(value.GetType().Name)
            {
                case "Bool":
                    Write((byte)((bool)value ? 1 : 0));
                    break;
                case "Char":
                    Write((char)value);
                    break;
                case "DateTime":
                    // write number of seconds between the date/time of the value and the unix epoch (1/1/1970 00:00:00)
                    Write((uint)((DateTime)value).Subtract(DateTime.UnixEpoch).Seconds);
                    break;
                case "Int16":
                    Write((short)value);
                    break;
                case "Int32":
                    Write((int)value);
                    break;
                case "UInt16":
                    Write((ushort)value);
                    break;
                case "UInt32":
                    Write((uint)value);
                    break;
                case "Single":
                    Write((float)value);
                    break;
                case "Double":
                    Write((double)value);
                    break;
                case "String":
                    Write((string)value);
                    break;
                case "Nibbles":
                    Write((Nibbles)value);
                    break;
                case "BitField":
                    Write((BitField)value);
                    break;
                case "BitField2":
                    Write((BitField2)value);
                    break;
            }
        }

        public void WriteVarData(object value)
        {
            bool evenByte = (BaseStream.Position % 2) == 0 ? true : false;
            if (evenByte)
            {
                // If we are on an even byte in the stream and we are writing one of the 
                // following types, then we need to add a padding byte as these need to start writing on an even byte
                // (writing the type code would leave the stream position on an odd byte)
                switch (value.GetType().Name)
                {
                    case "Int16":
                    case "Int32":
                    case "UInt16":
                    case "UInt32":
                    case "Single":
                    case "Double":
                        // write a padding byte
                        Write((byte)0);
                        break;
                }
            }
            // write the field type code
            Write((byte)VarData.GetFieldTypeCode(value));

            // write the data field
            Write(value);
        }
    }
}
