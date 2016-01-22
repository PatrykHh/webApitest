using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebaApiTest
{
    public class Request : WebRequest
    {


        //public Request(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        //{
        //}

        //public Request()
        //{
        //}


        //public override string Method { get; set; } = "GET";
        //public override int Timeout { get; set; } = 3000;

        //public int statusCode { get; set; }

        //public string StatusDescription { get; set; }

        //public string RequestContentJson { get; set; }

        //public string RequestContentString { get; set; }

        //public string url { get; set; }

        //public WebResponse WebResponse
        //{
        //    get
        //    {
        //        return WebResponse;
        //    }
        //    set { WebResponse = value; }
        //}



        //public Request Create()
        //{
        //    this.Create(url);
        //    return this;
        //}

        //public Request Create(Uri url)
        //{
        //    this.Create(url);
        //    return this;
        //}

        //public Request Create(string url)
        //{
        //    return Create(url);

        //}

        //public void Get(string url)
        //{
        //    this.WebResponse = Create(url).GetResponse();
        //    var contentString = ;
        //    var formatedResponse = (HttpWebResponse) this.WebResponse;
        //    this.statusCode = (int) formatedResponse.StatusCode;
        //    this.StatusDescription = formatedResponse.StatusDescription;
        //    this.RequestContentJson = contentJson;
        //    this.RequestContentString = contentString;
        //}

    }

}
