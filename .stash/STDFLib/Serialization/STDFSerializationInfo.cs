using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace STDFLib2.Serialization
{
    public class STDFSerializationInfo
    {
        private static List<STDFSerializationInfo> InfoCache { get; } = new List<STDFSerializationInfo>();
        private STDFSerializationInfo(Type objType, ISTDFFormatterConverter converter)
        {
            // Class can only store data from objects implmenting the ISTDFRecord interface
            Type = objType;
            
            Converter = converter;
        }
        protected List<STDFSerializationInfoEntry> PropertyValues { get; } = new List<STDFSerializationInfoEntry>();
        protected ISTDFFormatterConverter Converter { get; }
        public static STDFSerializationInfo Create(Type objType, ISTDFFormatterConverter converter)
        {
            var info = InfoCache.Find(x => x.Type.Name == objType.Name);         
            
            if (info == null)
            {
                info = new STDFSerializationInfo(objType, converter);
            }

            info.Clear();

            return info;
        }
        public IEnumerator<STDFSerializationInfoEntry> GetEnumerator() => PropertyValues.GetEnumerator();
        public int PropertyCount { get => PropertyValues.Count; }
        public Type Type { get; }
        public void Clear()
        {
            PropertyValues.Clear();
        }
        public void Remove(STDFSerializationInfoEntry item)
        {
            PropertyValues.Remove(item);
        }

        // Add methods convert the passed value to it's byte representation and is stored internally
        // as a byte array along with the original type information.
        public void AddValue(string name, object value, Type objType)
        {
            // convert to byte array if not already a byte array
            if (!(value is byte[]))
            {
                value = Converter.GetBytes(value);
            }
            PropertyValues.Add(new STDFSerializationInfoEntry(name, value, objType));
        }
        public void AddValue(string name, object value) => AddValue(name, value, value.GetType());
        public void AddValue(string name, bool value) => AddValue(name, value);
        public void AddValue(string name, byte value) => AddValue(name, value);
        public void AddValue(string name, char value) => AddValue(name, value);
        public void AddValue(string name, DateTime value) => AddValue(name, value);
        public void AddValue(string name, double value) => AddValue(name, value);
        public void AddValue(string name, short value) => AddValue(name, value);
        public void AddValue(string name, int value) => AddValue(name, value);
        public void AddValue(string name, sbyte value) => AddValue(name, value);
        public void AddValue(string name, float value) => AddValue(name, value);
        public void AddValue(string name, ushort value) => AddValue(name, value);
        public void AddValue(string name, uint value) => AddValue(name, value);

        // GetValue methods convert the internal byte representations back to the requested type.
        // NOTE: a type conversion exception will be thrown if the value cannot be converted.
        protected object GetValue(string name) {
            return GetValue(name, null);
        }
        public object GetValue(string name, Type type)
        {
            var propValue = PropertyValues.Find(x => x.Name == name);

            if (propValue != null)
            {
                type ??= propValue.Type;

                // If requesting the data as the internal byte representation, then just return it
                if (type.Name == "Byte[]")
                {
                    return propValue.Value;
                }

                // convert the byte array back to the original type
                var value = Converter.Convert(propValue.Value, type);
                
                // if the requested type and original type are the same, the just return as is
                if (type.Equals(value.GetType()))
                {
                    return value;
                }

                // try to convert the value into the given type
                return Convert.ChangeType(value, type);
            }
            return null;
        }      
        public bool GetBoolean(string name) => (bool)GetValue(name);
        public byte GetByte(string name) => (byte)GetValue(name);
        public char GetChar(string name) => (char)GetValue(name);
        public DateTime GetDateTime(string name) => (DateTime)GetValue(name);
        public double GetDouble(string name) => (double)GetValue(name);
        public short GetInt16(string name) => (short)GetValue(name);
        public int GetInt32(string name) => (int)GetValue(name);
        public ushort GetUInt16(string name) => (ushort)GetValue(name);
        public uint GetUInt32(string name) => (uint)GetValue(name);
        public sbyte GetSByte(string name) => (sbyte)GetValue(name);
        public float GetSingle(string name) => (float)GetValue(name);
        public string GetString(string name) => (string)GetValue(name);
    }
}
