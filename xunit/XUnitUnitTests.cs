using System.Collections.Generic;
using WebaApiTest;
using Xunit;


namespace xunit
{
    public class XUnitUnitTests
    {
        [Fact]
        public void XSimpleGetCall()
        {
            Request request = new Request("/get");
            request.CallService();
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
        }

        [Fact]
        public void XGetWithHeadersUsingParams()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Host", "httpbin.org");
            Request request = new Request("/headers");
            request.CallService(Request.Methods.Get, "Json", null, null, headers);
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.True(request.ResponseContentString.Contains("\"Host\": \"httpbin.org\""));
        }

        [Fact]
        public void XGetWithHeaders()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Host", "httpbin.org");
            Request request = new Request("/headers");
            request.AddHeader(headers).CallService();
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.True(request.ResponseContentString.Contains("\"Host\": \"httpbin.org\""));
        }

        [Fact]
        public void XGetWithAuthorization()
        {
            Request request = new Request("/basic-auth/user/passwd");
            request.Authenticate("user", "passwd").CallService();
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
        }

        [Fact]
        public void XNotSufficientTimeout()
        {
            Request request = new Request("/get");
            request.Timeout = 10;
            request.CallService();
            Assert.False(request.AssertStatusCode(200));
            Assert.False(request.AssertStatusDescription("OK"));
        }

        [Fact]
        public void XPost()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("ContentType", "application/json");
            Request request = new Request("/post");
            request.AddHeader(headers).CallService(Request.Methods.Post, "String", "Test", "String");
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.True(request.ResponseContentString.Contains("\"data\": \"Test\""));
        }

        [Fact]
        public void XPut()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("ContentType", "application/json");
            Request request = new Request("/put");
            request.AddHeader(headers).CallService(Request.Methods.Put, "String", "Test", "String");
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.True(request.ResponseContentString.Contains("\"data\": \"Test\""));
        }

        [Fact]
        public void XDelete()
        {
            Request request = new Request("/delete");
            request.CallService(Request.Methods.Delete);
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
        }

        [Fact]
        public void XGetImage()
        {
            Request request = new Request("/image/png");
            request.CallService();
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.Equal("image/png", request.ResponseContentType);
        }

        [Fact]
        public void XGetGzip()
        {
            Request request = new Request("/gzip");
            request.CallService();
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.Equal("application/json", request.ResponseContentType);
        }
    }
}

