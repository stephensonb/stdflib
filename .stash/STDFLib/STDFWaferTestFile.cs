using System;
using System.Collections.Generic;
using System.IO;

namespace STDFLib2.Serialization
{
    public class STDFWaferTestFile : STDFFile, ISTDFWaferTestFile
    {
        bool _wcrWritten = false;

        protected STDFWaferTestFile(string path, FileMode mode, FileAccess access, bool coerceInvalidValues = true) : base(path, mode, access, coerceInvalidValues)
        {
        }

        public override void WriteRecord(ISTDFRecord record)
        {
            switch (record)
            {
                case WCR _:
                    if (_wcrWritten)
                    {
                        throw new STDFFormatException("STDF file format exception.  Wafer configuration record already written to file.");
                    }
                    break;
            }

            base.WriteRecord(record);
        }

        public void WriteWaferConfiguration(float waferSize = 0, WaferUnits sizeUnits = 0, float dieHeight = 0,
                                          float dieWidth = 0, WaferDirections flatOrientation = WaferDirections.Unknown,
                                          short centerX = -32768, short centerY = -32768,
                                          WaferDirections positiveX = WaferDirections.Unknown,
                                          WaferDirections positiveY = WaferDirections.Unknown)
        {
            string directionCodes = " UDLR";

            WriteRecord(new WCR()
            {
                WAFR_SIZ = waferSize,
                WF_UNITS = (byte)sizeUnits,
                DIE_HT = dieHeight,
                DIE_WID = dieWidth,
                WF_FLAT = directionCodes[(int)flatOrientation],
                CENTER_X = centerX,
                CENTER_Y = centerY,
                POS_X = directionCodes[(int)positiveX],
                POS_Y = directionCodes[(int)positiveY]
            });

            _wcrWritten = true;
        }

        public void WriteWaferInfo(DateTime startTime, byte headNumber, byte siteGroup = 255, string waferId = "")
        {
            // make sure no wafer under test with same head number and site group
            if (GetLastWaferInfo(headNumber, siteGroup) != null)
            {
                throw new STDFFormatException("STDF format exception.  File has previous WIR with no matching WRR.");
            }

            WIR wir = new WIR() { START_T = startTime, HEAD_NUM = headNumber, SITE_GRP = siteGroup, WAFER_ID = waferId };

            // Add to the record collection and write to the stream
            WriteRecord(wir);
        }

        public void WriteWaferResults(DateTime finishTime, byte headNumber, byte siteGroup = 255, string waferId = "",
                                 string fabWaferId = "", string waferFrameId = "", string waferMaskId = "",
                                 string userWaferDescription = "", string executiveWaferDescription = "")
        {
            // make sure no wafer under test with same head number and site group
            if (GetLastWaferInfo(headNumber, siteGroup) == null)
            {
                throw new STDFFormatException("STDF format exception, mismatched end wafer test.  Trying to end wafer testing without corresponding start wafer test.");
            }

            WRR wrr = new WRR()
            {
                HEAD_NUM = headNumber,
                SITE_GRP = siteGroup,
                FINISH_T = finishTime,
                WAFER_ID = waferId,
                FABWF_ID = fabWaferId,
                FRAME_ID = waferFrameId,
                MASK_ID = waferMaskId,
                USR_DESC = userWaferDescription,
                EXC_DESC = executiveWaferDescription
            };

            // Add the record to the record collection and write to the output stream
            WriteRecord(wrr);
        }

        protected WIR GetLastWaferInfo(byte headNumber, byte siteGroup = 255)
        {
            for (int i = Records.Count - 1; i > 0; i++)
            {
                if (Records[i] is WRR wrr && wrr.HEAD_NUM == headNumber && wrr.SITE_GRP == siteGroup)
                {
                    break;
                }
                if (Records[i] is WIR wir && wir.HEAD_NUM == headNumber && wir.SITE_GRP == siteGroup)
                {
                    return wir;
                }
            }
            return null;
        }

        protected WRR GetLastWaferResults(byte headNumber, byte siteGroup)
        {
            for (int i = Records.Count - 1; i > 0; i++)
            {
                if (Records[i] is WIR wir && wir.HEAD_NUM == headNumber && wir.SITE_GRP == siteGroup)
                {
                    break;
                }
                if (Records[i] is WRR wrr && wrr.HEAD_NUM == headNumber && wrr.SITE_GRP == siteGroup)
                {
                    return wrr;
                }
            }
            return null;
        }

        public IEnumerable<IPartData> GetWaferParts(byte headNumber, byte siteNumber)
        {
            List<IPartData> parts = new List<IPartData>();
            IPartData partData;
            int startIndex = 0;

            do
            {
                partData = GetPart(headNumber, siteNumber, out int endIndex, startIndex);
                if (partData != null)
                {
                    parts.Add(partData);
                    startIndex = endIndex;
                }
            } while (partData != null);

            return parts;
        }

        public IPartData GetWaferPart(byte headNumber, byte siteNumber, int startIndex = 0)
        {
            return GetPart(headNumber, siteNumber, out _, startIndex);
        }

        protected IPartData GetPart(byte headNumber, byte siteNumber, out int endIndex, int startIndex = 0)
        {
            PartData partData = null;
            endIndex = 0;

            if (startIndex < 0 || startIndex > Records.Count)
            {
                throw new IndexOutOfRangeException("Starting record index out of range (less than zero or greater than number of records.");
            }

            for (int i = startIndex; i < Records.Count; i++)
            {
                ISTDFRecord record = Records[i];
                string typeName = Records[i].GetType().Name;

                switch (typeName)
                {
                    case "PIR":
                        if (partData == null)
                        {
                            var pir = record as PIR;
                            if (pir.HEAD_NUM == headNumber && pir.SITE_NUM == siteNumber)
                            {
                                partData = new PartData(pir);
                            }
                        }
                        else
                        {
                            throw new STDFFormatException(string.Format("STDF file format exception.  Unexpected record type {0} found", typeName));
                        }
                        break;
                    case "PTR":
                    case "MPR":
                    case "FTR":
                        if (partData != null)
                        {
                            var tr = record as ITestResult;
                            if (tr.HEAD_NUM == partData.PartInfo.HEAD_NUM && tr.SITE_NUM == partData.PartInfo.SITE_NUM)
                            {
                                partData.TestResults.Add(tr);
                            }
                        }
                        else
                        {
                            throw new STDFFormatException(string.Format("STDF file format exception.  Unexpected record type {0} found", typeName));
                        }
                        break;
                    case "PRR":
                        if (partData != null)
                        {
                            var prr = record as PRR;
                            if (prr.HEAD_NUM == partData.PartInfo.HEAD_NUM && prr.SITE_NUM == partData.PartInfo.SITE_NUM)
                            {
                                // set the part result record for the current part
                                partData.PartResults = prr;
                            }
                        }
                        else
                        {
                            throw new STDFFormatException(string.Format("STDF file format exception.  Unexpected record type {0} found", typeName));
                        }
                        break;
                }
                // stop searching if wafer results record found
                if (partData?.PartResults != null)
                {
                    endIndex = i;
                    break;
                }
            }
            return partData;
        }

        public IEnumerable<IWaferData> GetWafers(byte headNumber, byte siteGroup = 255)
        {
            List<IWaferData> wafers = new List<IWaferData>();
            IWaferData waferData;
            int startIndex = 0;

            do
            {
                waferData = GetWafer("", headNumber, siteGroup, startIndex, out int endIndex);
                if (waferData != null)
                {
                    wafers.Add(waferData);
                    startIndex = endIndex;
                }
            } while (waferData != null);

            return wafers;
        }

        public IWaferData GetWafer(string waferId, byte headNumber, byte siteGroup = 255, int startIndex = 0)
        {
            return GetWafer(waferId, headNumber, siteGroup, startIndex, out _);
        }

        protected IWaferData GetWafer(string waferId, byte headNumber, byte siteGroup, int startIndex, out int endIndex)
        {
            IWaferData waferData = null;
            SDR siteDescription = null;
            IPartData partData = null;
            endIndex = 0;

            if (startIndex < 0 || startIndex > Records.Count)
            {
                throw new IndexOutOfRangeException("Starting record index out of range (less than zero or greater than number of records.");
            }

            for (int i = startIndex; i < Records.Count; i++)
            {
                switch (Records[i])
                {
                    case SDR sdr:
                        // save sdr for matching site group
                        if (siteDescription == null)
                        {
                            if (sdr.SITE_GRP == siteGroup)
                            {
                                siteDescription = sdr;
                            }
                        }
                        break;
                    case WIR wir:
                        if (waferData == null)
                        {
                            if (wir.HEAD_NUM == headNumber && wir.SITE_GRP == siteGroup)
                            {
                                if (waferId == "" || wir.WAFER_ID == waferId)
                                {
                                    waferData = new WaferData() { Info = wir };
                                }
                            }
                        }
                        else
                        {
                            throw new STDFFormatException(string.Format("STDF file format exception.  Unexpected record type {0} found", Records[i].GetType().Name));
                        }
                        break;
                    case PIR pir:
                        if (waferData != null && partData == null)
                        {
                            partData = GetPart(pir.HEAD_NUM, pir.SITE_NUM, out endIndex, i);
                            if (partData?.PartResults != null)
                            {
                                waferData.Parts.Add(partData);
                                i = endIndex;
                            }
                        }
                        else
                        {
                            throw new STDFFormatException(string.Format("STDF file format exception.  Unexpected record type {0} found", Records[i].GetType().Name));
                        }
                        break;
                    case WRR wrr:
                        if (waferData != null && partData == null)
                        {
                            if (wrr.HEAD_NUM == waferData.Info.HEAD_NUM && wrr.SITE_GRP == waferData.Info.SITE_GRP)
                            {
                                waferData.Results = wrr;
                            }
                        }
                        else
                        {
                            throw new STDFFormatException(string.Format("STDF file format exception.  Unexpected record type {0} found", Records[i].GetType().Name));
                        }
                        break;
                }
                // stop searching if wafer results record found
                if (waferData?.Results != null)
                {
                    endIndex = i;
                    break;
                }
            }
            return waferData;
        }

    }
}
