namespace STDFLib.Serialization
{
    public interface IPinMapping
    {
        void AddPin(byte headNumber, byte siteNumber, ushort index, ushort channelType, string channelName, string physicalPinName, string logicalPinName);
        void AddPin(ushort index, ushort channelType, string channelName, string physicalPinName, string logicalPinName);
        void AddPinGroup(ushort index, string groupName, params ushort[] pinIndexes);
        void AddPinList(ushort[] indexes, PinGroupModes[] modes, DisplayRadix[] radixes, string[] programStateEncodingR = null, string[] programStateEncodingL = null, string[] returnStateEncodingR = null, string[] returnStateEncodingL = null);
    }
}