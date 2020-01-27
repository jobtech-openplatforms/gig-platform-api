using Raven.Client.Documents;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Store.Config
{
    public interface IDocumentStoreHolder
    {
        IDocumentStore Store { get;  }
    }
}
