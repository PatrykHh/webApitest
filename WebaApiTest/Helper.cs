using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebaApiTest
{
    internal class Helper
    {

        /// <summary>
        /// Gets response
        /// </summary>
        /// <param name="response">Call response</param>
        /// <returns>Response content</returns>
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

        /// <summary>
        /// Json object parser
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>JObject</returns>
        public static JObject ParseToJson(string str)
        {
            JObject json = JObject.Parse(str);
            return json;
        }

        public static T ParseToObject<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
