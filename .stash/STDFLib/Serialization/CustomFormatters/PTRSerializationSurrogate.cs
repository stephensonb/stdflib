using System.Reflection;

namespace STDFLib2.Serialization
{
    public class PTRSerializationSurrogate : STDFTestRecordSerializationSurrogate
    {
        public PTRSerializationSurrogate(ISTDFFormatterConverter converter) : base(converter) { }

        protected override void SerializeProperty(STDFSerializationContext context, STDFSerializationInfoEntry propValue)
        {
            if (context.Record is PTR ptr)
            {
                // Handle optional data
                switch (propValue.BaseProperty.Name)
                {
                    case "OPT_FLAG":
                        // set the record length to exclude the optional flag - optional flag can be excluded from
                        // serialization if it is the last field in the record (all other optional data is invalid/missing).
                        // if an optional property has a valid value, the record length will be reset later and the 
                        // optional flag field will be included in the record.
                        RecordLength = (ushort)(context.Writer.Position - RecordStartStreamPosition);
                        if (ptr.OPT_FLAG == 0)
                        {
                            ptr.OPT_FLAG = (byte)PTROptionalData.AllOptionalDataValid;
                            ptr.OPT_FLAG |= (byte)(ptr.RES_SCAL == null ? PTROptionalData.RES_SCAL_Invalid : 0);
                            ptr.OPT_FLAG |= (byte)(ptr.LLM_SCAL == null ? PTROptionalData.LO_LIMIT_LLM_SCAL_Invalid : 0);
                            ptr.OPT_FLAG |= (byte)(ptr.HLM_SCAL == null ? PTROptionalData.HI_LIMIT_HLM_SCAL_Invalid : 0);
                            ptr.OPT_FLAG |= (byte)(ptr.LO_LIMIT == null ? PTROptionalData.LO_LIMIT_LLM_SCAL_Invalid | PTROptionalData.NoLoLimitThisTest : 0);
                            ptr.OPT_FLAG |= (byte)(ptr.HI_LIMIT == null ? PTROptionalData.HI_LIMIT_HLM_SCAL_Invalid | PTROptionalData.NoHiLimitThisTest : 0);
                            ptr.OPT_FLAG |= (byte)(ptr.LO_SPEC == null ? PTROptionalData.NoLoLimitSpec : 0);
                            ptr.OPT_FLAG |= (byte)(ptr.HI_SPEC == null ? PTROptionalData.NoHiLimitSpec : 0);
                        }
                        context.Writer.Write((byte)ptr.OPT_FLAG);
                        return;
                    case "RES_SCAL":
                        if ((ptr.OPT_FLAG & (byte)PTROptionalData.RES_SCAL_Invalid) > 0)
                        {
                            context.Writer.Write((byte)0);
                            return;
                        }
                        break;
                    case "LLM_SCAL":
                        if ((ptr.OPT_FLAG & (byte)(PTROptionalData.LO_LIMIT_LLM_SCAL_Invalid | PTROptionalData.NoLoLimitThisTest)) > 0)
                        {
                            context.Writer.Write((byte)0);
                            return;
                        }
                        break;
                    case "HLM_SCAL":
                        if ((ptr.OPT_FLAG & (byte)(PTROptionalData.HI_LIMIT_HLM_SCAL_Invalid | PTROptionalData.NoHiLimitThisTest)) > 0)
                        {
                            context.Writer.Write((byte)0);
                            return;
                        }
                        break;
                    case "LO_LIMIT":
                        if ((ptr.OPT_FLAG & (byte)(PTROptionalData.LO_LIMIT_LLM_SCAL_Invalid | PTROptionalData.NoLoLimitThisTest)) > 0)
                        {
                            context.Writer.Write(0F);
                            return;
                        }
                        break;
                    case "HI_LIMIT":
                        if ((ptr.OPT_FLAG & (byte)(PTROptionalData.HI_LIMIT_HLM_SCAL_Invalid | PTROptionalData.NoHiLimitThisTest)) > 0)
                        {
                            context.Writer.Write(0F);
                            return;
                        }
                        break;
                    case "LO_SPEC":
                        if ((ptr.OPT_FLAG & (byte)PTROptionalData.NoLoLimitSpec) > 0)
                        {
                            context.Writer.Write(0F);
                            return;
                        }
                        break;
                    case "HI_SPEC":
                        if ((ptr.OPT_FLAG & (byte)PTROptionalData.NoHiLimitSpec) > 0)
                        {
                            context.Writer.Write(0F);
                            return;
                        }
                        break;
                }
                base.SerializeProperty(context, propValue);
            }
        }
    }
}
