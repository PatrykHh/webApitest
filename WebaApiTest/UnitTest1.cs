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
        public void SimpleGetCall()
        {
            Request request = new Request("/get");
            request.CallService();
            Assert.IsTrue(request.CheckStatusCode(200));
            Assert.IsTrue(request.CheckStatusDescription("OK"));
        }

        [TestMethod]
        public void GetWithHeaders()
        {
            Dictionary<string,string> headers = new Dictionary<string,string>();
            headers.Add("Host", "httpbin.org");
            Request request = new Request("/get");
            request.CallService("Get","Json","","", headers,"" );
            Assert.IsTrue(request.CheckStatusCode(200));
            Assert.IsTrue(request.CheckStatusDescription("OK"));
        }

        [TestMethod]
        public void GetWithHeaders2()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Host", "httpbin.org");
            Request request = new Request("/get");
            request.AddHeader(headers).CallService();
            Assert.IsTrue(request.CheckStatusCode(200));
            Assert.IsTrue(request.CheckStatusDescription("OK"));
        }



        [TestMethod]
        public void NotSufficientTimeout()
        {
            Request request = new Request("/get");
            request.Timeout = 10;
            request.CallService();
            Assert.IsFalse(request.CheckStatusCode(200));
            Assert.IsFalse(request.CheckStatusDescription("OK"));
        }
    }
}
