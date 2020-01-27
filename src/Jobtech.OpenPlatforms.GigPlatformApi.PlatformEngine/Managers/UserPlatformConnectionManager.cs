using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.ValueObjects;
using AF.GigPlatform.Store.Config;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Threading.Tasks;

namespace AF.GigPlatform.PlatformEngine.Managers
{
    public class UserPlatformConnectionManager : IUserPlatformConnectionManager
    {
        private readonly IDocumentStore _documentStore;

        public UserPlatformConnectionManager(IDocumentStoreHolder documentStore)
        {
            _documentStore = documentStore.Store;
        }

        public async Task<UserPlatformConnection> GetAsync(PlatformToken platformToken, UserName username)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                var connection = await session.Query<UserPlatformConnection>().FirstOrDefaultAsync(f => f.PlatformToken == platformToken.Value && f.Username == username.Value);

                return connection;
            }
        }
    }
}