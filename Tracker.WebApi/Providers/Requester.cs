namespace Crawler
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    #endregion

    public class Requester
    {
        private const string DefaultUserAgent =
            "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36";

        private string userAgent;

        public string UserAgent
        {
            get
            {
                if (this.userAgent == null)
                {
                    return DefaultUserAgent;
                }

                return this.userAgent;
            }

            set
            {
                this.userAgent = value;
            }
        }

        public string Get(string url, Dictionary<string, string[]> cookies = null)
        {
            return this.MakeRequest(url, "GET", null, cookies);
        }

        public string Post(string url, string data, Dictionary<string, string[]> cookies = null)
        {
            return this.MakeRequest(url, "POST", data, cookies);
        }

        private string MakeRequest(string url, string method, string data, Dictionary<string, string[]> cookies)
        {
            if (url.IndexOf("http") != 0)
            {
                return null;
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Method = method.ToUpper();

            if (cookies != null && cookies.Count > 0)
            {
                var cookieContainer = new CookieContainer();
                foreach (var cookieKey in cookies.Keys)
                {
                    cookieContainer.Add(
                        new Cookie(cookieKey, cookies[cookieKey][0], cookies[cookieKey][1], cookies[cookieKey][2]));
                }

                httpWebRequest.CookieContainer = cookieContainer;
            }

            if (method.ToUpper() == "POST")
            {
                var byteArray = Encoding.UTF8.GetBytes(data);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = byteArray.Length;

                var dataStream = httpWebRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            httpWebRequest.UserAgent = this.UserAgent;
            var remainingAttempts = 5;
            string result = null;
            while (result == null && remainingAttempts > 0)
            {
                result = this.GetResponse(httpWebRequest);
                remainingAttempts--;
            }

            return result;
        }

        private string GetResponse(HttpWebRequest httpWebRequest)
        {
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}