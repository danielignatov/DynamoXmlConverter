namespace DynamoXmlConverter.UI.Tests.Mocks
{
    internal class MockHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }
}