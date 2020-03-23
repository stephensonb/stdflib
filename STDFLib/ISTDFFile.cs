using System.Reflection;

namespace STDFLib
{
    public interface ISTDFFile
    {
        ISTDFFile ReadFile(string path);
        void WriteFile(string path);
    }
}

