using AF.Gig.Common.Models;
using AF.GigPlatform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Connectivity.Models
{

    public class PlatformResponse
    {
        public Guid PlatformId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Webpage { get;  set; }
        public string LogoUrl { get;  set; }
        public PlatformAuthenticationMechanism AuthMechanism { get;  set; }
        public bool IsConnected { get;  set; }
    }

    public static class PlatformResponseExtensions
    {
        public static PlatformResponse AsResponse(this Platform platform)//, Project project)
            => new PlatformResponse { PlatformId = platform.Id
                //, Description = project.Description
                //, Name = project.Name
                //, Webpage = project.Webpage
                //, LogoUrl = project.LogoUrl
            };

        public static IEnumerable<PlatformResponse> AsResponse(this IEnumerable<Platform> platforms)
        {
            foreach (var p in platforms)
            {
                yield return p.AsResponse();
            }
        }
    }
}
