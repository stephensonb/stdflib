using System;

namespace STDFLib
{
    /// <summary>
    /// Defines field used in the Generic Data Record (GDR) format.
    /// </summary>
    public class VData
    {
        public object Value { get; set; } = null;

        public VData(object value)
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

        public int GetFieldTypeCode()
        {
            Type fieldType = Value.GetType();
            for (int i = 0; i < field_data_types.Length; i++)
            {
                if (field_data_types[i] == fieldType)
                {
                    return i;
                }
            }

            return -1;
        }

        public override string ToString()
        {
            return string.Format("{0,25}:{1,-30}", FieldType, Value.ToString());
        }
    }
}
