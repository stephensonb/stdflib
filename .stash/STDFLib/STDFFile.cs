using STDFLib2.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace STDFLib2
{
    public partial class STDFFile : ISTDFFile
    {
        protected STDFFile(string path, FileMode mode, FileAccess access, bool coerceInvalidValues = true)
        {
            // Open the file stream
            STDFStream = File.Open(path, mode, access);

            // Default record formatter
            FileFormatter = new STDFFileFormatter();

            // If file opened for writing, then write the FAR record
            if (STDFStream.CanWrite)
            {
                FileFormatter.BeginStreamingSerialization(STDFStream);
            }
            else
            {
                // File opened as read, so try to read the records of the file into the STDFFile
                Reader = new STDFBinaryReader(STDFStream);
                Records = (List<ISTDFRecord>)FileFormatter.Deserialize(STDFStream);
                STDFStream.Close();
                STDFStream = null;
            }

            CoerceInvalidValues = coerceInvalidValues;
        }

        private readonly string AlphaNumericCodes = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
        private readonly List<PRR> _partsUnderTest = new List<PRR>();
        private readonly List<ITestResult> _testDefaults = new List<ITestResult>();
        private readonly List<TSR> _tsrRecords = new List<TSR>();
        private readonly Stack<BPS> _bpsRecords = new Stack<BPS>();
        private readonly List<ITestBin> _binRecords = new List<ITestBin>();
        private readonly List<PCR> _partCounts = new List<PCR>();
        
        protected ISTDFBinaryReader Reader { get; set; } = null;
        protected ISTDFBinaryWriter Writer { get; set; } = null;

        public bool CoerceInvalidValues { get; private set; }
        public Stream STDFStream { get; private set; }
        public STDFFileFormatter FileFormatter { get; private set; }
        public List<ISTDFRecord> Records { get; private set; } = new List<ISTDFRecord>();
        public MIR Info { get; private set; } = null;
        public MRR Results { get; private set; } = null;

        public static ISTDFFile Open(string path, bool coerceInvalidValues = true)
        {
            STDFFile sfile = new STDFFile(path, FileMode.Create, FileAccess.ReadWrite, coerceInvalidValues);
            return sfile;
        }

        // Read in an STDF file into memory
        public static ISTDFFile ReadFile(string path)
        {
            return new STDFFile(path, FileMode.Open, FileAccess.Read);
        }

        // Write the current state of the STDFFile to a new file path
        public void WriteFile(string path)
        {
            if (STDFStream != null && STDFStream.CanWrite)
            {
                throw new InvalidOperationException("Cannot write file, file already opened for streaming output.");
            }
            using Stream outStream = File.OpenWrite(path);
        }

        public T[] FilterRecords<T>(Predicate<T> predicate) where T : ISTDFRecord
        {
            List<T> returnRecords = new List<T>();

            foreach (var record in Records)
            {
                if (predicate.Invoke((T)record))
                {
                    returnRecords.Add((T)record);
                }
            }

            return returnRecords.ToArray();
        }

        public FTR GetFTRDefaults(byte headNumber, byte siteNumber, uint testNumber)
        {
            return GetTestDefaults<FTR>(headNumber, siteNumber, testNumber);
        }

        public HBR GetHardBin(byte headNumber, byte siteNumber, ushort binNumber)
        {
            return GetTestBin<HBR>(headNumber, siteNumber, binNumber);
        }

        public PIR GetLastPartInfo()
        {
            PIR lastrecord = null;

            for (int i = Records.Count - 1; i >= 0; i--)
            {
                if (Records[i] is PRR)
                {
                    break;
                }
                if (Records[i] is PIR pir)
                {
                    lastrecord = pir;
                }
            }

            return lastrecord;
        }

        public PIR GetLastPartInfo(byte headNumber, byte siteNumber)
        {
            PIR lastrecord = null;

            for (int i = Records.Count - 1; i >= 0; i--)
            {
                if (Records[i] is PRR prr && prr.HEAD_NUM == headNumber && prr.SITE_NUM == siteNumber)
                {
                    break;
                }
                if (Records[i] is PIR pir && pir.HEAD_NUM == headNumber && pir.SITE_NUM == siteNumber)
                {
                    lastrecord = (PIR)Records[i];
                }
            }

            return lastrecord;
        }

        public PRR GetLastPartResults(byte headNumber, byte siteNumber)
        {
            PRR lastrecord = null;

            for (int i = Records.Count - 1; i >= 0; i--)
            {
                if (Records[i] is PIR pir && pir.HEAD_NUM == headNumber && pir.SITE_NUM == siteNumber)
                {
                    break;
                }
                if (Records[i] is PRR prr && prr.HEAD_NUM == headNumber && prr.SITE_NUM == siteNumber)
                {
                    lastrecord = (PRR)Records[i];
                }
            }

            return lastrecord;
        }

        public MPR GetMPRDefaults(byte headNumber, byte siteNumber, uint testNumber)
        {
            return GetTestDefaults<MPR>(headNumber, siteNumber, testNumber);
        }

        public IEnumerable<ushort> GetPinMapIndexes(bool includeGroupIndexes = false)
        {
            List<ushort> indexes = new List<ushort>();

            // Get indexes for all pin map records
            indexes.AddRange(Records.FindAll(x => x is PMR).Select(x => (x as PMR).PMR_INDX));

            if (includeGroupIndexes)
            {
                // Include pin group indexes if requested
                indexes.AddRange(Records.FindAll(x => x is PGR).Select(x => (x as PGR).GRP_INDX));
            }

            return indexes;
        }

        public PTR GetPTRDefaults(byte headNumber, byte siteNumber, uint testNumber)
        {
            return GetTestDefaults<PTR>(headNumber, siteNumber, testNumber);
        }

        public T[] GetRecords<T>() where T : ISTDFRecord
        {
            List<T> filteredRecords = new List<T>();

            foreach (ISTDFRecord record in Records)
            {
                if (record is T)
                {
                    filteredRecords.Add((T)record);
                }
            }
            return filteredRecords.ToArray();
        }

        public SBR GetSoftBin(byte headNumber, byte siteNumber, ushort binNumber)
        {
            return GetTestBin<SBR>(headNumber, siteNumber, binNumber);
        }

        public T GetTestBin<T>(byte headNumber, byte siteNumber, ushort binNumber) where T : ITestBin
        {
            return (T)_binRecords.Find(x => (x is T) &&
                                         x.HEAD_NUM == headNumber &&
                                         x.SITE_NUM == siteNumber &&
                                         x.BIN_NUM == binNumber);
        }

        public T GetTestDefaults<T>(byte headNumber, byte siteNumber, uint testNumber) where T : ITestResult
        {
            return (T)_testDefaults.Find(test => (test is T) && test.HEAD_NUM == headNumber && test.SITE_NUM == siteNumber && test.TEST_NUM == testNumber);
        }

        public TSR GetTestSynopsis(byte headNumber, byte siteNumber, uint testNumber)
        {
            return _tsrRecords.Find(tsr => tsr.HEAD_NUM == headNumber &&
                                           tsr.SITE_NUM == siteNumber &&
                                           tsr.TEST_NUM == testNumber);
        }

        public bool HasOne<T>(Predicate<T> predicate) where T : ISTDFRecord
        {
            foreach (T record in Records)
            {
                if (predicate.Invoke(record))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasSite(SDR siteDescription, byte siteNumber)
        {
            if (siteDescription?.SITE_NUM != null)
            {
                for (int i = 0; i < siteDescription.SITE_NUM.Length; i++)
                {
                    if (siteDescription.SITE_NUM[i] == siteNumber)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual void WriteAuditTrail(string commandLine, DateTime modificationTime)
        {
            WriteAuditTrail(new ATR() { CMD_LINE = commandLine, MOD_TIM = modificationTime });
        }

        public virtual void WriteAuditTrail(ATR record)
        {
            WriteRecord(record);
        }

        public virtual void WriteBeginProgramSection(string sequenceName)
        {
            WriteBeginProgramSection(new BPS() { SEQ_NAME = sequenceName });
        }

        public virtual void WriteBeginProgramSection(BPS record) 
        {
            WriteRecord(record);
        }

        public virtual void WriteDatalogText(string data)
        {
            WriteDatalogText(new DTR() { TEXT_DAT = data });
        }

        public virtual void WriteDatalogText(DTR record) 
        {
            WriteRecord(record);
        }

        public virtual void WriteEndProgramSection()
        {
            WriteRecord(new EPS());
        }

        public virtual void WriteEndProgramSection(EPS record) 
        {
            WriteRecord(record);
        }

        public virtual void WriteFileAttributes()
        {
            if (Records.Count == 0)
            {
                WriteFileAttributes(new FAR() { CPU_TYPE = (byte)STDFCpuTypes.i386, STDF_VER = (byte)STDFVersions.STDFVer4 });
            }
        }

        public virtual void WriteFileAttributes(FAR record)
        {
            WriteRecord(record);
        }

        public virtual void WriteFunctionalTestResult(byte headNumber, byte siteNumber, uint testNumber,
                                                    uint? testTimeInMilliseconds = null,
                                                    TestResultFlags resultFlags = TestResultFlags.AllBitsZero,
                                                    uint? vectorCycleCount = null, uint? relativeVectorAddress = null,
                                                    uint? vectorRepeatCount = null, uint? failingPinCount = null,
                                                    int? xLogicalDeviceFailureAddress = null,
                                                    int? yLogicalDeviceFailureAddress = null, short? vectorOffset = null,
                                                    ushort[] pinMapIndexes = null, byte[] returnedStates = null,
                                                    ushort[] programmedStateIndexes = null,
                                                    byte[] programmedStates = null, byte[] failingPins = null,
                                                    string vectorModulePatternName = "", string timeSetName = "",
                                                    string vectorOpCode = "", string testText = "", string alarmId = "",
                                                    string programmingText = "", string additionalResultInfo = "",
                                                    byte patternGeneratorNumber = 255, byte[] spinMap = null)
        {
            WriteFunctionalTestResult(new FTR()
            {
                HEAD_NUM = headNumber,
                SITE_NUM = siteNumber,
                TEST_NUM = testNumber,
                TEST_FLG = (byte)resultFlags,
                CYCL_CNT = vectorCycleCount,
                REL_VADR = relativeVectorAddress,
                REPT_CNT = vectorRepeatCount,
                NUM_FAIL = failingPinCount,
                XFAIL_AD = xLogicalDeviceFailureAddress,
                YFAIL_AD = yLogicalDeviceFailureAddress,
                VECT_OFF = vectorOffset,
                RTN_INDX = pinMapIndexes,
                RTN_STAT = returnedStates,
                PGM_INDX = programmedStateIndexes,
                PGM_STAT = programmedStates,
                FAIL_PIN = failingPins,
                VECT_NAM = vectorModulePatternName,
                TIME_SET = timeSetName,
                OP_CODE = vectorOpCode,
                TEST_TXT = testText,
                ALARM_ID = alarmId,
                PROG_TXT = programmingText,
                RSLT_TXT = additionalResultInfo,
                PATG_NUM = patternGeneratorNumber,
                SPIN_MAP = spinMap
            }, testTimeInMilliseconds);
        }

        public virtual void WriteFunctionalTestResult(FTR record, uint? testTimeInMilliseconds=null) 
        {
            var prr = GetPartUnderTest(record.HEAD_NUM, record.SITE_NUM);

            if (prr == null)
            {
                throw new STDFFormatException("STDF format exception.  No active part record found trying to write FTR record.");
            }
            var defaults = GetTestDefaults<FTR>(record.HEAD_NUM, record.SITE_NUM, record.TEST_NUM);

            if (defaults == null)
            {
                _testDefaults.Add(record);
            }
            else
            {
                // set the optional data flag for missing/invalid data
                record.OPT_FLAG = (byte)FTROptionalData.AllOptionalDataValid;

                // set the optional data flag
                record.OPT_FLAG = (byte)FTROptionalData.AllOptionalDataValid;
                record.OPT_FLAG |= (byte)((record.REL_VADR == null && defaults.REL_VADR == null) ? FTROptionalData.RelativeVectorAddressIsInvalid : 0);
                record.OPT_FLAG |= (byte)((record.REPT_CNT == null && defaults.REPT_CNT == null) ? FTROptionalData.RepeatCountOfVectorIsInvalid : 0);
                record.OPT_FLAG |= (byte)((record.NUM_FAIL == null && defaults.NUM_FAIL == null) ? FTROptionalData.NumberOfFailsIsInvalid: 0);
                record.OPT_FLAG |= (byte)((record.XFAIL_AD == null && defaults.XFAIL_AD == null) ? FTROptionalData.XYFailAddressIsInvalid: 0);
                record.OPT_FLAG |= (byte)((record.YFAIL_AD == null && defaults.YFAIL_AD == null) ? FTROptionalData.XYFailAddressIsInvalid : 0);
                record.OPT_FLAG |= (byte)((record.VECT_OFF == null && defaults.VECT_OFF == null) ? FTROptionalData.VectorOffsetDataIsInvalid : 0);
                record.OPT_FLAG |= (byte)((record.CYCL_CNT == null && defaults.CYCL_CNT == null) ? FTROptionalData.CycleCountDataIsInvalid : 0);

                // If values are same as default values, then set as missing to save space in the record.
                record.PATG_NUM = record.PATG_NUM == defaults.PATG_NUM ? (byte)255 : record.PATG_NUM;
                if (record.SPIN_MAP_LEN > 0 && (record.SPIN_MAP_LEN == defaults.SPIN_MAP_LEN))
                {
                    bool isEqual = true;
                    for (int i = 0; i < record.SPIN_MAP_LEN; i++)
                    {
                        if (record.SPIN_MAP[i] != defaults.SPIN_MAP[i])
                        {
                            isEqual = false;
                        }
                    }
                    
                    // if SPIN_MAP is the same as the default, then don't save it.
                    if (isEqual)
                    {
                        record.SPIN_MAP = new byte[0];
                    }
                }
            }

            WriteRecord(record);

            // update the test counter for the part
            prr.NUM_TEST++;

            // update the test synopsis record for this test
            UpdateTestSynopsis(record, testTimeInMilliseconds);
        }

        public virtual void WriteGenericData(params object[] args)
        {
            WriteGenericData(new GDR() { FieldData = (VarDataField[])args });
        }

        public virtual void WriteGenericData(GDR record) 
        {
            WriteRecord(record);
        }

        public virtual void WriteHardBin(byte headNumber, byte siteNumber, ushort binNumber, char passFailIndication, string binName = "")
        {
            HBR bin = GetHardBin(headNumber, siteNumber, binNumber);
            bin.HBIN_NAM = binName;
            bin.HBIN_PF = passFailIndication;

            WriteHardBin(bin);
        }

        public virtual void WriteHardBin(HBR record)
        {
            record.HBIN_PF = char.ToUpper(record.HBIN_PF);

            if (!"PF ".Contains(record.HBIN_PF))
            {
                throw new ArgumentOutOfRangeException("passFailIndication");
            }
 
            WriteRecord(record);
        }

        public virtual void WriteMasterInformation(DateTime setupTime, DateTime startTime, byte testerStationNumber,
                                           string lotId, string partType, string nodeName, string testerType,
                                           string jobName, char testModeCode = ' ', char retestCode = ' ',
                                           char dataProtectionCode = ' ', ushort burnInTime = 65535,
                                           char commandModeCode = ' ', string jobRevision = "", string sublotId = "",
                                           string operationName = "", string executiveType = "",
                                           string executiveVersion = "", string testCode = "",
                                           string testTemperature = "", string userText = "", string auxFile = "",
                                           string packageType = "", string familyId = "", string dateCode = "",
                                           string facilityId = "", string floorId = "", string processId = "",
                                           string operationFrequency = "", string specName = "", string specVersion = "",
                                           string flowId = "", string setupId = "", string designRevision = "",
                                           string engineeringLotId = "", string romCode = "",
                                           string testerSerialNumber = "", string supervisorName = "")
        {
            Info.SETUP_T = setupTime;
            Info.START_T = startTime;
            Info.STAT_NUM = testerStationNumber;
            Info.MODE_COD = testModeCode;
            Info.RTST_COD = retestCode;
            Info.PROT_COD = dataProtectionCode;
            Info.BURN_TIM = burnInTime;
            Info.CMOD_COD = commandModeCode;
            Info.LOT_ID = lotId;
            Info.PART_TYP = partType;
            Info.NODE_NAM = nodeName;
            Info.TSTR_TYP = testerType;
            Info.JOB_NAM = jobName;
            Info.JOB_REV = jobRevision;
            Info.SBLOT_ID = sublotId;
            Info.OPER_NAM = operationName;
            Info.EXEC_TYP = executiveType;
            Info.EXEC_VER = executiveVersion;
            Info.TEST_COD = testCode;
            Info.TST_TEMP = testTemperature;
            Info.USER_TXT = userText;
            Info.AUX_FILE = auxFile;
            Info.PKG_TYP = packageType;
            Info.FAMLY_ID = familyId;
            Info.DATE_COD = dateCode;
            Info.FACIL_ID = facilityId;
            Info.FLOOR_ID = floorId;
            Info.PROC_ID = processId;
            Info.OPER_FRQ = operationFrequency;
            Info.SPEC_NAM = specName;
            Info.SPEC_VER = specVersion;
            Info.FLOW_ID = flowId;
            Info.SETUP_ID = setupId;
            Info.DSGN_REV = designRevision;
            Info.ENG_ID = engineeringLotId;
            Info.ROM_COD = romCode;
            Info.SERL_NUM = testerSerialNumber;
            Info.SUPR_NAM = supervisorName;

            WriteMasterInformation(Info);

        }

        public virtual void WriteMasterInformation(MIR record) 
        {
            record.MODE_COD = char.ToUpper(record.MODE_COD);
            record.RTST_COD = char.ToUpper(record.RTST_COD);
            record.PROT_COD = char.ToUpper(record.PROT_COD);
            record.CMOD_COD = char.ToUpper(record.CMOD_COD);

            if (!" ACDEMPQ0123456789".Contains(record.MODE_COD))
            {
                throw new ArgumentOutOfRangeException(string.Format("Invalid test mode code '{0}'.  Valid test mode codes are A,C,D,E,M,P,Q or digits 0-9", record.MODE_COD));
            }

            if (!AlphaNumericCodes.Contains(record.MODE_COD))
            {
                throw new ArgumentOutOfRangeException(string.Format("Invalid retest mode code '{0}'.  Valid retest codes are characters A-Z and digits 0-9", record.MODE_COD));
            }

            if (!AlphaNumericCodes.Contains(record.PROT_COD))
            {
                throw new ArgumentOutOfRangeException(string.Format("Invalid data protection code '{0}'.  Valid data protection codes are characters A-Z and digits 0-9", record.MODE_COD));
            }

            if (!AlphaNumericCodes.Contains(record.CMOD_COD))
            {
                throw new ArgumentOutOfRangeException(string.Format("Invalid command mode code '{0}'.  Valid command mode codes are characters A-Z and digits 0-9", record.MODE_COD));
            }


            WriteRecord(Info);
        }

        public virtual void WriteMasterResults(DateTime finishTime, char lotDispositionCode = ' ',
                                       string userLotDescription = "", string executiveLotDescription = "")
        {
            Results.FINISH_T = finishTime;
            Results.USR_DESC = userLotDescription;
            Results.EXC_DESC = executiveLotDescription;
            Results.DISP_COD = lotDispositionCode;

            WriteMasterResults(Results);
        }

        public virtual void WriteMasterResults(MRR record) 
        {
            record.DISP_COD = char.ToUpper(record.DISP_COD);

            if (!AlphaNumericCodes.Contains(record.DISP_COD))
            {
                throw new ArgumentOutOfRangeException(string.Format("Invalid lot disposition code '{0}'.  Valid lot disposition codes are characters A-Z and digits 0-9", record.DISP_COD));
            }

            WriteRecord(Results);
        }

        public virtual void WriteMultiresultParametricTestResult(byte headNumber, byte siteNumber, uint testNumber, TestResultFlags resultFlags,
                                                       TestParameterFlags parameterFlags, byte[] returnedStates, float[] results,
                                                       uint? testTimeInMilliseconds = null, string testText = "", string alarmId = "",
                                                       sbyte? resultScalingExponent = null, sbyte? loLimitScalingExponent = null,
                                                       sbyte? hiLimitScalingExponent = null, float? loTestLimit = null, float? hiTestLimit = null,
                                                       float? inputStartValue = null, float? inputIncrement = null, ushort[] pinMapIndexes = null,
                                                       string units = "", string inputUnits = "",
                                                       string resultFormat = null, string loLimitFormat = null, string hiLimitFormat = null,
                                                       float? loSpecValue = null, float? hiSpecValue = null)
        {
            WriteMultiresultParametricTestResult(new MPR()
            {
                HEAD_NUM = headNumber,
                SITE_NUM = siteNumber,
                TEST_NUM = testNumber,
                RTN_RSLT = results,
                RTN_STAT = returnedStates,
                TEST_FLG = (byte)resultFlags,
                PARM_FLG = (byte)parameterFlags,
                TEST_TXT = testText,
                ALARM_ID = alarmId,
                RES_SCAL = resultScalingExponent,
                LLM_SCAL = loLimitScalingExponent,
                HLM_SCAL = hiLimitScalingExponent,
                UNITS = units,
                START_IN = inputStartValue,
                INCR_IN = inputIncrement,
                RTN_INDX = pinMapIndexes,
                UNITS_IN = inputUnits,
                C_RESFMT = resultFormat,
                C_LLMFMT = loLimitFormat,
                C_HLMFMT = hiLimitFormat,
                LO_LIMIT = loTestLimit,
                HI_LIMIT = hiTestLimit,
                LO_SPEC = loSpecValue,
                HI_SPEC = hiSpecValue
            }, testTimeInMilliseconds);

        }

        public virtual void WriteMultiresultParametricTestResult(MPR record, uint? testTimeInMilliseconds=null) 
        {
            var prr = GetPartUnderTest(record.HEAD_NUM, record.SITE_NUM);

            if (prr == null)
            {
                throw new STDFFormatException("STDF format exception.  No active part record found trying to write MPR record.");
            }
            var defaults = GetTestDefaults<MPR>(record.HEAD_NUM, record.SITE_NUM, record.TEST_NUM);

            if (defaults == null)
            {
                _testDefaults.Add(record);
            } 
            else
            {
                // set the optional data flag
                record.OPT_FLAG = 0;
                record.OPT_FLAG |= (byte)((record.RES_SCAL == null && defaults.RES_SCAL == null) ? MPROptionalData.RES_SCAL_Invalid : 0);
                record.OPT_FLAG |= (byte)((record.LLM_SCAL == null && defaults.LLM_SCAL == null) ? MPROptionalData.LO_LIMIT_LLM_SCAL_Invalid : 0);
                record.OPT_FLAG |= (byte)((record.HLM_SCAL == null && defaults.HLM_SCAL == null) ? MPROptionalData.HI_LIMIT_HLM_SCAL_Invalid : 0);
                record.OPT_FLAG |= (byte)((record.START_IN == null && defaults.START_IN == null) ? MPROptionalData.START_IN_INCR_IN_Invalid : 0);
                record.OPT_FLAG |= (byte)((record.INCR_IN  == null && defaults.INCR_IN  == null) ? MPROptionalData.START_IN_INCR_IN_Invalid : 0);
                record.OPT_FLAG |= (byte)((record.LO_LIMIT == null && defaults.LO_LIMIT == null) ? MPROptionalData.NoLoLimitThisTest : 0);
                record.OPT_FLAG |= (byte)((record.HI_LIMIT == null && defaults.HI_LIMIT == null) ? MPROptionalData.NoHiLimitThisTest : 0);
                record.OPT_FLAG |= (byte)((record.LO_SPEC  == null && defaults.LO_SPEC  == null) ? MPROptionalData.NoLoLimitSpec : 0);
                record.OPT_FLAG |= (byte)((record.HI_SPEC  == null && defaults.HI_SPEC  == null) ? MPROptionalData.NoHiLimitSpec : 0);

                // If values are same as default values, then set as missing to save space in the record.
                record.RES_SCAL = record.RES_SCAL == defaults.RES_SCAL ? null : record.RES_SCAL;
                record.LLM_SCAL = record.LLM_SCAL == defaults.LLM_SCAL ? null : record.LLM_SCAL;
                record.HLM_SCAL = record.HLM_SCAL == defaults.HLM_SCAL ? null : record.HLM_SCAL;
                record.LO_LIMIT = record.LO_LIMIT == defaults.LO_LIMIT ? null : record.LO_LIMIT;
                record.HI_LIMIT = record.HI_LIMIT == defaults.HI_LIMIT ? null : record.HI_LIMIT;
                record.START_IN = record.START_IN == defaults.START_IN ? null : record.START_IN;
                record.INCR_IN  = record.INCR_IN  == defaults.INCR_IN  ? null : record.INCR_IN;
                record.UNITS    = record.UNITS    == defaults.UNITS    ? ""   : record.UNITS;
                record.UNITS_IN = record.UNITS_IN == defaults.UNITS_IN ? ""   : record.UNITS_IN;
                record.C_RESFMT = record.C_RESFMT == defaults.C_RESFMT ? ""   : record.C_RESFMT;
                record.C_LLMFMT = record.C_LLMFMT == defaults.C_LLMFMT ? ""   : record.C_LLMFMT;
                record.C_HLMFMT = record.C_HLMFMT == defaults.C_HLMFMT ? ""   : record.C_HLMFMT;
                record.LO_SPEC  = record.LO_SPEC  == defaults.LO_SPEC ? null  : record.LO_SPEC;
                record.HI_SPEC  = record.HI_SPEC  == defaults.HI_SPEC ? null  : record.HI_SPEC;
            }

            WriteRecord(record);

            // update test counter for the part
            prr.NUM_TEST++;

            // update the test synopsis record for this test
            UpdateTestSynopsis(record, testTimeInMilliseconds);
        }

        public virtual void WriteParametricTestResult(byte headNumber, byte siteNumber, uint testNumber,
                                    TestResultFlags resultFlags, TestParameterFlags parameterFlags,
                                    float result, uint? testTimeInMilliseconds = null, string testText = "", string alarmId = "",
                                    sbyte? resultScalingExponent = null, sbyte? loLimitScalingExponent = null,
                                    sbyte? hiLimitScalingExponent = null, string units = "", string resultFormat = "",
                                    string loLimitFormat = "", string hiLimitFormat = "", float? loTestLimit = null,
                                    float? hiTestLimit = null, float? loSpecValue = null, float? hiSpecValue = null)
        {
            WriteParametricTestResult(new PTR()
            {
                HEAD_NUM = headNumber,
                SITE_NUM = siteNumber,
                TEST_NUM = testNumber,
                RESULT = result,
                TEST_FLG = (byte)resultFlags,
                PARM_FLG = (byte)parameterFlags,
                TEST_TXT = testText,
                ALARM_ID = alarmId,
                RES_SCAL = resultScalingExponent,
                LLM_SCAL = loLimitScalingExponent,
                HLM_SCAL = hiLimitScalingExponent,
                UNITS = units,
                C_RESFMT = resultFormat,
                C_LLMFMT = loLimitFormat,
                C_HLMFMT = hiLimitFormat,
                LO_LIMIT = loTestLimit,
                HI_LIMIT = hiTestLimit,
                LO_SPEC = loSpecValue,
                HI_SPEC = hiSpecValue
            }, testTimeInMilliseconds);
        }

        public virtual void WriteParametricTestResult(PTR record, uint? testTimeInMilliseconds=null) 
        {
            var prr = GetPartUnderTest(record.HEAD_NUM, record.SITE_NUM);

            if (prr == null)
            {
                throw new STDFFormatException("STDF format exception.  No active part record found trying to write PTR record.");
            }

            var defaults = GetTestDefaults<PTR>(record.HEAD_NUM, record.SITE_NUM, record.TEST_NUM);

            if (defaults == null)
            {
                _testDefaults.Add(record);
            } else
            {
                // set the optional data flag
                record.OPT_FLAG = (byte)PTROptionalData.AllOptionalDataValid;
                record.OPT_FLAG |= (byte)((record.RES_SCAL == null && defaults.RES_SCAL == null) ? PTROptionalData.RES_SCAL_Invalid : 0);
                record.OPT_FLAG |= (byte)((record.LLM_SCAL == null && defaults.LLM_SCAL == null) ? PTROptionalData.LO_LIMIT_LLM_SCAL_Invalid : 0);
                record.OPT_FLAG |= (byte)((record.HLM_SCAL == null && defaults.HLM_SCAL == null) ? PTROptionalData.HI_LIMIT_HLM_SCAL_Invalid : 0);
                record.OPT_FLAG |= (byte)((record.LO_LIMIT == null && defaults.LO_LIMIT == null) ? PTROptionalData.NoLoLimitThisTest : 0);
                record.OPT_FLAG |= (byte)((record.HI_LIMIT == null && defaults.HI_LIMIT == null) ? PTROptionalData.NoLoLimitThisTest : 0);
                record.OPT_FLAG |= (byte)((record.LO_SPEC == null && defaults.LO_SPEC == null) ? PTROptionalData.NoLoLimitSpec : 0);
                record.OPT_FLAG |= (byte)((record.HI_SPEC == null && defaults.HI_SPEC == null) ? PTROptionalData.NoHiLimitSpec : 0);

                // If values are same as default values, then set as missing to save space in the record.
                record.RES_SCAL = record.RES_SCAL == defaults.RES_SCAL ? null : record.RES_SCAL;
                record.LLM_SCAL = record.LLM_SCAL == defaults.LLM_SCAL ? null : record.LLM_SCAL;
                record.HLM_SCAL = record.HLM_SCAL == defaults.HLM_SCAL ? null : record.HLM_SCAL;
                record.LO_LIMIT = record.LO_LIMIT == defaults.LO_LIMIT ? null : record.LO_LIMIT;
                record.HI_LIMIT = record.HI_LIMIT == defaults.HI_LIMIT ? null : record.HI_LIMIT;
                record.UNITS    = record.UNITS    == defaults.UNITS    ? null : record.UNITS;
                record.C_RESFMT = record.C_RESFMT == defaults.C_RESFMT ? null : record.C_RESFMT;
                record.C_LLMFMT = record.C_LLMFMT == defaults.C_LLMFMT ? null : record.C_LLMFMT;
                record.C_HLMFMT = record.C_HLMFMT == defaults.C_HLMFMT ? null : record.C_HLMFMT;
                record.LO_SPEC  = record.LO_SPEC  == defaults.LO_SPEC  ? null  : record.LO_SPEC;
                record.HI_SPEC  = record.HI_SPEC  == defaults.HI_SPEC  ? null  : record.HI_SPEC;
            }

            // update test counter for part
            prr.NUM_TEST++;

            // update the test synopsis record for this test
            UpdateTestSynopsis(record, testTimeInMilliseconds);

            WriteRecord(record);
        }

        public virtual void WritePartCountSummary()
        {
            PCR pcr = _partCounts.Find(x => x.HEAD_NUM == 255);
            if (pcr != null)
            {
                WritePartCountSummary(pcr);
            }
        }

        public virtual void WritePartCountSummary(PCR record) 
        {
            WriteRecord(record);
        }

        public virtual void WritePartCounts()
        {
            foreach (PCR pcr in _partCounts)
            {
                if (pcr.HEAD_NUM != 255)
                {
                    WritePartCounts(pcr);
                }
            }
        }

        public virtual void WritePartCounts(PCR record) 
        {
            WriteRecord(record);
        }

        public virtual void WritePartInfo(byte headNumber, byte siteNumber)
        {
            WritePartInfo(new PIR() { HEAD_NUM = headNumber, SITE_NUM = siteNumber });
        }

        public virtual void WritePartInfo(PIR record) 
        {
            // Add a new part under test
            AddPartUnderTest(record.HEAD_NUM, record.SITE_NUM);

            WriteRecord(record);
        }

        public virtual void WritePartResults(byte headNumber, byte siteNumber, PartFlags partFlag, ushort hardBin, ushort softBin = 65535,
                                short xCoordinate = -32768, short yCoordinate = -32768, uint testTime = 0,
                                string partId = "", string partText = "", byte[] partFix = null)
        {
            WritePartResults(new PRR()
            {
                PART_FLG = (byte)partFlag,
                HARD_BIN = hardBin,
                SOFT_BIN = softBin,
                X_COORD = xCoordinate,
                Y_COORD = yCoordinate,
                TEST_T = testTime,
                PART_ID = partId,
                PART_TXT = partText,
                PART_FIX = partFix
            });
        }

        public virtual void WritePartResults(PRR record)
        {
            var prr = GetPartUnderTest(record.HEAD_NUM, record.SITE_NUM);

            if (record == null)
            {
                throw new STDFFormatException("STDF format exception.  No active part record found trying to write part result record.");
            }

            prr.PART_FLG = record.PART_FLG;
            prr.HARD_BIN = record.HARD_BIN;
            prr.SOFT_BIN = record.SOFT_BIN;
            prr.X_COORD = record.X_COORD;
            prr.Y_COORD = record.Y_COORD;
            prr.TEST_T = record.TEST_T;
            prr.PART_ID = record.PART_ID;
            prr.PART_TXT = record.PART_TXT;
            prr.PART_FIX = record.PART_FIX;

            // validate the part flag value
            if ((prr.PART_FLG & 0b00000011) > 0)
            {
                throw new ArgumentException("Retest flags for PART_ID and XCOORD/YCOORD cannot both be set (bits 0 and 1).");
            }

            if ((prr.PART_FLG & 0b11100000) > 0)
            {
                throw new ArgumentException("Part flag bits 5 through 7 are reserved and must be set to 0.");
            }

            if (prr.HARD_BIN > 32767)
            {
                throw new ArgumentOutOfRangeException(string.Format("Invalid hard bin value {0}.  Valid values are > 0 and < 32767.", prr.HARD_BIN));
            }

            if (prr.SOFT_BIN > 32767 && prr.SOFT_BIN < 65535)
            {
                throw new ArgumentOutOfRangeException(string.Format("Invalid soft bin value {0}.  Valid values are > 0 and < 32767.", prr.SOFT_BIN));
            }

            // Update the hard and soft bin counters
            UpdateTestBin<HBR>(prr.HEAD_NUM, prr.SITE_NUM, prr.HARD_BIN);
            UpdateTestBin<SBR>(prr.HEAD_NUM, prr.SITE_NUM, prr.SOFT_BIN);

            // update the part count summary record
            PCR pcr = _partCounts.Find(x => x.HEAD_NUM == 255) ?? new PCR() { HEAD_NUM = 255, SITE_NUM = 0 };
            pcr.ABRT_CNT += (uint)((prr.PART_FLG & (byte)PartFlags.AbnormalEnd) > 0 ? 1 : 0);
            pcr.FUNC_CNT += (uint)((prr.PART_FLG & (byte)PartFlags.AbnormalEnd) > 0 ? 0 : 1);
            pcr.GOOD_CNT += (uint)((prr.PART_FLG & (byte)PartFlags.PartFailed) > 0 ? 0 : 1);
            pcr.RTST_CNT += (uint)((prr.PART_FLG & (byte)(PartFlags.RetestPartID | PartFlags.RetestXYCoord)) > 0 ? 1 : 0);
            pcr.PART_CNT++;

            // update the part count record for head/site
            pcr = _partCounts.Find(x => x.HEAD_NUM == prr.HEAD_NUM) ?? new PCR() { HEAD_NUM = prr.HEAD_NUM, SITE_NUM = 0 };
            pcr.ABRT_CNT += (uint)((prr.PART_FLG & (byte)PartFlags.AbnormalEnd) > 0 ? 1 : 0);
            pcr.FUNC_CNT += (uint)((prr.PART_FLG & (byte)PartFlags.AbnormalEnd) > 0 ? 0 : 1);
            pcr.GOOD_CNT += (uint)((prr.PART_FLG & (byte)PartFlags.PartFailed) > 0 ? 0 : 1);
            pcr.RTST_CNT += (uint)((prr.PART_FLG & (byte)(PartFlags.RetestPartID | PartFlags.RetestXYCoord)) > 0 ? 1 : 0);
            pcr.PART_CNT++;

            WriteRecord(prr);

            // remove the part from the part under test list
            RemovePartUnderTest(prr);
        }

        public virtual void WritePinGroup(ushort index, ushort[] pinMapIndexes, string groupName = "")
        {
            WritePinGroup(new PGR()
            {
                GRP_INDX = index,
                PMR_INDX = pinMapIndexes,
                GRP_NAM = groupName
            });
        }

        public virtual void WritePinGroup(PGR record) 
        {
            var definedIndexes = GetPinMapIndexes();

            // make sure all pin map indexes have a corresponding PMR defined already
            for (int i = 0; i < record.PMR_INDX.Length; i++)
            {
                if (!definedIndexes.Contains(record.PMR_INDX[i]))
                {
                    throw new STDFFormatException(string.Format("STDF format exception.  Pin map index {0} not defined.", record.PMR_INDX[i]));
                }
            }

            if (record.GRP_INDX >= 32768)
            {
                if (Records.FindIndex(x => (x as PGR)?.GRP_INDX == record.GRP_INDX) >= 0)
                {
                    throw new STDFFormatException("STDF format exception.  Duplicate pin group index.");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Pin map index out of range, must be >= 32768 and <= 65535");
            }

            WriteRecord(record);
        }

        public virtual void WritePinList(ushort[] indexes, PinGroupModes[] modes = null, DisplayRadix[] radixes = null,
                               string[] programStateEncodingR = null, string[] programStateEncodingL = null,
                               string[] returnStateEncodingR = null, string[] returnStateEncodingL = null)
        {
            WritePinList(new PLR()
            {
                GRP_INDX = indexes,
                GRP_MODE = modes.Select(x => (ushort)x).ToArray(),
                GRP_RADX = radixes.Select(x => (byte)x).ToArray(),
            });
        }

        public virtual void WritePinList(PLR record) 
        {
            int count = record.GRP_INDX.Length;

            // valid group modes
            ushort[] validModes = (ushort[])Enum.GetValues(typeof(PinGroupModes));

            // valid radix values
            byte[] validRadix = (byte[])Enum.GetValues(typeof(DisplayRadix));

            // get all pin map and pin group indexes defined
            var definedIndexes = GetPinMapIndexes(true);

            // set default state encoding strings if they are not passed in
            if (record.GRP_MODE?.Length == count && (record.PGM_CHAR == null || record.RTN_CHAR == null))
            {
                for (int i = 0; i < count; i++)
                {
                    // Set default program state representations
                    if (record.RTN_CHAR == null)
                    {
                        record.RTN_CHAR = new string[count];
                        record.RTN_CHAL = new string[count];
                        switch ((PinGroupModes)record.GRP_MODE[i])
                        {
                            case PinGroupModes.Normal:
                                record.RTN_CHAR[i] = "01LHMVXW";
                                record.RTN_CHAL[i] = "";
                                break;
                            case PinGroupModes.Unknown:
                                record.RTN_CHAR[i] = "";
                                record.RTN_CHAL[i] = "";
                                break;
                            case PinGroupModes.SCIO:
                            case PinGroupModes.SCIOMidband:
                            case PinGroupModes.SCIOValid:
                            case PinGroupModes.SCIOWindowSustain:
                                record.RTN_CHAR[i] = "0011LHMX";
                                record.RTN_CHAL[i] = "0111    ";
                                break;
                            case PinGroupModes.DualDrive:
                            case PinGroupModes.DualDriveMidband:
                            case PinGroupModes.DualDriveValid:
                            case PinGroupModes.DualDriveWindowSustain:
                                record.RTN_CHAR[i] = "LHMXLHMX";
                                record.RTN_CHAL[i] = "00001111";
                                break;
                            default:
                                break;
                        };
                    }
                }
                // Set default return state representations
                if (record.RTN_CHAR == null)
                {
                    for (int i = 0; i < record.GRP_MODE.Length; i++)
                    {
                        record.RTN_CHAR[i] = "01MGXLHMGOS";
                        record.RTN_CHAL[i] = "00000111111";
                    }
                }
            }
            else
            {
                // make sure count of indexes, modes, radixes, programStateEncodingChars, and returnStateEncodingChars are equal
                if (record.GRP_MODE?.Length != count || record.GRP_RADX?.Length != count || record.PGM_CHAR?.Length != count ||
                    record.RTN_CHAR?.Length != count)
                {
                    throw new STDFFormatException("STDF format exception.  Count of mode, radix, or encodings net equal to count of indexes,");
                }
            }

            // validate all values before writing the record
            for (int i = 0; i < record.GRP_INDX.Length; i++)
            {
                // make sure all indexes have a corresponding PMR or PGR defined already
                if (!definedIndexes.Contains(record.GRP_INDX[i]))
                {
                    throw new STDFFormatException(string.Format("STDF format exception.  Pin Map/Group index {0} at index {1} not defined.", record.GRP_INDX[i], i));
                }

                // validate group/pin operating mode
                if ((ushort)record.GRP_MODE[i] < 32768 && !validModes.Contains((ushort)record.GRP_MODE[i]))
                {
                    // mode not > 32767 or not one of the valid group modes
                    throw new ArgumentOutOfRangeException(string.Format("Invalid group operating mode '{0}' at index {1}.  Valid values are {2}", record.GRP_MODE[i], i, string.Join(',', validModes)));
                }

                // validate group/pin radix
                if (!validRadix.Contains((byte)record.GRP_RADX[i]))
                {
                    // radix is not valid
                    throw new ArgumentOutOfRangeException(string.Format("Invalid group display radix of '{0}' at index {1}.  Valid values are {2}.", record.GRP_RADX[i], i, string.Join(',', validRadix)));
                }
            }

            WriteRecord(record);
        }

        public virtual void WritePinMap(ushort index, ushort channelType, string channelName, string physicalName,
                              string logicalName, byte headNumber = 1, byte siteNumber = 1)
        {
            WritePinMap(new PMR()
            {
                PMR_INDX = index,
                CHAN_TYP = channelType,
                CHAN_NAM = channelName,
                PHY_NAM = physicalName,
                LOG_NAM = logicalName,
                HEAD_NUM = headNumber,
                SITE_NUM = siteNumber
            });
        }

        public virtual void WritePinMap(PMR record) 
        {
            if (record.PMR_INDX < 1 || record.PMR_INDX > 32767)
            {
                if (Records.FindIndex(x => (x as PMR)?.PMR_INDX == record.PMR_INDX) < 0)
                {
                    WriteRecord(record);
                }
                else
                {
                    throw new STDFFormatException("STDF format exception.  Duplicate pin map index.");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Pin map index out of range, must be > 1 and < 32,767");
            }

            WriteRecord(record);
        }

        public virtual void WriteRetestData(ushort[] retestBins)
        {
            WriteRetestData(new RDR() { RTST_BIN = retestBins });
        }

        public virtual void WriteRetestData(RDR record) 
        {
            WriteRecord(record);
        }

        public virtual void WriteSiteDescription(byte head, byte siteGroup, byte[] siteNumbers, string handlerType = "",
                                       string handlerId = "", string cardType = "", string cardId = "",
                                       string loadType = "", string loadId = "", string dibType = "", string dibId = "",
                                       string cableType = "", string cableId = "", string contactorType = "",
                                       string contactorId = "", string laserType = "", string laserId = "",
                                       string extraEqType = "", string extraEqId = "")
        {
            WriteSiteDescription(new SDR()
            {
                HEAD_NUM = head,
                SITE_GRP = siteGroup,
                SITE_NUM = siteNumbers,
                HAND_TYP = handlerType,
                HAND_ID = handlerId,
                CARD_TYP = cardType,
                CARD_ID = cardId,
                LOAD_TYP = loadType,
                LOAD_ID = loadId,
                DIB_TYP = dibType,
                DIB_ID = dibId,
                CABL_TYP = cableType,
                CABL_ID = cableId,
                CONT_TYP = contactorType,
                CONT_ID = contactorId,
                LASR_TYP = laserType,
                LASR_ID = laserId,
                EXTR_TYP = extraEqType,
                EXTR_ID = extraEqId
            });
        }

        public virtual void WriteSiteDescription(SDR record) 
        {
            // make sure site group is unique in the file
            if (Records.Exists(x => x is SDR && (x as SDR).SITE_GRP == record.SITE_GRP))
            {
                throw new STDFFormatException(
                    string.Format("STDF format exception.  Duplicate site group {0} found trying to add a new SDR record.",
                    record.SITE_GRP));
            };

            WriteRecord(record);
        }

        public virtual void WriteSoftBin(byte headNumber, byte siteNumber, ushort binNumber, char passFailIndication, string binName = "")
        {
            SBR bin = GetSoftBin(headNumber, siteNumber, binNumber);
            bin.SBIN_NAM = binName;
            bin.SBIN_PF = passFailIndication;

            WriteSoftBin(bin);
        }

        public virtual void WriteSoftBin(SBR record)
        {
            record.SBIN_PF = char.ToUpper(record.SBIN_PF);

            if (!"PF ".Contains(record.SBIN_PF))
            {
                throw new ArgumentOutOfRangeException("passFailIndication");
            }

            WriteRecord(record);
        }

        public virtual void WriteTestSynopsis(byte headNumber, byte siteNumber, uint testNumber, TestTypes testType, string testName = "")
        {
            WriteTestSynopsis(new TSR()
            {
                HEAD_NUM = headNumber,
                SITE_NUM = siteNumber,
                TEST_NUM = testNumber,
                TEST_TYP = TestTypeToChar(testType),
                TEST_NAM = testName
            });
        }

        public virtual void WriteTestSynopsis(TSR record)
        {
            TSR synopsis = _tsrRecords.Find(x => x.HEAD_NUM == record.HEAD_NUM &&
                                                 x.SITE_NUM == record.SITE_NUM &&
                                                 x.TEST_TYP == record.TEST_TYP &&
                                                 x.TEST_NUM == record.TEST_NUM);
            synopsis.TEST_NAM = record.TEST_NAM;

            if (synopsis.TEST_TIM != null)
            {
                // get average execution time.
                synopsis.TEST_TIM /= (float)synopsis.EXEC_CNT;
            }

            WriteRecord(synopsis);
        }

        public virtual void WriteTestSynopsisSiteSummary(byte siteNumber, uint testNumber, TestTypes testType)
        {

            WriteTestSynopsisSiteSummary(new TSR()
            {
                HEAD_NUM = 255,
                SITE_NUM = siteNumber,
                TEST_NUM = testNumber,
                TEST_TYP = TestTypeToChar(testType)
            });  
        }

        public virtual void WriteTestSynopsisSiteSummary(TSR record) 
        {
            var synopses = _tsrRecords.FindAll(x => x.SITE_NUM == record.SITE_NUM &&
                                                    x.TEST_TYP == record.TEST_TYP &&
                                                    x.TEST_NUM == record.TEST_NUM);

            foreach (TSR test in synopses)
            {
                record.EXEC_CNT += test.EXEC_CNT;
                record.FAIL_CNT += test.FAIL_CNT;
                record.ALRM_CNT += test.ALRM_CNT;
                if ((record.TEST_LBL ?? "").Length == 0) record.TEST_LBL = test.TEST_LBL;
                if ((record.TEST_NAM ?? "").Length == 0) record.TEST_LBL = test.TEST_NAM;
                if ((record.SEQ_NAM ?? "").Length == 0) record.SEQ_NAM = test.SEQ_NAM;
            }

            record.OPT_FLAG = (byte)TSROptionalData.AllOptionalDataIsValid;
            record.OPT_FLAG |= (byte)((record.TEST_MAX == null) ? TSROptionalData.TEST_MAX_Invalid : 0);
            record.OPT_FLAG |= (byte)((record.TEST_MIN == null) ? TSROptionalData.TEST_MIN_Invalid : 0);
            record.OPT_FLAG |= (byte)((record.TEST_TIM == null) ? TSROptionalData.TEST_TIM_Invalid : 0);
            record.OPT_FLAG |= (byte)((record.TST_SQRS == null) ? TSROptionalData.TST_SQRS_Invalid : 0);
            record.OPT_FLAG |= (byte)((record.TST_SUMS == null) ? TSROptionalData.TST_SUMS_Invalid : 0);

            WriteRecord(record);
        }

        public virtual void WriteRecord(ISTDFRecord record)
        {
            ISTDFRecord lastrecord = null;

            if (!STDFStream.CanWrite)
            {
                throw new InvalidOperationException("STDF output stream opened as read only, unable to write record.");
            }

            lastrecord = Records.Count > 0 ? Records[^1] : null;

            // no records can be added after MRR record is written
            if (lastrecord is MRR)
            {
                throw new STDFFormatException("STDF format exception.  MRR record is already written.  No other records can be added.");
            }

            // validate record is OK to add
            switch (record)
            {
                case FAR _:
                    // FAR must be first record in the file
                    if (Records.Count != 0)
                    {
                        throw new STDFFormatException("STDF format exception.  FAR record must be the first record in the file");
                    }
                    break;
                case ATR _:
                case MIR _:
                    // ATR and MIR must come immediately after FAR or other ATR records
                    if (!(lastrecord is FAR || lastrecord is ATR))
                    {
                        throw new STDFFormatException("STDF format exception.  ATR and MIR records must come after FAR or other ATR records.");
                    }
                    break;
                case RDR _:
                    // must come immediately after MIR record
                    if (!(lastrecord is MIR))
                    {
                        throw new STDFFormatException("STDF format exception.  RDR record must come after MIR and only one RDR record allowed per file.");
                    }
                    break;
                case SDR _:
                    // must come after MIR, RDR or other SDR records
                    if (!(lastrecord is MIR || lastrecord is RDR || lastrecord is SDR))
                    {
                        throw new STDFFormatException("STDF format exception.  SDR record must come after MIR, RDR or other SDR records.");
                    }
                    break;
                case ITestResult tr:
                    if (!IsValidTestResultSequence(tr))
                    {
                        throw new STDFFormatException("STDF format exception.  PTR/MPR/FTR records must come after corresponding PIR and before corresponding PRR");
                    }
                    break;
                case BPS bps:
                    if (GetLastPartInfo() == null)
                    {
                        throw new STDFFormatException("STDF format exception.  BPS record must come after a PIR and before an enclosing PRR");
                    }
                    else
                    {
                        _bpsRecords.Push(bps);
                    }
                    break;
                case EPS _:
                    if (_bpsRecords.Count > 0)
                    {
                        _bpsRecords.Pop();
                    }
                    else
                    {
                        throw new STDFFormatException("STDF format exception.  Mismatched end program sequence record.  No matching begin program sequence record found.");
                    }
                    break;
                default:
                    // all other records must come after initial sequence (FAR -> ATR* -> MIR -> RDR? -> SDR*) and before MRR
                    if (lastrecord is FAR || lastrecord is ATR)
                    {
                        throw new STDFFormatException("STDF format exception.  Invalid record sequence.  Record to be added must come after MIR, RDR, or SDR, and before MRR");
                    }
                    break;
            }

            // sequence valid, add the record
            Records.Add(record);

            // IF writeable, write the record to the stream
            if (STDFStream != null)
            {
                FileFormatter.SerializeRecord(record);
            }
        }

        protected bool IsValidTestResultSequence(ITestResult test)
        {
            bool isValid = false;

            // valid sequence must be after a corresponding PIR and before any corresponding PRR.
            // so search backwards from end of record list for a PIR matching the head and site number.  If 
            // we find a PRR with the same head and site number before we find a matching PIR, OR we do not
            // find a matching PIR. then the sequence is invalid.
            for (int i = Records.Count - 1; i >= 0; i--)
            {
                if (Records[i] is PRR prr)
                {
                    // found matching PRR before finding a matching PIR.  Sequence is invalid.
                    if (prr.HEAD_NUM == test.HEAD_NUM && prr.SITE_NUM == test.SITE_NUM)
                    {
                        break;
                    }
                }
                else if (Records[i] is PIR pir)
                {
                    // found matching PIR - sequence is valid
                    if (pir.HEAD_NUM == test.HEAD_NUM && pir.SITE_NUM == test.SITE_NUM)
                    {
                        isValid = true;
                        break;
                    }
                }
            }
            return isValid;
        }

        protected void AddPartUnderTest(byte headNumber, byte siteNumber)
        {
            PRR prr = GetPartUnderTest(headNumber, siteNumber);

            // already have a part under test for the given headnumber and sitenumber - error
            if (prr != null)
            {
                throw new STDFFormatException(string.Format("STDF format exception.  Part already under test for head {0} site {1}.", headNumber, siteNumber));
            }

            _partsUnderTest.Add(new PRR()
            {
                HEAD_NUM = headNumber,
                SITE_NUM = siteNumber
            });
        }

        protected PRR GetPartUnderTest(byte headNumber, byte siteNumber)
        {
            var prr = _partsUnderTest.Find(x => x.HEAD_NUM == headNumber &&
                                    x.SITE_NUM == siteNumber);
            return prr;
        }

        protected void RemovePartUnderTest(PRR partResult)
        {
            _partsUnderTest.Remove(partResult);
        }

        protected void UpdateTestSynopsis(ITestResult test, uint? testTimeInMilliseconds = null)
        {
            char type = test switch
            {
                PTR _ => 'P',
                MPR _ => 'M',
                FTR _ => 'F',
                _ => ' '
            };

            TSR synopsis = _tsrRecords.Find(x => x.HEAD_NUM == test.HEAD_NUM &&
                                      x.SITE_NUM == test.SITE_NUM &&
                                      x.TEST_TYP == type &&
                                      x.TEST_NUM == test.TEST_NUM);
            if (synopsis == null)
            {
                synopsis = new TSR()
                {
                    HEAD_NUM = test.HEAD_NUM,
                    SITE_NUM = test.SITE_NUM,
                    TEST_TYP = type,
                    TEST_NUM = test.TEST_NUM
                };
            }

            bool resultIsValid = (test.TEST_FLG & (byte)TestResultFlags.InvalidResult) == 0;
            bool testFailed = (test.TEST_FLG & (byte)TestResultFlags.NoPassFail) == 0 &&
                              (test.TEST_FLG & (byte)TestResultFlags.TestFailed) > 0;
            bool testExecuted = (test.TEST_FLG & (byte)TestResultFlags.TestNotExecuted) == 0;
            bool testAlarmed = (test.TEST_FLG & (byte)TestResultFlags.AlarmDetected) > 0;
            if ((synopsis.TEST_LBL ?? "").Length == 0) synopsis.TEST_LBL = test.TEST_TXT;
            if ((synopsis.SEQ_NAM ?? "").Length == 0 && _bpsRecords.Count > 0) synopsis.SEQ_NAM = _bpsRecords.Peek().SEQ_NAME;
            synopsis.EXEC_CNT += (uint)(testExecuted ? 0 : 1);
            synopsis.FAIL_CNT += (uint)(testFailed ? 1 : 0);
            synopsis.ALRM_CNT += (uint)(testAlarmed ? 1 : 0);
            if (testTimeInMilliseconds != null && synopsis.TEST_TIM == null)
            {
                synopsis.TEST_TIM = 0;
            }
            synopsis.TEST_TIM += (float)testTimeInMilliseconds / 1000F;

            if (test is PTR ptr)
            {
                synopsis.TEST_TYP = 'P';
                if (resultIsValid)
                {
                    if (synopsis.TEST_MAX < ptr.RESULT) synopsis.TEST_MAX = ptr.RESULT;
                    if (synopsis.TEST_MIN > ptr.RESULT) synopsis.TEST_MIN = ptr.RESULT;
                    synopsis.TST_SUMS += ptr.RESULT;
                    synopsis.TST_SQRS += (ptr.RESULT * ptr.RESULT);
                }
            }
            else if (test is MPR mpr)
            {
                synopsis.TEST_TYP = 'M';
                if (resultIsValid)
                {
                    if (synopsis.TEST_MAX < mpr.RTN_RSLT?.Max()) synopsis.TEST_MAX = mpr.RTN_RSLT.Max();
                    if (synopsis.TEST_MIN > mpr.RTN_RSLT?.Min()) synopsis.TEST_MIN = mpr.RTN_RSLT.Min();
                }
            }
        }

        protected bool ValidateOptionalData<T>(T record, T defaultValues, string propertyName, bool hasMissingData) where T : ITestResult
        {
            return ValidateOptionalData(record, defaultValues, propertyName, 0, hasMissingData, (x => x == null));
        }

        protected bool ValidateOptionalData<T>(T record, T defaultValues, string propertyName, bool hasMissingData, Func<T, bool> missingDataComparer) where T : ITestResult
        {
            return ValidateOptionalData(record, defaultValues, propertyName, 0, hasMissingData, missingDataComparer);
        }

        protected bool ValidateOptionalData<T>(T record, T defaultValues, string propertyName, byte invalidFlag, bool hasMissingData) where T : ITestResult
        {
            return ValidateOptionalData(record, defaultValues, propertyName, invalidFlag, hasMissingData, (x => x == null));
        }

        protected bool ValidateOptionalData<T>(T record, T defaultValues, string propertyName, byte invalidFlag, bool hasMissingData, Func<T, bool> missingDataComparer) where T : ITestResult
        {
            // get the value of the property or null if property does not exist on record
            PropertyInfo propInfo = record.GetType().GetProperty(propertyName);
            object defaultValue;
            object propValue;

            if (propInfo != null)
            {
                propValue = propInfo.GetValue(record);
                defaultValue = propInfo.GetValue(defaultValues);

                if (propValue != null && (missingDataComparer != null && !missingDataComparer(record)))
                {
                    // we have a valid non-empty value, check if previous optional data was missing 
                    if (hasMissingData)
                    {
                        // We have a valid value for the field but we have previous missing/optional data.
                        // This violates the STDF spec (no valid data after missing/invalid data in the record).
                        // We can make this valid IF (1) we are allowed to coerce the data to be valid (set it to null) 
                        // OR (2) the current value is the same as the default value meaning we can omit the field
                        if (CoerceInvalidValues || propValue == defaultValue)
                        {
                            record.OPT_FLAG |= invalidFlag;
                            propInfo.SetValue(record, null);
                        }
                        else
                        {
                            // unable to make the field to be valid for the current state of the record, so throw an exception.
                            throw new STDFFormatException("STDF format exception.  Valid value found in optional data after invalid values.");
                        }
                    }
                }
                else
                {
                    // if prop value is null, set the bit corresponding to the missing/invalid value
                    record.OPT_FLAG |= invalidFlag;
                    hasMissingData = true;
                }
            }
            return hasMissingData;
        }

        protected char TestTypeToChar(TestTypes testType)
        {
            return "PMF"[(int)testType];
        }

        protected void UpdateTestBin<T>(byte headNumber, byte siteNumber, ushort binNumber) where T : ITestBin, new()
        {
            ITestBin bin = _binRecords.Find(x => x is T &&
                                             x.HEAD_NUM == headNumber &&
                                             x.SITE_NUM == siteNumber &&
                                             x.BIN_NUM == binNumber);
            if (bin == null)
            {
                bin = new T()
                {
                    HEAD_NUM = headNumber,
                    SITE_NUM = siteNumber,
                    BIN_NUM = binNumber
                };
                _binRecords.Add(bin);
            }
            bin.BIN_CNT++;

            // Update the all site summary bin record 
            if (bin.HEAD_NUM != 255)
            {
                UpdateTestBin<T>(255, 0, binNumber);
            }
        }

        public void Close()
        {
            FileFormatter.EndStreamingSerialization();
            STDFStream?.Close();
        }

        #region IDisposable Support
        protected bool disposedValue = false; // To detect redundant calls

        // This code added to correctly implement the disposable pattern.
        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    Close();
                    STDFStream?.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion IDisposable

    }
}
