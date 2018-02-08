using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GPWebpayNet.Sdk.Spec.Services
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            throw new NotImplementedException("Mock me");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}