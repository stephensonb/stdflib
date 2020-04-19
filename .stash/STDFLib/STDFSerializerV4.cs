using System;
using System.Reflection;
using System.IO;

namespace STDFLib
{
    public class STDFSerializerV4
    {
        public static PropertyInfo[] GetSerializeableProperties(ISTDFRecord record)
        {
            // Get the list of properties defined by the record type, then filter by
            // those that are decorated with the STDF attribute.  
            // Properties will be returned sorted based on the Order property of the STDFAttribute
            return record.GetType().GetProperties();
        }

        private static ISTDFRecord CreateRecord(RecordType recordType)
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

        public static ISTDFRecord Deserialize(STDFBinaryReader reader)
        {
            if (reader.BaseStream.Position >= reader.BaseStream.Length)
            {
                throw new EndOfStreamException();
            }
            long startPosition = reader.BaseStream.Position;

            // read the header
            int recordLength = reader.ReadUInt16();
            RecordType recordType = reader.ReadUInt16();

            // FOR TESTING ONLY : checking only 3 types of records.

            if (recordType.TypeCode == (ushort)RecordTypes.PGR)
            {
                // skip to next record
                //reader.BaseStream.Seek(recordLength, System.IO.SeekOrigin.Current);
                //return null;
                _ = "Stop on PGR";
            }

            ISTDFRecord record = CreateRecord(recordType);

            if (record == null)
            {
                return null;
            }
            else
            {
                // Get the list of properties that can be serialized for the given record
                PropertyInfo[] props = GetSerializeableProperties(record);

                // If no properties to serialize, then just return.
                if (props.Length == 0)
                {
                    return record;
                }

                // Holds the STDF attribute for each property to be deserialized
                STDFAttribute memberAttribute;

                // The number of data items to be written to the stream for array types
                int dataLength;
                int itemCount;
                object result;

                foreach (var prop in props)
                {
                    if (record is FTR)
                    {
                        if ((((FTR)record).TEST_FLG & 128) > 0)
                        {
                            _ = "Failed pins FTR Record";
                        }
                    }
                    memberAttribute = prop.GetCustomAttribute<STDFAttribute>();
                    if (memberAttribute == null)
                    {
                        // not an STDF serializable property, so skip
                        continue;
                    }

                    dataLength = memberAttribute.DataLength;
                    itemCount = record.GetItemCount(memberAttribute.ItemCountProperty);

                    if (prop.PropertyType.IsArray && record is GDR)
                    {
                        result = reader.ReadVarDataArray(itemCount);
                    }
                    else
                    {
                        result = reader.Read(prop.PropertyType, itemCount);
                    }

                    if (result != null)
                    {
                        prop.SetValue(record, result);
                    }

                    if ((reader.BaseStream.Position - startPosition - 4) >= recordLength)
                    {
                        // if we have reached the end of the record then exit loop
                        break;
                    }
                }
            }

            return record;
        }

        public static int Serialize(STDFBinaryWriter writer, ISTDFRecord record)
        {
            long startPosition = writer.BaseStream.Position;

            // write length field = 0, will update after record has been written
            writer.Write((ushort)0);

            // write the record type and sub type codes
            writer.Write(record.RecordType.REC_TYP);
            writer.Write(record.RecordType.REC_SUB);

            // Get the list of properties that can be deserialized for the given record
            PropertyInfo[] props = GetSerializeableProperties(record);

            // If no properties to deserialize, then just return.
            if (props.Length == 0)
            {
                return 0;
            }

            // Holds the STDF attribute for each property to be deserialized
            STDFAttribute memberAttribute;

            // The number of data items to be written to the stream for array types
            int dataLength;
            int itemCount;
            object propValue;

            foreach (var prop in props)
            {
                memberAttribute = prop.GetCustomAttribute<STDFAttribute>();

                dataLength = memberAttribute.DataLength;
                itemCount = record.GetItemCount(memberAttribute.ItemCountProperty);
                propValue = prop.GetValue(record);

                // If the property is an array, then handle special
                if (prop.PropertyType.IsArray)
                {
                    // If a property name was specified for the ArrayCountProvider property in the data member attribute, 
                    // then get the number of array items to be written from this property
                    if (itemCount < 1)
                    {
                        itemCount = ((object[])propValue).Length;
                        // No item count provider, so assume that the first byte holds the length of the array
                        writer.Write((byte)itemCount);
                    }

                    if (itemCount > 0)
                    {
                        if (record is GDR)
                        {
                            writer.WriteVarDataArray((object[])propValue);
                        }
                        else
                        {
                            writer.WriteArray((object[])propValue, itemCount);
                        }
                    }
                }
                else
                {
                    writer.Write(propValue);
                }
            }

            long endPosition = writer.BaseStream.Position;
            int recLength = (int)(endPosition - startPosition);
            // Seek back to start of record
            writer.Seek(-recLength, System.IO.SeekOrigin.Current);
            // write the record length (subtract 4 from end minus start to account for the 4 header bytes)
            writer.Write((ushort)(recLength - 4));
            // position back to the end of the record
            writer.Seek(recLength - 2, System.IO.SeekOrigin.Current);

            return recLength - 4;
        }
    }
}

