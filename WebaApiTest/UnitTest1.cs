using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebaApiTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //GetClass getClass = new GetClass();
            //Dictionary<string,string> headers = new Dictionary<string, string>();
            //headers.Add("Referer","www.wp.pl");
            //var test = getClass.GetJson("",headers);
            //getClass.GetString("",true);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Request request = new Request("httpbin.org/");
            request.Method = "POST";
            request.Timeout = 3000;
            request.CallService("Get");
            Assert.IsTrue(request.CheckStatusCode(200));
            Assert.IsTrue(request.CheckStatusDescription("OK"));
        }
    }
}
