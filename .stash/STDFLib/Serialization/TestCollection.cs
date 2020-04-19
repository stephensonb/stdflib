// Formatter - serializes an object to or from a binary stream
// Converter - converts custom objects to a value of a specific type
// Surrogate - provides custom serialization for an object
// SurrogateSelector - determines which surrogate to use for serialization of custom types
// ByteConverter - converts value types to/from binary representation, honoring endianness

// To setup for serialization
// 
// Formatter = new Formatter(

namespace STDFLib.Serialization
{
    public class TestCollection : ITestCollection
    {
        public int BpsCount { get; private set; } = 0;
        public PTR DefaultPTR { get; private set; } = null;
        public MPR DefaultMPR { get; private set; } = null;
        public FTR DefaultFTR { get; private set; } = null;
        private ISTDFFile STDF;

        public TestCollection(ISTDFFile stdfFile)
        {
            STDF = stdfFile;
        }

        public void BeginProgramSection(string sectionName) 
        {
            var newRecord = new BPS() { SEQ_NAME = sectionName };
            STDF.WriteRecord(newRecord);
            BpsCount++;
        }

        public void EndProgramSection() 
        { 
            // check if there is an open BPS record
            if(BpsCount > 0)
            {
                // close it (add the eps and remove the bps record from the stack)
                STDF.WriteRecord(new EPS());
                BpsCount--;
            } else
            {
                throw new STDFFormatException("Mismatched End Program Section.  No matching begin program section found.");
            }
        }

        public void AddParametricTestResult(
            uint testNumber,
            byte headNumber,
            byte siteNumber,
            float result,
            TestResultFlags resultFlags = TestResultFlags.AllBitsZero,
            TestParameterFlags parameterFlags = TestParameterFlags.AllBitsZero,
            string testText = "",
            string alarmId = ""
            )
        {

        }

        public void AddParametricTestResult(
            uint testNumber,
            byte headNumber,
            byte siteNumber,
            float result,
            TestResultFlags resultFlags,
            TestParameterFlags parameterFlags,
            sbyte? resultScalingExponent = null,
            sbyte? loLimitScalingExponent = null,
            sbyte? hiLimitScalingExponent = null,
            ResultUnits? units = null,
            string? resultFormat = null,
            string? loLimitFormat = null,
            string? hiLimitFormat = null,
            string? testText = null,
            string? alarmId = null,
            float? loTestLimit = null,
            float? hiTestLimit = null,
            float? loSpecValue = null,
            float? hiSpecValue = null
            )
        {
            var newPTR = new PTR()
            {
                HEAD_NUM = headNumber,
                SITE_NUM = siteNumber,
                TEST_NUM = testNumber,
                RESULT = result,
                TEST_FLG = (byte)resultFlags,
                PARM_FLG = (byte)parameterFlags,
                RES_SCAL = resultScalingExponent,
                LLM_SCAL = loLimitScalingExponent,
                HLM_SCAL = hiLimitScalingExponent,
                UNITS = units?.ToString(),
                C_RESFMT = resultFormat,
                C_LLMFMT = loLimitFormat,
                C_HLMFMT = hiLimitFormat,
                TEST_TXT = testText,
                ALARM_ID = alarmId,
                LO_LIMIT = loTestLimit,
                HI_LIMIT = hiTestLimit,
                LO_SPEC = loSpecValue,
                HI_SPEC = hiSpecValue
            };

            STDF.WriteRecord(newPTR);
        }

        public void AddMultiresultParametricTestResult(
            uint testNumber,
            byte headNumber,
            byte siteNumber,
            float[] results,
            Nibbles returnedStates = null,
            TestResultFlags resultFlags = TestResultFlags.AllBitsZero,
            TestParameterFlags parameterFlags = TestParameterFlags.AllBitsZero,
            string testText = "",
            string alarmId = ""
            )
        { }

        public void AddMultiresultParametricTestResult(
            uint testNumber,
            byte headNumber,
            byte siteNumber,
            float[] results,
            ReturnedStateCodes returnedStates,
            TestResultFlags resultFlags,
            TestParameterFlags parameterFlags,
            sbyte resultScalingExponent,
            sbyte loLimitScalingExponent,
            sbyte hiLimitScalingExponent,
            ResultUnits units,
            string resultFormat,
            string loLimitFormat,
            string hiLimitFormat,
            string testText = "",
            string alarmId = "",
            float? loTestLimit = null,
            float? hiTestLimit = null,
            float? loSpecValue = null,
            float? hiSpecValue = null
            )
        { }

        public void AddFunctionalTestResult(
            uint testNumber,
            byte headNumber,
            byte siteNumber,
            float[] results,
            TestResultFlags resultFlags = TestResultFlags.AllBitsZero,
            TestParameterFlags parameterFlags = TestParameterFlags.AllBitsZero,
            uint? vectorCycleCount = null,
            uint? relativeVectorAddress = null,
            uint? failingPinCount = null,
            int? xLogicalDeviceFailureAddress = null,
            int? yLogicalDeviceFailureAddress = null,
            int? vectorOffset = null,
            ushort[] pinMapIndexes = null,
            Nibbles returnedStates = null,
            ushort[] programmedStateIndexes = null,
            Nibbles programmedStates = null,
            BitField2 failingPins = null,
            string vectorModulePatternName = "",
            string timeSetName = "",
            string vectorOpCode = "",
            string testText = "",
            string alarmId = "",
            string programmingText = "",
            string additionalResultInfo = "",
            byte patternGeneratorNumber = 255,
            BitField2 spinMap = null
            )
        { }

    }
}
