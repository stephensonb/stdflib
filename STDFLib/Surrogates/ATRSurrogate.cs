using System;

namespace STDFLib
{
    /// <summary>
    /// Audit Trail Record Serialization Surrogate
    /// </summary>
    public class ATRSurrogate : Surrogate<ATR>
    {
        public override void GetObjectData(ATR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.MOD_TIM);
            SerializeValue(1, obj.CMD_LINE);
        }

        public override void SetObjectData(ATR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.MOD_TIM = DeserializeValue<DateTime>(0);
            obj.CMD_LINE = DeserializeValue<string>(1);
        }
    }
}
