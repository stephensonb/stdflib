using System;

namespace STDFLib
{
    /// <summary>
    /// STDF Variable Data Field.  Encapsulates a data type code and the data together in a single structure.
    /// For use in Generic Data (GDR) type records.
    /// </summary>
    public class VarData
    {
        /// <summary>
        /// Creates a VarData object and initializes it with the passed value.
        /// </summary>
        /// <param name="value">Value to initialize the VarData object with.  The Type Code will is determined from the base type of
        /// the the object passed.  If the type is unknown an exception will be thrown. </param>
        /// <exception cref="InvalidCastException"></exception>
        public VarData(object value)
        {
            Value = value;

            // Check type of passed value to make sure we support it.
            switch (value)
            {
                case byte _:
                case sbyte _:
                case short _:
                case int _:
                case float _:
                case double _:
                case string _:
                case ushort _:
                case uint _:
                case ByteArray _:
                case BitArray _:
                case Nibble _:
                    break;
                default:
                    throw new InvalidCastException("Unsupported data type for VarData field.");
            }
        }

        /// <summary>
        /// Value of the VarData object.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Type code representing the type of data contained in the Value property.
        /// </summary>
        public GenericDataTypes TypeCode => Value switch
        {
            byte _ => GenericDataTypes.Byte,
            sbyte _ => GenericDataTypes.SByte,
            short _ => GenericDataTypes.Int16,
            int _ => GenericDataTypes.Int32,
            float _ => GenericDataTypes.Float,
            double _ => GenericDataTypes.Double,
            string _ => GenericDataTypes.String,
            ushort _ => GenericDataTypes.UInt16,
            uint _ => GenericDataTypes.UInt32,
            ByteArray _ => GenericDataTypes.ByteArray,
            BitArray _ => GenericDataTypes.BitArray,
            Nibble _ => GenericDataTypes.Nibble,
            _ => GenericDataTypes.Padding
        };

        /// <summary>
        /// Static method to convert the given object to a base type.  Conversion is based on the Type Code of the VarData object.
        /// </summary>
        /// <typeparam name="T">Type to try and convert the value of the VarData object to.</typeparam>
        /// <param name="fld">Object containing the type code and value to convert.</param>
        /// <returns></returns>
        public static T ConvertTo<T>(VarData fld)
        {
            if (fld.Value is T)
            {
                return (T)fld.Value;
            }
            throw new InvalidCastException();
        }

        #region Explicit conversion operators to convert a VarData object to and from base types where possible
        public static explicit operator byte(VarData value)
        {
            return ConvertTo<byte>(value);
        }

        public static explicit operator sbyte(VarData value)
        {
            return ConvertTo<sbyte>(value);
        }

        public static explicit operator BitArray(VarData value)
        {
            return ConvertTo<BitArray>(value);
        }

        public static explicit operator ByteArray(VarData value)
        {
            return ConvertTo<ByteArray>(value);
        }

        public static explicit operator double(VarData value)
        {
            return ConvertTo<double>(value);
        }

        public static explicit operator float(VarData value)
        {
            return ConvertTo<float>(value);
        }

        public static explicit operator int(VarData value)
        {
            return ConvertTo<int>(value);
        }

        public static explicit operator Nibble(VarData value)
        {
            return ConvertTo<Nibble>(value);
        }

        public static explicit operator short(VarData value)
        {
            return ConvertTo<short>(value);
        }

        public static explicit operator string(VarData value)
        {
            return ConvertTo<string>(value);
        }

        public static explicit operator uint(VarData value)
        {
            return ConvertTo<uint>(value);
        }

        public static explicit operator ushort(VarData value)
        {
            return ConvertTo<ushort>(value);
        }

        public static explicit operator VarData(byte value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(sbyte value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(BitArray value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(ByteArray value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(double value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(float value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(int value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(Nibble value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(short value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(string value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(uint value)
        {
            return new VarData(value);
        }

        public static explicit operator VarData(ushort value)
        {
            return new VarData(value);
        }
        #endregion
    }
}
