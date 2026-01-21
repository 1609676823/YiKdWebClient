namespace YiKdWebClient.Tests
{
    public class SmokeTests
    {
        [Fact]
        public void Can_create_client()
        {
            
            var client = new YiK3CloudClient();
            Assert.NotNull(client);
            Assert.NotNull(client.AppSettingsModel);
          
        }
    }
}
