using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using nugetory.Endpoint;

namespace nugetory.tests.Support
{
    public static class HttpClient
    {
        internal static HttpStatusCode Invoke(string url, string method, out string responseData)
        {
            return Invoke(url, method, null, out responseData, null, 0);
        }


        internal static HttpStatusCode Invoke(string url, string method, byte[] requestData, out string responseData,
            string requestContentType, int requestContentLength)
        {
            HttpWebResponse response = Request(url, method, requestData, requestContentType, requestContentLength);

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
            return Invoke(url, method, null, null, 0);
        }

        internal static HttpStatusCode Invoke(string url, string method, byte[] requestData, string requestContentType,
            int requestContentLength)
        {
            HttpWebResponse response = Request(url, method, requestData, requestContentType, requestContentLength);

            return response.StatusCode;
        }

        private static HttpWebResponse Request(string url, string method, byte[] requestData, string requestContentType,
            int requestContentLength)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

            request.Method = method;
            request.ProtocolVersion = Version.Parse("1.0");

            if (!string.IsNullOrEmpty(ValidateAuthenticationAttribute.ApiKey))
                request.Headers.Add("X-Nuget-ApiKey", ValidateAuthenticationAttribute.ApiKey);

            if (requestData != null)
            {
                request.SendChunked = false;
                request.ContentType = requestContentType;
                request.ContentLength = requestContentLength;
                Stream writeStream = request.GetRequestStream();
                writeStream.Write(requestData, 0, requestData.Length);
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
