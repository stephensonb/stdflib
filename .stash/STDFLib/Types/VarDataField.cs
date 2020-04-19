using System;

namespace STDFLib2
{
    public class VarDataField
    {
        public int TypeCode
        {
            get
            {
                return GetFieldTypeCode(Value);
            }
        }

        public object Value { get; set; }

        public VarDataField(object value)
        {
            Value = value;
        }

        private static readonly Type[] field_data_types = new Type[]
        {
            null,               // Type code = 0 - reserved for padding byte
            typeof(Byte),       // Type code = 1
            typeof(UInt16),     // Type code = 2
            typeof(UInt32),     // Type code = 3
            typeof(SByte),      // Type code = 4
            typeof(Int16),      // Type code = 5
            typeof(Int32),      // Type code = 6 
            typeof(Single),     // Type code = 7 
            typeof(Double),     // Type code = 8 
            null,               // Type code = 9 - NOT USED
            typeof(String),     // Type code = 10
            typeof(BitField),   // Type code = 11
            typeof(BitField2),  // Type code = 12
            typeof(Nibbles)     // Type code = 13
        };

        public static T ConvertTo<T>(VarDataField fld)
        {
            if (fld.Value is T)
            {
                return (T)fld.Value;
            }
            throw new InvalidCastException();
        }

        public static Type GetFieldDataType(byte varDataTypeCode)
        {
            if (varDataTypeCode >= 0 && varDataTypeCode != 9 && varDataTypeCode <= 13)
            {
                return field_data_types[varDataTypeCode];
            }

            throw new ArgumentException(string.Format("Unsupported variable data type code {0}", varDataTypeCode));
        }

        public static int GetFieldTypeCode(object value)
        {
            return GetFieldTypeCode(value.GetType());
        }

        public static int GetFieldTypeCode(Type fieldType)
        {
            for (int i = 0; i < field_data_types.Length; i++)
            {
                if (field_data_types[i] == fieldType)
                {
                    return i;
                }
            }

            return -1;
        }

        public static explicit operator byte(VarDataField value) => ConvertTo<byte>(value);
        public static explicit operator sbyte(VarDataField value) => ConvertTo<sbyte>(value);
        public static explicit operator BitField(VarDataField value) => ConvertTo<BitField>(value);
        public static explicit operator BitField2(VarDataField value) => ConvertTo<BitField2>(value);
        public static explicit operator double(VarDataField value) => ConvertTo<double>(value);
        public static explicit operator float(VarDataField value) => ConvertTo<float>(value);
        public static explicit operator int(VarDataField value) => ConvertTo<int>(value);
        public static explicit operator Nibbles(VarDataField value) => ConvertTo<Nibbles>(value);
        public static explicit operator short(VarDataField value) => ConvertTo<short>(value);
        public static explicit operator string(VarDataField value) => ConvertTo<string>(value);
        public static explicit operator uint(VarDataField value) => ConvertTo<uint>(value);
        public static explicit operator ushort(VarDataField value) => ConvertTo<ushort>(value);

        public static explicit operator VarDataField(byte value) => new VarDataField(value);
        public static explicit operator VarDataField(sbyte value) => new VarDataField(value);
        public static explicit operator VarDataField(BitField value) => new VarDataField(value);
        public static explicit operator VarDataField(BitField2 value) => new VarDataField(value);
        public static explicit operator VarDataField(double value) => new VarDataField(value);
        public static explicit operator VarDataField(float value) => new VarDataField(value);
        public static explicit operator VarDataField(int value) => new VarDataField(value);
        public static explicit operator VarDataField(Nibbles value) => new VarDataField(value);
        public static explicit operator VarDataField(short value) => new VarDataField(value);
        public static explicit operator VarDataField(string value) => new VarDataField(value);
        public static explicit operator VarDataField(uint value) => new VarDataField(value);
        public static explicit operator VarDataField(ushort value) => new VarDataField(value);
    }
}
