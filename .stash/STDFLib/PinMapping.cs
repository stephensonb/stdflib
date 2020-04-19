using System;
using System.Collections.Generic;

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
    public class PinMapping : IPinMapping
    {
        ISTDFFile STDF { get; set; }

        public PinMapping(ISTDFFile file)
        {
            STDF = file;
        }

        public void AddPin(
            ushort index,
            ushort channelType,
            string channelName,
            string physicalPinName,
            string logicalPinName
            )
        {
            AddPin(1, 1, index, channelType, channelName, physicalPinName, logicalPinName);
        }

        public void AddPin(
            byte headNumber,
            byte siteNumber,
            ushort index,
            ushort channelType,
            string channelName,
            string physicalPinName,
            string logicalPinName
            )
        {
            if (index > 32767)
            {
                throw new IndexOutOfRangeException(string.Format("Invalid pin index of {0}.  Valid indexes are 0 to 32,767", index));
            }

            if (STDF.HasOne<PMR>(x => x.PMR_INDX == index))
            {
                throw new DuplicateIndexException("Duplicate pin index = " + index);
            }

            STDF.WriteRecord(new PMR()
            {
                HEAD_NUM = headNumber,
                SITE_NUM = siteNumber,
                PMR_INDX = index,
                CHAN_TYP = channelType,
                CHAN_NAM = channelName,
                PHY_NAM = physicalPinName,
                LOG_NAM = logicalPinName
            });
        }

        public void AddPinGroup(
            ushort index,
            string groupName,
            params ushort[] pinIndexes
            )
        {
            if (index < 32768)
            {
                throw new IndexOutOfRangeException(string.Format("Invalid pin group index of {0}.  Valid indexes are 32,768 to 65,535", index));
            }

            List<ushort> undefinedPins = new List<ushort>();

            // check for duplicate group
            if (STDF.HasOne<PGR>(x => x.GRP_INDX == index))
            {
                throw new DuplicateIndexException("Duplicate group index = " + index);
            }

            // check for pins/groups that are referenced before they are defined.
            foreach (var pin in pinIndexes)
            {
                if (pin < 32768)
                {
                    if (!STDF.HasOne<PMR>(x => x.PMR_INDX == pin))
                    {
                        undefinedPins.Add(pin);
                    }
                }
                else
                {
                    if (!STDF.HasOne<PGR>(x => x.GRP_INDX == pin))
                    {
                        undefinedPins.Add(pin);
                    }
                }
            }
            if (undefinedPins.Count > 0)
            {
                throw new UndefinedPinOrGroupException("Undefined pin(s) or group(s) = " + string.Join(',', undefinedPins));
            }

            STDF.WriteRecord(new PGR() { GRP_INDX = index, GRP_NAM = groupName, PMR_INDX = pinIndexes });
        }

        public void AddPinList(
            ushort[] indexes,
            PinGroupModes[] modes,
            DisplayRadix[] radixes,
            string[] programStateEncodingR = null,
            string[] programStateEncodingL = null,
            string[] returnStateEncodingR = null,
            string[] returnStateEncodingL = null
            )
        {
            // check for pins/groups that are referenced before they are defined.
            List<ushort> undefinedPins = new List<ushort>();
            foreach (var pin in indexes)
            {
                if (pin < 32768)
                {
                    if (!STDF.HasOne<PMR>(x => x.PMR_INDX == pin))
                    {
                        undefinedPins.Add(pin);
                    }
                }
                else
                {
                    if (!STDF.HasOne<PGR>(x => x.GRP_INDX == pin))
                    {
                        undefinedPins.Add(pin);
                    }
                }
            }
            if (undefinedPins.Count > 0)
            {
                throw new UndefinedPinOrGroupException("Undefined pin(s) or group(s) = " + string.Join(',', undefinedPins));
            }

            // set default state encoding strings if they are not passed in
            if (indexes.Length == modes.Length &&
                indexes.Length == radixes.Length &&
               (indexes.Length == programStateEncodingR?.Length || programStateEncodingR == null) &&
               (indexes.Length == returnStateEncodingR?.Length || returnStateEncodingL == null))
            {
                // Set default program state representations
                if (programStateEncodingR == null)
                {
                    programStateEncodingR = new string[indexes.Length];
                    programStateEncodingL = new string[indexes.Length];
                    for (int i = 0; i < modes.Length; i++)
                    {
                        switch (modes[i])
                        {
                            case PinGroupModes.Normal:
                                programStateEncodingR[i] = "01LHMVXW";
                                programStateEncodingL[i] = "";
                                break;
                            case PinGroupModes.Unknown:
                                programStateEncodingR[i] = "";
                                programStateEncodingL[i] = "";
                                break;
                            case PinGroupModes.SCIO:
                            case PinGroupModes.SCIOMidband:
                            case PinGroupModes.SCIOValid:
                            case PinGroupModes.SCIOWindowSustain:
                                programStateEncodingR[i] = "0011LHMX";
                                programStateEncodingL[i] = "0111    ";
                                break;
                            case PinGroupModes.DualDrive:
                            case PinGroupModes.DualDriveMidband:
                            case PinGroupModes.DualDriveValid:
                            case PinGroupModes.DualDriveWindowSustain:
                                programStateEncodingR[i] = "LHMXLHMX";
                                programStateEncodingL[i] = "00001111";
                                break;
                            default:
                                break;
                        };
                    }
                }

                // Set default return state representations
                if (returnStateEncodingR == null)
                {
                    returnStateEncodingR = new string[indexes.Length];
                    returnStateEncodingL = new string[indexes.Length];
                    for (int i = 0; i < modes.Length; i++)
                    {
                        returnStateEncodingR[i] = "01MGXLHMGOS";
                        returnStateEncodingL[i] = "00000111111";
                    }
                }
            }

            STDF.WriteRecord(new PLR()
            {
                GRP_INDX = indexes,
                GRP_MODE = Array.ConvertAll(modes, mode => Convert.ToUInt16(mode)),
                GRP_RADX = Array.ConvertAll(radixes, radix => Convert.ToByte(radix)),
                PGM_CHAR = programStateEncodingR,
                PGM_CHAL = programStateEncodingL,
                RTN_CHAR = returnStateEncodingR,
                RTN_CHAL = returnStateEncodingL
            });
        }
    }
}
