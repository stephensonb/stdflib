using STDFLib2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace STDFConsole
{
    class Program
    {
        public static void CompareRecords(string path, ISTDFRecord[] records)
        {
            DateTime start = DateTime.Now;

            ISTDFRecord record;
            
            using Stream stream = File.OpenRead(path);

            using STDFRecordFormatter recordFormatter = new STDFRecordFormatter();

            int index = 0;
            int mismatch = 0;
            do
            {
                if (index == 40650)
                {
                    ;
                }
                record = (ISTDFRecord)recordFormatter.Deserialize(stream);
                if (record != null)
                {
                    if (record.RecordType == records[index].RecordType &&
                        record.RecordLength != records[index].RecordLength)
                    {
                        mismatch++;
                        Console.WriteLine(string.Format("Record length mismatch.  Record # {0}, Type {1}, original length = {2}, new length = {3}",
                                                        index, record.GetType().Name, records[index].RecordLength, record.RecordLength));
                    }
                    index++;
                }
            } while (!recordFormatter.EndOfStream);

            DateTime end = DateTime.Now;

            double execTime = (end - start).TotalMilliseconds;

            Console.WriteLine(string.Format("{0} records read from file in {1} milliseconds.  {2,3:P0} of records passed length comparison.", records.Length, execTime, (double)(1-mismatch/index)));
            stream.Close();
        }

        public static IEnumerable<ISTDFRecord> ReadFile(string path)
        {
            DateTime start = DateTime.Now;

            List<ISTDFRecord> Records = new List<ISTDFRecord>();
            ISTDFRecord record;

            using Stream stream = File.OpenRead(path);

            using STDFRecordFormatter recordFormatter = new STDFRecordFormatter();

            recordFormatter.Converter.SwapBytes = true;

            do
            {
                record = (ISTDFRecord)recordFormatter.Deserialize(stream);
                if (record != null)
                {
                    //Console.WriteLine(record.ToString());
                    Records.Add(record);
                }
            } while (!recordFormatter.EndOfStream);

            DateTime end = DateTime.Now;

            double execTime = (end - start).TotalMilliseconds;

            Console.WriteLine(string.Format("{0} records read from file in {1} milliseconds.", Records.Count, execTime));

            stream.Close();

            return Records;
        }

        public static void WriteFile(string path, IEnumerable<ISTDFRecord> records)
        {
            DateTime start = DateTime.Now;

            using Stream stream = File.OpenWrite(path);

            using STDFRecordFormatter recordFormatter = new STDFRecordFormatter();

            int count = 0;

            foreach (var record in records)
            {
                if (count == 10204)
                {
                    ;
                }
                recordFormatter.Serialize(stream, record);
                count++;
            }

            DateTime end = DateTime.Now;

            double execTime = (end - start).TotalMilliseconds;

            Console.WriteLine(string.Format("{0} records written to file in {1} milliseconds.", count, execTime));

            stream.Close();
        }

        static void Main(string[] args)
        {
            string inPath = "C:\\Users\\brste\\source\\repos\\STDFDemo\\lot3.stdf";
            string outPath = "C:\\Users\\brste\\source\\repos\\STDFDemo\\test-out.stdf";
            IEnumerable<ISTDFRecord> records = ReadFile(inPath);
            WriteFile(outPath, records);
            CompareRecords(outPath, records.ToArray());
        }
    }
}
