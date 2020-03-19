using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace STDFLib
{
    /// <summary>
    /// Encapsulates STDF file records and related functions
    /// </summary>
    public class STDFFileV4 : STDFFile
    {
        private ISTDFReader _reader;

        public override ISTDFRecord CreateRecord(RecordType recordType)
        {
            RecordTypes typeCode = (RecordTypes)(recordType.TypeCode);

            switch (typeCode)
            {
                case RecordTypes.FAR: return new FAR();  // File Attributes
                case RecordTypes.ATR: return new ATR();  // Audit Trail
                case RecordTypes.MIR: return new MIR();  // Master Information
                case RecordTypes.MRR: return new MRR();  // Master Results
                case RecordTypes.PCR: return new PCR();  // Part Count
                case RecordTypes.HBR: return new HBR();  // Hard Bin
                case RecordTypes.SBR: return new SBR();  // Soft Bin
                case RecordTypes.PMR: return new PMR();  // Pin Map
                case RecordTypes.PGR: return new PGR();  // Pin Group
                case RecordTypes.PLR: return new PLR();  // Pin List
                case RecordTypes.RDR: return new RDR();  // Retest Data
                case RecordTypes.SDR: return new SDR();  // Site Description
                case RecordTypes.WIR: return new WIR();  // Wafer Information
                case RecordTypes.WRR: return new WRR();  // Wafer Results
                case RecordTypes.WCR: return new WCR();  // Wafer Configuration
                case RecordTypes.PIR: return new PIR();  // Part Information
                case RecordTypes.PRR: return new PRR();  // Part Results
                case RecordTypes.TSR: return new TSR();  // Test Synopsis
                case RecordTypes.PTR: return new PTR();  // Parametric Test
                case RecordTypes.MPR: return new MPR();  // Multiple Result Parametric Test
                case RecordTypes.FTR: return new FTR();  // Functional Test 
                case RecordTypes.BPS: return new BPS();  // Begin Program Segment
                case RecordTypes.EPS: return new EPS();  // End Program Segment
                case RecordTypes.GDR: return new GDR();  // Generic Data
                case RecordTypes.DTR: return new DTR();  // Datalog Text
            }

            throw new ArgumentException(string.Format("Unsupported record type {0} sub type {1}", recordType.REC_TYP, recordType.REC_SUB));
        }

        public override ISTDFRecord DeserializeRecord(ISTDFReader reader)
        {
            _reader = reader;

            long startPosition = reader.Position;

            ISTDFRecord record = CreateRecord(reader.CurrentRecordType);

            if (record == null)
            {
                return null;
            }

            if (record is GDR)
            {
                return DeserializeGDR(reader);
            }
            else
            {
                // Get the list of properties that can be deserialized for the given record
                PropertyInfo[] props = GetSerializeableProperties(record);

                // If no properties to deserialize, then just return.
                if (props.Length == 0)
                {
                    return record;
                }

                // Holds the STDF attribute for each property to be deserialized
                STDFAttribute memberAttribute;

                // The number of data items to be written to the stream for array types
                int dataLength;
                int itemCount;

                foreach (var prop in props)
                {
                    memberAttribute = prop.GetCustomAttribute<STDFAttribute>();

                    dataLength = memberAttribute.DataLength;

                    // If the property is an array, then handle special
                    if (prop.PropertyType.IsArray)
                    {
                        // If a property name was specified for the ArrayCountProvider property in the data member attribute, 
                        // then get the number of array items to be written from this property
                        if (memberAttribute.ItemCountProvider != null)
                        {
                            var cnt = record.GetType().GetProperty(memberAttribute.ItemCountProvider)?.GetValue(record);
                            itemCount = 0x0000 + (ushort)cnt;
                        }
                        else
                        {
                            // No item count provider, so assume that the first byte holds the length of the array
                            itemCount = reader.ReadByte();
                        }
                       
                        if (itemCount > 0)
                        {
                            prop.SetValue(record, reader.Read(prop.PropertyType.GetElementType(), itemCount, dataLength));
                        }
                    }
                    else
                    {
                        if (memberAttribute.ItemCountProvider != null)
                        {
                            var cnt = record.GetType().GetProperty(memberAttribute.ItemCountProvider)?.GetValue(record);
                            dataLength = 0x0000 + (ushort)cnt;
                        }

                        prop.SetValue(record, reader.Read(prop.PropertyType, dataLength));
                    }
                  
                    if (reader.Position-startPosition >= reader.CurrentRecordLength)
                    {
                        // if we have reached the end of the record then exit loop
                        break;
                    }
                }
            }

            return record;
        }

        public ISTDFRecord DeserializeGDR(ISTDFReader reader)
        {
            GDR gdrRecord = new GDR();

            // Get the number of data fields to read in (first two bytes).  Note, we assume we have already read in the header from the stream and we are sitting on the first byte
            // of the data field count.
            int numFields = reader.ReadUInt16();

            // Save the start position of the first data record.  this will be used to make sure all record data reads start on an even by boundary
            long startPosition = reader.Position;

            // flag indicating if we are on an odd or even byte
            bool evenByte = true;

            // Holds the string representation of the field data type     
            Type fieldDataType;

            // Holds the value read from the stream
            object fieldValue = null;

            // Read in numFields fields
            for (int i = 0; i < numFields; i++)
            {
                // Determine if we are on an even byte in the data stream
                evenByte = ((reader.Position - startPosition) % 2) == 0;

                // read the type code for the next field in the stream
                byte fieldTypeCode = (byte)reader.ReadByte();

                // if the field started on an even byte, check for a padding byte (code = 0)
                if (evenByte && fieldTypeCode == 0)
                {
                    // started on an even byte and a padding byte was found, so read the next byte for the actual field type.
                    fieldTypeCode = (byte)reader.ReadByte();
                } 

                fieldDataType = GDR.GetFieldDataType(fieldTypeCode);

                // The type code was on an even byte.  Check to make sure the field data type does not need to
                // begin on an even byte.  If it does, then we have a problem and need to throw an exception
                if (evenByte)
                {
                    switch (fieldDataType?.Name)
                    {
                        // These types must start on an even byte alignment.  If the field type is one of these then we have an 
                        // error because we are sitting on an odd byte in the stream.
                        case "Int16":
                        case "Int32":
                        case "UInt16":
                        case "UInt32":
                        case "Single":
                        case "Double":
                            throw new STDFFormatException(string.Format("Invalid Generic Data Record format, misaligned field found.  Expected a padding byte for data type {0}, field starting at position {1}", fieldDataType, reader.Position));
                    }
                }

                // If the data type is unknown, throw an error.
                if (fieldDataType == null)
                {
                    throw new STDFFormatException(string.Format("Invalid file format or unsupported Generic Record field data type found in data stream.  Field data type code '{0}' found at position {1}", fieldTypeCode, reader.Position));
                }

                fieldValue = reader.Read(fieldDataType,-1);

                if (fieldValue != null)
                {
                    // Add the field to the generic data record
                    gdrRecord.Add(fieldValue);
                }
            }

            // Return the generic data record
            return gdrRecord;
        }
    }
}

