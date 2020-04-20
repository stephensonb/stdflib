namespace STDFLib2
{
    public class DefaultRecordDataProvider : ISurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info)
        {
            if (obj is ISTDFRecord record)
            {
                var data = STDFFormatterServices.GetObjectData(obj);
                info.SetValues(data);
            }
        }

        public void SetObjectData(object obj, SerializationInfo info)
        {
            STDFFormatterServices.PopulateObject(obj, info);
        }
    }
}
