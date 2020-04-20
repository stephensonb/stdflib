using System;
using System.Text;

namespace STDFLib
{
    // The following class provides methods to convert various data types from/to their binary representations as a series of bytes.
    // They convert the given types using the BitConverter class to generate the sequence of bytes.  The byte conversions 
    // are Endianness aware.  If you are reading or writing to a stream that has a different byte order than than the 
    // system that this software is compiled for, then you can match the byte order by setting calling SwapBytes 
    // with true or false to set whether the bytes are swapped to correct for endianness mismatch.  
    //
    // For example:
    // Library running on LittleEndian arch, reading data from BigEndian arch, call SwapBytes(true);
    // Library running on BigEndingian arch, reading data from LittleEndian arch, call SwapByte(true);
    // Libray running on either arch, reading data from a matching endian arch, call SwapBytes(false);
    //
    // Default is SwapBytes(false).  
 
    /// <summary>
    /// Provides methods for converting types to byte representations for serialization or from byte represenations back to their
    /// base types.
    /// </summary>
    public class STDFFormatterConverter : IFormatterConverter
    {
        /// <summary>
        /// Returns an STDFFormatterConverter with default character encoding (ASCII).
        /// </summary>
        public STDFFormatterConverter() : this(Encoding.ASCII) { }

        /// <summary>
        /// Returns an STDFFormatterConverter configured to handle characters using the specified Encoding.
        /// </summary>
        /// <param name="encoding">Encoding to use when converting character data.</param>
        public STDFFormatterConverter(Encoding encoding)
        {
            SwapBytes = false;
            Encoding = encoding;
        }
        /// <summary>
        /// Indicates whether to swap (reverse) the bytes when converting to/from the base type representations.  Set to true if the
        /// endianness of the source that wrote the data is different than the endianness of the target that is reading the data.
        /// </summary>
        public bool SwapBytes { get; set; }
        private Endianness _endianness;
        /// <summary>
        /// Returns the character encoding that this Converter is configured for.  Read only.
        /// </summary>
        public Encoding Encoding { get; }
        /// <summary>
        /// Returns the endianness that the Converter is configured for.  If the current machine architecture is LittleEndian and SwapBytes 
        /// is set to true then this will return BigEndian.
        /// </summary>
        public Endianness Endianness
        {
            get => (SwapBytes && BitConverter.IsLittleEndian ? Endianness.BigEndian : Endianness.LittleEndian);

            private set
            {
                _endianness = value;
                // If the converter endianness is now different than the underlying architecture endiannes, 
                // set the SwapBytes flag to indicate any bytes read from a stream that are to be converted into 
                // a numeric value need to be swapped.
                if (_endianness != (BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian))
                {
                    SwapBytes = true;
                }
                else
                {
                    SwapBytes = false;
                }
            }
        }

        /// <summary>
        /// Converts the value to the given Type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="toType">Type to conver the value to.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public object Convert(object value, Type toType)
        {
            if (toType.Name == "Nullable`1")
            {
                toType = toType.GetGenericArguments()[0];
            }

            return toType.Name switch
            {
                "Boolean" => ToBoolean(value),
                "Byte" => ToByte(value),
                "Char" => ToChar(value),
                "Double" => ToDouble(value),
                "Single" => ToSingle(value),
                "Int16" => ToInt16(value),
                "Int32" => ToInt32(value),
                "UInt16" => ToUInt16(value),
                "UInt32" => ToUInt32(value),
                "DateTime" => ToDateTime(value),
                "String" => ToString(value),
                "SByte" => ToSByte(value),
                "ByteArray" => ToByteArray(value),
                "BitArray" => ToBitArray(value),
                "NibbleArray" => ToNibbleArray(value),
                "Nibble" => ToNibble(value),
                _ => System.Convert.ChangeType(value, toType)
            };
        }
        /// <summary>
        /// Converts a value to a byte.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public byte ToByte(object value)
        {
            if (value is byte[] buffer)
            {
                return buffer[0];
            }
            return System.Convert.ToByte(value);
        }
        /// <summary>
        /// Converts a value to a signed byte.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public sbyte ToSByte(object value)
        {
            if (value is byte[] buffer)
            {
                return (sbyte)buffer[0];
            }
            return System.Convert.ToSByte(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to a char according to the current Encoding.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public char ToChar(object value)
        {
            if (value is byte[] buffer)
            {
                return Encoding.GetChars(buffer)[0];
            }
            return System.Convert.ToChar(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to a Single (floating point) data type.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public float ToSingle(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(float));
                }
                return BitConverter.ToSingle(buffer);
            }
            return System.Convert.ToSingle(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to a double floating point data type.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public double ToDouble(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(double));
                }
                return BitConverter.ToDouble(buffer);
            }

            // do default conversion
            return System.Convert.ToDouble(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to a DateTime data type.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public DateTime ToDateTime(object value)
        {
            if (value is byte[] buffer)
            {
                return DateTime.UnixEpoch.AddSeconds(ToUInt32(buffer));
            }
            return System.Convert.ToDateTime(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to a string data type.  For STDF files, the characters are encoded
        /// as ASCII (7 bit encoding).
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public string ToString(object value)
        {
            if (value != null)
            {
                if (value is byte[] buffer)
                {
                    // special handling for string length 1 with zero as the only char in the string.
                    // This represents NULL value internally, so convert to null.  Nulls will be converted back
                    // to same layout (length byte = 1, string byte = 0) when writing out to a stream.
                    if (buffer.Length == 1 && buffer[0] == 0)
                    {
                        return null;
                    }
                    return Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                }

                return System.Convert.ToString(value);
            }
            return null;
        }
        /// <summary>
        /// Converts a value or an array of bytes to a signed 16 bit integer.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public short ToInt16(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(short));
                }
                return BitConverter.ToInt16(buffer);
            }

            return System.Convert.ToInt16(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to an unsigned 16 bit integer.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public ushort ToUInt16(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(ushort));
                }
                return BitConverter.ToUInt16(buffer);
            }

            return System.Convert.ToUInt16(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to a signed 32 bit integer
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public int ToInt32(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(int));
                }
                return BitConverter.ToInt32(buffer);
            }
            return System.Convert.ToInt32(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to an unsigned 32 bit integer.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public uint ToUInt32(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(uint));
                }
                return BitConverter.ToUInt32(buffer);
            }

            return System.Convert.ToUInt32(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to a boolean value.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public bool ToBoolean(object value)
        {
            if (value is byte[] buffer)
            {
                return buffer[0] == 0 ? false : true;
            }

            return System.Convert.ToBoolean(value);
        }
        /// <summary>
        /// Converts a value or an array of bytes to a Nibble.  Only the 4 low order bits are used, anything in the higher 
        /// order bits are ignored.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public Nibble ToNibble(object value)
        {
            return value as Nibble;
        }
        /// <summary>
        /// Converts an array of bytes to a nibble array.  The nibbles are packed where odd numbered nibbles (nibble indexing begins with 1) are the lower 4 bits
        /// of each byte and the even numbered nibbles are the upper 4 bits of each byte.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public NibbleArray ToNibbleArray(object value)
        {
            if (value == null || value is byte[] || value is NibbleArray)
            {
                return (NibbleArray)value;
            }
            throw new InvalidCastException(string.Format("Cannot cast type {0} to NibbleArray.", value.GetType().Name));
        }
        /// <summary>
        /// Converts a value or an array of bytes to a ByteArray data type.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public ByteArray ToByteArray(object value)
        {
            if (value == null || value is byte[] || value is ByteArray)
            {
                return (ByteArray)value;
            }
            throw new InvalidCastException(string.Format("Cannot cast type {0} to ByteArray.", value.GetType().Name));
        }
        /// <summary>
        /// Converts a value or an array of bytes to a BitArray data type.
        /// </summary>
        /// <param name="value">Object or byte array to convert.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public BitArray ToBitArray(object value)
        {
            if (value == null || value is byte[] || value is BitArray)
            {
                return (BitArray)value;
            }
            throw new InvalidCastException(string.Format("Cannot cast type {0} to BitArray.", value.GetType().Name));
        }
        /// <summary>
        /// Converts a value to an array of bytes.
        /// </summary>
        /// <param name="value">Object to convert to array of bytes.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public byte[] GetBytes(object value)
        {
            if (value == null)
            {
                return null;
            }
            return GetBytes(value, value.GetType());
        }

        /// <summary>
        /// Converts a value as the given value type to an array of bytes.
        /// </summary>
        /// <param name="value">Object to convert to an array of bytes.</param>
        /// <param name="valueType">Type to cast the object to before converting.</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns></returns>
        public byte[] GetBytes(object value, Type valueType)
        {
            byte[] barray;
            switch (valueType.Name)
            {
                case "Bool": return new byte[] { (byte)((bool)value ? 1 : 0) };
                case "Byte": return new byte[] { (byte)value };
                case "Int16": return BitConverter.GetBytes((short)value);
                case "UInt16": return BitConverter.GetBytes((ushort)value);
                case "Int32": return BitConverter.GetBytes((int)value);
                case "UInt32": return BitConverter.GetBytes((uint)value);
                case "Single": return BitConverter.GetBytes((float)value);
                case "Double": return BitConverter.GetBytes((double)value);
                case "DateTime":
                    uint seconds = (uint)(((DateTime)value).Subtract(DateTime.UnixEpoch).TotalSeconds);
                    return BitConverter.GetBytes(seconds);
                case "String":
                    if (value == null)
                    {
                        // special null handling, represented with length byte = 1 and a single char
                        // value of zero as the string (empty string).
                        return new byte[1] { 0 };
                    }
                    int length = ((string)value).Length;
                    barray = new byte[length + 1];
                    barray[0] = (byte)length;
                    // copy the string chars to the buffer using ASCII (7-bit) encoding.
                    Encoding.ASCII.GetBytes((string)value).CopyTo(barray, 1);
                    return barray;
                case "Char":
                    return new byte[] { (byte)((char)value) };
                //return new byte[] { (byte)System.Convert.ChangeType(value, typeof(byte)) };
                case "ByteArray":
                    ByteArray byteArray = (ByteArray)value;
                    barray = new byte[(byteArray?.ByteCount ?? 0) + 1];
                    if (barray.Length > 1)
                    {
                        barray[0] = (byte)byteArray.ByteCount;
                        Array.Copy(byteArray, 0, barray, 1, byteArray.ByteCount);
                    }
                    return barray;
                case "BitArray":
                    BitArray bitArray = (BitArray)value;
                    barray = new byte[(bitArray?.ByteCount ?? 0) + 2];
                    if (barray.Length > 2)
                    {
                        Array.Copy(GetBytes((ushort)(bitArray.BitCount)), 0, barray, 0, 2);
                        Array.Copy(bitArray, 0, barray, 2, bitArray.ByteCount);
                    }
                    return barray;
                default: return new byte[] { };
            }
        }
        /// <summary>
        /// Reverses an array of bytes.  Used to correct for mismatched endianness between source and target machines.
        /// </summary>
        /// <param name="buffer">Array of bytes to reverse.</param>
        /// <param name="length">Length of segment to reverse.</param>
        /// <param name="start">Starting index of segment to reverse, defaults to 0 (beginning of array)</param>
        /// <returns></returns>
        private void ReverseBytes(byte[] buffer, int length, int start = 0)
        {
            if (buffer.Length != 0)
            {
                byte[] rbuff = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    rbuff[i] = buffer[start + length - i - 1];
                }
                rbuff.CopyTo(buffer, start);
            }
        }
    }
}
