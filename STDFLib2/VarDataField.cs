using System;

namespace STDFLib2
{
    public class VarDataField
    {
        public VarDataField(object value)
        {
            Value = value;
        }
        public object Value { get; set; }
        public GenericDataTypes TypeCode
        {
            get
            {
                return Value switch
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
            }
        }
        public static T ConvertTo<T>(VarDataField fld)
        {
            if (fld.Value is T)
            {
                return (T)fld.Value;
            }
            throw new InvalidCastException();
        }
        public static explicit operator byte(VarDataField value) => ConvertTo<byte>(value);
        public static explicit operator sbyte(VarDataField value) => ConvertTo<sbyte>(value);
        public static explicit operator BitArray(VarDataField value) => ConvertTo<BitArray>(value);
        public static explicit operator ByteArray(VarDataField value) => ConvertTo<ByteArray>(value);
        public static explicit operator double(VarDataField value) => ConvertTo<double>(value);
        public static explicit operator float(VarDataField value) => ConvertTo<float>(value);
        public static explicit operator int(VarDataField value) => ConvertTo<int>(value);
        public static explicit operator Nibble(VarDataField value) => ConvertTo<Nibble>(value);
        public static explicit operator short(VarDataField value) => ConvertTo<short>(value);
        public static explicit operator string(VarDataField value) => ConvertTo<string>(value);
        public static explicit operator uint(VarDataField value) => ConvertTo<uint>(value);
        public static explicit operator ushort(VarDataField value) => ConvertTo<ushort>(value);

        public static explicit operator VarDataField(byte value) => new VarDataField(value);
        public static explicit operator VarDataField(sbyte value) => new VarDataField(value);
        public static explicit operator VarDataField(BitArray value) => new VarDataField(value);
        public static explicit operator VarDataField(ByteArray value) => new VarDataField(value);
        public static explicit operator VarDataField(double value) => new VarDataField(value);
        public static explicit operator VarDataField(float value) => new VarDataField(value);
        public static explicit operator VarDataField(int value) => new VarDataField(value);
        public static explicit operator VarDataField(Nibble value) => new VarDataField(value);
        public static explicit operator VarDataField(short value) => new VarDataField(value);
        public static explicit operator VarDataField(string value) => new VarDataField(value);
        public static explicit operator VarDataField(uint value) => new VarDataField(value);
        public static explicit operator VarDataField(ushort value) => new VarDataField(value);
    }
}
