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
    public interface ITestCollection
    {
        void AddParametricTestResult(
            uint testNumber,
            byte headNumber,
            byte siteNumber,
            float result,
            TestResultFlags resultFlags = TestResultFlags.AllBitsZero,
            TestParameterFlags parameterFlags = TestParameterFlags.AllBitsZero,
            string testText = "",
            string alarmId = ""
            );

        void AddParametricTestResult(
            uint testNumber,
            byte headNumber,
            byte siteNumber,
            float result,
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
            );

        void AddMultiresultParametricTestResult(
            uint testNumber,
            byte headNumber,
            byte siteNumber,
            float[] results,
            Nibbles returnedStates = null,
            TestResultFlags resultFlags = TestResultFlags.AllBitsZero,
            TestParameterFlags parameterFlags = TestParameterFlags.AllBitsZero,
            string testText = "",
            string alarmId = ""
            );

        void AddMultiresultParametricTestResult(
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
            );

        void AddFunctionalTestResult(
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
            );

        void BeginProgramSection(string sectionName);
        void EndProgramSection();
    }
}
