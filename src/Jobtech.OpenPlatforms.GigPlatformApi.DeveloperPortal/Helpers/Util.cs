using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Helpers
{
    public static class Util
    {
  
        public static IEnumerable<string> UriErrors(Dictionary<string, string> uriStrings, ILogger logger)
        {
            foreach (var uri in uriStrings)
            {
                var valid = false;
                try
                {
                    new Uri(uri.Value);
                    valid = true;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Invalid URI {uri}", uri.Value);
                }
                if (!valid)
                {
                    yield return uri.Key; // Just return the ID of the error field
                }
            }
        }
    }
}