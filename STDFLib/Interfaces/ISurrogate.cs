namespace STDFLib
{
    /// <summary>
    /// Generic version of ISurrogate interface.
    /// </summary>
    /// <typeparam name="T">Type of objects that the surrogate implementing this interface can provide serialization support.</typeparam>
    public interface ISurrogate<T>
    {
        /// <summary>
        /// Populates a serialization info object with data to be serialized to an output stream.
        /// </summary>
        /// <param name="obj">The object of type T being serialized</param>
        /// <param name="info">Container that holds information about each property/field to be serialized.</param>
        void GetObjectData(T obj, SerializationInfo info);
        /// <summary>
        /// Populates an object with data from the serialization info object that was deserialized from an input stream.
        /// </summary>
        /// <param name="obj">The object of type T to be populated.</param>
        /// <param name="info">Container that holds the information about each property/field the was deserialized from an input stream.</param>
        void SetObjectData(T obj, SerializationInfo info);
    }

    /// <summary>
    /// Classes implenting this interface provide support for getting/setting fields/properties of a serializable object.
    /// </summary>
    public interface ISurrogate
    {
        /// <summary>
        /// Populates a serialization info object with data to be serialized to an output stream.
        /// </summary>
        /// <param name="obj">The object being serialized</param>
        /// <param name="info">Container that holds information about each property/field to be serialized.</param>
        void GetObjectData(object obj, SerializationInfo info);
        /// <summary>
        /// Populates an object with data from the serialization info object that was deserialized from an input stream.
        /// </summary>
        /// <param name="obj">The object to be populated.</param>
        /// <param name="info">Container that holds the information about each property/field the was deserialized from an input stream.</param>
        void SetObjectData(object obj, SerializationInfo info);
    }
}
