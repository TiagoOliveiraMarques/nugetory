using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace nugetory.tests.Support
{
    public static class HttpClient
    {
        internal static HttpStatusCode Invoke(string url, string method, out string responseData)
        {
            return Invoke(url, method, null, null, out responseData);
        }

        internal static HttpStatusCode Invoke(string url, string method, IDictionary<string, string> requestHeaders,
            out string responseData)
        {
            return Invoke(url, method, requestHeaders, null, out responseData);
        }

        internal static HttpStatusCode Invoke(string url, string method, IDictionary<string, string> requestHeaders,
            string requestData, out string responseData)
        {
            HttpWebResponse response = Request(url, method, requestHeaders, requestData);

            Stream responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
                StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8);
                responseData = readStream.ReadToEnd();
            }
            else
            {
                responseData = null;
            }

            return response.StatusCode;
        }

        internal static HttpStatusCode Invoke(string url, string method)
        {
            return Invoke(url, method, null, null);
        }

        internal static HttpStatusCode Invoke(string url, string method, IDictionary<string, string> requestHeaders)
        {
            return Invoke(url, method, requestHeaders, null);
        }

        internal static HttpStatusCode Invoke(string url, string method, IDictionary<string, string> requestHeaders,
            string requestData)
        {
            HttpWebResponse response = Request(url, method, requestHeaders, requestData);

            return response.StatusCode;
        }

        private static HttpWebResponse Request(string url, string method, IDictionary<string, string> requestHeaders,
            string requestData)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

            request.Method = method;
            request.ProtocolVersion = Version.Parse("1.0");

            if (requestHeaders != null)
            {
                foreach (string key in requestHeaders.Keys)
                {
                    request.Headers.Add(key, requestHeaders[key]);
                }
            }

            if (requestData != null)
            {
                request.SendChunked = false;
                request.ContentType = "application/json";
                request.ContentLength = requestData.Length;
                Stream writeStream = request.GetRequestStream();
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(requestData);
                writeStream.Write(bytes, 0, bytes.Length);
                writeStream.Flush();
                writeStream.Close();
            }
            else
            {
                request.ContentLength = 0;
            }

            try
            {
                return (HttpWebResponse) request.GetResponse();
            }
            catch (WebException e)
            {
                return (HttpWebResponse) e.Response;
            }
        }
    }
}
