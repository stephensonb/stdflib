using System;
using System.IO;
using System.Reflection;

namespace STDFLib
{
    public interface ISTDFRecord
    {
        uint REC_LEN { get; }
        byte REC_TYP { get; }
        byte REC_SUB { get; }    
    }

    public class STDFRecord : ISTDFRecord
    {
        private RecordType typeCode { get; set; } = 0x0000;
        private byte[] buffer;
        public uint REC_LEN { get; private set; }
        public byte REC_TYP { get => typeCode.REC_TYP; }
        public byte REC_SUB { get => typeCode.REC_SUB; }
        public string Description { get; private set; } = "Unknown record type";

        public STDFRecord(RecordType recType, byte[] buffer)
        {
            typeCode = recType;
            this.buffer = buffer;
        }

        public override string ToString()
        {
            using (StringWriter sw = new StringWriter()) 
            {
                sw.WriteLine("=========================================================");
                sw.WriteLine(" Record Type: {0:2x}   Record Subtype: {1:2x}  Length: {2:5d}", REC_TYP, REC_SUB, REC_LEN);
                sw.WriteLine(" Record Description: {0}", Description);
                sw.WriteLine("=========================================================");
                
                foreach(var prop in this.GetType().GetProperties())
                {
                
                }

                sw.Flush();
                return sw.ToString();
            } 
        }
    }

    public enum SeekDirection 
    {
        Next,
        Previous,
        First,
        Last,
        End,
        Start
    }

    public class RecordType
    {
        public ushort TypeCode;
        public byte REC_TYP { get => (byte)((TypeCode & 0xFF00) >> 8); }
        public byte REC_SUB { get => (byte)(TypeCode & 0x00FF); }

        public static implicit operator RecordType(int v)
        {
            return new RecordType() { TypeCode = (ushort)v };
        }
    }

    public class STDFile
    {
        long currentPosition = 0;
        public long seekRecord(SeekDirection dir) { return currentPosition; }
        public long seekFile(SeekDirection dir) { return currentPosition; }
        public long seekRecordType(SeekDirection dir) { return currentPosition; }
    }

    public enum Endianness
    {
        LittleEndian=0,
        BigEndian=1
    }

    public class ByteConverter
    {
        Endianness endianness;

        public ByteConverter()
        {
            this.endianness = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;
        }

        public void SetEndianness(Endianness endianness)
        {
            this.endianness = endianness;
        }

        public short ToInt16(byte[] buffer, int start=0)
        {
            return (short)ToUInt16(buffer, start);
        }

        public ushort ToUInt16(byte[] buffer, int start=0)
        {
            return (ushort)ToUInt(buffer, start, 2);
        }

        public int ToInt32(byte[] buffer, int start = 0)
        {
            return (int)ToUInt(buffer, start, 4);
        }

        public uint ToUInt32(byte[] buffer, int start = 0)
        {
            return (uint)ToUInt(buffer, start, 4);
        }

        public long ToInt64(byte[] buffer, int start = 0)
        {
            return (long)ToUInt(buffer, start, 8);
        }

        public ulong ToUInt64(byte[] buffer, int start = 0)
        {
            return (uint)ToUInt(buffer, start, 8);
        }

        private ulong ToUInt(byte[] buffer, int start, int length)
        {
            ulong result = 0;
            for(int i=0;i<length;i++)
            {
                if (endianness == Endianness.BigEndian)
                {
                    result = (result << 8) | buffer[i];
                } else
                {
                    result = (result << 8) | buffer[length - i - 1];
                }
            }
            return result;
        }

        private void ReverseBytes(byte[] buffer, int length, int start= 0)
        {
            byte[] rbuff = new byte[length];
            for(int i=0;i<length;i++)
            {
                rbuff[i] = buffer[start + length - i - 1];
            }
            rbuff.CopyTo(buffer, start);
        }

        public float ToFloat(byte[] buffer, int start=0)
        {
            if(endianness == Endianness.BigEndian)
            {
                ReverseBytes(buffer, 4,start);
            }
            return BitConverter.ToSingle(buffer, start);
        }

        public double ToDouble(byte[] buffer, int start=0)
        {
            if(endianness == Endianness.BigEndian)
            {
                ReverseBytes(buffer, 8, start);
            }
            return BitConverter.ToDouble(buffer, start);
        }
    }

    public class STDFReader
    {
        FileStream fs = null;
        ushort currentRecordLength;
        public int CPU_TYPE { get; private set; }
        public int STDF_VER { get; private set; }

        private ByteConverter Converter;

        public bool EOF
        {
            get
            {
                return (fs == null || Position >= fs.Length || Position < 0);
            }
        }

        public STDFReader(string path)
        {
            Converter = new ByteConverter();
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            Rewind();

            // First record must be a FAR record type
            if (CurrentRecordType.TypeCode == 10)
            {
                if (currentRecordLength == 512)
                {
                    Converter.SetEndianness(Endianness.BigEndian);
                }
                CPU_TYPE = fs.ReadByte();
                STDF_VER = fs.ReadByte();
            } else
            {
                throw new FormatException("Invalid STDF format.");
            }

            ReadHeader();
        }

        public ISTDFRecord ReadRecord()
        {
            byte[] buffer = new byte[currentRecordLength];
            fs.Read(buffer, 0, currentRecordLength);
            STDFRecord record = new STDFRecord(CurrentRecordType, buffer);
            ReadHeader();
            return record;
        }

        public void Rewind()
        {
            fs.Seek(0, SeekOrigin.Begin);
            ReadHeader();
        }

        public void ReadHeader()
        {
            byte[] buffer = new byte[4];
            fs.Read(buffer, 0, 4);
            currentRecordLength = Converter.ToUInt16(buffer, 0);
            CurrentRecordType = (ushort)(buffer[2] << 8 | buffer[3]);
        }

        public bool SeekNextRecordType(RecordType type)
        {
            bool found = false;
 
            while(!found)
            {
                SeekNextRecord();
                if (CurrentRecordType.TypeCode == type.TypeCode)
                {
                    found = true;
                }
            }

            return found;
        }

        public long SeekNextRecord()
        {
            long currentPos = Position;
            
            if (currentPos >= fs.Length || currentPos < 0)
            {
                return -1;
            }

            try
            {
                fs.Seek(currentRecordLength+2, SeekOrigin.Current);
            } catch(EndOfStreamException e)
            {
                fs.Seek(0, SeekOrigin.End);
                return -1;
            }

            ReadHeader();

            return Position;
        }

        public long Position
        {
            get
            {
                return fs.Position;
            }
        }

        public RecordType CurrentRecordType { get; private set; }
    }


}
