using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Store.Config
{
    public class DocumentStoreHolder : IDocumentStoreHolder
    {
        public IDocumentStore Store { get; }

        public DocumentStoreHolder(IOptions<RavenConfig> ravenConfig)
        {
            var settings = ravenConfig.Value;
            var cert = GetCert();

            Store = new DocumentStore
            {
                Urls = settings.Urls,//.Split(','),
                // Urls = Environment.GetEnvironmentVariable("GigDataServiceUrl").Split(','), // Docker
                Database = settings.Database,
                Certificate = cert
            }.Initialize();
        }

        private static X509Certificate2 GetCert()
        {
            var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            var certCollection = certStore.Certificates.Find(
                X509FindType.FindByThumbprint,
                "D249EC57413D2ABDB3E23B7EC8408EF4E7BEF8D8", // Raven HQ cert imported to Ravendb Cloud
                //"e2ac6526b195d935aaf149e444bc23beb16d9be1", // Ravendb Cloud
                false);

            var cert = certCollection[0];

            certStore.Close();

            return cert;
        }
    }
}