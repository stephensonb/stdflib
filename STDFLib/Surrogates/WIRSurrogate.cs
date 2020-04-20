using System;

namespace STDFLib
{
    /// <summary>
    /// Wafer Information Record Serialization Surrogate
    /// </summary>
    public class WIRSurrogate : Surrogate<WIR>
    {
        public override void GetObjectData(WIR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.HEAD_NUM);
            SerializeValue(1, obj.SITE_GRP);
            SerializeValue(2, obj.START_T);
            SerializeValue(3, obj.WAFER_ID);
        }

        public override void SetObjectData(WIR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.HEAD_NUM = DeserializeValue<byte>(0);
            obj.SITE_GRP = DeserializeValue<byte>(1);
            obj.START_T = DeserializeValue<DateTime>(2);
            obj.WAFER_ID = DeserializeValue<string>(3);
        }
    }
}
