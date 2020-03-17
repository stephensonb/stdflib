namespace STDFLib
{
    public static class STDFUnits
    {
        public static class Atto
        {
            public const sbyte Scale = 18;
            public const double Magnitude = 10e-18;
            public const string Prefix = "a";
        }

        public static class Femto
        {
            public const sbyte Scale = 15;
            public const double Magnitude = 10e-15;
            public const string Prefix = "f";
        }
        public static class Pico
        {
            public const sbyte Scale = 12;
            public const double Magnitude = 10e-12;
            public const string Prefix = "p";
        }
        public static class Nano
        {
            public const sbyte Scale = 9;
            public const double Magnitude = 10e-9;
            public const string Prefix = "n";
        }
        public static class Micro
        {
            public const sbyte Scale = 6;
            public const double Magnitude = 10e-6;
            public const string Prefix = "u";
        }
        public static class Milli
        {
            public const sbyte Scale = 3;
            public const double Magnitude = 10e-3;
            public const string Prefix = "m";
        }
        public static class Percent
        {
            public const sbyte Scale = 2;
            public const double Magnitude = 10e-2;
            public const string Prefix = "%";
        }
        public static class Kilo
        {
            public const sbyte Scale = -3;
            public const double Magnitude = 10e3;
            public const string Prefix = "K";
        }
        public static class Mega
        {
            public const sbyte Scale = -6;
            public const double Magnitude = 10e6;
            public const string Prefix = "M";
        }
        public static class Giga
        {
            public const sbyte Scale = -9;
            public const double Magnitude = 10e9;
            public const string Prefix = "G";
        }
        public static class Tera
        {
            public const sbyte Scale = -12;
            public const double Magnitude = 10e12;
            public const string Prefix = "T";
        }
    }
}
