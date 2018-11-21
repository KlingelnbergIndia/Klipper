using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Common
{
    public static class CommonHelper
    {
        static public HttpClient GetClient(string baseAddress)
        {
            HttpClient client = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(10000) };
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
