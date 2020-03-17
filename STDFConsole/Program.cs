using System;
using STDFLib;

namespace STDFConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\brste\\source\\repos\\STDFDemo\\a595.stdf";

            STDFReader reader = new STDFReader(path);
            STDFRecord record;

            while(!reader.EOF)
            {
                record = reader.ReadRecord();
                Console.WriteLine(string.Format("Record type: {0:x2}  Record Subtype: {1:x2}", record.REC_TYP, record.REC_SUB));
            }
        }
    }
}
