using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using STDFLib.STDF.Records;

namespace STDFLib.STDF
{
    public enum StringBinaryFormat
    {
        LengthPrefixByte = 0x01,     // The length of the string is given by the first byte.  Max length is 255 bytes.  
        LengthPrefixWord = 0x02,     // The length of the string is given by the first two bytes (first word).  Read as an Int16.  Max length is 65535.
        LengthPrefixDWord = 0x03,    // The length of the string is given by the first four bytes (first double word).  Read as an Int32.  Max length is 4294967295 bytes.
        ZeroTerminated = 0x05,       // Strings are zero-terminated.  
        NoPrefix = 0x06,             // No prefix, indicates this is a fixed format string.  Must call the Read/Write string methods that specify a fixed length.
        Default = 0xFE,              // Specifies to use the default StringBinaryFormatting for the type of stream.           
        Inherit = 0xFF               // Uses the previously specified StringBinaryFormat (or default formatting if never specified for the stream).
    }

    public enum ExtStreamByteOrder : ushort
    {
        BigEndian = 0xFEFF,    // MSB on left
        LittleEndian = 0xFFFE  // MSB on right
    }

    public class ExtBinaryStream : IDisposable
    {

        public Stream BaseStream { get; private set; }


        public ExtBinaryStream(Stream baseStream)
        {
            BaseStream = baseStream;
        }

        public bool CanRead => BaseStream.CanRead;

        public bool CanSeek => BaseStream.CanSeek;

        public bool CanWrite => BaseStream.CanWrite;

        public long Length => BaseStream.Length;

        public long Position { get => BaseStream.Position; set => BaseStream.Position = value; }

        public void Flush()
        {
            BaseStream.Flush();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var result = BaseStream.Read(buffer, offset, count);

            if (reverse_byte_order)
            {
                buffer = buffer.Reverse().ToArray();
            }

            return result;
        }

        public long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (reverse_byte_order)
            {
                buffer = buffer.Reverse().ToArray();
            }

            BaseStream.Write(buffer, offset, count);
        }

        public virtual StringBinaryFormat GetDefaultStringFormat()
        {
            return StringBinaryFormat.ZeroTerminated; 
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ExtBinaryStream() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class ExtBinaryReader : BinaryReader
    {
        // private field to hold the current string format option for the stream
        private StringBinaryFormat current_string_format = StringBinaryFormat.Default;

        // private field to hold a flag to indicate that the read/write functions should reverse the byte order 
        // of the byte buffers to be read/written based on the endianness of the stream.
        private bool reverse_byte_order = false;

        #region Constructors
        public ExtBinaryReader(Stream dataStream) : base(dataStream) { }

        public ExtBinaryReader(Stream dataStream, Encoding dataEncoding) : base(dataStream, dataEncoding) { }

        public ExtBinaryReader(Stream dataStream, Encoding dataEncoding, bool leaveOpen) : base(dataStream, dataEncoding, leaveOpen) { }
        #endregion

        #region Helper methods

        // Set the byte order based on the byte order of the computer we are running on.  For x86 (Intel) architecture, this is Little Endian (MSB on the right).
        public ExtStreamByteOrder StreamByteOrder { get; set; } = BitConverter.IsLittleEndian ? ExtStreamByteOrder.LittleEndian : ExtStreamByteOrder.BigEndian;

        // Get the default string format for this reader
        public virtual StringBinaryFormat GetDefaultStringFormat()
        {
            return StringBinaryFormat.ZeroTerminated;  // Default for a basic ExtBinaryReader stream type.
        }

        // Return the current string formatting option for the stream
        public StringBinaryFormat StringFormat
        {
            get
            {
                // If current string format option is the default format, then call the default string format function to return it
                if (current_string_format == StringBinaryFormat.Default)
                {
                    return GetDefaultStringFormat();
                }

                // Otherwise return the current value of the string format option.
                return current_string_format;
            }

            // Set the current string format option for the stream.  Subsequent reads and writes from the stream will use this option.
            set
            {
                switch(value)
                {
                    // Specified to use the default for the stream type
                    case StringBinaryFormat.Default:
                        current_string_format = GetDefaultStringFormat();
                        break;
                    // Specified to keep using the current setting, so just ignore - don't change the current option setting
                    case StringBinaryFormat.Inherit:
                        break;
                    default:
                        // Set to option setting to the new value.
                        current_string_format = value;
                        break;
                }
            }
        }

        // Helper method - returns current stream position for the next byte to be read.
        public long Position
        {
            get
            {
                return BaseStream.Position;
            }

            set
            {
                BaseStream.Position = value;
            }
        }

        // Helper method - returns the current stream length in bytes
        public long Length
        {
            get
            {
                return BaseStream.Length;
            }
        }

        // Helper method - sets the current stream length.
        public void SetLength(long length)
        {
            BaseStream.SetLength(length);
        }

        // Helper method - seek to the specified position relative to the seek origin.
        public long Seek(long offset, SeekOrigin origin)
        {
            BaseStream.Seek(offset, origin);
            return Position;
        }

        // Helper method - flush the underlying stream buffer to end device.
        public void Flush()
        {
            BaseStream.Flush();
        }

        // Helper method - get or set the time allowed for an IO operation to complete
        public int IOTimeOut
        {
            get
            {
                return BaseStream.ReadTimeout;
            }

            set
            {
                BaseStream.ReadTimeout = value;
            }
        }

        #endregion

        #region Read array functions
        public T[] ReadArray<T>(int count, Func<T> ReadFunc)
        {
            List<T> result = new List<T>();
            int i = 0;

            try
            {
                for (i = 0; i < count; i++)
                {
                    result.Add(ReadFunc.Invoke());
                }
            }
            catch 
            {
                // If an error without reading anything into the source array, then just return an empty array of type T.
                // The caller should compare the size of the returned array vs. the requested count to determine if additional error 
                // checking is needed (check if the end of stream reached for example).
            }

            return result.ToArray();
        }

        public Boolean[] ReadBooleans(int count)
        {
            return ReadArray(count, ReadBoolean);
        }

        public decimal[] ReadDecimals(int count)
        {
            return ReadArray(count, ReadDecimal);
        }

        public double[] ReadDoubles(int count)
        {
            return ReadArray(count, ReadDouble);
        }

        public UInt16[] ReadInt16s(int count)
        {
            return ReadArray(count, ReadUInt16);
        }

        public Int32[] ReadInt32s(int count)
        {
            return ReadArray(count, ReadInt32);
        }

        public SByte[] ReadSBytes(int count)
        {
            return ReadArray(count, ReadSByte);
        }

        public Single[] ReadSingles(int count)
        {
            return ReadArray(count, ReadSingle);
        }

        public UInt16[] ReadUInt16s(int count)
        {
            return ReadArray(count, ReadUInt16);
        }

        public UInt32[] ReadUInt32s(int count)
        {
            return ReadArray(count, ReadUInt32);
        }

        public UInt64[] ReadUInt64s(int count)
        {
            return ReadArray(count, ReadUInt64);
        }
        #endregion

        #region String Handling Overloads

        // Read a zero terminated string from the stream
        public string ReadZeroTerminatedString()
        {
            StringBuilder result = new StringBuilder();

            int nextChar;

            // Read until we find a zero byte to indicate string termination.  NOTE: if the end of the stream is reached before finding the terminator,
            // this will throw and EndOfStream exception on the ReadChar() that the caller should handle.
            while(true)
            {
                nextChar = ReadChar();
                    
                if (nextChar != 0)
                {
                    result.Append(nextChar);
                }
                else
                {
                    return result.ToString();  // End of string found, return the string
                }
            }
        }

        // Reads the next 'stringLength' chars in and returns them as a string.  NOTE: if the end of the stream is reached before the requested number of characters are read,
        // then this will throw an EndOfStream exception on the ReadChars() that the caller should handle.
        public string ReadString(int stringLength)
        {
            return new string(ReadChars(stringLength));
        }
               
        // Read in 'count' number of fixed length strings, length given by 'stringLength'
        public string[] ReadStrings(int count, int stringLength)
        {
            string[] result = new string[count];
            int i = 0;

            for(i = 0; i < count; i++)
            {
                result[i] = ReadString(stringLength);
            }

            return result;
        }

        // Reads in 'count' number of strings.  The string termination/length of each string to read is given by the StringFormat option.
        public string[] ReadStrings(int count)
        {
            return ReadArray(count, ReadString);
        }


#endregion
        
        #region Override base functions

        public override int Read()
        {
            return base.Read();
        }

        public override int Read(char[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

        public override int Read(byte[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

        public override bool ReadBoolean()
        {
            return base.ReadBoolean();
        }

        public override byte ReadByte()
        {
            return base.ReadByte();
        }

        public override byte[] ReadBytes(int count)
        {
            return base.ReadBytes(count);
        }

        public override char ReadChar()
        {
            byte[] buffer = ReadBytes(sizeof(char));
            
            if (reverse_byte_order)
            {
                buffer = buffer.Reverse().ToArray();
            }

            return BitConverter.ToChar(buffer,0);
        }

        public override char[] ReadChars(int count)
        {
            return base.ReadChars(count);
        }

        public override decimal ReadDecimal()
        {
            return base.ReadDecimal();
        }

        public override double ReadDouble()
        {
            return base.ReadDouble();
        }

        public override short ReadInt16()
        {
            return base.ReadInt16();
        }

        public override int ReadInt32()
        {
            return base.ReadInt32();
        }

        public override long ReadInt64()
        {
            return base.ReadInt64();
        }

        public override sbyte ReadSByte()
        {
            return base.ReadSByte();
        }

        public override float ReadSingle()
        {
            return base.ReadSingle();
        }

        // Reads in a string.  Termination of the string is determined from the current value of the StringFormat option.
        public override string ReadString()
        {
            int stringLength=0;

            // Validate the prefix size, can only be 1 byte (8 bit int), 2 bytes (16 bit int) or 4 bytes (32 bit int) in length;
            switch(current_string_format)
            {
                // If zero terminated string, then return the result of the ReadZeroTerminateString method
                case StringBinaryFormat.ZeroTerminated:
                    return ReadZeroTerminatedString();

                // Get the length from the first byte
                case StringBinaryFormat.LengthPrefixByte:
                    stringLength = ReadByte();
                    break;

                // Get the length from the first two bytes
                case StringBinaryFormat.LengthPrefixWord:
                    stringLength = ReadInt16(); 
                    break;

                // Get the length from the first four bytes
                case StringBinaryFormat.LengthPrefixDWord:
                    stringLength = ReadInt32(); 
                    break;

                // Should never happen, but just in case...
                default:
                    throw new InvalidDataException("Unknown string formatting option");
            }

            if (stringLength <= 0)
            {
                // Should never happen, but just in case.
                throw new InvalidDataException("Unknown string formatting option");
            }

            return new string(ReadChars((int)stringLength));
        }

        public override ushort ReadUInt16()
        {
            return base.ReadUInt16();
        }

        public override uint ReadUInt32()
        {
            return base.ReadUInt32();
        }

        public override ulong ReadUInt64()
        {
            return base.ReadUInt64();
        }

        #endregion
    }

    public enum STDFDataStreamSeekOrigin
    {
        BeginStream = 0x01,
        Current = 0x02,
        EndStream = 0x03,
        BeginCurrentRecord = 0x04,
        EndCurrentRecord = 0x05
    }

    // STDF Data Stream class.  Contains helper methods to provide a STDFDataReader, STDFDataWriter and utility methods/constants used in working with STDF streams.
    public class STDFDataStream
    {
        private Stream _dataStream;
        private STDFBinaryReader _reader;
        //private STDFDataWriter _writer = null;
        private long _startOfCurrentRecordHeader = 0;
        private long _endOfCurrentRecord = 0;

        public STDFDataStream(Stream dataStream)
        {
            _reader = new STDFBinaryReader(dataStream);
            // _writer = new STDFDataWriter(dataStream);
        }

        private static readonly Type[] stdf_data_types = new Type[]
        {
            typeof(Byte),       // Type code = 0 - single byte pad field
            typeof(Byte),       // Type code = 1
            typeof(UInt16),     // Type code = 2
            typeof(UInt32),     // Type code = 3
            typeof(SByte),      // Type code = 4
            typeof(Int16),      // Type code = 5
            typeof(Int32),      // Type code = 6 
            typeof(Single),     // Type code = 7 
            typeof(Double),     // Type code = 8 
            null,               // Type code = 9 - NOT USED
            typeof(Char[]),     // Type code = 10
            typeof(Byte[]),     // Type code = 11
            typeof(BitField2),  // Type code = 12
            typeof(Nibbles)     // Type code = 13
        };

        private static readonly Dictionary<ushort, Type> stdf_record_types = new Dictionary<ushort, Type>()
        {
            { 0x000A, typeof(FAR) },   // Rec_Typ =   0, Rec_Sub = 10
            { 0x0014, typeof(ATR) },   // Rec_Typ =   0, Rec_Sub = 20
            { 0x010A, typeof(MIR) },   // Rec_Typ =   1, Rec_Sub = 10
            { 0x0114, typeof(MRR) },   // Rec_Typ =   1, Rec_Sub = 20
            { 0x011E, typeof(PCR) },   // Rec_Typ =   1, Rec_Sub = 30
            { 0x0128, typeof(HBR) },   // Rec_Typ =   1, Rec_Sub = 40
            { 0x0132, typeof(SBR) },   // Rec_Typ =   1, Rec_Sub = 50
            { 0x013C, typeof(PMR) },   // Rec_Typ =   1, Rec_Sub = 60
            { 0x013E, typeof(PGR) },   // Rec_Typ =   1, Rec_Sub = 62
            { 0x013F, typeof(PLR) },   // Rec_Typ =   1, Rec_Sub = 63
            { 0x0146, typeof(RDR) },   // Rec_Typ =   1, Rec_Sub = 70
            { 0x0150, typeof(SDR) },   // Rec_Typ =   1, Rec_Sub = 80
            { 0x020A, typeof(WIR) },   // Rec_Typ =   2, Rec_Sub = 10
            { 0x0214, typeof(WRR) },   // Rec_Typ =   2, Rec_Sub = 20
            { 0x021E, typeof(WCR) },   // Rec_Typ =   2, Rec_Sub = 30
            { 0x050A, typeof(PIR) },   // Rec_Typ =   5, Rec_Sub = 10
            { 0x0514, typeof(PRR) },   // Rec_Typ =   5, Rec_Sub = 20
            { 0x0A1E, typeof(TSR) },   // Rec_Typ =  10, Rec_Sub = 30
            { 0x0F0A, typeof(PTR) },   // Rec_Typ =  15, Rec_Sub = 10
            { 0x0F0F, typeof(MPR) },   // Rec_Typ =  15, Rec_Sub = 15
            { 0x0F14, typeof(FTR) },   // Rec_Typ =  15, Rec_Sub = 20
            { 0x140A, typeof(BPS) },   // Rec_Typ =  20, Rec_Sub = 10
            { 0x1414, typeof(EPS) },   // Rec_Typ =  20, Rec_Sub = 20
            { 0x320A, typeof(GDR) },   // Rec_Typ =  50, Rec_Sub = 10
            { 0x321E, typeof(DTR) },   // Rec_Typ =  50, Rec_Sub = 30
//            { 0xB400, typeof(EX1) },   // Rec_Typ = 180, Rec_Sub = 00
//            { 0xB500, typeof(EX2) },   // Rec_Typ = 181, Rec_Sub = 00
    };

        // Return the object data type (class) that corresponds to the given type code
        public static Type GetSTDFDataType(byte dataTypeCode)
        {
            // Unrecognized type code received
            if (dataTypeCode < 0 || dataTypeCode > 13)
            {
                throw new InvalidOperationException("STDF data type code out of range.");
            }

            return stdf_data_types[dataTypeCode];
        }

        // Return the STDF record object type (class) that corresponds to the given major and minor record type codes.
        public static Type GetSTDFRecordType(byte recordMajorType, byte recordMinorType)
        {
            ushort recType =  (ushort)(recordMajorType << 8 | recordMinorType);

            // Unrecognized type code received
            if ( !stdf_record_types.Keys.Contains(recType) )
            {
                throw new InvalidOperationException(string.Format("Invalid STDF record type.  Was passed major type code {0}, minor type code {1}.", recordMajorType, recordMinorType));
            }

            return stdf_record_types[recType];
        }

        // returns the number of bytes required to hold the number of nibble (4 bit values) passed.
        public static int NibblesToBytes(int nibbleCount)
        {
            return (nibbleCount / 2) + (nibbleCount % 2);
        }

        // returns the number of bytes required to hold the number of bits passed.
        public static int BitsToBytes(int bitCount)
        {
            return (bitCount / 8) + ((bitCount % 8) > 0 ? 1 : 0);
        }

        // Returns the current cursor position of the underlying stream
        public long Position
        {
            get
            {
                return _dataStream.Position;
            }
        }

        // Move the cursor position of the underlying stream to the specified offset from the origin type specified.  This is an overload of the base Seek method 
        // to provide an addition origin that points to the start of the current record being read.
        public long Seek(long offset, STDFDataStreamSeekOrigin origin)
        {
            // Only valid for data streams that can seek
            if (_dataStream.CanSeek)
            {
                switch(origin)
                {
                    // Add the offset to the saved position of the start of the current record.  The position of the start of the current record is saved each time a new 
                    // record header is read.
                    case STDFDataStreamSeekOrigin.BeginCurrentRecord:
                        offset = _startOfCurrentRecordHeader + offset;
                        break;

                    // Add the offset to the saved position of the end of the current record.  The position of the end of the current record is calculated each time a new 
                    // record header is read.
                    case STDFDataStreamSeekOrigin.EndCurrentRecord:
                        offset = _endOfCurrentRecord + offset;
                        break;
                }

                /// TODO: Add some intelligence to determine closest origin point to seek from
                ///       For disk/memory based access this may not be too big of a deal but for other device types it might

                // Seek to the total offset position using the start of the stream as the origin
                return _dataStream.Seek(offset, SeekOrigin.Begin);
            }

            return offset;
        }

        // Reads the next record in the stream.  This method assumes that the stream cursor is positioned at the first byte of a header record.  It does not search for the next record.
        public ISTDFRecord ReadNextRecord() 
        {
            ISTDFRecord nextRecord = null;

            // Read the header fields for the next record
            ushort rec_len = _reader.ReadUInt16();
            byte rec_type = _reader.ReadByte();
            byte rec_subtype = _reader.ReadByte();

            // Create a new object of the correct type
            try
            {
                nextRecord = (ISTDFRecord)Activator.CreateInstance(GetSTDFRecordType(rec_type, rec_subtype));
            } catch
            {
                // return null if record creation was unsuccessful
                return null;
            }

            // If we have successfully read a new record, set the header data read from the stream.
            if (nextRecord != null)
            {
                nextRecord.REC_LEN = rec_len;
                nextRecord.REC_TYP = rec_type;
                nextRecord.REC_SUB = rec_subtype;
            }

            // If this record type is decorated with an STDF attribute, then set the reader options according to the data member options.  Currently this only
            // sets the StringFormatting option
            var recordAttributes = nextRecord.GetType().GetCustomAttributes(typeof(STDFAttribute), true);

            if (recordAttributes.Length > 0)
            {
                // Set the string format on the current reader to the first STDF attribute found.  Note, if multiple STDF attributes are applied to the record,
                // all attributes after the first one will be ignored.
                _reader.StringFormat = ((STDFAttribute)recordAttributes[0]).StringFormat;
            }
            
            // Get the properties that are decorated with the STDF attribute
            var props = nextRecord.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(STDFAttribute), false).Count() > 0);
            
            return nextRecord;
        }

        // Create a new STDF record object and return it
        public ISTDFRecord CreateRecord(Type t)
        {
            if (t != null)
            {
                return (ISTDFRecord)Activator.CreateInstance(t);
            }

            return null;
        }

        // Create a new STDF record object based on the packed type code (major type + minor type)
        public ISTDFRecord CreateRecord(ushort recordType)
        {
            return CreateRecord(STDFRecord.GetClassType(recordType));
        }
    }
    
    // Extend the ExtBinaryReader class to add methods to read nibbles, bitfields, and variable data records from an STDF file.
    public class STDFBinaryReader : ExtBinaryReader
    {
        private long _currentPosition;

        // Indicates how strings are to be interpreted in the binary stream when reading.  Default is strings are zero terminated.
        public StringBinaryFormat StringFormat { get; set; } = StringBinaryFormat.ZeroTerminated;
        
        // NOTE: for binary STDF files, encoding is always ASCII encoding
        public STDFBinaryReader(Stream dataStream) : base(dataStream, Encoding.ASCII)
        {
            // Save the current position upon creation
            _currentPosition = Position;
        }

        // NOTE: for binary STDF files, encoding is always ASCII encoding
        public STDFBinaryReader(Stream dataStream, bool leaveOpen) : base(dataStream, Encoding.ASCII, leaveOpen)
        {
            // Save the current position upon creation
            _currentPosition = Position;
        }

        // Reads a series of bytes into a BitField structure, number of bits in the bitfield is given in the first byte
        public BitField ReadBitField()
        {
            BitField b = new BitField();

            b.Length = ReadByte();  // First byte is the length if the bit array in bytes;

            if (b.Length == 0) return null;  // If the bit array length is zero, then return.

            b.BitArray = ReadBytes(b.Length);

            return b;
        }

        // Reads a series of bytes into a BitField structure, number of bits in the bitfield is given by the first two bytes
        public BitField2 ReadBitField2()
        {
            BitField2 b = new BitField2();

            b.Length = ReadUInt16();  // First 2 bytes is the bitfield length in bits.  NOTE: this must be converted to bytes 

            if (b.Length == 0) return null; // If zero, then return, nothing else to read.

            var slen = STDFDataStream.BitsToBytes(b.Length);

            b.BitArray = ReadBytes(slen);

            return b;
        }

        // Read a single nibble from the data stream
        public Nibbles ReadNibble()
        {
            Nibbles nibbleArray = new Nibbles(1);

            // Read in a single nibble.  High (or order depending on endianness) 4 bits are zero.
            nibbleArray.SetNibbleBytes(ReadBytes(1));

            return nibbleArray;
        }

        // Read a multiple nibbles from the data stream
        public Nibbles ReadNibbles(int count)
        {
            Nibbles nibbleArray = new Nibbles(count);

            // Read in 'count' number of nibbles.  High (or order depending on endianness) 4 bits or last byte read are zero for odd number of nibbles.
            nibbleArray.SetNibbleBytes(ReadBytes((count / 2) + 1));

            return nibbleArray;
        }

        // Reads the next byte as an STDF data type code and returns the corresponding STDFDataReader type.  The stream position will be the next byte following the type code.
        public Type ReadVariableDataFieldType()
        {
            int dataType = ReadByte();  // Read the next byte from the stream

            if (dataType < 0 || dataType > 13 || dataType == 0x09)
            {
                throw new STDFFormatException(string.Format("Invalid data type read from STDF data stream.  Data type read = {0} at stream position {1}", dataType,Position));
            }

            return STDFDataStream.stdf_data_types[dataType];
        }

        // Reads Variable data from the STDF stream.  First two bytes represent the number fields to read.  Each field contains:
        // 
        // For field aligned to start on an even byte
        //      Data Type: Byte 0 
        //      For all data types except integer/real data types:
        //      Field Data: Bytes 1 through n 
        //      For integer/real data types:
        //      Byte 1: Pad Byte (0x00)
        //      Byte 2-n: Field data
        //
        // For fields aligned to start on an odd byte
        //      Data Type: Byte 0 
        //      Byte 1-n: Field data (first byte will start on an even byte in the stream, so no need to add padding to align)
        //

        public object[] ReadVariableDataRecord()
        {
            List<object> variableDataRecord = new List<object>();

            // Get the number of data fields to read in (first two bytes)
            int numFields = ReadInt16();

            // Get the start position of the data record.
            long startPosition = Position; 

            string dataType="";

            for (int i = 0; i < numFields; i++)
            {
                // Read in the data type for the first field
                dataType = ReadVariableDataFieldType()?.Name;    

                // if dataType is an integer or real data type, then check to make sure the next byte is zero (a padding byte)
                switch(dataType)
                {
                    case "Int16":
                    case "Int32":
                    case "UInt16":
                    case "UInt32":
                    case "Single":
                    case "Double":
                        if (ReadByte() != 0)
                        {
                            throw new STDFFormatException(string.Format("Invalid Variable Data Record format, misaligned field found.  Expected a padding byte for data type {0}, field starting at position {1}", dataType, startPosition));
                        }
                        break;
                        
                }

                // Now read in the field according to the data type and add it to the data record 
                switch(dataType)
                {
                    case "Int16":
                        variableDataRecord.Add(ReadInt16());
                        break;
                    case "Int32":
                        variableDataRecord.Add(ReadInt16());
                        break;
                    case "UInt16":
                        variableDataRecord.Add(ReadInt16());
                        break;
                    case "UInt32":
                        variableDataRecord.Add(ReadInt16());
                        break;
                    case "Single":
                        variableDataRecord.Add(ReadInt16());
                        break;
                    case "Double":
                        variableDataRecord.Add(ReadInt16());
                        break;
                    case "Char[]":
                        variableDataRecord.Add(ReadString());
                        break;
                    case "Byte[]":
                        variableDataRecord.Add(ReadString());
                        break;
                    case "BitField2":
                        variableDataRecord.Add(ReadBitField2());
                        break;
                    case "Nibbles":
                        variableDataRecord.Add(ReadNibble());
                        break;
                }
            }

            // Convert to a simple array of values and return
            return variableDataRecord.ToArray();
        }
        public ISTDFRecord ReadRe




        // Read in an Audit Trail Record
        public ATR ReadATR()
        {
            ATR record = new ATR();

            record.MOD_TIM = ReadUInt32();
            record.CMD_LINE = ReadStringWithByteLengthPrefix();

            return record;
        }

        // Read a Begin Program Segment Record        
        public BPS ReadBPS()
        {
            BPS record = new BPS();

            record.SEQ_NAME = ReadStringWithByteLengthPrefix();

            return record;
        }

        // Read a Datalog Text Record
        public DTR ReadDTR()
        {
            DTR record = new DTR();

            record.TEXT_DAT = ReadStringWithByteLengthPrefix();

            return record;
        }

        // Read an End Program Segment Record
        public EPS ReadEPS()
        {
            EPS record = new EPS();

            return record;
        }
        
        // Read a File Attributes Record
        public FAR ReadFAR()
        {
            FAR record = new FAR();

            record.CPU_TYPE = ReadByte();
            record.STDF_VER = ReadByte();

            return record;
        }

        // Read a Functional Test Record
        public FTR ReadFTR()
        {
            FTR record = new FTR();

            record.TEST_NUM = ReadUInt32();
            record.HEAD_NUM = ReadByte();
            record.SITE_NUM = ReadByte();
            record.TEST_FLG = ReadByte();
            record.OPT_FLG = ReadByte();
            record.CYCL_CNT = ReadUInt32();
            record.REL_VADR = ReadUInt32();
            record.REPT_CNT = ReadUInt32();
            record.NUM_FAIL = ReadUInt32();
            record.XFAIL_AD = ReadInt32();
            record.YFAIL_AD = ReadInt32();
            record.VECT_OFF = ReadInt16();
            record.RTN_ICNT = ReadUInt16();
            record.PGM_ICNT = ReadUInt16();
            record.RTN_INDX = ReadUInt16s(record.RTN_ICNT);
            record.RTN_STAT = ReadNibbles(record.RTN_ICNT);
            record.PGM_INDX = ReadUInt16s(record.PGM_ICNT);
            record.PGM_STAT = ReadNibbles(record.PGM_ICNT);
            record.FAIL_PIN = ReadBitField2();
            record.VECT_NAM = ReadStringWithByteLengthPrefix();
            record.TIME_SET = ReadStringWithByteLengthPrefix();
            record.OP_CODE = ReadStringWithByteLengthPrefix();
            record.TEST_TXT = ReadStringWithByteLengthPrefix();
            record.ALARM_ID = ReadStringWithByteLengthPrefix();
            record.PROG_TXT = ReadStringWithByteLengthPrefix();
            record.RSLT_TXT = ReadStringWithByteLengthPrefix();
            record.PATG_NUM = ReadByte();
            record.SPIN_MAP = ReadBitField2();

            return record;
        }

        public GDR ReadGDR()
        {
            GDR record = new GDR();

            record.FLD_CNT = ReadUInt16();

            // Read in the list of variable data records
            record.GEN_DATA = new List<object[]>(ReadArray(record.FLD_CNT, ReadVariableDataRecord));

            return record;
                        
        }

        public HBR ReadHBR()
        {
            HBR record = new HBR();

            record.HEAD_NUM = ReadByte();
            record.SITE_NUM = ReadByte();
            record.HBIN_NUM = ReadUInt16();
            record.HBIN_CNT = ReadUInt16();
            record.HBIN_PF = ReadChar();
            record.HBIN_NAM = ReadStringWithByteLengthPrefix();

            return record;
        }

        public MIR ReadMIR()
        {
            MIR record = new MIR();

            record.SETUP_T = ReadUInt32();
            record.START_T = ReadUInt32();
            record.STAT_NUM = ReadByte();
            record.MODE_COD = ReadChar();
            record.RTST_COD = ReadChar();
            record.BURN_TIM = ReadUInt16();
            record.CMOD_COD = ReadChar();
            record.LOT_ID = ReadStringWithByteLengthPrefix();
            record.PART_TYP = ReadStringWithByteLengthPrefix();
            record.NODE_NAM = ReadStringWithByteLengthPrefix();
            record.TSTR_TYP = ReadStringWithByteLengthPrefix();
            record.JOB_NAM  = ReadStringWithByteLengthPrefix();
            record.JOB_REV  = ReadStringWithByteLengthPrefix();
            record.SBLOT_ID = ReadStringWithByteLengthPrefix();
            record.OPER_NAM = ReadStringWithByteLengthPrefix();
            record.EXEC_TYP = ReadStringWithByteLengthPrefix();
            record.EXEC_VER = ReadStringWithByteLengthPrefix();
            record.TEST_COD = ReadStringWithByteLengthPrefix();
            record.TST_TEMP = ReadStringWithByteLengthPrefix();
            record.USER_TXT = ReadStringWithByteLengthPrefix();
            record.AUX_FILE = ReadStringWithByteLengthPrefix();
            record.PKG_TYP  = ReadStringWithByteLengthPrefix();
            record.FAMLY_ID = ReadStringWithByteLengthPrefix();
            record.DATE_COD = ReadStringWithByteLengthPrefix();
            record.FACIL_ID = ReadStringWithByteLengthPrefix();
            record.FLOOR_ID = ReadStringWithByteLengthPrefix();
            record.PROC_ID  = ReadStringWithByteLengthPrefix();
            record.OPER_FRQ = ReadStringWithByteLengthPrefix();
            record.SPEC_NAM = ReadStringWithByteLengthPrefix();
            record.SPEC_VER = ReadStringWithByteLengthPrefix();
            record.FLOW_ID  = ReadStringWithByteLengthPrefix();
            record.SETUP_ID = ReadStringWithByteLengthPrefix();
            record.DSGN_REV = ReadStringWithByteLengthPrefix();
            record.ENG_ID   = ReadStringWithByteLengthPrefix();
            record.ROM_COD  = ReadStringWithByteLengthPrefix();
            record.SERL_NUM = ReadStringWithByteLengthPrefix();
            record.SUPR_NAM = ReadStringWithByteLengthPrefix();

            return record;
        }

        public MPR ReadMPR()
        {
        }

        public MRR ReadMRR()
        {

        }

        public PCR ReadPCR()
        {

        }

        public PGR ReadPGR()
        {

        }

        public PIR ReadPIR()
        {

        }

        public PLR ReadPLR()
        {

        }

        public PMR ReadPMR()
        {

        }

        public PRR ReadPRR()
        {

        }

        public PTR ReadPTR()
        {

        }

        public RDR ReadRDR()
        {

        }

        public SBR ReadSBR()
        {

        }

        public SDR ReadSDR()
        {

        }

        public TSR ReadTSR()
        {

        }

        public WCR ReadWCR()
        {

        }

        public WIR ReadWIR()
        {

        }

        public WRR ReadWRR()
        {

        }
    }

    public class STDFFormatException : Exception
    {
        public STDFFormatException() : base()
        {
        }

        public STDFFormatException(string message) : base(message)
        {
        }

        public STDFFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected STDFFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
