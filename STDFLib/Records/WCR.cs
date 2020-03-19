using System;

namespace STDFLib
{
    /// <summary>
    /// Wafer Configuration Record
    /// </summary>
    public class WCR : STDFRecord
    {
        private char _pos_x = DirectionCodes.Unknown;
        private char _pos_y = DirectionCodes.Unknown;
        private char _wf_flat = DirectionCodes.Unknown;
        private byte _wf_units = (byte)WaferUnits.Unknown;

        public override RecordType TypeCode => 0x021E;

        [STDF(Order = 1)]
        public float WAFR_SIZ { get; set; } = 0;

        [STDF(Order = 2)]
        public float DIE_HT { get; set; } = 0;

        [STDF(Order = 3)]
        public float DIE_WID { get; set; } = 0;

        [STDF(Order = 4)]
        public byte WF_UNITS
        {
            get => _wf_units;

            set
            {
                switch(value)
                {
                    case (byte)WaferUnits.Centimeters:
                    case (byte)WaferUnits.Inches:
                    case (byte)WaferUnits.Millimeters:
                    case (byte)WaferUnits.Mils:
                    case (byte)WaferUnits.Unknown:
                        _wf_units = (byte)value;
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unsupported Wafer Units (WF_UNITS) value passed.  Valid values are 0,1,2,3 or 4.  Value received was '{0}'.", value));
                }
}
        }

        [STDF(Order = 5)]
        public char WF_FLAT
        {
            get => _wf_flat;

            set
            {
                switch(char.ToUpper(value))
                {
                    case DirectionCodes.Unknown:
                    case DirectionCodes.Up:
                    case DirectionCodes.Down:
                    case DirectionCodes.Left:
                    case DirectionCodes.Right:
                        _wf_flat = char.ToUpper(value);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unsupported Wafer Flat (WF_FLAT) value.  Valid values are L,R,U,D or a space.  Value received was '{0}'.", value));
                }
            }
        }

        [STDF(Order = 6)]
        public short CENTER_X { get; set; } = -32768;

        [STDF(Order = 7)]
        public short CENTER_Y { get; set; } = -32768;

        [STDF(Order = 8)]
        public char POS_X
        {
            get => _pos_x;

            set
            {
                switch (char.ToUpper(value))
                {
                    case DirectionCodes.Unknown:
                    case DirectionCodes.Left:
                    case DirectionCodes.Right:
                        _pos_x = char.ToUpper(value);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unsupported Positive X direction (POS_X) value.  Valid values are L,R or a space.  Value received was '{0}'.", value));
                }
            }
        }

        [STDF(Order = 9)]
        public char POS_Y
        {
            get => _pos_y;

            set
            {
                switch (char.ToUpper(value))
                {
                    case DirectionCodes.Unknown:
                    case DirectionCodes.Up:
                    case DirectionCodes.Down:
                        _pos_y = char.ToUpper(value);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unsupported Positive Y direction (POS_Y) value.  Valid values are U,D or a space.  Value received was '{0}'.", value));
                }
            }
        }
        public override string Description => "Wafer Configuration Record";
    }
}
