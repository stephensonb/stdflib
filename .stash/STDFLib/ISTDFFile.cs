using STDFLib2.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace STDFLib2
{
    public interface ISTDFFile : IDisposable
    {
        bool CoerceInvalidValues { get; }
        STDFFileFormatter FileFormatter { get; }
        MIR Info { get; }
        List<ISTDFRecord> Records { get; }
        MRR Results { get; }
        Stream STDFStream { get; }

        void Close();
        T[] FilterRecords<T>(Predicate<T> predicate) where T : ISTDFRecord;
        FTR GetFTRDefaults(byte headNumber, byte siteNumber, uint testNumber);
        HBR GetHardBin(byte headNumber, byte siteNumber, ushort binNumber);
        PIR GetLastPartInfo();
        PIR GetLastPartInfo(byte headNumber, byte siteNumber);
        PRR GetLastPartResults(byte headNumber, byte siteNumber);
        MPR GetMPRDefaults(byte headNumber, byte siteNumber, uint testNumber);
        IEnumerable<ushort> GetPinMapIndexes(bool includeGroupIndexes = false);
        PTR GetPTRDefaults(byte headNumber, byte siteNumber, uint testNumber);
        T[] GetRecords<T>() where T : ISTDFRecord;
        SBR GetSoftBin(byte headNumber, byte siteNumber, ushort binNumber);
        T GetTestBin<T>(byte headNumber, byte siteNumber, ushort binNumber) where T : ITestBin;
        T GetTestDefaults<T>(byte headNumber, byte siteNumber, uint testNumber) where T : ITestResult;
        TSR GetTestSynopsis(byte headNumber, byte siteNumber, uint testNumber);
        bool HasOne<T>(Predicate<T> predicate) where T : ISTDFRecord;
        bool HasSite(SDR siteDescription, byte siteNumber);
        void WriteAuditTrail(string commandLine, DateTime modificationTime);
        void WriteBeginProgramSection(string sequenceName);
        void WriteDatalogText(string data);
        void WriteEndProgramSection();
        void WriteFile(string path);
        void WriteFileAttributes();
        void WriteFunctionalTestResult(byte headNumber, byte siteNumber, uint testNumber, uint? testTimeInMilliseconds = null, TestResultFlags resultFlags = TestResultFlags.AllBitsZero, uint? vectorCycleCount = null, uint? relativeVectorAddress = null, uint? vectorRepeatCount = null, uint? failingPinCount = null, int? xLogicalDeviceFailureAddress = null, int? yLogicalDeviceFailureAddress = null, short? vectorOffset = null, ushort[] pinMapIndexes = null, byte[] returnedStates = null, ushort[] programmedStateIndexes = null, byte[] programmedStates = null, byte[] failingPins = null, string vectorModulePatternName = "", string timeSetName = "", string vectorOpCode = "", string testText = "", string alarmId = "", string programmingText = "", string additionalResultInfo = "", byte patternGeneratorNumber = 255, byte[] spinMap = null);
        void WriteGenericData(params object[] args);
        void WriteMasterInformation(DateTime setupTime, DateTime startTime, byte testerStationNumber, string lotId, string partType, string nodeName, string testerType, string jobName, char testModeCode = ' ', char retestCode = ' ', char dataProtectionCode = ' ', ushort burnInTime = ushort.MaxValue, char commandModeCode = ' ', string jobRevision = "", string sublotId = "", string operationName = "", string executiveType = "", string executiveVersion = "", string testCode = "", string testTemperature = "", string userText = "", string auxFile = "", string packageType = "", string familyId = "", string dateCode = "", string facilityId = "", string floorId = "", string processId = "", string operationFrequency = "", string specName = "", string specVersion = "", string flowId = "", string setupId = "", string designRevision = "", string engineeringLotId = "", string romCode = "", string testerSerialNumber = "", string supervisorName = "");
        void WriteMasterResults(DateTime finishTime, char lotDispositionCode = ' ', string userLotDescription = "", string executiveLotDescription = "");
        void WriteMultiresultParametricTestResult(byte headNumber, byte siteNumber, uint testNumber, TestResultFlags resultFlags, TestParameterFlags parameterFlags, byte[] returnedStates, float[] results, uint? testTimeInMilliseconds = null, string testText = "", string alarmId = "", sbyte? resultScalingExponent = null, sbyte? loLimitScalingExponent = null, sbyte? hiLimitScalingExponent = null, float? loTestLimit = null, float? hiTestLimit = null, float? inputStartValue = null, float? inputIncrement = null, ushort[] pinMapIndexes = null, string units = "", string inputUnits = "", string resultFormat = null, string loLimitFormat = null, string hiLimitFormat = null, float? loSpecValue = null, float? hiSpecValue = null);
        void WriteParametricTestResult(byte headNumber, byte siteNumber, uint testNumber, TestResultFlags resultFlags, TestParameterFlags parameterFlags, float result, uint? testTimeInMilliseconds = null, string testText = "", string alarmId = "", sbyte? resultScalingExponent = null, sbyte? loLimitScalingExponent = null, sbyte? hiLimitScalingExponent = null, string units = "", string resultFormat = "", string loLimitFormat = "", string hiLimitFormat = "", float? loTestLimit = null, float? hiTestLimit = null, float? loSpecValue = null, float? hiSpecValue = null);
        void WritePartCountSummary();
        void WritePartCounts();
        void WritePartInfo(byte headNumber, byte siteNumber);
        void WritePartResults(byte headNumber, byte siteNumber, PartFlags partFlag, ushort hardBin, ushort softBin = ushort.MaxValue, short xCoordinate = short.MinValue, short yCoordinate = short.MinValue, uint testTime = 0, string partId = "", string partText = "", byte[] partFix = null);
        void WritePinGroup(ushort index, ushort[] pinMapIndexes, string groupName = "");
        void WritePinList(ushort[] indexes, PinGroupModes[] modes, DisplayRadix[] radixes = null, string[] programStateEncodingR = null, string[] returnStateEncodingR = null, string[] programStateEncodingL = null, string[] returnStateEncodingL = null);
        void WritePinMap(ushort index, ushort channelType, string channelName, string physicalName, string logicalName, byte headNumber = 1, byte siteNumber = 1);
        void WriteRecord(ISTDFRecord record);
        void WriteRetestData(ushort[] retestBins);
        void WriteSiteDescription(byte head, byte siteGroup, byte[] siteNumbers, string handlerType = "", string handlerId = "", string cardType = "", string cardId = "", string loadType = "", string loadId = "", string dibType = "", string dibId = "", string cableType = "", string cableId = "", string contactorType = "", string contactorId = "", string laserType = "", string laserId = "", string extraEqType = "", string extraEqId = "");
        void WriteTestSynopsis(byte headNumber, byte siteNumber, uint testNumber, TestTypes testType, string testName = "");
        void WriteTestSynopsisSiteSummary(byte siteNumber, uint testNumber, TestTypes testType);
    }
}