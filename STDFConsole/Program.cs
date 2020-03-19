using System;
using STDFLib;

namespace STDFConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\brste\\source\\repos\\STDFDemo\\a595.stdf";

            STDFFileV4 newFile = (STDFFileV4)ISTDFFile.ReadFile(path);
        }
    }
}
