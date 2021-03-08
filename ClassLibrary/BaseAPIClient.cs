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

        protected HttpRequestMessage CreateBaseRequestMessage(HttpMethod method, string url, string authHeaderName, string authHeaderValue, StringContent requestContent = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method: method, requestUri: url);
            request.Headers.Add(authHeaderName, authHeaderValue);
            if (requestContent != null) request.Content = requestContent;
            return request;
        }

        protected async Task<string> GetResponseContent(HttpRequestMessage request)
        {
            HttpResponseMessage response = await GetResponse(request);
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<HttpResponseMessage> GetResponse(HttpRequestMessage request)
        {
            HttpResponseMessage response = await Client.SendAsync(request);
            while (response.StatusCode == (HttpStatusCode)429)
            {
                RetryConditionHeaderValue retryAfter = response.Headers.RetryAfter;
                await Task.Delay(retryAfter == null ? 1000 : (int)retryAfter.Delta.Value.TotalMilliseconds);
                HttpRequestMessage newRequest = DuplicateRequest(request);
                response = await Client.SendAsync(newRequest);
            }

            while (response.StatusCode == (HttpStatusCode)401 || response.StatusCode == (HttpStatusCode)403)
            {
                HttpRequestMessage newRequest = DuplicateRequest(request);
                await UpdateRequestAuthorizationToSucceed(request);
                response = await Client.SendAsync(newRequest);
            }
            return response;
        }

        protected abstract Task UpdateRequestAuthorizationToSucceed(HttpRequestMessage request);

        protected static HttpRequestMessage DuplicateRequest(HttpRequestMessage originalRequest)
        {
            HttpRequestMessage newRequest = new HttpRequestMessage(method: originalRequest.Method, requestUri: originalRequest.RequestUri);
            foreach (KeyValuePair<string, IEnumerable<string>> header in originalRequest.Headers) newRequest.Headers.Add(header.Key, header.Value);
            if (originalRequest.Content != null)
            {
                newRequest.Content = originalRequest.Content;
                newRequest.Content.Headers.ContentType = originalRequest.Content.Headers.ContentType;
            }
            return newRequest;
        }
    }
}
