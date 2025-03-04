namespace SwishClient.Config
{
    public class SwishConfig
    {
        public SwishConfig(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string ClientId { get; }
        public string ClientSecret { get; }
    }
}