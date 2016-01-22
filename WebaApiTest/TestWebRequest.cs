using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WebaApiTest
{
    public class Request : WebRequest
    {

        private HttpWebRequest _request;

        public Request(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public Request(string apiAddress)
        {
            _request = (HttpWebRequest) WebRequest.Create(apiAddress);
        }

        public override string Method { get; set; } = "GET";

        public override int Timeout { get; set; } = 3000;

        public override Uri RequestUri { get; }

        public string Accept {
            get { return _request.Accept; }
            set { _request.Accept = value; }
        }

        public string ContentType
        {
            get { return _request.ContentType; }
            set { _request.ContentType = value; }
        }

        public string Host
        {
            get { return _request.Host; }
            set { _request.Host = value; }
        }

        public string MediaType
        {
            get { return _request.MediaType; }
            set { _request.MediaType = value; }
        }

        public string Referer
        {
            get { return _request.Referer; }
            set { _request.Referer = value; }
        }

        public string Username
        {
            get { return Username; }
            set { Username = value; }
        }

        public string Password
        {
            get { return Password; }
            set { Password = value; }
        }


        public int statusCode { get; set; }

        public string StatusDescription { get; set; }

        public JObject RequestContentJson { get; set; }

        public string ResponseContentString { get; set; }

        public WebResponse WebResponse { get; set; }


        public HttpWebRequest AddHeader(HttpWebRequest request, Dictionary<string, string> headers)
        {
            if (headers.Keys.Count > 0)
            {
                foreach (var key in headers.Keys)
                {
                    switch (key)
                    {
                        case "Referer":
                            request.Referer = headers[key];
                            break;
                        case "MediaType":
                            request.MediaType = headers[key];
                            break;
                        case "Host":
                            request.Host = headers[key];
                            break;
                        case "ContentType":
                            request.ContentType = headers[key];
                            break;
                        case "Accept":
                            request.Accept = headers[key];
                            break;
                        default:
                            request.Headers.Add(key, headers[key]);
                            break;
                    }
                }
            }

            return request;
        }

        public HttpWebRequest AllowAutoRedirect(HttpWebRequest request, bool isAllowed)
        {
            request.AllowAutoRedirect = isAllowed;

            return request;
        }

        public HttpWebRequest Authenticate(HttpWebRequest request, string username, string password, string authenticationType = "Basic")
        {
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(RequestUri, authenticationType, new NetworkCredential(username, password));
            request.Credentials = credentialCache;
            return request;
        }

        public void RefreshProperties()
        {
            _request.Method = Method;
        }

        public Request CallService()
        {
            CallService("GET", "", "","", new Dictionary<string, string>(),"");
            return this;
        }

        public Request CallService(string responseType)
        {
            CallService("GET", responseType, "", "", new Dictionary<string, string>(), "");
            return this;
        }

        public Request CallService(string requestType,string responseType, string requestContent, string contentType)
        {
            CallService(requestType, responseType, requestContent, contentType, new Dictionary<string, string>(), "");
            return this;
        }


        public Request CallService(string requestType, string responseType, string requestContent, string contentType, Dictionary<string,string> headers, string authenticationType)
        {
            _request.Method = requestType;
            AddHeader(_request, headers);

            if (_request.Method != "GET")
            {
                AddRequestContent(_request, requestContent, contentType);
            }

            if (authenticationType != String.Empty)
            {
                Authenticate(_request, Username, Password, authenticationType);
            }

            try
            {
                this.WebResponse = _request.GetResponse();

            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    var response = (HttpWebResponse) e.Response;
                    this.statusCode = (int) response.StatusCode;
                    this.StatusDescription = response.StatusDescription;
                }
                this.ResponseContentString = e.Message;
                return this;
           }

            var contentString = Helper.GetResponseString((HttpWebResponse)this.WebResponse);
            var formatedResponse = (HttpWebResponse)this.WebResponse;
            this.statusCode = (int)formatedResponse.StatusCode;
            this.StatusDescription = formatedResponse.StatusDescription;
            this.ResponseContentString = contentString;
            switch (responseType)
            {
                case ("JSON"):
                    this.RequestContentJson = Helper.ParseToJson(ResponseContentString);
                    break;
            }
            return this;
        }

        public HttpWebRequest AddRequestContent(HttpWebRequest request, object content, string contentType)
        {
            byte[] byteArray = null;
            switch (contentType)
            {
                case "String":
                    byteArray = Encoding.UTF8.GetBytes((string)content);
                    break;
                case "Json":
                    string json = content.ToString();
                    byteArray = Encoding.UTF8.GetBytes(json);
                    break;
                default:
                    BinaryFormatter bf = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bf.Serialize(ms, content);
                        byteArray = ms.ToArray();
                    }
                    break;
            }

            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            return request;
        }


        #region Asserts
        public bool CheckStatusCode(int expectedStatusCode)
        {
            return statusCode.Equals(expectedStatusCode);
        }

        public bool CheckStatusDescription(string expectedStatusDescription)
        {
            return StatusDescription.Equals(expectedStatusDescription);
        }

        public bool CheckResponseContent(string expectedResponseContent)
        {
            return ResponseContentString.Equals(expectedResponseContent);
        }

        #endregion

    }

}
