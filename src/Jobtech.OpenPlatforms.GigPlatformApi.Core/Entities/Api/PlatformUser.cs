using System;
using System.Collections.Generic;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities.Api
{
    public class PlatformUser
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserAccessToken { get; set; } = Guid.NewGuid().ToString();
        public IEnumerable<PlatformInteraction> Interactions { get; set; } = new List<PlatformInteraction>();
    }
}