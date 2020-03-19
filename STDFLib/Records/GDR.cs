using System;
using System.Collections.Generic;

namespace STDFLib
{
    /// <summary>
    /// Generic Data Record
    /// </summary>
    public class GDR : STDFRecord
    {
        private List<GenericRecordDataField> _gen_data { get; set; } = new List<GenericRecordDataField>();
        private ushort _gen_data_count = 0;

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
            typeof(Byte[]),     // Type code = 11
            typeof(BitField2),  // Type code = 12
            typeof(Nibbles)     // Type code = 13
        };

        public override RecordType TypeCode => 0x320A;

        [STDF(Order = 1)]
        public ushort FLD_CNT
        {
            get => (ushort)_gen_data.Count;

            set
            {
                _gen_data_count = value;
            }
        }

        [STDF(Order = 2)]
        public GenericRecordDataField[] FieldData
        {
            get
            {
                return _gen_data.ToArray();
            }

            set
            {
                if (value != null)
                {
                    _gen_data.AddRange(value);                
                }
            }
        }

        public override string Description => "Generic Data Record";

        public GDR() { }

        public static Type GetFieldDataType(byte varDataTypeCode)
        {
            if (varDataTypeCode >= 0 && varDataTypeCode != 9 && varDataTypeCode <= 13)
            {
                return field_data_types[varDataTypeCode];
            }

            throw new ArgumentException(string.Format("Unsupported variable data type code {0}", varDataTypeCode));
        }

        public static int GetFieldTypeCode(Type fieldType)
        {
            for(int i=0;i<field_data_types.Length;i++)
            {
                if (field_data_types[i] == fieldType)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Add(object fieldValue)
        {
            int typeCode = GetFieldTypeCode(fieldValue.GetType());

            if (typeCode < 0)
            {
                throw new ArgumentException(string.Format("Trying to add an unsupported field type {0} to a generic data record.", fieldValue.GetType().Name));
            }

            _gen_data.Add(new GenericRecordDataField() { FieldType = (byte)typeCode, Value = fieldValue });
        }

        public GenericRecordDataField this[int i]
        {
            get
            {
                if (i >= 0 && i < FLD_CNT)
                {
                    return _gen_data[i];
                }

                throw new IndexOutOfRangeException("Field index on generic data record out of range.");
            }
        }
    }
}
