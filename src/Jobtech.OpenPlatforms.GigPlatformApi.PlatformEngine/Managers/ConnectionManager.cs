using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.Exceptions;
using AF.GigPlatform.Connectivity.Handlers;
using AF.GigPlatform.Connectivity.Models;
using AF.GigPlatform.Core.ValueObjects;
using AF.GigPlatform.Store.Config;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AF.GigPlatform.PlatformEngine.Managers
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly IDocumentStore _documentStore;
        private readonly IPlatformManager _platformManager;
        private readonly IConnectionUserManager _connectionUserManager;
        private readonly IPlatformHttpClient _httpClient;

        public ConnectionManager(IDocumentStoreHolder documentStore,
                                    IPlatformHttpClient httpClient,
                                    IPlatformManager platformManager,
                                    IConnectionUserManager connectionUserManager)
        {
            _documentStore = documentStore.Store;
            _platformManager = platformManager;
            _httpClient = httpClient;
            _platformManager = platformManager;
            _connectionUserManager = connectionUserManager;
        }

    //    public async Task<bool> ConnectionExistsAsync(string id)
    //    {
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //        {
    //            return await session.Query<Connection>().AnyAsync(a => a.Id == $"connections/{id}");
    //        }
    //    }

    //    public async Task<bool> ConnectionExistsAsync(User user, Platform platform)
    //    {
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //        {
    //            return await session.Query<Connection>().AnyAsync(a => a.PlatformId == platform.Id && a.UserId == user.Id);
    //        }
    //    }

    //    public async Task<bool> ConnectionExistsAsync(PlatformId platformId, UserId userId)
    //    {
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //        {
    //            return await session.Query<Connection>().Where(a => a.PlatformId == platformId && a.UserId == userId).AnyAsync();
    //        }
    //    }

    //    public async Task<Connection> GetConnectionAsync(User user, Platform platform)
    //    {
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //        {
    //            return await session.Query<Connection>().Where(a => a.PlatformId == platform.Id && a.UserId == user.Id).FirstOrDefaultAsync();
    //        }
    //    }

    //    public async Task<Connection> GetConnectionAsync(ConnectionId id)
    //    {
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //        {
    //            return await session.LoadAsync<Connection>(id.Value);
    //        }
    //    }

    //    public async Task<IEnumerable<Connection>> GetConnectionsAsync()
    //    {
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //        {
    //            return await session.Query<Connection>().ToListAsync();
    //        }
    //    }

    //    public async Task<IEnumerable<Connection>> GetConnectionsAsync(UserId userId)
    //    {
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //        {
    //            return await session.Query<Connection>().Where(c => c.UserId == userId).ToListAsync();
    //        }
    //    }

    //    public async Task<UserDataRequest> GetRequestTokenAsync(Connection connection)
    //    {
    //        var platform = await _platformManager.GetPlatformAsync(connection.PlatformId);
    //        return new UserDataRequest ( platform.MyGigDataToken, connection.UserAccessToken, "");
    //    }

    //    public async Task RemoveConnectionAsync(ConnectionId id)
    //    {
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //        {
    //            var connection = await session.LoadAsync<Connection>($"{id}");
    //            session.Delete(connection);
    //            await session.SaveChangesAsync();
    //        }
    //    }

    //    public async Task<Connection> EstablishConnectionAsync(Platform platform, User user)
    //    {
    //        if (await ConnectionExistsAsync(platform.Id, user.Id))
    //        {
    //            return await GetConnectionAsync(user, platform);
    //        }

    //        try
    //        {
    //            var requestObject = new PlatformWebhookRequest { MyGigDataToken = platform.MyGigDataToken, Email = user.Email };
    //            var accessModelResponse = await _httpClient.GetUserAccessTokenAsync(requestObject, platform.ExportDataUri);

    //            // Save connection

    //            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //            {
    //                var connection = new Connection
    //                {
    //                    PlatformId = platform.Id,
    //                    PlatformToken = platform.PlatformToken,
    //                    UserId = user.Id,
    //                    UserAccessToken = accessModelResponse.UserAccessToken
    //                };

    //                user.Connections.Add(connection);
    //                platform.Connections.Add(connection);

    //                await session.StoreAsync(connection);
    //                await session.SaveChangesAsync();

    //                await UpdateUserDataAsync(connection.Id);

    //                return connection;
    //            }
    //        }
    //        catch (HttpRequestException httpRequestException)
    //        {
    //            throw new ApiException(httpRequestException, System.Net.HttpStatusCode.UnprocessableEntity);
    //        }
    //    }

    //    public async Task<Connection> SynchronizeConnectionAsync(Platform platform, User user)
    //    {
    //        var connection = await EstablishConnectionAsync(platform, user);
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //            return await SynchronizeConnectionAsync(connection);
    //    }

    //    public async Task<Connection> SynchronizeConnectionAsync(ConnectionId connectionId)
    //    {
    //        var connection = await GetConnectionAsync(connectionId);
    //        using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
    //            return await SynchronizeConnectionAsync(connection);
    //    }

    //    public async Task<Connection> SynchronizeConnectionAsync(Connection connection)
    //    {
    //        await UpdateUserDataAsync(connection.Id);
    //            return connection;
    //    }
    

    //public async Task UpdateUserDataAsync(ConnectionId connectionId)
    //{
    //    var connection = await GetConnectionAsync(connectionId);

    //    if (connection == null)
    //        throw new ApiException($"Seems like this connection between you and the platform was lost. [{connectionId}]");

    //    var platform = await _platformManager.GetPlatformAsync(connection.PlatformId);

    //    /// TODO: Get the latest updated date and send along with the request

    //    var result = await _httpClient.GetUserDataFromPlatformAsync(await GetRequestTokenAsync(connection), platform.ExportDataUri);

    //    /// TODO: Compare interactions to only save the relevant ones
    //    ///

    //    /// TODO: If successful, save the last updated date to the database
    //    ///

    //    await _connectionUserManager.SaveAsync(result, connection);
    //}
}
}