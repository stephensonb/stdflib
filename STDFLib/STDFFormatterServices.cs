using System;
using System.Collections.Generic;
using System.Reflection;

namespace STDFLib
{
    /// <summary>
    /// Static helper class to provide common methods for serialization and deserialization of STDF objects
    /// </summary>
    public class STDFFormatterServices
    {
        /// <summary>
        /// Converts an STDF record type code (type and subtype) into the corresponding concrete class representing the 
        /// record type in code.
        /// </summary>
        /// <param name="recordType">Unsigned integer with the high byte the Type code and the low byte the sub type code.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the recordType code is not recognized.</exception>
        /// <returns></returns>
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
        /// <summary>
        /// Static method to return the serializable fields for an STDF record object.  Fields are serializable if they
        /// have been decorate with the STDF or STDFOptional attribute.
        /// </summary>
        /// <param name="objType">Object type to inspect and return the serializable fields.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the record type enumeration member for the corresponding class Type.
        /// </summary>
        /// <param name="recordType">Class type to return the RecordType for.</param>
        /// <exception cref="IndexOutOfRangeException">Class type has no corresponding RecordType code.</exception>
        /// <returns></returns>
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
        /// <summary>
        /// Creates and returns an uninialized object of the given Type.  Used for deserialization.
        /// </summary>
        /// <param name="type">Object type to create.</param>
        /// <returns></returns>
        public static object GetUninitializedObject(Type type)
        {
            return Activator.CreateInstance(type);
        }
        /// <summary>
        /// Does a simple population of the given STDF object with field data from the given SerializationInfo object.
        /// </summary>
        /// <param name="obj">Object to populate.</param>
        /// <param name="info">Field data used to populate the object.</param>
        /// <returns></returns>
        public static object PopulateObject(object obj, SerializationInfo info)
        {
            Type objType = obj.GetType();

            foreach (SerializationInfoEntry property in info)
            {
                objType.GetProperty(property.Name).SetValue(obj, property.Value);
            }

            return obj;
        }
    }
}
