using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace DynamoXmlConverter.API.Tests.Mocks
{
    internal class MockWebHostEnvironment : IWebHostEnvironment
    {
        internal MockWebHostEnvironment()
        {
            
        }

        public string WebRootPath { get => Constants.WEB_ROOT_PATH; set => throw new NotImplementedException(); }
        public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EnvironmentName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
