using Microsoft.Extensions.Logging;
using Moq;

namespace GPWebpayNet.Sdk.Spec.Services
{
    public abstract class ABaseTest
    {
        protected Mock<ILogger<T>> GetLoggerMock<T>()
        {
            var loggerMock = new Mock<ILogger<T>>();
            return loggerMock;
        }
    }
}