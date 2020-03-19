namespace Jobtech.OpenPlatforms.GigPlatformApi.Store.Config
{
    public class RavenConfig
    {
        public string[] Urls { get; set; }
        public string Database { get; set; }
        public string CertPwd { get; set; }
        public string CertPath { get; set; }
        public string KeyPath { get; set; }
        public bool LoadCertFromStore { get; set; }
        public string CertStoreThumbprint { get; set; }
    }
}
