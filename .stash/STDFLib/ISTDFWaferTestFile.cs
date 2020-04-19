using System;
using System.Collections.Generic;

namespace STDFLib2.Serialization
{
    public interface ISTDFWaferTestFile : ISTDFFile
    {
        IWaferData GetWafer(string waferId, byte headNumber, byte siteGroup = 255, int startIndex = 0);
        IPartData GetWaferPart(byte headNumber, byte siteNumber, int startIndex = 0);
        IEnumerable<IPartData> GetWaferParts(byte headNumber, byte siteNumber);
        IEnumerable<IWaferData> GetWafers(byte headNumber, byte siteGroup = 255);
        void WriteWaferConfiguration(float waferSize = 0, WaferUnits sizeUnits = WaferUnits.Unknown, float dieHeight = 0, float dieWidth = 0, WaferDirections flatOrientation = WaferDirections.Unknown, short centerX = short.MinValue, short centerY = short.MinValue, WaferDirections positiveX = WaferDirections.Unknown, WaferDirections positiveY = WaferDirections.Unknown);
        void WriteWaferInfo(DateTime startTime, byte headNumber, byte siteGroup = 255, string waferId = "");
        void WriteWaferResults(DateTime finishTime, byte headNumber, byte siteGroup = 255, string waferId = "", string fabWaferId = "", string waferFrameId = "", string waferMaskId = "", string userWaferDescription = "", string executiveWaferDescription = "");
    }
}