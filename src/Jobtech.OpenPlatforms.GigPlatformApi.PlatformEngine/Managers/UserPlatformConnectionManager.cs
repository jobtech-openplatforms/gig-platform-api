using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public class UserPlatformConnectionManager : IUserPlatformConnectionManager
    {
        private readonly IDocumentStore _documentStore;

        public UserPlatformConnectionManager(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
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