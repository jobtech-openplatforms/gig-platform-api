﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities.Api;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public class ConnectionUserManager : IConnectionUserManager
    {
        private readonly IDocumentStore _documentStore;

        public ConnectionUserManager(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task<PlatformUser> GetAsync(ConnectionId id)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<PlatformUser>(id.Value);
            }
        }

        public async Task<User> GetUserAsync(UserId userId)
        {

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<User>(userId.Value);
            }
        }

        public async Task<User> GetUserAsync(UserName username)
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                return await session.Query<User>().Where(u => u.UserName == username.Value).FirstOrDefaultAsync();
            }
        }

        public async Task SaveAsync(PlatformUser user, Connection connection)
        {

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(user);
                var upd = DateTime.UtcNow; // Do we need to adjust for time passed during the update request?
                connection.LastUpdatedUtc = IsoDateTimeUtc.FromDateTime(new DateTime(upd.Year, upd.Month,upd.Day,upd.Hour,upd.Minute,0));
                await session.StoreAsync(connection);
                await session.SaveChangesAsync();
            }
        }
    }
}