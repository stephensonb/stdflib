using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace STDFLib2.Serialization
{
    public class InvalidValue
    {
        public Type TargetType { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class STDFFormatterServices
    {

        private static List<InvalidValue> MissingOrInvalidValues { get; } = new List<InvalidValue>();

        public static void SetMissingValue(Type targetType, string propertyName, object value)
        {
            var missingValue = MissingOrInvalidValues.Find(x => x.TargetType.Name == targetType.Name && x.Name == propertyName);

            if (missingValue == null)
            {
                missingValue = new InvalidValue() { TargetType = targetType, Name = propertyName };
                MissingOrInvalidValues.Add(missingValue);
            }

            missingValue.Value = value;
        }

        public static bool TryGetMissingValue(Type targetType, string propertyName, out object value)
        {
            var missingValue = MissingOrInvalidValues.Find(x => x.TargetType.Name == targetType.Name && x.Name == propertyName);

            if (missingValue == null)
            {
                value = null;
                return false;
            }

            value = missingValue.Value;

            return true;
        }

        public static PropertyInfo[] GetSerializeableProperties(Type recordType)
        {
            // Get the list of properties defined by the record type, then filter by
            // those that are decorated with the STDF attribute.  
            List<PropertyInfo> props = new List<PropertyInfo>();

            foreach (var prop in recordType.GetProperties())
            {
                var isSTDF = prop.GetCustomAttribute<STDFAttribute>();
                var isOptional = prop.GetCustomAttribute<STDFOptionalAttribute>();

                if (isSTDF != null || isOptional != null)
                {
                    props.Add(prop);
                }
            }
            return props.ToArray();
        }

        public static PropertyInfo[] GetSerializeableProperties(ISTDFRecord record)
        {
            return GetSerializeableProperties(record.GetType());
        }

        public static RecordTypes ConvertToRecordType(Type type)
        {
            return type.Name switch
            {
                "FAR" => RecordTypes.FAR,// File Attributes
                "ATR" => RecordTypes.ATR,// Audit Trail
                "MIR" => RecordTypes.MIR,// Master Information
                "MRR" => RecordTypes.MRR,// Master Results
                "PCR" => RecordTypes.PCR,// Part Count
                "HBR" => RecordTypes.HBR,// Hard Bin
                "SBR" => RecordTypes.SBR,// Soft Bin
                "PMR" => RecordTypes.PMR,// Pin Map
                "PGR" => RecordTypes.PGR,// Pin Group
                "PLR" => RecordTypes.PLR,// Pin List
                "RDR" => RecordTypes.RDR,// Retest Data
                "SDR" => RecordTypes.SDR,// Site Description
                "WIR" => RecordTypes.WIR,// Wafer Information
                "WRR" => RecordTypes.WRR,// Wafer Results
                "WCR" => RecordTypes.WCR,// Wafer Configuration
                "PIR" => RecordTypes.PIR,// Part Information
                "PRR" => RecordTypes.PRR,// Part Results
                "TSR" => RecordTypes.TSR,// Test Synopsis
                "PTR" => RecordTypes.PTR,// Parametric Test
                "MPR" => RecordTypes.MPR,// Multiple Result Parametric Test
                "FTR" => RecordTypes.FTR,// Functional Test 
                "BPS" => RecordTypes.BPS,// Begin Program Segment
                "EPS" => RecordTypes.EPS,// End Program Segment
                "GDR" => RecordTypes.GDR,// Generic Data
                "DTR" => RecordTypes.DTR,// Datalog Text
                _ => throw new ArgumentException(string.Format("Unsupported record type {0}", type.Name))
            };
        }

        public static Type GetTypeFromRecordType(RecordTypes recordType)
        {
            return recordType.ToString() switch
            {
                "FAR" => typeof(FAR),// File Attributes
                "ATR" => typeof(ATR),// Audit Trail
                "MIR" => typeof(MIR),// Master Information
                "MRR" => typeof(MRR),// Master Results
                "PCR" => typeof(PCR),// Part Count
                "HBR" => typeof(HBR),// Hard Bin
                "SBR" => typeof(SBR),// Soft Bin
                "PMR" => typeof(PMR),// Pin Map
                "PGR" => typeof(PGR),// Pin Group
                "PLR" => typeof(PLR),// Pin List
                "RDR" => typeof(RDR),// Retest Data
                "SDR" => typeof(SDR),// Site Description
                "WIR" => typeof(WIR),// Wafer Information
                "WRR" => typeof(WRR),// Wafer Results
                "WCR" => typeof(WCR),// Wafer Configuration
                "PIR" => typeof(PIR),// Part Information
                "PRR" => typeof(PRR),// Part Results
                "TSR" => typeof(TSR),// Test Synopsis
                "PTR" => typeof(PTR),// Parametric Test
                "MPR" => typeof(MPR),// Multiple Result Parametric Test
                "FTR" => typeof(FTR),// Functional Test 
                "BPS" => typeof(BPS),// Begin Program Segment
                "EPS" => typeof(EPS),// End Program Segment
                "GDR" => typeof(GDR),// Generic Data
                "DTR" => typeof(DTR),// Datalog Text
                _ => throw new ArgumentException(string.Format("Unsupported record type {0}", recordType))
            };
        }

        public static ISTDFRecord CreateSTDFRecord(Type type)
        {
            return CreateSTDFRecord(type.Name);
        }

        public static ISTDFRecord CreateSTDFRecord(RecordTypes recordType)
        {
            return CreateSTDFRecord(recordType.ToString());
        }

        public static ISTDFRecord CreateSTDFRecord(string recordTypeName)
        { 
            return recordTypeName switch
            {
                "FAR" => new FAR(),// File Attributes
                "ATR" => new ATR(),// Audit Trail
                "MIR" => new MIR(),// Master Information
                "MRR" => new MRR(),// Master Results
                "PCR" => new PCR(),// Part Count
                "HBR" => new HBR(),// Hard Bin
                "SBR" => new SBR(),// Soft Bin
                "PMR" => new PMR(),// Pin Map
                "PGR" => new PGR(),// Pin Group
                "PLR" => new PLR(),// Pin List
                "RDR" => new RDR(),// Retest Data
                "SDR" => new SDR(),// Site Description
                "WIR" => new WIR(),// Wafer Information
                "WRR" => new WRR(),// Wafer Results
                "WCR" => new WCR(),// Wafer Configuration
                "PIR" => new PIR(),// Part Information
                "PRR" => new PRR(),// Part Results
                "TSR" => new TSR(),// Test Synopsis
                "PTR" => new PTR(),// Parametric Test
                "MPR" => new MPR(),// Multiple Result Parametric Test
                "FTR" => new FTR(),// Functional Test 
                "BPS" => new BPS(),// Begin Program Segment
                "EPS" => new EPS(),// End Program Segment
                "GDR" => new GDR(),// Generic Data
                "DTR" => new DTR(),// Datalog Text
                _ => throw new ArgumentException(string.Format("Unsupported record type {0}", recordTypeName))
            };
        }

        public static void CopySTDFRecord(ISTDFRecord source, ISTDFRecord target, bool copyOptionalOnly=true)
        {
            if (source == null || target == null)
            {
                return;
            }

            Type sourceType = source.GetType();
            Type targetType = target.GetType();

            if (sourceType.Name == targetType.Name)
            {
                foreach (var prop in source.GetType().GetProperties())
                {
                    bool isOptional = prop.GetCustomAttribute<STDFOptionalAttribute>() != null;
                    if (!copyOptionalOnly || isOptional)
                    {
                        object sourceValue = prop.GetValue(source);

                        // Deep copy an array reference
                        if (sourceValue is Array values && sourceValue != null)
                        {
                            sourceValue = Array.CreateInstance(values.GetType().GetElementType(), values.Length);
                            Array.Copy(values, (Array)sourceValue, values.Length);
                        }

                        // copy the source record property value to the target record property.
                        prop.SetValue(target, sourceValue);
                    }
                }
            }
        }
    }
}
