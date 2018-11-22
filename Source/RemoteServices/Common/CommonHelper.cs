using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

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

        static public HttpClient GetClient(string baseAddress, string token)
        {
            HttpClient client = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(10000) };
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization =
               new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        static public string ToSha256(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.ASCII.GetBytes(input);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }

        }

    }
}
