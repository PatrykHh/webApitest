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
    internal class Helper
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
}
