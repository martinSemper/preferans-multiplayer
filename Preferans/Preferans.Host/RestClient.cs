using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host
{
    public enum HttpVerb { GET, POST, PUT }
    class RestClient
    {
        private string _serviceUri;

        public RestClient(string serviceUri)
        {
            _serviceUri = serviceUri;
        }

        public HttpVerb Method { get; set; }

        public string ContentType { get; set; }

        public string EndPoint { get; set; }

        public string Body { get; set; }

        public Cookie Cookie { get; set; }

        public string MakeRequest()
        {
            return MakeRequest("");
        }
                

        public string MakeRequest(string parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_serviceUri + "/" + EndPoint + parameters);

            request.Method = Method.ToString();
            request.ContentType = ContentType;

            if (request.CookieContainer == null) request.CookieContainer = new CookieContainer();

            if (Cookie != null) request.CookieContainer.Add(Cookie);

            if (!string.IsNullOrEmpty(Body))
            {

                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(Body);
                    writer.Flush();
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                string responseValue = String.Empty;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    string message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                    throw new Exception(message);
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }

                return responseValue;
            }
        }

    }
}
