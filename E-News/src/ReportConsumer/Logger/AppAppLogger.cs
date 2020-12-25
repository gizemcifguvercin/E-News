using System;
using System.Text.Json;

namespace ReportConsumer
{
      public class AppAppLogger : IAppLogger
    {
        public void WriteLogForHttpCall(object logModel)
        {
            Console.WriteLine(JsonSerializer.Serialize(logModel));
        }
    }
}
