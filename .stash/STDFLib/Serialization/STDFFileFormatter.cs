using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace STDFLib2.Serialization
{
    public class STDFFileFormatter 
    {
        public STDFFileFormatter() { }

        public MemoryStream Buffer { get; protected set; }
        public ISTDFSurrogateSelector SurrogateSelector { get; set; }
        protected ISTDFFormatterConverter Converter { get; set; }
        protected Stream SerializationStream { get; set; }

        protected void ReadHeader(Stream stream, out ushort recordLength, out RecordTypes recordType)
        {
            byte[] buffer = new byte[2];

            // Read record length bytes into buffer
            stream.Read(buffer, 0, 2);
            recordLength = Converter.ToUInt16(buffer);

            // Read record type bytes into buffer
            stream.Read(buffer, 0, 2);
            recordType = (RecordTypes)(Converter.ToUInt16(buffer));
        }

        protected void WriteHeader(ushort recordLength, Type recordType)
        {
            RecordTypes type = STDFFormatterServices.ConvertToRecordType(recordType);
            WriteUInt16(recordLength);
            WriteUInt16((ushort)type);
        }

        public object Deserialize(Stream stream)
        {
            List<ISTDFRecord> records = new List<ISTDFRecord>();

            // use a memory stream as a serialization buffer for record deserialization (length is max record length
            // for an STDF file).
            Buffer = new MemoryStream(ushort.MaxValue);

            // Create a reader to do formatted reads from the buffer;
            STDFBinaryReader reader = new STDFBinaryReader(Buffer);

            // Create the binary formatter/type converter
            Converter = new STDFFormatterConverter();

            // Determine endianness of file
            ReadHeader(stream, out ushort recordLength, out RecordTypes recordType);

            // length of first record of file should be 2 bytes.  If length read is 512, then bytes are reversed for 
            // the current architecture.  Need to swap the bytes.
            if (recordLength == 512)
            {
                Converter.SwapBytes = true;
                recordLength = 2;
            }

            // First record must be a FAR record type and the record length must be 2 bytes
            if (recordType != RecordTypes.FAR || recordLength != 2)
            {
                // if not, throw and invalid format exception
                throw new STDFFormatException("File is not a valid STDF format file.");
            }

            // Rewind to start of input file
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                try
                {
                    // Reset the serialization buffer
                    reader.BaseStream.SetLength(0);

                    // Read the record header
                    ReadHeader(stream, out recordLength, out recordType);

                    // Create the serialization info store to hold property values read from the stream
                    var info = STDFSerializationInfo.Create(STDFFormatterServices.GetTypeFromRecordType(recordType), Converter);

                    // Create the new record to hold the deserialized data
                    ISTDFRecord record = STDFFormatterServices.CreateSTDFRecord(info.Type);

                    // Get the surrogate that knows how to deserialize the data for the current record type
                    var surrogate = SurrogateSelector.GetSurrogate(info.Type, out ISTDFSurrogateSelector selector);

                    // read the bytes of the record in the input stream into the serialization buffer
                    Buffer.Write(reader.ReadBytes(recordLength));

                    // Populate the serialization info from the binary data in the buffer
                    DeserializeRecord(reader, info);

                    // Set the property values for the new record.
                    surrogate.SetObjectData(record, info, selector);

                    if (record != null)
                    {
                        // Add to the internal record list
                        records.Add(record);
                    }
                }
                catch (EndOfStreamException)
                {
                    break;
                }
            }
            return records;
        }

        protected virtual void DeserializeRecord(STDFBinaryReader reader, STDFSerializationInfo info)
        {
            bool EndOfRecord;
            string itemCountProperty;

            // walk through the serializeable properties to decode the byte stream into appropriate
            // data types and save to the serialization store
            foreach (var prop in STDFFormatterServices.GetSerializeableProperties(info.Type))
            {
                // Check if we are at end of record
                EndOfRecord = (reader.BaseStream.Position >= reader.BaseStream.Length);

                if (EndOfRecord)
                {
                    // current property is optional, so just continue to next property
                    continue;
                }

                itemCountProperty = prop.GetCustomAttribute<STDFAttribute>().ItemCountProperty;

                if (prop.PropertyType.IsArray)
                {
                    // Handle arrays
                    int length = itemCountProperty != null ? info.GetInt32(itemCountProperty) : 0;

                    if (length > 0)
                    {
                        var array = Array.CreateInstance(prop.PropertyType.GetElementType(), length);
                        info.AddValue(prop.Name, reader.Read(array, 0, length));
                    }
                }
                else
                {
                    info.AddValue(prop.Name, reader.Read(prop.PropertyType));
                }
            }
        }

        public void Serialize(Stream stream, object obj)
        {
            if (obj is ISTDFFile file)
            {
                BeginStreamingSerialization(stream);

                foreach (var record in file.Records)
                {
                    try
                    {
                        if (record != null)
                        {
                            SerializeRecord(record);
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        break;
                    }
                }

                EndStreamingSerialization();
            }
        }

        public void BeginStreamingSerialization(Stream stream)
        {
            if (SerializationStream != null)
            {
                throw new InvalidOperationException("Streaming already in progress.  Cannot begin another streaming session with this formatter");
            }
            SerializationStream = stream;
            Converter = new STDFFormatterConverter();
            Buffer.SetLength(0);
        }

        public void EndStreamingSerialization()
        {
            SerializationStream.Flush();
            SerializationStream.Close();
            SerializationStream.Dispose();
            SerializationStream = null;
        }

        public virtual void SerializeRecord(ISTDFRecord record)
        {
            if (SerializationStream == null)
            {
                throw new InvalidOperationException("Must call BeginStreamingSerialization before serializing data.");
            }

            Buffer.SetLength(0);

            ushort recordLength = 0;

            // create the serialization store for this record
            var info = STDFSerializationInfo.Create(record.GetType(), Converter);

            // get the surrogate object that will serialize this record
            var surrogate = SurrogateSelector.GetSurrogate(record.GetType(), out ISTDFSurrogateSelector selector);

            // get the data to serialize from the record
            surrogate.GetObjectData(record, info);

            // calculate record length and copy record bytes to buffer
            foreach (var prop in info)
            {
                if (prop.Value is byte[] values)
                {
                    recordLength += (ushort)values.Length;
                    WriteMember(values);
                }
            }

            // Write the header bytes to the output stream
            WriteHeader((ushort)Buffer.Length, info.Type);

            // now write the buffer to the output stream
            SerializationStream.Write(Buffer.ToArray(), 0, (int)Buffer.Length);
        }

        protected void WriteArray(object obj, Type memberType)
        {
            if (obj is Array values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    WriteMember(Converter.GetBytes(values.GetValue(i)));
                }
            }
        }
        protected void WriteBoolean(bool val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteByte(byte val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteChar(char val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteDateTime(DateTime val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteDecimal(decimal val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteDouble(double val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteInt16(short val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteInt32(int val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteInt64(long val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteSByte(sbyte val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteSingle(float val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteTimeSpan(TimeSpan val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteUInt16(ushort val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteUInt32(uint val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteUInt64(ulong val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteString(string val) => Buffer.Write(Converter.GetBytes(val));
        protected void WriteValueType(object obj, Type memberType) => Buffer.Write(Converter.GetBytes(obj));
        protected virtual void WriteMember(object data)
        {
            switch(data)
            {
                case Array values: WriteArray(values, data.GetType().GetElementType()); break;
                case bool val: WriteBoolean(val); break;
                case byte val: WriteByte(val); break;
                case char val: WriteChar(val); break;
                case DateTime val:  WriteDateTime(val); break;
                case double val:  WriteDouble(val); break;
                case short val:  WriteInt16(val); break;
                case int val:  WriteInt32(val); break;
                case sbyte val: WriteSByte(val); break;
                case float val: WriteSingle(val); break;
                case ushort val: WriteUInt16(val); break;
                case uint val: WriteUInt32(val); break;
                case string val: WriteString(val); break;
            };
        }
    }
}
