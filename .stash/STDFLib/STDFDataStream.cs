using System;
using System.Linq;
using System.Text;
using System.IO;
using Syroot.BinaryData;

namespace STDFLib
{
    /// <summary>
    /// Class <c>STDFDataStream</c> extends the BinaryStream class to read and write STDF data with default options and additional STDF related data types.
    /// </summary>
    public partial class STDFDataStream : BinaryStream
    {
        private long _startOfCurrentRecordHeader = 0;
        private long _endOfCurrentRecord = 0;

        // Two bytes follow the file header - CPU_TYPE and STDF_VER bytes.
        public ushort REC_LEN { get; protected set; } = 2;

        // The record type for the file header record.  
        public byte REC_TYP { get; protected set; } = 0;

        // The record sub type for the file header record.
        public byte REC_SUB { get; protected set; } = 10;

        // Default CPU type is i386
        public STDFCpuTypes CPU_TYPE { get; protected set; } = STDFCpuTypes.i386;

        // STDF Version
        public STDFVersions STDF_VER { get; protected set; } = STDFVersions.STDFVer4;

        /// <summary>
        /// Initializes a new instance of the <see cref="STDFDataStream"/> class with the given default configuration.
        /// </summary>
        /// <param name="baseStream">The output stream.</param>
        /// <param name="converter">The <see cref="ByteConverter"/> to use. Defaults to
        /// <see cref="ByteConverter.System"/>.</param>
        /// <param name="encoding">The character encoding to use. Defaults to <see cref="Encoding.UTF8"/>.</param>
        /// <param name="booleanCoding">The <see cref="BinaryData.BooleanCoding"/> data format to use  for
        /// <see cref="Boolean"/> values.</param>
        /// <param name="dateTimeCoding">The <see cref="BinaryData.DateTimeCoding"/> data format to use for
        /// <see cref="DateTime"/> values.</param>
        /// <param name="stringCoding">The <see cref="BinaryData.StringCoding"/> data format to use for
        /// <see cref="String"/> values.</param>
        /// <param name="leaveOpen"><c>true</c> to leave the base stream open after the <see cref="BinaryStream"/>
        /// object is disposed; otherwise <c>false</c>.</param>
        /// <exception cref="ArgumentException">The stream does not support writing or is already closed.</exception>
        /// <exception cref="ArgumentNullException">output is null.</exception>
        /// <exception cref="InvalidDataException">File format is invalid or version is unsupported.</exception>
        /// <exception cref="IOException">output is null.</exception>
        public STDFDataStream(Stream stream, ByteConverter converter = null, Encoding encoding = null, BooleanCoding booleanCoding = BooleanCoding.Byte, 
                               DateTimeCoding dateTimeCoding = DateTimeCoding.CTime, StringCoding stringCoding = StringCoding.ByteCharCount, bool leaveOpen = false)
            : base(stream, converter, encoding, booleanCoding, dateTimeCoding, stringCoding, leaveOpen)
        {
            // Set the default encoding for an STDF file as ASCII
            Encoding = Encoding.ASCII;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="STDFDataStream"/> class, opening a file stream given by Path Name, and with default data format configuration settings for STDF files.
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <param name="converter"></param>
        /// <param name="encoding"></param>
        /// <param name="booleanCoding"></param>
        /// <param name="dateTimeCoding"></param>
        /// <param name="stringCoding"></param>
        /// <param name="leaveOpen"></param>
        /// <exception cref="ArgumentException">The stream does not support writing or is already closed.</exception>
        /// <exception cref="ArgumentNullException">output is null.</exception>
        /// <exception cref="NotSupportedException">output is null.</exception>
        /// <exception cref="FileNotFoundException">output is null.</exception>
        /// <exception cref="IOException">output is null.</exception>
        /// <exception cref="System.Security.SecurityException">output is null.</exception>
        /// <exception cref="DirectoryNotFoundException">output is null.</exception>
        /// <exception cref="UnauthorizedAccessException">output is null.</exception>
        /// <exception cref="PathTooLongException">output is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">output is null.</exception>
        public STDFDataStream(string pathName, FileMode mode, FileAccess access, ByteConverter converter = null, Encoding encoding = null, BooleanCoding booleanCoding = BooleanCoding.Byte,
                              DateTimeCoding dateTimeCoding = DateTimeCoding.CTime, StringCoding stringCoding = StringCoding.ByteCharCount, bool leaveOpen = false) : 
                              this(new FileStream(pathName, mode, access), converter, encoding, booleanCoding, dateTimeCoding, stringCoding, leaveOpen)
        {
        }

        #region ----- PUBLIC MEMBERS -----

        public virtual object[] ReadSTDFValues(string elementTypeName, int itemCount = 0, int dataLength = int.MinValue)
        {
            // Create an array of the specified elementType and size of itemCount
            object[] values = (object[])Array.CreateInstance(Type.GetType(elementTypeName), itemCount);

            // Fill the array
            for (int i = 0; i < itemCount; i++)
            {
                values[i] = ReadSTDFValue(elementTypeName, dataLength);
            }

            return values;
        }

        public virtual object ReadSTDFValue(string typeName, int dataLength = int.MinValue)
        {
            switch (typeName)
            {
                case "Byte": return Read1Byte();
                case "SByte": return (SByte)Read1Byte();
                case "Int16": return ReadInt16();
                case "Int32": return ReadInt32();
                case "UInt16": return ReadUInt16();
                case "UInt32": return ReadUInt32();
                case "Single": return ReadSingle();
                case "Double": return ReadDouble();
                case "DateTime": return ReadDateTime();
                case "Char[]":
                    if (dataLength == 0) return new char[] { };
                    if (dataLength > 0)
                    {
                        return ReadString(dataLength).ToCharArray();
                    }
                    else
                    {
                        // Assume 1st byte has length of array to read
                        return ReadString().ToCharArray();
                    }
                case "String":
                    if (dataLength == 0) return "";
                    if (dataLength > 0)
                    {
                        return ReadString(dataLength);
                    }
                    else
                    {
                        // Assume 1st byte has length of array to read
                        return ReadString();
                    }
                case "BitField2": return ReadBitField2();
                case "Nibbles": return ReadNibbles(1);
                case "Byte[]":
                    if (dataLength == 0) return new byte[] { };
                    if (dataLength > 0)
                    {
                        return ReadBytes(dataLength);
                    }
                    else
                    {
                        // Assume 1st byte has length of array to read
                        byte byteCount = Read1Byte();
                        if (byteCount > 0) return ReadBytes(byteCount);
                    }
                    break;
            }

            return null;
        }

        // Read in an array of nibbles from the stream.
        public Nibbles ReadNibbles(int nibbleCount = 1)
        {
            Nibbles nArray = new Nibbles();

            if (nibbleCount > 0)
            {
                // number of bytes to read are nibble count / 2 + 1.  Each byte contains two nibbles
                nArray.AddRange(ReadBytes(nibbleCount / 2 + 1));
            }

            return nArray;
        }

        public BitField ReadBitField()
        {
            BitField b = new BitField();
            int byteCount = 0;

            byteCount = Read1Byte();

            if (byteCount == 0) return null; // If zero, then return, nothing else to read.

            // Convert the number of bits to read to bytes, then read in that number of bytes into the bitfield bit array
            b.SetBits(ReadBytes(byteCount));

            return b;
        }

        public BitField2 ReadBitField2()
        {
            BitField2 b = new BitField2();
            int byteCount = 0;
            int bitCount = 0;

            bitCount = ReadUInt16();
            byteCount = (bitCount / 8) + ((bitCount % 8) > 0 ? 1 : 0);

            if (byteCount == 0) return null; // If zero, then return, nothing else to read.

            // Convert the number of bits to read to bytes, then read in that number of bytes into the bitfield bit array
            b.SetBits(ReadBytes(byteCount));

            return b;
        }

        // Write out an array of STDF values
        public virtual void WriteSTDFValues(object[] values, int itemCount = int.MinValue, int dataLength = int.MinValue)
        {
            string arrayType = values.GetType().Name;

            // If the number of items to write is greater than zero
            if (itemCount >= 0)
            {
                if (itemCount > values.Length)
                {
                    throw new ArgumentException(string.Format("Error in WriteSTDValues.  Request to write {0} items but only {1} items exist in the passed values array.", itemCount, values.Length));
                }

                // Write each value from the array
                for (int i = 0; i < itemCount; i++)
                {
                    WriteSTDFValue(values[i], dataLength);
                }
            }
        }

        public virtual void WriteSTDFValue(object value, int dataLength = int.MinValue)
        {
            // Array to use for padding/truncating strings/char[]/byte[] to a fixed length based on dataLength
            byte[] fixedLengthArray = null;

            if (dataLength > 0)
            {
                fixedLengthArray = new byte[dataLength];
            }

            // Call appropriate Write function on stream to write out the value type
            switch (value.GetType().Name)
            {
                case "Byte":
                    Write((Byte)value);
                    break;
                case "SByte":
                    Write((SByte)value);
                    break;
                case "Int16":
                    Write((Int16)value);
                    break;
                case "Int32":
                    Write((Int32)value);
                    break;
                case "UInt16":
                    Write((UInt16)value);
                    break;
                case "UInt32":
                    Write((UInt32)value);
                    break;
                case "Single":
                    Write((Single)value);
                    break;
                case "Double":
                    Write((Double)value);
                    break;
                case "DateTime":
                    Write((DateTime)value);
                    break;
                case "String":
                    // If datalength specified, then this is a fixed length string.  Truncate or pad with zeros to meet dataLength.
                    if (dataLength > 0)
                    {
                        // Handle for fixed length strings -> use Linq select to transform each char in the string to a byte in the source array
                        Array.Copy(((string)value).Select(x => (byte)x).ToArray(), fixedLengthArray, Math.Min(((char[])value).Length, dataLength));

                        Write(fixedLengthArray);
                    }
                    else
                    {
                        // if empty string, write zero for the length byte
                        if ((string)value == "")
                        {
                            Write((byte)0);
                        }
                        else
                        {
                            // Write the string, stream will encode 1st byte as length
                            Write((String)value);
                        }
                    }
                    break;
                case "char[]":
                    // If datalength specified, then this is a fixed length char array.  Truncate or pad with zeros to meet dataLength.
                    if (dataLength > 0)
                    {
                        // Handle for fixed length char arrays -> use Linq select to transform each char to a byte in the source array
                        Array.Copy(((char[])value).Select(x => (byte)x).ToArray(), fixedLengthArray, Math.Min(((char[])value).Length, dataLength));

                        Write(fixedLengthArray);
                    }
                    else
                    {
                        // If char array is empty, write zero for length byte
                        if (((char[])value).Length == 0)
                        {
                            Write((byte)0);
                        }
                        else
                        {
                            // Write the length byte of the char array
                            WriteByte((byte)((char[])value).Length);

                            // Write the character array
                            Write(((char[])value).Select(x => (byte)x).ToArray());
                        }
                    }
                    break;
                case "byte[]":
                    if (dataLength > 0)
                    {
                        // Handle for fixed length byte arrays
                        Array.Copy((byte[])value, fixedLengthArray, Math.Min(((char[])value).Length, dataLength));

                        Write(fixedLengthArray);
                    }
                    else
                    {
                        // If byte array is empty, write zero for length byte
                        if (((byte[])value).Length == 0)
                        {
                            Write((byte)0);
                        }
                        else
                        {
                            // Write the length byte of the byte array
                            WriteByte((byte)((char[])value).Length);

                            // Write the byte array
                            Write((byte[])value);
                        }
                    }
                    break;
                case "BitField":
                    Write((BitField)value);
                    break;
                case "BitField2":
                    Write((BitField2)value);
                    break;
                case "Nibbles":
                    Write((Nibbles)value);
                    break;
            }
        }

        public void Write(BitField bitField)
        {
            // Write out the bitfield size as the first byte or first two bytes depending on bit field encoding
            switch (bitField.Encoding)
            {
                case BitFieldEncoding.PrefixedWithByteCount:
                    // If null or empty, then write 0 as length and return
                    if (bitField == null || bitField.BitCount == 0)
                    {
                        WriteByte(0);
                        return;
                    }
                    else
                    {
                        WriteByte(bitField.ByteCount);
                    }
                    break;
                case BitFieldEncoding.PrefixedWithBitCount:
                    // If null or empty, then write 0 as length and return
                    if (bitField == null || bitField.BitCount == 0)
                    {
                        WriteUInt16(0);
                        return;
                    }
                    else
                    {
                        WriteUInt16(bitField.ByteCount);
                    }
                    break;
            }

            // Write the bitfield bytes to the stream
            WriteBytes(bitField.GetBytes());
        }

        public void Write(BitField2 bitField)
        {
            Write((BitField)bitField);
        }

        public void Write(Nibbles nibbles, int count = 1)
        {
            if (count < 0)
            {
                throw new ArgumentException(string.Format("Invalid count specified when writing nibbles array.  Nibble count of {0} given.  When specifying nibble count to write, must be >= 0.", count));
            }

            if (nibbles != null && nibbles.Count > 0 && count > 0)
            {
                byte[] nibs = nibbles.GetBytes();

                // Make sure there are enough bytes in the nibble array to satisfy the number of nibbles to be written
                if (nibs.Length < (count / 2 + 1))
                {
                    throw new ArgumentException(string.Format("Error when writing nibble array.  Requested to write {0} nibble bytes, but only {1} nibble bytes are in nibble structure.", count/2+1, nibs.Length));
                }

                // Count / 2 => two nibbles per byte
                for (int i=0;i<(count/2 + 1);i++)
                {
                    WriteByte(nibs[i]);
                }
            }
        }

        #endregion

        #region --- PRIVATE MEMBERS ---

        #endregion
    }
}
