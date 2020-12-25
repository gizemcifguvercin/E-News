using System;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ReportConsumer
{
 public class CustomLoggingFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly ILoggerFactory _loggerFactory;

        public CustomLoggingFilter(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            return (builder) =>
            {
                next(builder);

                builder.AdditionalHandlers.Insert(0, new CustomLoggingScopeHttpMessageHandler(new AppAppLogger()));
            };
        }
    }
}
