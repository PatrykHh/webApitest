using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace WebaApiTest
{
    class Helper
    {

        public static string GetResponseString(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                var content = reader.ReadToEnd();
                reader.Close();
                return content;
            }
        }

        public static JObject ParseToJson(string str)
        {
            JObject json = JObject.Parse(str);
            return json;
        }
    }

        //public static Dictionary<string, string> ParseResponseToDictionary(HttpStatusCode responseCode, string responseStatus,
        //    string ResponseContent)
        //{
        //    Dictionary<string,string> response = new Dictionary<string, string>();
        //    var responseCodetemp = (int) responseCode;
        //    response.Add("Code",responseCodetemp.ToString());
        //    response.Add("Status", responseStatus);
        //    response.Add("Content",ResponseContent);

        //    return response;
        //}

        //public static HttpWebRequest AddHeaders(HttpWebRequest webRequest, Dictionary<string, string> headers)
        //{
        //    if (headers.Keys.Count > 0)
        //    {
        //        foreach (string key in headers.Keys)
        //        {
        //            switch (key)
        //            {
        //                case "Accept":
        //                    webRequest.Accept = headers[key];
        //                    break;
        //                case "Content-Type":
        //                    webRequest.ContentType = headers[key];
        //                    break;
        //                case "Host":
        //                    webRequest.Host = headers[key];
        //                    break;
        //                case "Media-Type":
        //                    webRequest.MediaType = headers[key];
        //                    break;
        //                case "Referer":
        //                    webRequest.Referer = headers[key];
        //                    break;
        //                default:
        //                    webRequest.Headers.Add(key, headers[key]);
        //                    break;
        //            }
        //        }
        //    }

        //    return webRequest;
        //}

        //public static HttpWebRequest Authentication(HttpWebRequest webrequest, string userName, string password)
        //{
        //    webrequest.Credentials = new NetworkCredential(userName, password);
        //    webrequest.PreAuthenticate = true;
        //    return webrequest;
        //}

    }
