using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.ApplicationApi;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/test/app/")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ApplicationTestController : ControllerBase
    {
        private IProjectManager _projectManager;
        private readonly IApplicationTestHttpClient _applicationTestHttpClient;
        private readonly IDocumentStore _documentStore;
        private readonly IPlatformAdminUserManager _platformAdminUserManager;
        private readonly ILogger<ApplicationTestController> _logger;

        public ApplicationTestController(
            IProjectManager projectManager,
            IApplicationTestHttpClient applicationTestHttpClient,
            IPlatformAdminUserManager platformAdminUserManager,
            IDocumentStore documentStoreHolder,
            ILogger<ApplicationTestController> logger)
        {
            _projectManager = projectManager;
            _applicationTestHttpClient = applicationTestHttpClient;
            _platformAdminUserManager = platformAdminUserManager;
            _documentStore = documentStoreHolder;
            _logger = logger;
        }

        [HttpGet("{projectType}/{id}/auth/")]
        public async Task<IActionResult> Auth([FromRoute]string projectType, [FromRoute]string id)
                => await AuthTest($"{projectType}/{id}", "completed");

        [HttpGet("{projectType}/{id}/auth/cancel")]
        public async Task<IActionResult> AuthCancel([FromRoute]string projectType, [FromRoute]string id)
                => await AuthTest($"{projectType}/{id}", "cancelled");

        private async Task<IActionResult> AuthTest(string projectId, string resultOutcome)
        {
            try
            {
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
                var testMode = TestProjectId.IsValidIdentity(projectId) && !ProjectId.IsValidIdentity(projectId);
                var project = testMode ? await _projectManager.GetTest((TestProjectId)projectId, session) : await _projectManager.Get((ProjectId)projectId, session);

                var result = await _applicationTestHttpClient.SendAuthCallback(project.Applications.First(), Guid.NewGuid().ToString(), resultOutcome, Guid.NewGuid().ToString());
                return Ok(result);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Unable to test application. {@request}", projectId);
                // return error message if there was an exception

                return Ok(GenericResponse.Failed(ex.Message, (System.Net.HttpStatusCode)ex.StatusCode));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to test application. {@request}", projectId);
                // return error message if there was an exception

                return Ok(GenericResponse.Failed(ex.Message, System.Net.HttpStatusCode.BadRequest));
            }

        }


        [HttpGet("{projectType}/{id}/[action]")]
        public async Task<IActionResult> Dummy([FromRoute]string projectType, [FromRoute]string id)
        {
            var projectId = $"{projectType}/{id}";
            _logger.LogInformation("Dummy data for {projectId}", projectId);
            using var session = _documentStore.OpenAsyncSession();
            //var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);
            var testMode = TestProjectId.IsValidIdentity(projectId) && !ProjectId.IsValidIdentity(projectId);
            var project = testMode ? await _projectManager.GetTest((TestProjectId)projectId, session) : await _projectManager.Get((ProjectId)projectId, session);

            var payload = new PlatformConnectionUpdateNotificationPayload
            {
                PlatformId = Guid.NewGuid(),
                PlatformName = "Dummy Data Test Platform",
                PlatformConnectionState = GigDataCommon.Library.PlatformConnectionState.Connected,
                UserId = Guid.NewGuid(),
                Updated = DateTimeOffset.UtcNow.AddHours(-1).ToUnixTimeSeconds(),
                AppSecret = project.Applications.FirstOrDefault()?.SecretKey,
                Reason = GigDataCommon.Library.NotificationReason.DataUpdate,
                PlatformData = new PlatformDataPayload
                {
                    AverageRating = new PlatformRatingPayload(new PlatformRating(Guid.NewGuid(), 5, 1, 5, 3)),
                    NumberOfRatings = 3,
                    NumberOfRatingsThatAreDeemedSuccessful = 3,
                    NumberOfGigs = 3,
                    PeriodStart = DateTime.UtcNow.AddYears(-1),
                    PeriodEnd = DateTime.UtcNow
                }
            };

            _logger.LogInformation("Dummy data for {projectId} {payload}", projectId, payload);
            return Ok(payload);
        }
    }



    //public class PlatformConnectionUpdateNotificationPayload
    //{
    //    public Guid PlatformId { get; set; }
    //    public string PlatformName { get; set; }
    //    [JsonConverter(typeof(StringEnumConverter))]
    //    public PlatformConnectionState PlatformConnectionState { get; set; }
    //    public Guid UserId { get; set; }
    //    public long Updated { get; set; }
    //    public PlatformDataPayload PlatformData { get; set; }
    //    public string AppSecret { get; set; }
    //    [JsonConverter(typeof(StringEnumConverter))]
    //    public NotificationReason Reason { get; set; }
    //}
    //public class PlatformDataPayload
    //{
    //    public int NumberOfGigs { get; set; }
    //    public int NumberOfRatings { get; set; }
    //    public int NumberOfRatingsThatAreDeemedSuccessful { get; set; }
    //    [JsonConverter(typeof(YearMonthDayDateTimeConverter))]
    //    public DateTimeOffset? PeriodStart { get; set; }
    //    [JsonConverter(typeof(YearMonthDayDateTimeConverter))]
    //    public DateTimeOffset? PeriodEnd { get; set; }
    //    public PlatformRatingPayload AverageRating { get; set; }
    //    public IList<PlatformReviewPayload> Reviews { get; set; }
    //    public IList<PlatformAchievementPayload> Achievements { get; set; }
    //}
    //public class PlatformReviewPayload
    //{
    //    public string ReviewId { get; set; }
    //    [JsonConverter(typeof(YearMonthDayDateTimeConverter))]
    //    public DateTimeOffset? ReviewDate { get; set; }
    //    public PlatformRatingPayload Rating { get; set; }
    //    public string ReviewHeading { get; set; }
    //    public string ReviewText { get; set; }
    //    public string ReviewerName { get; set; }
    //    public string ReviewerAvatarUri { get; set; }
    //}
    //public class PlatformAchievementPayload
    //{
    //    public string AchievementId { get; set; }
    //    public string Name { get; set; }
    //    public string AchievementPlatformType { get; set; }
    //    [JsonConverter(typeof(StringEnumConverter))]
    //    public PlatformAchievementType AchievementType { get; set; }
    //    public string Description { get; set; }
    //    public string ImageUrl { get; set; }
    //    public PlatformAchievementScorePayload Score { get; set; }
    //}
    //public class PlatformAchievementScorePayload
    //{
    //    public string Value { get; set; }
    //    public string Label { get; set; }
    //}
    //public class PlatformRatingPayload
    //{
    //    public PlatformRatingPayload(decimal value, decimal min, decimal max, bool isSuccessful)
    //    {
    //        Value = value;
    //        Min = min;
    //        Max = max;
    //        IsSuccessful = isSuccessful;
    //    }

    //    public decimal Value { get; set; }
    //    public decimal Min { get; set; }
    //    public decimal Max { get; set; }
    //    public bool IsSuccessful { get; set; }
    //}
    //public enum PlatformAchievementType
    //{
    //    QualificationAssessment,
    //    Badge
    //}

    //public enum PlatformConnectionState
    //{
    //    AwaitingOAuthAuthentication,
    //    AwaitingEmailVerification,
    //    Connected,
    //    Synced,
    //    Removed
    //}

}