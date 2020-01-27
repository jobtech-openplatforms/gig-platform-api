using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jobtech.OpenPlatforms.GigPlatformApi.GigDataService.AuthenticationHandlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthenticationConfigService _authenticationConfigService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthenticationConfigService authenticationConfigService
            )
            : base(options, logger, encoder, clock)
        {
            _authenticationConfigService = authenticationConfigService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("X-Auth-Token"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["X-Auth-Token"]);

                if (authHeader == null)
                    return AuthenticateResult.Fail("Invalid Authorization Header");

                if (authHeader.Parameter != _authenticationConfigService.Token)
                {
                    return AuthenticateResult.Fail("Invalid Authorization Token");
                }
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header. "+ ex.Message);
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, "CVData"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}