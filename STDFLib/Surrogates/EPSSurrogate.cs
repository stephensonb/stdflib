namespace STDFLib
{
    /// <summary>
    /// End Program Sequence Serialization Surrogate
    /// </summary>
    public class EPSSurrogate : Surrogate<EPS>
    {
        public override void GetObjectData(EPS obj, SerializationInfo info)
        {
            // no implemention needed
        }

        public override void SetObjectData(EPS obj, SerializationInfo info)
        {
            // no implementation needed.
        }
    }
}
