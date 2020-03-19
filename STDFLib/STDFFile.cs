using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace STDFLib
{ 
// Example usage - Create new STDF File
// STDFFile sf = new STDFFile("myfile.stdf");
//
// sf.SetupTime = DateTime.Now();
// sf.StartTime = DateTime.Now();
// 
    public abstract class STDFFile : ISTDFFile
    {
        protected List<ISTDFRecord> Records { get; private set; } = new List<ISTDFRecord>();

        // Required order of the records in an STDFFile
        public FAR FileAttributes { get; set; } = null;
        public List<ATR> AuditTrails { get; set; } = new List<ATR>();
        public MIR MasterInformation { get; set; } = null;
        public RDR RetestData { get; set; } = null;
        public List<SDR> SiteDescriptions { get; set; } = new List<SDR>();
        public WCR WaferConfiguration { get; set; } = null;
        public List<PCR> PartCounts { get; set; } = new List<PCR>();
        public List<HBR> HardBins { get; set; } = new List<HBR>();
        public List<SBR> SoftwareBins { get; set; } = new List<SBR>();
        public List<PMR> PinMaps { get; set; } = new List<PMR>();
        public List<PGR> PinGroups { get; set; } = new List<PGR>();
        public List<PLR> PinLists { get; set; } = new List<PLR>();
        public List<WIR> WaferInformation { get; set; } = new List<WIR>();
        public List<TSR> TestSynopsis { get; set; } = new List<TSR>();
        public List<GDR> GenericData { get; set; } = new List<GDR>();
        public List<DTR> DatalogText { get; set; } = new List<DTR>();
        public MRR MasterResults { get; set; } = null;

        /// <summary>
        /// Opens, parses and reads into memory an entire STDF file
        /// </summary>
        /// <param name="pathName"></param>
        /// <summary>
        /// Opens, parses and reads into memory an entire STDF file
        /// </summary>
        /// <param name="pathName"></param>
        public ISTDFFile ReadFile(ISTDFReader reader)
        {
            Records.Clear();

            // set reader position to start of file
            reader.Rewind();

            while (!reader.EOF)
            {
                // read the header for the next record;
                reader.ReadHeader();
                ISTDFRecord nextRecord = DeserializeRecord(reader);
                if (nextRecord == null) break;
                Records.Add(nextRecord);
            }

            Console.SetOut(new StreamWriter("testOut.txt", false));

            // Print out the contents of the file read in for testing.
            foreach (var record in Records)
            {
                Console.WriteLine(record.ToString());
            }

            return this;
        }

        public abstract ISTDFRecord CreateRecord(RecordType recordType);
        public abstract ISTDFRecord DeserializeRecord(ISTDFReader reader);

        public PropertyInfo[] GetSerializeableProperties(ISTDFRecord record)
        {
            // Get the list of properties defined by the record type, then filter by
            // those that are decorated with the STDF attribute.  
            // Properties will be returned sorted based on the Order property of the STDFAttribute
            return record.GetType().GetProperties().Where(x => x.GetCustomAttributes<STDFAttribute>().Count() > 0).OrderBy(x => x.GetCustomAttribute<STDFAttribute>().Order).ToArray();
        }

        /*

        public T[] GetResults<T>(WIR forWafer) where T : ISTDFRecord
        {
            int i = Records.IndexOf(forWafer);

            byte[] siteNumbers = Records.OfType<SDR>().Where(x => x.SITE_GRP == forWafer.SITE_GRP).Single().SITE_NUM.ToArray();

            List<T> ptrs = new List<T>();

            if (!(new string[] { "PTR", "PMR", "FTR" }).Contains(typeof(T).GetType().Name))
            {
                return ptrs.ToArray();
            }

            if (i >= 0)
            {
                for (int j = i; j < Records.Count; j++)
                {
                    // Scan until we find the part result record that matches the head from the WIR and has a site specified in the WIR site group
                    if (Records[j] is PRR)
                    {
                        if (((PRR)Records[j]).HEAD_NUM == forWafer.HEAD_NUM && siteNumbers.Contains(((PRR)Records[j]).SITE_NUM))
                        {
                            break;
                        }
                    }
                    // If this is the correct record, then add it to the result list
                    if (Records[j] is T)
                    {
                        ptrs.Add((T)Records[j]);
                    }
                }
            }
            return ptrs.ToArray();
        }

        public T[] GetResults<T>(PIR forPart) where T : ISTDFRecord
        {
            int i = Records.IndexOf(forPart);
            List<T> ptrs = new List<T>();

            if (!(new string[] { "PTR", "PMR", "FTR" }).Contains(typeof(T).GetType().Name))
            {
                return ptrs.ToArray();
            }

            if (i >= 0)
            {
                for (int j = i; j < Records.Count; j++)
                {
                    // Scan until we find the part result record that matches the head and site number
                    if (Records[j] is PRR)
                    {
                        if (((PRR)Records[j]).HEAD_NUM == forPart.HEAD_NUM && ((PRR)Records[j]).SITE_NUM == forPart.SITE_NUM)
                        {
                            break;
                        }
                    }
                    // If this is a PTR record, then add it to the result list
                    if (Records[j] is T)
                    {
                        ptrs.Add((T)Records[j]);
                    }
                }
            }
            return ptrs.ToArray();
        }

        public bool IsValid
        {
            get
            {
                return true;
            }
        }

        public bool IsInitialSequenceValid
        {
            get
            {
                if (Records.OfType<FAR>().Count() == 1       // Has exactly one FAR record
                     && Records.OfType<MIR>().Count() == 1)  // Has exactly one MIR record
                {
                    return true;
                }
                return false;
            }
        }
        */
    }
}
