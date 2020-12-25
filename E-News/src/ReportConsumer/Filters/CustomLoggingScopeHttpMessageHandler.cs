using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportConsumer
{
    public class CustomLoggingScopeHttpMessageHandler : DelegatingHandler
    {
        private readonly IAppLogger _logger;

        public CustomLoggingScopeHttpMessageHandler(IAppLogger logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var stp = new Stopwatch();
            stp.Start();

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (response.Content != null)
            {
                var responseLog = new HttpLog()
                {
                    ResponseBody = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode,
                    ResponseTime = stp.ElapsedMilliseconds,
                    Url = request.RequestUri.ToString(),
                    RequestBody = request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty
                };
                _logger.WriteLogForHttpCall(responseLog);
            }
            else
            {
                var responseLog = new HttpLog()
                {
                    ResponseBody = string.Empty,
                    StatusCode = 404,
                    ResponseTime = stp.ElapsedMilliseconds,
                    Url = request.RequestUri.ToString(),
                    RequestBody = await request.Content.ReadAsStringAsync()
                };
                _logger.WriteLogForHttpCall(responseLog);
            }
            return response;
        }
    }
}
