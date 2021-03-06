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

        protected HttpRequestMessage GetRequestMessagePreparedWithAuthorizationHeaders(HttpMethod method, string url, string authHeaderName, string authHeaderValue, StringContent requestContent = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method: method, requestUri: url);
            request.Headers.Add(authHeaderName, authHeaderValue);
            if (requestContent != null) request.Content = requestContent;
            return request;
        }

        protected async Task<HttpResponseMessage> GetResponse(HttpRequestMessage originalRequest)
        {
            HttpResponseMessage response = await Client.SendAsync(originalRequest);
            while (response.StatusCode == (HttpStatusCode)429)
            {
                RetryConditionHeaderValue retryAfter = response.Headers.RetryAfter;
                await Task.Delay(retryAfter == null ? 1000 : (int)retryAfter.Delta.Value.TotalMilliseconds);
                HttpRequestMessage newRequest = new HttpRequestMessage(method: originalRequest.Method, requestUri: originalRequest.RequestUri);
                foreach (KeyValuePair<string, IEnumerable<string>> header in originalRequest.Headers) newRequest.Headers.Add(header.Key, header.Value);
                if (originalRequest.Content != null) newRequest.Content = originalRequest.Content;
                response = await Client.SendAsync(newRequest);
            }
            return response;
        }
    }
}
