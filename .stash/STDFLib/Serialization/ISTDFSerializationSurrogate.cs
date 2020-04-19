namespace STDFLib2.Serialization
{
    public interface ISTDFSerializationSurrogate
    {
        void GetObjectData(object obj, STDFSerializationInfo info);
        void SetObjectData(object obj, STDFSerializationInfo info, ISTDFSurrogateSelector selector);
    }
}
