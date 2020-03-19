using System.Reflection;

namespace STDFLib
{
    public interface ISTDFFile
    {
        ISTDFFile ReadFile(ISTDFReader reader);
        ISTDFRecord CreateRecord(RecordType recordType);
        ISTDFRecord DeserializeRecord(ISTDFReader reader);
        PropertyInfo[] GetSerializeableProperties(ISTDFRecord record);

        /// <summary>
        /// Opens, parses and reads into memory an entire STDF file
        /// </summary>
        /// <param name="pathName"></param>
        /// <summary>
        /// Opens, parses and reads into memory an entire STDF file
        /// </summary>
        /// <param name="pathName"></param>
        static ISTDFFile ReadFile(string path)
        {
            using ISTDFReader reader = new STDFReader(path);

            switch (reader.STDF_VER)
            {
                case STDFVersions.STDFVer4:
                    STDFFileV4 stdf = new STDFFileV4();
                    return stdf.ReadFile(reader);
                default:
                    throw new STDFFormatException(string.Format("Cannot process file version.  Version found is {0}", reader.STDF_VER));
            }
        }

    }
}

