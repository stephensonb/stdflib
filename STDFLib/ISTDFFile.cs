using System;

namespace STDFLib
{
    public interface ISTDFFile
    {
        void ReadFile(string pathName);
        ISTDFRecord CreateRecord(RecordType recordType);
    }
}

