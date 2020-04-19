using System.Reflection;

namespace STDFLib2.Serialization
{
    public class MPRSerializationSurrogate : STDFTestRecordSerializationSurrogate
    {
        public MPRSerializationSurrogate(ISTDFFormatterConverter converter) : base(converter) { }

        protected override void SerializeProperty(STDFSerializationContext context, STDFSerializationInfoEntry propValue)
        {
            if (context.Record is MPR mpr)
            {
                // if we do not need to save optional data for the record, skip serializing remaining fields
                switch (propValue.BaseProperty.Name)
                {
                    case "OPT_FLAG":
                        RecordLength = (ushort)(context.Writer.Position - RecordStartStreamPosition);
                        if (mpr.OPT_FLAG == 0)
                        {
                            mpr.OPT_FLAG |= (byte)(mpr.RES_SCAL == null ? MPROptionalData.RES_SCAL_Invalid : 0);
                            mpr.OPT_FLAG |= (byte)(mpr.START_IN == null || mpr.INCR_IN == null ? MPROptionalData.START_IN_INCR_IN_Invalid : 0);
                            mpr.OPT_FLAG |= (byte)(mpr.LLM_SCAL == null ? MPROptionalData.LO_LIMIT_LLM_SCAL_Invalid : 0);
                            mpr.OPT_FLAG |= (byte)(mpr.HLM_SCAL == null ? MPROptionalData.HI_LIMIT_HLM_SCAL_Invalid : 0);
                            mpr.OPT_FLAG |= (byte)(mpr.LO_LIMIT == null ? MPROptionalData.LO_LIMIT_LLM_SCAL_Invalid | MPROptionalData.NoLoLimitThisTest : 0);
                            mpr.OPT_FLAG |= (byte)(mpr.HI_LIMIT == null ? MPROptionalData.HI_LIMIT_HLM_SCAL_Invalid | MPROptionalData.NoHiLimitThisTest : 0);
                            mpr.OPT_FLAG |= (byte)(mpr.LO_SPEC == null ? MPROptionalData.NoLoLimitSpec : 0);
                            mpr.OPT_FLAG |= (byte)(mpr.HI_SPEC == null ? MPROptionalData.NoHiLimitSpec : 0);
                        }
                        context.Writer.Write((byte)mpr.OPT_FLAG);
                        return;
                    case "RES_SCAL":
                        if ((mpr.OPT_FLAG & (byte)PTROptionalData.RES_SCAL_Invalid) > 0)
                        {
                            context.Writer.Write((byte)0);
                            return;
                        }
                        break;
                    case "START_IN":
                    case "INCR_IN":
                        if ((mpr.OPT_FLAG & (byte)MPROptionalData.START_IN_INCR_IN_Invalid) > 0)
                        {
                            context.Writer.Write(0F); // START_IN
                            return;
                        }
                        break;
                    case "LLM_SCAL":
                        if ((mpr.OPT_FLAG & (byte)(MPROptionalData.LO_LIMIT_LLM_SCAL_Invalid | MPROptionalData.NoLoLimitThisTest)) > 0)
                        {
                            context.Writer.Write((byte)0);
                            return;
                        }
                        break;
                    case "HLM_SCAL":
                        if ((mpr.OPT_FLAG & (byte)(MPROptionalData.HI_LIMIT_HLM_SCAL_Invalid | MPROptionalData.NoHiLimitThisTest)) > 0)
                        {
                            context.Writer.Write((byte)0);
                            return;
                        }
                        break;
                    case "LO_LIMIT":
                        if ((mpr.OPT_FLAG & (byte)(MPROptionalData.LO_LIMIT_LLM_SCAL_Invalid | MPROptionalData.NoLoLimitThisTest)) > 0)
                        {
                            context.Writer.Write(0F);
                            return;
                        }
                        break;
                    case "HI_LIMIT":
                        if ((mpr.OPT_FLAG & (byte)(MPROptionalData.HI_LIMIT_HLM_SCAL_Invalid | MPROptionalData.NoHiLimitThisTest)) > 0)
                        {
                            context.Writer.Write(0F);
                            return;
                        }
                        break;
                    case "LO_SPEC":
                        if ((mpr.OPT_FLAG & (byte)MPROptionalData.NoLoLimitSpec) > 0)
                        {
                            context.Writer.Write(0F);
                            return;
                        }
                        break;
                    case "HI_SPEC":
                        if ((mpr.OPT_FLAG & (byte)MPROptionalData.NoHiLimitSpec) > 0)
                        {
                            context.Writer.Write(0F);
                            return;
                        }
                        break;
                }
            }

            // OK, optional data has a value, so write it and set the new record length
            base.SerializeProperty(context, propValue);
        }
    }
}
