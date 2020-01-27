using System;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Messages;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Retry.Simple;

namespace Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.MessageHandlers
{
    public class PlatformUserDataMessageHandler : IHandleMessages<PlatformUserDataMessage>, IHandleMessages<IFailed<PlatformUserDataMessage>>
    {
        private readonly IDocumentStore _documentStore;
        private readonly IConnectionManager _connectionManager;
        private readonly IBus _bus;

        public PlatformUserDataMessageHandler(IConnectionManager connectionManager, IDocumentStore documentStore, IBus bus)
        {
            _connectionManager = connectionManager;
            _documentStore = documentStore;
            _bus = bus;
        }

        public async Task Handle(PlatformUserDataMessage message)
        {
            await Task.CompletedTask;
            //return await Task.CompletedTask;
            //using (var session = _documentStore.OpenAsyncSession())
            //{
            //    var platformDataId = await HandleFetchDataResult(message.UserId, message.Result, session);

            //    var user = await session.LoadAsync<User>(message.UserId);
            //    var platformConnection = user.PlatformConnections.Single(pc => pc.Type == message.PlatformType);

            //    await session.SaveChangesAsync();
            //}

        }

        public async Task Handle(IFailed<PlatformUserDataMessage> message)
        {
            await _bus.Advanced.TransportMessage.Defer(TimeSpan.FromSeconds(60));
        }

        private async Task<string> HandleFetchDataResult(string userId, PlatformDataFetchResult result, IAsyncDocumentSession session)
        {
            return await Task.FromResult( userId);
            //var platformData = await _connectionManager.AddPlatformData(userId, result.NumberOfGigs, result.PeriodStart,
            //    result.PeriodEnd, result.Ratings, result.AverageRating, result.Reviews, result.RawData, session);

            //var user = await session.LoadAsync<User>(userId);
            //var platformConnection = user.PlatformConnections.Single(pc => pc.Type == type);
            //platformConnection.MarkAsDataFetchSuccessful();

            //return platformData.Id;
        }
    }
}
