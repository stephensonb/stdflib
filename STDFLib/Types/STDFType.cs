using System;
using System.IO;

namespace STDFLib
{
    public abstract class STDFType<T>
    {
        protected virtual T Value { get; set; }
        public static readonly ByteConverter Converter = new ByteConverter();
        
        public virtual byte[] ToByteArray()
        {
            // default implentation handles value types only.
            if (this.GetType().IsValueType)
            {
                return Converter.GetBytes(Value);
            }

            // override in subclass to handle object types wrapped by this class.  Default implementation just
            // returns empty byte array
            return new byte[] { };
        }

        public STDFType(T value) {
            Value = value;
        }

        public STDFType() { }

        public abstract int SerializedLength { get; }
        public abstract byte[] Serialize();
        public abstract long Deserialize(byte[] buffer, long start);
    }

    public abstract class STDFType
    {
        /*
        public static int SizeOf(Type objType)
        {
            if(objType.IsValueType)
            {
                switch(objType.Name)
                {
                    case "Bool": return sizeof(bool);
                    case "Byte": return sizeof(byte);
                    case "SByte": return sizeof(sbyte);
                    case "Int16": return sizeof(short);
                    case "UInt16": return sizeof(ushort);
                    case "Int32": return sizeof(int);
                    case "UInt32": return sizeof(uint);
                    case "Int64": return sizeof(long);
                    case "UInt64": return sizeof(ulong);
                    case "Single": return sizeof(float);
                    case "Double": return sizeof(double);
                }
            }
            return 0;
        }

        public static int SerializedLength(object value)
        {
            if (value.GetType().IsValueType)
            {
                return SizeOf(value.GetType());
            }

            switch(value.GetType().Name)
            {
                case "String": return ((string)value).Length + 1;
                case "Nibbles": return ((Nibbles)value).ByteCount;
                case "BitField": return ((BitField)value).ByteCount + 1;
                case "BitField2": return ((BitField2)value).ByteCount + 1;
                default: return 0;
            }
        }
        */
        protected object Value;
        public static readonly ByteConverter Converter = new ByteConverter();
        protected STDFType(object val)
        {
            Value = val;
        }
        public abstract int SerializedLength { get; }
        public abstract BinaryWriter Serialize(BinaryWriter outStream);
        public abstract BinaryReader Deserialize(BinaryReader inStream);
    }
}
