﻿using System.Collections.Generic;
using NUnit.Framework;
using WebaApiTest;

namespace NUnit
{
    [TestFixture]
    public class UnitTest1
    {

        [Test]
        public void TestMethod()
        {
            Dictionary<string, string> headers = new Dictionary<string, string> { { "ContentType", "application/json" } };
            Request request = new Request("/post");
            request.AddHeader(headers)
                    .AddRequestContent(Request.Methods.Post, "abc", "String")
                    .CallService(Request.Methods.Post)
                    .ParseResponseToJson();
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.IsTrue(request.AssertJsonResponseContentEquals("data","abc"));
        }

        [Test]
        public void SimpleGetCall()
        {
            Request request = new Request(Address.Get.ToString());
            request.CallService();
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
        }

        [Test]
        public void GetWithHeadersUsingParams()
        {
            Dictionary<string, string> headers = new Dictionary<string, string> {{"Host", "httpbin.org"}};
            Request request = new Request("/headers");
            request.CallService(Request.Methods.Get,"Json",null,null, headers);
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.IsTrue(request.ResponseContentString.Contains("\"Host\": \"httpbin.org\""));
        }

        [Test]
        public void GetWithHeaders()
        {
            Dictionary<string, string> headers = new Dictionary<string, string> {{"Host", "httpbin.org"}};
            Request request = new Request("/headers");
            request.AddHeader(headers).CallService();
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.IsTrue(request.ResponseContentString.Contains("\"Host\": \"httpbin.org\""));
        }

        [Test]
        public void GetWithAuthorization()
        {
            Request request = new Request("/basic-auth/user/passwd");
            request.Authenticate("user", "passwd").CallService();
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
        }

        [Test]
        public void NotSufficientTimeout()
        {
            Request request = new Request("/get") {Timeout = 10};
            request.CallService();
            Assert.IsFalse(request.AssertStatusCode(200));
            Assert.IsFalse(request.AssertStatusDescription("OK"));
        }

        [Test]
        public void Post()
        {
            Dictionary<string, string> headers = new Dictionary<string, string> {{"ContentType", "application/json"}};
            Request request = new Request("/post");
            request.AddHeader(headers).CallService(Request.Methods.Post,"String","Test","String");
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.IsTrue(request.ResponseContentString.Contains("\"data\": \"Test\""));
        }

        [Test]
        public void Put()
        {
            Dictionary<string, string> headers = new Dictionary<string, string> {{"ContentType", "application/json"}};
            Request request = new Request("/put");
            request.AddHeader(headers).CallService(Request.Methods.Put, "String", "Test", "String");
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.IsTrue(request.ResponseContentString.Contains("\"data\": \"Test\""));
        }

        [Test]
        public void Delete()
        {
            Request request = new Request("/delete");
            request.CallService(Request.Methods.Delete);
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
        }

        [Test]
        public void GetImage()
        {
            Request request = new Request("/image/png");
            request.CallService();
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.AreEqual("image/png",request.ResponseContentType);
        }

        [Test]
        public void GetGzip()
        {
            Request request = new Request("/gzip");
            request.CallService();
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.AreEqual("application/json", request.ResponseContentType);
        }
    }
}
