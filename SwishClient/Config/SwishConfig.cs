namespace SwishClient.Config
{
    public class SwishConfig
    {
        public SwishConfig(string clientCertificateFilename, string clientCertificatePassword)
        {
            ClientCertificateFilename = clientCertificateFilename;
            ClientCertificatePassword = clientCertificatePassword;
        }

        public string ClientCertificateFilename { get; }
        public string ClientCertificatePassword { get; }
    }
}