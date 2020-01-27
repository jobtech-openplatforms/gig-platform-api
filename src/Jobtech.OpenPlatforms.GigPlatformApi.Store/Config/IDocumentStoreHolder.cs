using System;
using Raven.Client.Documents;

namespace AF.GigPlatform.Store.Config
{
    public interface IDocumentStoreHolder
    {
        IDocumentStore Store { get;  }
    }
}
