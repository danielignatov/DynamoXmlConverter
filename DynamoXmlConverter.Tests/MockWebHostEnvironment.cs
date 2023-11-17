using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace DynamoXmlConverter.Tests
{
    internal class MockWebHostEnvironment : IWebHostEnvironment
    {
        private string _webRootPath;

        public MockWebHostEnvironment(string webRootPath)
        {
            _webRootPath = webRootPath;
        }

        public string WebRootPath { get => _webRootPath; set => throw new NotImplementedException(); }
        public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EnvironmentName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
