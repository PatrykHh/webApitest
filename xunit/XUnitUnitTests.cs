
using System.Collections.Generic;
using Xunit;
using WebaApiTest;

namespace xunit
{
    public class XUnitUnitTests
    {
        [Fact]
        public void SimpleGetCall()
        {
            Request request = new Request("/get");
            request.CallService();
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
        }

        [Fact]
        public void GetWithHeadersUsingParams()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Host", "httpbin.org");
            Request request = new Request("/headers");
            request.CallService("Get", "Json", "", "", headers, "");
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.True(request.ResponseContentString.Contains("\"Host\": \"httpbin.org\""));
        }

        [Fact]
        public void GetWithHeaders()
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
        public void GetWithAuthorization()
        {
            Request request = new Request("/basic-auth/user/passwd");
            request.Authenticate("user", "passwd").CallService();
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
        }

        [Fact]
        public void NotSufficientTimeout()
        {
            Request request = new Request("/get");
            request.Timeout = 10;
            request.CallService();
            Assert.False(request.AssertStatusCode(200));
            Assert.False(request.AssertStatusDescription("OK"));
        }

        [Fact]
        public void Post()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("ContentType", "application/json");
            Request request = new Request("/post");
            request.AddHeader(headers).CallService("Post", "String", "Test", "String");
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.True(request.ResponseContentString.Contains("\"data\": \"Test\""));
        }

        [Fact]
        public void Put()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("ContentType", "application/json");
            Request request = new Request("/put");
            request.AddHeader(headers).CallService("Put", "String", "Test", "String");
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.True(request.ResponseContentString.Contains("\"data\": \"Test\""));
        }

        [Fact]
        public void Delete()
        {
            Request request = new Request("/delete");
            request.CallService("DELETE", "String", "", "String");
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
        }

        [Fact]
        public void GetImage()
        {
            Request request = new Request("/image/png");
            request.CallService();
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.Equal("image/png", request.ResponseContentType);
        }

        [Fact]
        public void GetGzip()
        {
            Request request = new Request("/gzip");
            request.CallService();
            Assert.True(request.AssertStatusCode(200));
            Assert.True(request.AssertStatusDescription("OK"));
            Assert.Equal("application/json", request.ResponseContentType);
        }
    }
}

