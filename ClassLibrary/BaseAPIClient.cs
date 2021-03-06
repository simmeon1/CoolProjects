using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public abstract class BaseAPIClient
    {
        protected HttpClient Client { get; private set; }
        public BaseAPIClient()
        {
            Client = new HttpClient();
        }

        protected async Task<string> GetResponse(HttpMethod method, string url, string authHeaderName, string authHeaderValue, StringContent requestContent = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method: method, requestUri: url);
            request.Headers.Add(authHeaderName, authHeaderValue);
            if (requestContent != null) request.Content = requestContent;
            HttpResponseMessage response = await Client.SendAsync(request);
            if (response.StatusCode == (HttpStatusCode)403) throw new Exception("Authorization is invalid.");
            while (response.StatusCode == (HttpStatusCode)429)
            {
                RetryConditionHeaderValue retryAfter = response.Headers.RetryAfter;
                await Task.Delay(retryAfter == null ? 1000 : (int)retryAfter.Delta.Value.TotalMilliseconds);
                request = new HttpRequestMessage(method: method, requestUri: url);
                request.Headers.Add(authHeaderName, authHeaderValue);
                if (requestContent != null) request.Content = requestContent;
                response = await Client.SendAsync(request);
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
