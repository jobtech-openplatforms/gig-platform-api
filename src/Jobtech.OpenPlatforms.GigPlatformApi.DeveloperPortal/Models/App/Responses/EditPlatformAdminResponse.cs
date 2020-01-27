using System.Collections.Generic;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models.App.Responses
{
    public class EditPlatformUserResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public static class EditPlatformUserResponseExtensions
    {
        public static EditPlatformUserResponse ForEditing(this PlatformAdminUser admin)
            => new EditPlatformUserResponse
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email
            };

        public static IEnumerable<EditPlatformUserResponse> ForEditing(this IEnumerable<PlatformAdminUser> admin)
        {
            foreach (var p in admin)
            {
                yield return p.ForEditing();
            }
        }
    }
}