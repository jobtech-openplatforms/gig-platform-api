using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Store.IoC
{
    public static class StoreServiceExtension
    {
        public static IServiceCollection AddRavenDb(this IServiceCollection collection, IConfiguration configuration)
        {
            var ravenDbSection = configuration.GetSection("Raven");

            var urls = new List<string>();
            ravenDbSection.GetSection("Urls").Bind(urls);
            var databaseName = ravenDbSection.GetValue<string>("DatabaseName");
            var certPwd = ravenDbSection.GetValue<string>("CertPwd");
            var certPath = ravenDbSection.GetValue<string>("CertPath");
            var keyPath = ravenDbSection.GetValue<string>("KeyPath");
            var loadCertFromStore = ravenDbSection.GetValue("LoadCertFromStore", false);
            var certStoreThumbprint = ravenDbSection.GetValue<string>("CertStoreThumbprint");

            DocumentStoreHolder.DatabaseName = databaseName;
            DocumentStoreHolder.Urls = urls.ToArray();
            DocumentStoreHolder.CertPwd = certPwd;
            DocumentStoreHolder.CertPath = certPath;
            DocumentStoreHolder.KeyPath = keyPath;
            DocumentStoreHolder.LoadCertFromStore = loadCertFromStore;
            DocumentStoreHolder.CertStoreThumbprint = certStoreThumbprint;
            collection.AddSingleton<IDocumentStore>(DocumentStoreHolder.Store);

            return collection;
        }
    }
}
