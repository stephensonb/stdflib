using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections;

namespace STDFLib
{
    /// <summary>
    /// Encapsulates STDF file records and related functions
    /// </summary>
    public class STDFFileV4 : ISTDFFile
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
        public ISTDFFile ReadFile(string path)
        {
            Records.Clear();

            STDFBinaryReader reader = new STDFBinaryReader(path);
            STDFSerializerV4 serializer = new STDFSerializerV4();

            while(true)
            {
                try
                {
                    ISTDFRecord record = STDFSerializerV4.Deserialize(reader);
                    if (record != null)
                    {
                        Records.Add(record);
                    }
                } catch(EndOfStreamException)
                {
                    break;
                }
            }

            Console.SetOut(new StreamWriter("testOut.txt", false));

            // Print out the contents of the file read in for testing.
            foreach (var record in Records)
            {
                Console.WriteLine(FormatRecord(serializer, record));
            }

            Console.Out.Close();
            Console.OpenStandardOutput();

            return this;
        }

        public void WriteFile(string path)
        {

        }

        // Pretty (maybe) formatting of the record contents for debugging
        public string FormatRecord(STDFSerializerV4 serializer, ISTDFRecord record)
        {
            var props = STDFSerializerV4.GetSerializeableProperties(record);

            StringBuilder sb = new StringBuilder();

            sb.Append("--------------------------------------------------------\n");
            sb.AppendFormat("{0,25}:{1,-30}\n", "RECORD", this.GetType().Name);
            sb.Append("========================= ==============================\n");

            foreach (var prop in props)
            {
                object propValue = prop.GetValue(record);

                if (propValue == null)
                {
                    sb.AppendFormat("{0,25}:{1,-30}\n", prop.Name, "Null");
                    continue;
                }

                if (prop.PropertyType.Name.EndsWith("[]") || prop.PropertyType.Name.StartsWith("List`1") && propValue != null)
                {
                    int count = 0;
                    sb.AppendFormat("{0,25}:LIST/ARRAY OF {1} ",
                        prop.Name,
                        propValue.GetType().Name.EndsWith("[]") ? propValue.GetType().GetElementType().Name : propValue.GetType().GetGenericArguments()[0].Name);

                    IEnumerator pv = ((IEnumerable)propValue)?.GetEnumerator();

                    pv.Reset();

                    while (pv.MoveNext())
                    {
                        if (count == 0)
                        {
                            sb.Append("\n");
                        }
                        sb.AppendFormat("{0,-55}\n", pv.Current.ToString());
                        count++;
                    }

                    if (count == 0)
                    {
                        sb.Append("(*EMPTY)\n");
                    }
                }
                else
                {
                    sb.AppendFormat("{0,25}:{1,-30}\n", prop.Name, propValue?.ToString());
                }
            }

            sb.AppendLine();

            return sb.ToString();
        }
    }
}

