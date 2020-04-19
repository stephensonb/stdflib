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
    public class WaferCollection : IWaferCollection
    {
        ISTDFFile STDF { get; set; }

        IPartCollection Parts { get; set; }

        public WaferCollection(ISTDFFile file)
        {
            STDF = file;
            Parts = new PartCollection(file);
        }

        public void SetConfiguration(float waferSize, float dieHeight, float dieWidth, WaferUnits units, WaferFlatOrientation flatOrientation, short centerX, short centerY, WaferDirections positiveX, WaferDirections positiveY)
        {
            throw new NotImplementedException();
        }

        public void StartWaferTest(byte headNumber, byte siteGroupNumber, string waferId, DateTime startTime)
        {
            throw new NotImplementedException();
        }

        public void EndWaferTest(byte headNumber, byte siteGroupNumber, DateTime endTime, string waferId, string fabWaferId, string frameId, string maskId, string userDescription, string execDescription)
        {
            throw new NotImplementedException();
        }
    }
}
