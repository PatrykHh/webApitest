using System.Collections.Generic;
using NUnit.Framework;
using WebaApiTest;

namespace NUnit
{
    public class UnitTest1
    {
        [Test]
        public void SimpleGetCall()
        {
            Request request = new Request("/get");
            request.CallService();
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
        }

        [Test]
        public void GetWithHeadersUsingParams()
        {
            Dictionary<string,string> headers = new Dictionary<string,string>();
            headers.Add("Host", "httpbin.org");
            Request request = new Request("/headers");
            request.CallService("Get","Json","","", headers,"" );
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.IsTrue(request.ResponseContentString.Contains("\"Host\": \"httpbin.org\""));
        }

        [Test]
        public void GetWithHeaders()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Host", "httpbin.org");
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
            Request request = new Request("/get");
            request.Timeout = 10;
            request.CallService();
            Assert.IsFalse(request.AssertStatusCode(200));
            Assert.IsFalse(request.AssertStatusDescription("OK"));
        }

        [Test]
        public void Post()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("ContentType", "application/json");
            Request request = new Request("/post");
            request.AddHeader(headers).CallService("Post","String","Test","String");
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.IsTrue(request.ResponseContentString.Contains("\"data\": \"Test\""));
        }

        [Test]
        public void Put()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("ContentType", "application/json");
            Request request = new Request("/put");
            request.AddHeader(headers).CallService("Put", "String", "Test", "String");
            Assert.IsTrue(request.AssertStatusCode(200));
            Assert.IsTrue(request.AssertStatusDescription("OK"));
            Assert.IsTrue(request.ResponseContentString.Contains("\"data\": \"Test\""));
        }

        [Test]
        public void Delete()
        {
            Request request = new Request("/delete");
            request.CallService("DELETE", "String", "", "String");
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
            Assert.IsTrue(request.AssertResponseContent("gzip"));
        }
    }
}
