using System;
using System.IO;
using STDFLib;

namespace STDFLib
{
    /// <summary>
    /// Generic Data Record Serializer - Serializes Generic Data Record (GDR) to/from byte representation for reading/writing to STDFDataStream.
    /// </summary>
    public class GenericDataRecordSerializerV4 
    { 
        // Reads Variable data from the STDF stream.  First two bytes represent the number fields to read.  Each field contains:
        // 
        // For field aligned to start on an even byte
        //      Data Type: Byte 0 
        //      For all data types except integer/real data types:
        //      Field Data: Bytes 1 through n 
        //      For integer/real data types:
        //      Byte 1: Pad Byte (0x00)
        //      Byte 2-n: Field data
        //
        // For fields aligned to start on an odd byte
        //      Data Type: Byte 0 
        //      Byte 1-n: Field data (first byte will start on an even byte in the stream, so no need to add padding to align)
        //
        public static ISTDFRecord Deserialize(ISTDFReader reader)
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
            string fieldDataType = "";

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

                    // actual field type was on an odd byte
                    evenByte = false;
                }

                fieldDataType = GDR.GetFieldDataType(fieldTypeCode)?.Name ?? "";

                // The type code was on an even byte.  Check to make sure the field data type does not need to
                // begin on an even byte.  If it does, then we have a problem and need to throw an exception
                if (evenByte)
                {
                    switch (fieldDataType)
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
                if (fieldDataType == "")
                {
                    throw new STDFFormatException(string.Format("Invalid file format or unsupported Generic Record field data type found in data stream.  Field data type code '{0}' found at position {1}", fieldTypeCode, reader.Position));
                }

                if (fieldValue != null)
                {
                    // Add the field to the generic data record
                    gdrRecord.Add(new GenericRecordDataField() { FieldType = fieldTypeCode, Value = fieldValue });
                }
            }

            // Return the generic data record
            return gdrRecord;
        }

        /*
        public static void Serialize(STDFDataStream stream, ISTDFRecord record) 
        {
            GDR gdrRecord = record as GDR;

            // If the record type is not a GDR record, then throw an exception
            if (gdrRecord == null)
            {
                throw new ArgumentOutOfRangeException(string.Format("Unsupported ISTDFRecord type passed to Serialize method.  Expecting 'GDR' record type, received {0}", record.GetType().Name));
            }

            // holds the record length - number of bytes in the record (excluding the header bytes)
            long recordLength = 0;

            // save the start position of the first byte of the record for tracking byte alignment
            long startPosition = 0;

            // flag indicating if we are on an odd or even byte
            bool evenByte = true;

            // holds the current field data type name
            string fieldDataType = "";

            // Create a new STDF stream buffer based on a memory stream to hold the serialized data before writing it to the real stream.
            using (STDFDataStream buffer = new STDFDataStream(new MemoryStream(256), stream.ByteConverter, stream.Encoding, stream.BooleanCoding, stream.DateTimeCoding, stream.StringCoding))
            {
                // Write out the header
                buffer.WriteUInt16(0);              // REC_LEN
                buffer.Write(gdrRecord.REC_TYP);    // REC_TYPE
                buffer.Write(gdrRecord.REC_SUB);    // REC_SUB

                // Write out the field count
                stream.WriteUInt16(gdrRecord.FLD_CNT);

                // Return if there is no field data in the generic gdrRecord.
                if (gdrRecord.FLD_CNT <= 0) return;

                // Now write out each of the fields in the record.
                foreach (GenericRecordDataField field in gdrRecord.FieldData)
                {
                    // Get the type name for the field
                    fieldDataType = GDR.GetFieldDataType(field.FieldType)?.Name ?? "";

                    // Set evenByte flag to true if we are positioned on an even byte in the stream
                    evenByte = ((stream.Position - startPosition) % 2) == 0;

                    // If we are on an even byte and the data type to be written next is required to start on an even byte, 
                    // then we must write a pad byte since writing the type code byte will cause the field data to start on an odd byte.
                    if (evenByte)
                    {
                        switch (fieldDataType)
                        {
                            // These types must start on an even byte alignment.  If the field type is one of these then we 
                            // need to write out a padding byte before writing the type code.
                            case "Int16":
                            case "Int32":
                            case "UInt16":
                            case "UInt32":
                            case "Single":
                            case "Double":
                                stream.WriteByte(0);
                                break;
                        }
                    }

                    // Now write out the field type code byte
                    stream.WriteByte(field.FieldType);

                    // Write out the field value
                    stream.WriteSTDFValue(field.Value);
                }

                // Calculate the record length.  Record length is the buffer position pointer after serialization minus 4 bytes for the header
                // (2 bytes for record length, one byte for record type and one byte for record subtype)
                recordLength = (ushort)(buffer.Position - 4);

                // Seek back to the start of the buffer
                buffer.Seek(0, SeekOrigin.Begin);

                // Now write the record length to the buffer
                buffer.Write(recordLength);

                // Make sure all data is written to the memory stream buffer.
                buffer.Flush();

                // Seek back to the start of the buffer
                buffer.Seek(0, SeekOrigin.Begin);

                // Now write the buffer to the real data stream.  Note, the buffer will be copied starting at the current position of the To stream.
                buffer.CopyTo(stream);
            }
        }
        */
    }
}

