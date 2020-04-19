using System;
using System.Collections.Generic;
using System.Text;
using STDFLib2.Serialization;

namespace STDFLib2.STDFRecords
{
    /*
     * File
     *      FAR->ATR|MIR
     *      ATR->MIR
     *      MIR->RDR|SDR
     *      RDR
     *      Head->SiteGroup->SDR->GDR|DTR
     *      Head->SiteGroup->PMR
     *      WCR
     *      PCR
     *      PGR
     *      PLR
     *      Head->SiteGroup->SDR
     *      Head->SiteGroup->WIR->Site->PIR->(PTR/MPR/FTR)->PRR->WRR
     *      Head->SiteGroup->Site->PIR->BPS->(PTR/MPR/FTR)->EPS->PRR
     *      
     *      Wafer1 = Head(1,0).Wafer("0");
     *      Wafer1.StartTest();
     *      Part1 = Wafer1.Part();
     *      Part1.StartTest();
     *      Part1.ParametricTestResults.zzzz;
     *      Part2.MultiresultParametricTestResults.zzzz;
     *      Part2.FunctionalTestResults.xxxxx;
     *      Part2.EndTest();
     *      Wafer1.Results().
     *      Wafer1.Results.Write();
     *      
     *      
     * 
     */

    public enum MIRFieldIndexes : int
    {
        SETUP_T = 1,
        START_ = 2,
        STAT_NUM = 3,
        MODE_COD = 4,
        RTST_COD = 5,
        PROT_COD = 6,
        BURN_TIM = 7,
        CMOD_COD = 8,
        LOT_ID = 9,
        PART_TYP = 10,
        NODE_NAM = 11,
        TSTR_TYP = 12,
        JOB_NAM = 13,
        JOB_REV = 14,
        SBLOT_ID = 15,
        OPER_NAM = 16,
        EXEC_TYP = 17,
        EXEC_VER = 18,
        TEST_COD = 19,
        TST_TEMP = 20,
        USER_TXT = 21,
        AUX_FILE = 22,
        PKG_TYP = 23,
        FAMLY_ID = 24,
        DATE_COD = 25,
        FACIL_ID = 26,
        FLOOR_ID = 27,
        PROC_ID = 28,
        OPER_FRQ = 29,
        SPEC_NAM = 30,
        SPEC_VER = 31,
        FLOW_ID = 32,
        SETUP_ID = 33,
        DSGN_REV = 34,
        ENG_ID = 35,
        ROM_COD = 36,
        SERL_NUM = 37,
        SUPR_NAM = 38
    }

    public struct STDFFieldSet
    {
        public long ObjectId;
        public Type RecordType;
        public object[] Fields;
    }

    public class STDFFile
    {
        private Dictionary<long, STDFFieldSet> Records = new Dictionary<long, STDFFieldSet>();
        private static long NextObjectId = 0;

        public STDFFile()
        {

        }

        public long GenerateObjectId()
        {
            return NextObjectId++;
        }

        public ISTDFRecord GetRecord(long objectId)
        {
            return CreateRecord(Records[objectId]);
        }

        public ISTDFRecord CreateRecord(STDFFieldSet fieldSet)
        {
            var record = CreateRecord(fieldSet.RecordType);
            return STDFFormatterServices.PopulateRecord(record, fieldSet);
        }

        public ISTDFRecord CreateRecord(Type recordType)
        {
            var properties = STDFFormatterServices.GetSerializeableProperties(recordType);
            var record = STDFFormatterServices.CreateSTDFRecord(recordType);

            var fieldSet = new STDFFieldSet();
            fieldSet.Fields = new object[properties.Length];
            fieldSet.ObjectId = GenerateObjectId();
            fieldSet.RecordType = recordType;

            for (int i = 0; i < properties.Length; i++)
            {
                fieldSet.Fields[i] = properties[i].GetValue(record);
            }

            Records.Add(fieldSet.ObjectId, fieldSet);

            return record;
        }
    }

    public class MasterInformation : STDFRecord
    { 
        public MasterInformation() : base(38)
        {
            // initialize default values
            SETUP_T = DateTime.UnixEpoch;
            START_T = DateTime.UnixEpoch;
            STAT_NUM = 0;
            MODE_COD = ' ';
            RTST_COD = ' ';
            PROT_COD = ' ';
            BURN_TIM = 65535;
            CMOD_COD = ' ';
            LOT_ID = "";
            PART_TYP = "";
            NODE_NAM = "";
            TSTR_TYP = "";
            JOB_NAM = "";
            JOB_REV = "";
            SBLOT_ID = "";
            OPER_NAM = "";
            EXEC_TYP = "";
            EXEC_VER = "";
            TEST_COD = "";
            TST_TEMP = "";
            USER_TXT = "";
            AUX_FILE = "";
            PKG_TYP = "";
            FAMLY_ID = "";
            DATE_COD = "";
            FACIL_ID = "";
            FLOOR_ID = "";
            PROC_ID = "";
            OPER_FRQ = "";
            SPEC_NAM = "";
            SPEC_VER = "";
            FLOW_ID = "";
            SETUP_ID = "";
            DSGN_REV = "";
            ENG_ID = "";
            ROM_COD = "";
            SERL_NUM = "";
            SUPR_NAM = "";
        }
        
        public long ObjectId { get => _object_id; }

        public DateTime SETUP_T { get => (DateTime)_record_data[0]; set => _record_data[0] = value; }
        public DateTime START_T { get => (DateTime)_record_data[1]; set => _record_data[1] = value; }
        public byte STAT_NUM { get => (byte)_record_data[2]; set => _record_data[2] = value; }
        public char MODE_COD { get => (char)_record_data[3]; set => _record_data[3] = value; }
        public char RTST_COD { get => (char)_record_data[4]; set => _record_data[4] = value; }
        public char PROT_COD { get => (char)_record_data[5]; set => _record_data[5] = value; }
        public ushort BURN_TIM { get => (ushort)_record_data[6]; set => _record_data[6] = value; }
        public char CMOD_COD { get => (char)_record_data[7]; set => _record_data[7] = value; }
        public string LOT_ID { get => (string)_record_data[8]; set => _record_data[8] = value; }
        public string PART_TYP { get => (string)_record_data[9]; set => _record_data[9] = value; }
        public string NODE_NAM { get => (string)_record_data[10]; set => _record_data[10] = value; }
        public string TSTR_TYP { get => (string)_record_data[11]; set => _record_data[11] = value; }
        public string JOB_NAM  { get => (string)_record_data[12]; set => _record_data[12] = value; }
        public string JOB_REV  { get => (string)_record_data[13]; set => _record_data[13] = value; }
        public string SBLOT_ID { get => (string)_record_data[14]; set => _record_data[14] = value; }
        public string OPER_NAM { get => (string)_record_data[15]; set => _record_data[15] = value; }
        public string EXEC_TYP { get => (string)_record_data[16]; set => _record_data[16] = value; }
        public string EXEC_VER { get => (string)_record_data[17]; set => _record_data[17] = value; }
        public string TEST_COD { get => (string)_record_data[18]; set => _record_data[18] = value; }
        public string TST_TEMP { get => (string)_record_data[19]; set => _record_data[19] = value; }
        public string USER_TXT { get => (string)_record_data[20]; set => _record_data[20] = value; }
        public string AUX_FILE { get => (string)_record_data[21]; set => _record_data[21] = value; }
        public string PKG_TYP  { get => (string)_record_data[22]; set => _record_data[22] = value; }
        public string FAMLY_ID { get => (string)_record_data[23]; set => _record_data[23] = value; }
        public string DATE_COD { get => (string)_record_data[24]; set => _record_data[24] = value; }
        public string FACIL_ID { get => (string)_record_data[25]; set => _record_data[25] = value; }
        public string FLOOR_ID { get => (string)_record_data[26]; set => _record_data[26] = value; }
        public string PROC_ID  { get => (string)_record_data[27]; set => _record_data[27] = value; }
        public string OPER_FRQ { get => (string)_record_data[28]; set => _record_data[28] = value; }
        public string SPEC_NAM { get => (string)_record_data[29]; set => _record_data[29] = value; }
        public string SPEC_VER { get => (string)_record_data[30]; set => _record_data[30] = value; }
        public string FLOW_ID  { get => (string)_record_data[31]; set => _record_data[31] = value; }
        public string SETUP_ID { get => (string)_record_data[32]; set => _record_data[32] = value; }
        public string DSGN_REV { get => (string)_record_data[33]; set => _record_data[33] = value; }
        public string ENG_ID   { get => (string)_record_data[34]; set => _record_data[34] = value; }
        public string ROM_COD  { get => (string)_record_data[35]; set => _record_data[35] = value; }
        public string SERL_NUM { get => (string)_record_data[36]; set => _record_data[36] = value; }
        public string SUPR_NAM { get => (string)_record_data[37]; set => _record_data[37] = value; }
    }
}
