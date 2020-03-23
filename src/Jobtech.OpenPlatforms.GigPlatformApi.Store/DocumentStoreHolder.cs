using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Store
{
    public static class DocumentStoreHolder
    {
        public static IDocumentStore Store => _store.Value;

        private static Lazy<IDocumentStore> _store = new Lazy<IDocumentStore>(CreateStore);

        public static ILogger Logger { get; set; }
        public static string[] Urls { get; set; }
        public static Type TypeInAssemblyContainingIndexesToCreate { get; set; }
        public static string DatabaseName { get; set; }

        public static string CertPwd { get; set; }
        public static string CertPath { get; set; }
        public static string KeyPath { get; set; }
        public static bool LoadCertFromStore { get; set; }
        public static string CertStoreThumbprint { get; set; }

        private static IDocumentStore CreateStore()
        {
            IDocumentStore store = null;

            var useCert = !string.IsNullOrEmpty(CertStoreThumbprint) || 
                          !string.IsNullOrEmpty(CertPath) || 
                          !string.IsNullOrEmpty(KeyPath) ||
                          !string.IsNullOrEmpty(CertPwd);

            if (!useCert)
            {
                Logger?.LogTrace("Will init DocumentStore WITHOUT certificate authorization.");
                try
                {
                    store = new DocumentStore()
                    {
                        Urls = Urls,
                        Database = DatabaseName
                    }.Initialize();
                }
                catch (Exception e)
                {
                    Logger?.LogError(e, "Error initializing DocumentStore");
                    throw;
                }
            }
            else
            {
                Logger?.LogTrace("Will init DocumentStore WITH certificate authorization.");

                try
                {
                    var cert = LoadCertFromStore ? GetCertFromStore() : GetCertFromFiles();

                    store = new DocumentStore()
                    {
                        Urls = Urls,
                        Database = DatabaseName,
                        Certificate = cert
                    }.Initialize();
                }
                catch (Exception e)
                {
                    Logger?.LogError(e, "Error initializing DocumentStore");
                    throw;
                }
            }

            if (TypeInAssemblyContainingIndexesToCreate != null)
            {
                Logger?.LogTrace($"Will create indices defined in assembly {TypeInAssemblyContainingIndexesToCreate.Name}");
                IndexCreation.CreateIndexes(TypeInAssemblyContainingIndexesToCreate.Assembly, store);
            }


            return store;
        }

        private static X509Certificate2 GetCertFromStore()
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

        private static X509Certificate2 GetCertFromFiles()
        {
            var cert = new X509Certificate2(CertPath);
            var privateKey = ReadKeyFromFile(KeyPath);

            var certWithPrivateKey = cert.CopyWithPrivateKey(privateKey);

            var certWithCredentials = new X509Certificate2(certWithPrivateKey.Export(X509ContentType.Pfx, CertPwd), CertPwd);

            return certWithCredentials;
        }

        private static RSA ReadKeyFromFile(string filename)
        {
            var pemContents = System.IO.File.ReadAllText(filename);
            const string rsaPrivateKeyHeader = "-----BEGIN RSA PRIVATE KEY-----";
            const string rsaPrivateKeyFooter = "-----END RSA PRIVATE KEY-----";

            if (!pemContents.Contains(rsaPrivateKeyHeader)) throw new InvalidOperationException();

            var startIdx = pemContents.IndexOf(rsaPrivateKeyHeader, StringComparison.Ordinal) + rsaPrivateKeyHeader.Length + 1;

            var endIdx = pemContents.IndexOf(
                rsaPrivateKeyFooter,
                rsaPrivateKeyHeader.Length,
                StringComparison.Ordinal);

            var length = endIdx - startIdx;

            var base64 = pemContents.Substring(
                startIdx,
                length);

            var der = Convert.FromBase64String(base64);

            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(der, out _);
            return rsa;

        }
    }
}