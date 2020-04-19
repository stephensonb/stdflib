
namespace STDFLib2
{
    public interface ITestBin
    {
        byte HEAD_NUM { get; set; }
        byte SITE_NUM { get; set; }
        ushort BIN_NUM { get; set; }
        uint BIN_CNT { get; set; }
        char BIN_PF { get; set; }
        string BIN_NAM { get; set; }
    }
}
