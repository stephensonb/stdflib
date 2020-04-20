namespace STDFLib
{
    /// <summary>
    /// Public interface to access common properties between FTR (Functional Test), PTR (Parametric Test) and 
    /// MPR (Multiresult Parametric Test) records. 
    /// </summary>
    public interface ITestResult
    {
        uint TEST_NUM { get; set; }
        byte HEAD_NUM { get; set; }
        byte SITE_NUM { get; set; }
        byte TEST_FLG { get; set; }
        byte OPT_FLAG { get; set; }
        string TEST_TXT { get; set; }
        string ALARM_ID { get; set; }
    }
}
