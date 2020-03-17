using System;
using System.Collections.Generic;
using System.IO;

namespace STDFLib
{
    /// <summary>
    /// Encapsulates STDF file records and related functions
    /// </summary>
    public class STDFFileV4 : STDFFile
    {
        public STDFFileV4()
        {
            RecordSerializer = new STDFRecordSerializerV4();
        }

        /// <summary>
        /// Opens, parses and reads into memory an entire STDF file
        /// </summary>
        /// <param name="pathName"></param>
        public override void ReadFile(string pathName)
        {
            using STDFReader reader = new STDFReader(pathName);
            
            Records.Clear();

            while (!reader.EOF)
            {
                ISTDFRecord nextRecord = RecordSerializer.Deserialize(reader);
                if (nextRecord == null) break;
                Records.Add(nextRecord);
                reader.SeekNextRecord();
            }

            Console.SetOut(new StreamWriter("testOut.txt",false));

            // Print out the contents of the file read in for testing.
            foreach(var record in Records)
            {
                Console.WriteLine(record.ToString());
            }
        }

        public static ISTDFRecord CreateRecord(RecordType recordType)
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
    }
}

