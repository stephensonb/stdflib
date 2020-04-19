using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace STDFLib2
{
    public class STDFFormatterServices
    {
        public static Type ConvertTypeCode(ushort recordType)
        {
            return recordType switch
            {
                0x000A => typeof(FAR),
                0x0014 => typeof(ATR),
                0x010A => typeof(MIR),
                0x0114 => typeof(MRR),
                0x011E => typeof(PCR),
                0x0128 => typeof(HBR),
                0x0132 => typeof(SBR),
                0x013C => typeof(PMR),
                0x013E => typeof(PGR),
                0x013F => typeof(PLR),
                0x0146 => typeof(RDR),
                0x0150 => typeof(SDR),
                0x020A => typeof(WIR),
                0x0214 => typeof(WRR),
                0x021E => typeof(WCR),
                0x050A => typeof(PIR),
                0x0514 => typeof(PRR),
                0x0A1E => typeof(TSR),
                0x0F0A => typeof(PTR),
                0x0F0F => typeof(MPR),
                0x0F14 => typeof(FTR),
                0x140A => typeof(BPS),
                0x1414 => typeof(EPS),
                0x320A => typeof(GDR),
                0x321E => typeof(DTR),
                _ => throw new IndexOutOfRangeException("Unknown type code.")
            };
        }
        public static object[] GetObjectData(object obj)
        {
            var properties = GetSerializeableFields(obj.GetType());
            object[] data = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                data[i] = properties[i].GetValue(obj);
            }
            return data;
        }
        public static FieldInfo[] GetSerializeableFields(Type objType)
        {
            // Get the list of properties defined by the record type, then filter by
            // those that are decorated with the STDF attribute.  
            List<FieldInfo> fields = new List<FieldInfo>();

            foreach (var field in objType.GetFields())
            {
                var isSTDF = field.GetCustomAttribute<STDFAttribute>();
                var isOptional = field.GetCustomAttribute<STDFOptionalAttribute>();
                if (isSTDF != null || isOptional != null)
                {
                    fields.Add(field);
                }
            }
            return fields.ToArray();
        }
        public static RecordTypes GetTypeCode(Type recordType)
        {
            return recordType.Name switch
            {
                "FAR" => RecordTypes.FAR,
                "ATR" => RecordTypes.ATR,
                "MIR" => RecordTypes.MIR,
                "MRR" => RecordTypes.MRR,
                "PCR" => RecordTypes.PCR,
                "HBR" => RecordTypes.HBR,
                "SBR" => RecordTypes.SBR,
                "PMR" => RecordTypes.PMR,
                "PGR" => RecordTypes.PGR,
                "PLR" => RecordTypes.PLR,
                "RDR" => RecordTypes.RDR,
                "SDR" => RecordTypes.SDR,
                "WIR" => RecordTypes.WIR,
                "WRR" => RecordTypes.WRR,
                "WCR" => RecordTypes.WCR,
                "PIR" => RecordTypes.PIR,
                "PRR" => RecordTypes.PRR,
                "TSR" => RecordTypes.TSR,
                "PTR" => RecordTypes.PTR,
                "MPR" => RecordTypes.MPR,
                "FTR" => RecordTypes.FTR,
                "BPS" => RecordTypes.BPS,
                "EPS" => RecordTypes.EPS,
                "GDR" => RecordTypes.GDR,
                "DTR" => RecordTypes.DTR,
                _ => throw new IndexOutOfRangeException("Unknown record type.")
            };
        }
        public static object GetUninitializedObject(Type type)
        {
            return Activator.CreateInstance(type);
        }
        public static object PopulateObject(object obj, SerializationInfo info)
        {
            Type objType = obj.GetType();

            foreach(SerializationInfoEntry property in info)
            {
                if (property.Name == "LO_SPEC")
                {
                    ;
                }
                objType.GetProperty(property.Name).SetValue(obj, property.Value);
            }

            return obj;
        }

        public static object ToNullable(object value)
        {
            if (value?.GetType().Name == "Nullable`1")
            {
                return value.GetType().GetGenericArguments()[0].Name switch
                {
                    "Int16" => (short)value,
                    "Int32" => (int)value,
                    "Byte" => (byte)value,
                    "SByte" => (sbyte)value,
                    "Single" => (float)value,
                    "Double" => (double)value,
                    "UInt16" => (ushort)value,
                    "UInt32" => (uint)value,
                    _ => value
                };
            }
            return value;
        }
    }
}
