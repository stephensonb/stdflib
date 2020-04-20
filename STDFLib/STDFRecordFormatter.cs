using System;
using System.IO;

namespace STDFLib
{
    /// <summary>
    /// Provides serialization and deserialization of STDF record to and from a data stream.
    /// </summary>
    public class STDFRecordFormatter : STDFBinaryFormatter
    {
        public STDFRecordFormatter()
        {
            // Add default provider
            TypeSurrogateSelector.AddSurrogate(typeof(ATR), new ATRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(BPS), new BPSSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(DTR), new DTRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(EPS), new EPSSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(FAR), new FARSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(FTR), new FTRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(GDR), new GDRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(HBR), new HBRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(MIR), new MIRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(MPR), new MPRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(MRR), new MRRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(PCR), new PCRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(PGR), new PGRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(PIR), new PIRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(PLR), new PLRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(PMR), new PMRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(PRR), new PRRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(PTR), new PTRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(RDR), new RDRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(SBR), new SBRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(SDR), new SDRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(TSR), new TSRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(WCR), new WCRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(WIR), new WIRSurrogate());
            TypeSurrogateSelector.AddSurrogate(typeof(WRR), new WRRSurrogate());
        }
        /// <summary>
        /// Flag to indicate if the end of the record has been reached during deserialization.
        /// </summary>
        public bool EndOfRecord { get; protected set; }
        /// <summary>
        /// Reads data from an STDF data stream and deserializes it into the corresponding STDF record type.
        /// </summary>
        /// <param name="stream">Stream object to read the record data from.</param>
        /// <returns></returns>
        public override object Deserialize(Stream stream)
        {
            SerializeStream = stream;
            EndOfStream = SerializeStream.Position >= SerializeStream.Length;
            if (EndOfStream)
            {
                return null;
            }
            ReadHeader(out ushort recordLength, out ushort recordTypeCode);
            if (SerializeStream.Position + recordLength > SerializeStream.Length)
            {
                throw new EndOfStreamException("Unexpected end of record during serialization.");
            }
            Type recordType = STDFFormatterServices.ConvertTypeCode(recordTypeCode);
            ISurrogate serializerSurrogate = TypeSurrogateSelector.GetSurrogate(recordType);
            if (serializerSurrogate == null)
            {
                // no surrogate to deserialize this type.  Skip to next record and return
                SerializeStream.Seek(recordLength, SeekOrigin.Current);
                //Console.WriteLine("Skipping record type " + recordType.Name);
                return null;
            }
            ISTDFRecord record = (ISTDFRecord)STDFFormatterServices.GetUninitializedObject(recordType);
            record.RecordLength = recordLength;
            SerializationInfo info = SerializationInfo.Create(recordType, Converter);
            long startPosition = SerializeStream.Position;
            foreach (SerializationInfoEntry field in info)
            {
                EndOfRecord = (SerializeStream.Position - startPosition) >= recordLength;
                if (EndOfRecord)
                {
                    // If end of record reached yet we still have more fields to serialize, then we can skip the rest of the record.
                    break;
                }
                if (field.ItemCountIndex >= 0)
                {
                    // Field has an item count property, so we are deserializing an array.
                    // Get the number of items to deserialize from the value that was deserialized earlier.
                    int itemCount = info.GetValue<int>((int)field.ItemCountIndex);
                    if (itemCount > 0)
                    {
                        info.SetValue(field.Index, ReadArray(field.Type.GetElementType(), itemCount));
                    }
                }
                else
                {
                    info.SetValue(field.Index, Read(field.Type));
                }
            }
            EndOfStream = SerializeStream.Position >= SerializeStream.Length;
            // Set the fields on the record
            serializerSurrogate.SetObjectData(record, info);
            return record;
        }
        /// <summary>
        /// Serializes and STDF record object to the output stream.
        /// </summary>
        /// <param name="stream">Stream object to serialized the STDF data to.</param>
        /// <param name="obj">STDF record object to serialize.</param>
        public override void Serialize(Stream stream, object obj)
        {
            Buffer.SetLength(0);
            SerializeStream = Buffer;
            EndOfStream = false;
            EndOfRecord = false;
            ushort recordLength = 0;
            if (obj is ISTDFRecord record)
            {
                ISurrogate typeSurrogate = TypeSurrogateSelector.GetSurrogate(record.GetType());
                if (typeSurrogate == null)
                {
                    return;
                }
                //WriteHeader(0, record.RecordType);
                //                long recordStartPosition = SerializeStream.Position;
                Type recordType = record.GetType();
                SerializationInfo info = SerializationInfo.Create(recordType, Converter);
                typeSurrogate.GetObjectData(record, info);
                foreach (SerializationInfoEntry field in info)
                {
                    if (field.ItemCountIndex >= 0)
                    {
                        // field has an item count property, so we are serializing an array.  Get the number of items
                        // to serialize from the item count field serialized earlier (we always serialize according
                        // to the item count field rather than the array size for the array field.  Normally these are 
                        // equal but it is not mandatory that they be equal).
                        int itemCount = field.ItemCountIndex != null ? info.GetValue<int>((int)field.ItemCountIndex) : 0;

                        if (itemCount > 0)
                        {
                            WriteArray(field.Value, field.Type.GetElementType(), 0, itemCount);
                        }
                    }
                    else
                    {
                        WriteMember(field.Value, field.Type);
                    }
                    // For optional fields, save the position after the last field written
                    // that does not have a missing value.  Per spec, we can truncate any contiguous missing
                    // value fields from the end of the record.
                    if (!field.IsMissingValue)
                    {
                        recordLength = (ushort)(SerializeStream.Position);
                    }
                }
                //recordLength = (ushort)(lastValidPosition - recordStartPosition);
                if (recordLength != record.RecordLength)
                {
                    throw new Exception("Mismatched record length.");
                }
                record.RecordLength = recordLength;
                //SerializeStream.Seek(-(SerializeStream.Position - recordStartPosition + 4), SeekOrigin.Current);
                SerializeStream.Flush();
                SerializeStream = stream;
                WriteHeader(record.RecordLength, record.RecordType);
                //                SerializeStream.Seek(record.RecordLength, SeekOrigin.Current);
                //                SerializeStream.SetLength(SerializeStream.Position);
                Buffer.SetLength(recordLength);
                Buffer.WriteTo(stream);
                EndOfRecord = true;
            }
        }
        /// <summary>
        /// Reads the four (4) header bytes from the input stream.
        /// </summary>
        /// <param name="recordLength">The length of the record as a 16 bit unsigned integer.</param>
        /// <param name="recordTypeCode">The Type and Subtype codes for the current record.</param>
        protected virtual void ReadHeader(out ushort recordLength, out ushort recordTypeCode)
        {
            byte[] buffer = new byte[2];
            // Read record length bytes into buffer
            SerializeStream.Read(buffer, 0, 2);
            recordLength = Converter.ToUInt16(buffer);
            // Read record type bytes into buffer
            SerializeStream.Read(buffer, 0, 2);
            recordTypeCode = (Converter.ToUInt16(buffer));
        }
        /// <summary>
        /// Write the four (4) byte header to the output sream.
        /// </summary>
        /// <param name="recordLength">Length of the record (not including the header length) in bytes.</param>
        /// <param name="recordTypeCode">The Type and Subtype bytes for the record being serialized.</param>
        protected virtual void WriteHeader(ushort recordLength, ushort recordTypeCode)
        {
            WriteUInt16(recordLength);
            WriteUInt16(recordTypeCode);
        }
    }
}
