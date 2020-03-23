using System;
using System.Collections.Generic;

namespace STDFLib
{
    /// <summary>
    /// Pin List Record
    /// </summary>
    public class PLR : STDFRecord
    {
        private ushort _grp_cnt = 0;
        private List<ushort> _grp_indx = new List<ushort>();
        private List<ushort> _grp_mode = new List<ushort>();
        private List<byte>   _grp_radx = new List<byte>();
        private List<string> _pgm_char = new List<string>();
        private List<string> _rtn_char = new List<string>();
        private List<string> _pgm_chal = new List<string>();
        private List<string> _rtn_chal = new List<string>();

        public PLR() : base(RecordTypes.PLR, "Pin List Record") { }

        [STDF] public ushort GRP_CNT
        {
            get => (ushort)_grp_indx.Count;

            set
            {
                _grp_cnt = value;
                GRP_INDX = new ushort[value];
                GRP_MODE = new ushort[value];
                GRP_RADX = new byte[value];
                PGM_CHAR = new string[value];
                RTN_CHAR = new string[value];
                PGM_CHAL = new string[value];
                RTN_CHAL = new string[value];

            }
        }

        [STDF("GRP_CNT")] public ushort[] GRP_INDX
        {
            get => _grp_indx.ToArray();
            set
            {
                _grp_indx.Clear();
                _grp_indx.AddRange(value);
            }
        }

        [STDF("GRP_CNT")] public ushort[] GRP_MODE
        {
            get => _grp_indx.ToArray();
            set
            {
                _grp_indx.Clear();
                _grp_indx.AddRange(value);
            }
        }

        [STDF("GRP_CNT")] public byte[] GRP_RADX
        {
            get => _grp_radx.ToArray();
            set
            {
                _grp_radx.Clear();
                _grp_radx.AddRange(value);
            }
        }

        [STDF("GRP_CNT")] public string[] PGM_CHAR
        {
            get => _pgm_char.ToArray();
            set
            {
                _pgm_char.Clear();
                _pgm_char.AddRange(value);
            }
        }

        [STDF("GRP_CNT")] public string[] RTN_CHAR
        {
            get => _rtn_char.ToArray();
            set
            {
                _rtn_char.Clear();
                _rtn_char.AddRange(value);
            }
        }

        [STDF("GRP_CNT")] public string[] PGM_CHAL
        {
            get => _pgm_chal.ToArray();
            set
            {
                _pgm_chal.Clear();
                _pgm_chal.AddRange(value);
            }
        }

        [STDF("GRP_CNT")] public string[] RTN_CHAL
        {
            get => _rtn_chal.ToArray();
            set
            {
                _rtn_chal.Clear();
                _rtn_chal.AddRange(value);
            }
        }

        public int Add(ushort index, ushort pinGroupMode=0, byte displayRadix=0, string programStateEncodingCharsRight="", string returnStateEncodingCharsRight="", string programStateEncodingCharsLeft="", string returnStateEncodingCharsLeft="")
        {
            // Validate the pin group mode
            switch (pinGroupMode)
            {
                case (ushort)PinGroupModes.Unknown:
                case (ushort)PinGroupModes.Normal:
                case (ushort)PinGroupModes.SCIO:
                case (ushort)PinGroupModes.SCIOMidband:
                case (ushort)PinGroupModes.SCIOValid:
                case (ushort)PinGroupModes.SCIOWindowSustain:
                case (ushort)PinGroupModes.DualDrive:
                case (ushort)PinGroupModes.DualDriveMidband:
                case (ushort)PinGroupModes.DualDriveValid:
                case (ushort)PinGroupModes.DualDriveWindowSustain:
                    break;
                default:
                    if (pinGroupMode < 32768)
                    {
                        throw new ArgumentException(string.Format("Unsupported Pin Group Mode value.  Value received was '{0}'.", pinGroupMode));
                    }
                    break;
            }

            // Validate the display radix
            switch (displayRadix)
            {
                case (byte)DisplayRadix.ProgramDefault:
                case (byte)DisplayRadix.Binary:
                case (byte)DisplayRadix.Octal:
                case (byte)DisplayRadix.Hexadecimal:
                case (byte)DisplayRadix.Decimal:
                case (byte)DisplayRadix.Symbolic:
                    break;
                default:
                    throw new ArgumentException(string.Format("Unsupported Display Radix value.  Value received was '{0}'.", displayRadix));
            }

            // Values are valid, so add to the arrays.
            _grp_indx.Add(index);
            _grp_mode.Add(pinGroupMode);
            _grp_radx.Add(displayRadix);
            _pgm_char.Add(programStateEncodingCharsRight);
            _rtn_char.Add(returnStateEncodingCharsRight);
            _pgm_char.Add(programStateEncodingCharsLeft);
            _rtn_char.Add(returnStateEncodingCharsLeft);

            // Return the index that the new values were added
            return _grp_indx.Count - 1;
        }
    }
}
