using System;
using System.Collections.Generic;

namespace STDFLib
{
    /// <summary>
    /// Pin List Record
    /// </summary>
    public class PLR : STDFRecord
    {
        private List<ushort> _grp_indx = new List<ushort>();
        private List<ushort> _grp_mode = new List<ushort>();
        private List<byte>   _grp_radx = new List<byte>();
        private List<string> _pgm_char = new List<string>();
        private List<string> _rtn_char = new List<string>();
        private List<string> _pgm_chal = new List<string>();
        private List<string> _rtn_chal = new List<string>();

        protected override RecordType TypeCode => 0x0163;

        [STDF(Order = 1)]
        public ushort GRP_CNT
        {
            get => (ushort)_grp_indx.Count;

            set
            {
                // Do nothing on set.  Only here for serialization.
            }
        }

        [STDF(Order = 2)]
        public ushort[] GRP_INDX
        {
            get => _grp_indx.ToArray();
            set
            {
                _grp_indx.Clear();
                _grp_indx.AddRange(value);
            }
        }

        [STDF(Order = 3)]
        public ushort[] GRP_MODE
        {
            get => _grp_indx.ToArray();
            set
            {
                _grp_indx.Clear();
                _grp_indx.AddRange(value);
            }
        }

        [STDF(Order = 4)]
        public byte[] GRP_RADX
        {
            get => _grp_radx.ToArray();
            set
            {
                _grp_radx.Clear();
                _grp_radx.AddRange(value);
            }
        }

        [STDF(Order = 5)]
        public string[] PGM_CHAR
        {
            get => _pgm_char.ToArray();
            set
            {
                _pgm_char.Clear();
                _pgm_char.AddRange(value);
            }
        }

        [STDF(Order = 6)]
        public string[] RTN_CHAR
        {
            get => _rtn_char.ToArray();
            set
            {
                _rtn_char.Clear();
                _rtn_char.AddRange(value);
            }
        }

        [STDF(Order = 7)]
        public string[] PGM_CHAL
        {
            get => _pgm_chal.ToArray();
            set
            {
                _pgm_chal.Clear();
                _pgm_chal.AddRange(value);
            }
        }

        [STDF(Order = 8)]
        public string[] RTN_CHAL
        {
            get => _rtn_chal.ToArray();
            set
            {
                _rtn_chal.Clear();
                _rtn_chal.AddRange(value);
            }
        }

        public override string Description => "Pin List Record";

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
