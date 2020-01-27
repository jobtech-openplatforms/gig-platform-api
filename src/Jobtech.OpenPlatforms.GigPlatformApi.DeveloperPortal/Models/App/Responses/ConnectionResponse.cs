using System.Collections.Generic;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models.App.Responses
{
    public class ConnectionResponse
    {
        public string Id { get; set; }
        public string PlatformId { get; set; }
        public string UserId { get; set; }
        public IsoDateTimeUtc LastUpdatedUtc { get; set; }
    }

    public static class ConnectionResponseExtensions
    {
        public static ConnectionResponse AsResponse(this Connection connection)
            => new ConnectionResponse { Id = connection.Id, PlatformId = connection.PlatformId, UserId = connection.UserId };

        public static IEnumerable<ConnectionResponse> AsResponse(this IEnumerable<Connection> Connections)
        {
            foreach (var p in Connections)
            {
                yield return p.AsResponse();
            }
        }
    }
}
