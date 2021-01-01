using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GreenPipes.Internals.Extensions;
using Models;
using Polly;

namespace ReportConsumer.Service
{
    public abstract class BaseClient
    {
        private readonly HttpClient _httpClient;

        public BaseClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task SendRequestToIntegration(StringContent message, AgencyInfo info)
        {
            HttpResponseMessage response = await SendRequest(message, info);

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var policy = Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(5));

                    await policy.ExecuteAsync(async () =>
                        {
                            HttpResponseMessage response = await SendRequest(message, info);
                            if (!response.IsSuccessStatusCode)
                            {
                                var responseString = await response.Content.ReadAsStringAsync();
                                throw new Exception(
                                    $"Polly Retry / StatusCode: {response.StatusCode}, " +
                                    $"Request: {JsonSerializer.Serialize(message)}, Response: {responseString}");
                            }
                        })
                        .OrTimeout(TimeSpan.FromMinutes(10));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private async Task<HttpResponseMessage> SendRequest(StringContent message, AgencyInfo info)
        {
            var svcCredentials =
                Convert.ToBase64String(Encoding.ASCII.GetBytes(info.User + ":" + info.Password));

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", svcCredentials);

            return await _httpClient.PostAsync(info.EndpointURL, message);
        }
    }
}