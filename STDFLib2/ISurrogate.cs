namespace STDFLib2
{
    public interface ISurrogate<T>
    {
        void GetObjectData(T obj, SerializationInfo info);
        void SetObjectData(T obj, SerializationInfo info);
    }

    public interface ISurrogate
    {
        void GetObjectData(object obj, SerializationInfo info);
        void SetObjectData(object obj, SerializationInfo info);
    }
}
