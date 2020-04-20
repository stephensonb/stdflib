using Microsoft.VisualStudio.TestTools.UnitTesting;
using STDFLib;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace STDFLibUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public int CompareRecords(string path, IEnumerable<ISTDFRecord> records)
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
                    if (record.RecordType == records.ElementAt(index).RecordType &&
                        record.RecordLength != records.ElementAt(index).RecordLength)
                    {
                        mismatch++;
                        Console.WriteLine(string.Format("Record length mismatch.  Record # {0}, Type {1}, original length = {2}, new length = {3}",
                                                        index, record.GetType().Name, records.ElementAt(index).RecordLength, record.RecordLength));
                    }
                    index++;
                }
            } while (!recordFormatter.EndOfStream);

            DateTime end = DateTime.Now;

            double execTime = (end - start).TotalMilliseconds;

            Console.WriteLine(string.Format("{0} records read from file in {1} milliseconds.  {2,3:P0} of records passed length comparison.", records.Count(), execTime, (double)(1 - mismatch / index)));
            stream.Close();

            return mismatch;
        }

        public IEnumerable<ISTDFRecord> ReadFile(string path)
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

        public int WriteFile(string path, IEnumerable<ISTDFRecord> records)
        {
            DateTime start = DateTime.Now;

            using Stream stream = File.Open(path, FileMode.Create, FileAccess.Write);

            using STDFRecordFormatter recordFormatter = new STDFRecordFormatter();

            int count = 0;
            foreach (var record in records)
            {
                recordFormatter.Serialize(stream, record);
                count++;
            }

            DateTime end = DateTime.Now;

            double execTime = (end - start).TotalMilliseconds;

            Console.WriteLine(string.Format("{0} records written to file in {1} milliseconds.", count, execTime));

            stream.Close();

            return count;
        }

        [TestMethod]
        public void Test_Read_Input_File_Record_Count()
        {
            string filePath = "..\\..\\..\\..\\test\\a595.stdf";
            IEnumerable<ISTDFRecord> records = ReadFile(filePath);
            Assert.IsTrue(records.Count() == 40651);

            filePath = "..\\..\\..\\..\\test\\lot2.stdf";
            records = ReadFile(filePath);
            Assert.IsTrue(records.Count() == 58020);
           

            filePath = "..\\..\\..\\..\\test\\lot3.stdf";
            records = ReadFile(filePath);
            Assert.IsTrue(records.Count() == 59890);
        }

        [TestMethod]
        public void Write_Output_Files()
        {
            string filePath = "..\\..\\..\\..\\test\\a595.stdf";
            string outPath = "..\\..\\..\\..\\test\\a595-rewrite.stdf";
            IEnumerable<ISTDFRecord> records = ReadFile(filePath);
            int writeCount = WriteFile(outPath, records);
            Assert.IsTrue(writeCount == 40651);

            filePath = "..\\..\\..\\..\\test\\lot2.stdf";
            outPath = "..\\..\\..\\..\\test\\lot2-rewrite.stdf";
            records = ReadFile(filePath);
            writeCount = WriteFile(outPath, records);
            Assert.IsTrue(writeCount == 58020);


            filePath = "..\\..\\..\\..\\test\\lot3.stdf";
            outPath = "..\\..\\..\\..\\test\\lot3-rewrite.stdf";
            records = ReadFile(filePath);
            writeCount = WriteFile(outPath, records);
            Assert.IsTrue(writeCount == 59890);
        }

        [TestMethod]
        public void Compare_Original_And_Rewritten_Files()
        {
            string filePath = "..\\..\\..\\..\\test\\a595.stdf";
            string outPath = "..\\..\\..\\..\\test\\a595-rewrite.stdf";
            IEnumerable<ISTDFRecord> records = ReadFile(filePath);
            WriteFile(outPath, records);
            int mismatchCount = CompareRecords(outPath, records);
            Assert.IsTrue(mismatchCount == 0);

            filePath = "..\\..\\..\\..\\test\\lot2.stdf";
            outPath = "..\\..\\..\\..\\test\\lot2-rewrite.stdf";
            records = ReadFile(filePath);
            WriteFile(outPath, records);
            mismatchCount = CompareRecords(outPath, records);
            Assert.IsTrue(mismatchCount == 0);


            filePath = "..\\..\\..\\..\\test\\lot3.stdf";
            outPath = "..\\..\\..\\..\\test\\lot3-rewrite.stdf";
            records = ReadFile(filePath);
            WriteFile(outPath, records);
            mismatchCount = CompareRecords(outPath, records);
            Assert.IsTrue(mismatchCount == 0);
        }
    }
}
