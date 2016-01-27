using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WebaApiTest
{
    public class Request : WebRequest
    {

        private HttpWebRequest _request;

        public Request(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
        /// <summary>
        /// Creates Request object and set from config file: host, username, password
        /// </summary>
        /// <param name="url"></param>
        public Request(string url)
        {
            var host = ConfigurationManager.AppSettings["Host"];
            _request = (HttpWebRequest) Create(host+ url);
            SetDefaultValues();
        }

        /// <summary>
        /// Set request method type. Get, Post, Put, Delete
        /// </summary>
        public override string Method
        {
            get { return _request.Method; }
            set { _request.Method = value; }
        }

        /// <summary>
        /// Set request execution timeout
        /// </summary>
        public override int Timeout
        {
            get { return _request.Timeout; }
            set { _request.Timeout = value; }
        }

        /// <summary>
        /// Get request uri
        /// </summary>
        public override Uri RequestUri => _request.RequestUri;

        /// <summary>
        /// Accept header
        /// </summary>
        private string Accept {
            get { return _request.Accept; }
            set { _request.Accept = value; }
        }

        /// <summary>
        /// ContentType header
        /// </summary>
        public override string ContentType
        {
            get { return _request.ContentType; }
            set { _request.ContentType = value; }
        }

        /// <summary>
        /// Host header
        /// </summary>
        private string Host
        {
            get { return _request.Host; }
            set { _request.Host = value; }
        }


        /// <summary>
        /// MediaType header
        /// </summary>
        private string MediaType
        {
            get { return _request.MediaType; }
            set { _request.MediaType = value; }
        }

        /// <summary>
        /// Referer header
        /// </summary>
        private string Referer
        {
            get { return _request.Referer; }
            set { _request.Referer = value; }
        }

        public bool AllowAutoRedirect
        {
            set { _request.AllowAutoRedirect = value; }
        }


        /// <summary>
        /// Request authentication username
        /// </summary>
        private string Username { get; set; }

        /// <summary>
        /// Request authentication password
        /// </summary>
        private string Password { get; set; }

        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Response status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Response status description
        /// </summary>
        public string StatusDescription { get; set; }


        /// <summary>
        /// Response parsed to Json object
        /// </summary>
        public JObject ResponseContentJson { get; set; }

        /// <summary>
        /// Response as a string
        /// </summary>
        public string ResponseContentString { get; set; }

        /// <summary>
        /// Response as a string
        /// </summary>
        public string ResponseContentType { get; set; }

        /// <summary>
        /// Response as a string
        /// </summary>
        public WebHeaderCollection ResponseHeaders { get; set; }

        /// <summary>
        /// Raw webrisponse
        /// </summary>
        public WebResponse WebResponse { get; set; }

        /// <summary>
        /// Sets values from config file. Password, Username, Timeout and sets methhod to Get as a default value.
        /// </summary>
        private void SetDefaultValues()
        {
            Password = "";
            Password = ConfigurationManager.AppSettings["Password"];
            Username = ConfigurationManager.AppSettings["Username"];
            Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);
            _request.Method = "GET";
        }

        /// <summary>
        /// Add header to the request
        /// </summary>
        /// <param name="key">Header name</param>
        /// <param name="value">Header value</param>
        /// <returns></returns>
        public Request AddHeader(string key, string value)
        {
            Dictionary<string, string> header = new Dictionary<string, string> {{key, value}};
            AddHeader(header); 
            return this;
        }

        /// <summary>
        /// Sets headers. 
        /// </summary>
        /// <param name="headers">Key, value dictionary of headers to be added to request.</param>
        /// <returns>HttpWebRequest</returns>
        public Request AddHeader(Dictionary<string, string> headers)
        {

            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    switch (key)
                    {
                        case "Referer":
                            Referer = headers[key];
                            break;
                        case "MediaType":
                            MediaType = headers[key];
                            break;
                        case "Host":
                            Host = headers[key];
                            break;
                        case "ContentType":
                            ContentType = headers[key];
                            break;
                        case "Accept":
                            Accept = headers[key];
                            break;
                        default:
                            Headers.Add(key, headers[key]);
                            break;
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Disable or enable auto redirect
        /// </summary>
        /// <param name="isAllowed">True if auto redirect is allowed</param>
        /// <returns>HttpWebRequest</returns>
        public Request AllowRedirect(bool isAllowed)
        {
            AllowAutoRedirect = isAllowed;
            return this;
        }

        /// <summary>
        /// Sets authentication. 
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="authenticationType">Authentication type</param>
        /// <returns>HttpWebRequest</returns>
        public Request Authenticate(string username, string password, string authenticationType = "Basic")
        {
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(RequestUri, authenticationType, new NetworkCredential(username, password));
            _request.Credentials = credentialCache;
            return this;
        }

       /// <summary>
        /// Call service. Url is passed via class constructor.
        /// </summary>
        /// <param name="requestType">Get, Post, Put or Delete</param>
        /// <param name="responseType">Response type, String or Json object</param>
        /// <param name="requestContent">Content to be send</param>
        /// <param name="contentType">Request content type: String, Json or any other object</param>
        /// <param name="headers">Key, value dictionary of headers to be added to request.</param>
        /// <param name="authenticationType">Authentication type</param>
        /// <returns>Request</returns>
        public Request CallService( string requestType = "Get", string responseType = "", object requestContent = null, string contentType = "", Dictionary<string,string> headers = null, string authenticationType = "")
        {
            Method = requestType;
            AddHeader(headers);

            if (requestContent != null)
            {
                AddRequestContent(Method, requestContent, contentType);
            }

            if (authenticationType != string.Empty)
            {
                Authenticate(Username, Password, authenticationType);
            }

            try
            {
                WebResponse = _request.GetResponse();                

            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    var response = (HttpWebResponse) e.Response;
                    StatusCode = (int) response.StatusCode;
                    StatusDescription = response.StatusDescription;
                }
                ResponseContentString = e.Message;
                return this;
           }

            ResponseHeaders = WebResponse.Headers;
            ResponseContentType = WebResponse.ContentType;
            var contentString = Helper.GetResponseString((HttpWebResponse)WebResponse);
            var formatedResponse = (HttpWebResponse)WebResponse;
            Cookies = formatedResponse.Cookies;
            StatusCode = (int)formatedResponse.StatusCode;
            StatusDescription = formatedResponse.StatusDescription;
            ResponseContentString = contentString;
            switch (responseType)
            {
                case ("Json"):
                    ParseResponseToJson();
                    break;
            }
            return this;
        }

        /// <summary>
        /// Adds content to be send by request
        /// </summary>
        /// <param name="content">Content to be send</param>
        /// <param name="contentType">Request content type: String, Json or any other object</param>
        /// <returns>HttpWebRequest</returns>
        public Request AddRequestContent(string method, object content, string contentType)
        {
            Method = method;
            byte[] byteArray;
            switch (contentType.ToLower())
            {
                case "string":
                    byteArray = Encoding.UTF8.GetBytes((string)content);
                    break;
                case "json":
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

            _request.ContentLength = byteArray.Length;
            Stream dataStream = _request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            return this;
        }

        /// <summary>
        /// Sets coookies from previous request
        /// </summary>
        public void SetCookiesFromPreviousRequest()
        {
            foreach (var cookie in Cookies)
            {
                _request.CookieContainer.Add((Cookie)cookie);
            }
                
        }

        /// <summary>
        /// Parse received response to Json Object
        /// </summary>
        public void ParseResponseToJson()
        {
            ResponseContentJson = Helper.ParseToJson(ResponseContentString);
        }


        #region Asserts
        /// <summary>
        /// Compare expected status code with response status code
        /// </summary>
        /// <param name="expectedStatusCode">Expected Status code</param>
        /// <returns>True if code matches expected status code</returns>
        public bool AssertStatusCode(int expectedStatusCode)
        {
            return StatusCode.Equals(expectedStatusCode);
        }

        /// <summary>
        /// Compare expected status description with response status code
        /// </summary>
        /// <param name="expectedStatusDescription">Expected Status description</param>
        /// <returns>True if description matches expected status description</returns>
        public bool AssertStatusDescription(string expectedStatusDescription)
        {
            if (StatusDescription == null)
            {
                return false;
            }
            return StatusDescription.Equals(expectedStatusDescription);
        }

        /// <summary>
        /// Compare expected response content with response content
        /// </summary>
        /// <param name="expectedResponseContent">Expected response content</param>
        /// <returns>True if response content matches expected response content</returns>
        public bool AssertResponseContent(string expectedResponseContent)
        {
            return ResponseContentString.Equals(expectedResponseContent);
        }

        /// <summary>
        /// Check if Json contains value for given key
        /// </summary>
        /// <param name="key">Property name</param>
        /// <param name="value">Property value</param>
        /// <returns>True if value for given key exists</returns>
        public bool AssertJsonResponseContentContains(string key, string value)
        {
            var a = ResponseContentJson.Properties();
            //var keySelector = a as JProperty[] ?? a.ToArray();
            var test = ResponseContentJson.Property(key);
            return test.Value.ToString().Contains(value);
        }

        /// <summary>
        /// Check if Json equals value for given key
        /// </summary>
        /// <param name="key">Property name</param>
        /// <param name="value">Property value</param>
        /// <returns>True if value for given key equals expected value</returns>
        public bool AssertJsonResponseContentEquals(string key, string value)
        {
            var a = ResponseContentJson.Properties();
            //var keySelector = a as JProperty[] ?? a.ToArray();
            var test = ResponseContentJson.Property(key);
            return test.Value.ToString().Equals(value);
        }

        /// <summary>
        /// Check if multiple key value pairs are present in response Json
        /// </summary>
        /// <param name="keyValueExpectedResponse">expected key value pairs</param>
        /// <returns></returns>
        public bool AssertJsonResponseContent(Dictionary<string, string> keyValueExpectedResponse)
        {
            return keyValueExpectedResponse.Keys.All(key => ResponseContentJson[key].ToString().Equals(keyValueExpectedResponse[key]));
        }

        #endregion

    }

}
