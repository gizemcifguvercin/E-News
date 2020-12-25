namespace ReportConsumer
{
    public class HttpLog
    {
        public string Url { get; set; }
        public object ResponseBody { get; set; }
        public int StatusCode { get; set; }
        public double ResponseTime { get; set; }
        public object RequestBody { get; set; }
    }
}