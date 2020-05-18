using System;
using System.Collections.Generic;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models.App.Responses
{
    public class EditPlatformResponse
    {
        public Guid Id { get; set; }
        public string PlatformToken { get; set; }
        public string ExportDataUri { get; set; }
    }

    public static class EditPlatformResponseExtensions
    {
        public static EditPlatformResponse ForEditing(this Core.Entities.Platform platform)
            => new EditPlatformResponse
            {
                Id = ((PlatformId)platform.Id),
                PlatformToken = platform.PlatformToken,
                ExportDataUri = platform.ExportDataUri
            };

        public static IEnumerable<EditPlatformResponse> ForEditing(this IEnumerable<Core.Entities.Platform> platforms)
        {
            foreach (var p in platforms)
            {
                yield return p.ForEditing();
            }
        }
    }
}