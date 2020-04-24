using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Models.GigDataService;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Store;
using Jobtech.OpenPlatforms.GigPlatformApi.Store.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Helpers;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ApplicationController : ControllerBase
    {
        private IProjectManager _projectManager;
        private readonly IApplicationHttpClient _applicationHttpClient;
        private readonly IDocumentStore _documentStore;
        private readonly ILogger<ApplicationController> _logger;
        private readonly IPlatformAdminUserManager _platformAdminUserManager;

        public ApplicationController(
            IProjectManager projectManager,
            IApplicationHttpClient applicationHttpClient,
            IPlatformAdminUserManager platformAdminUserManager,
            IDocumentStore documentStoreHolder,
            ILogger<ApplicationController> logger)
        {
            _projectManager = projectManager;
            _applicationHttpClient = applicationHttpClient;
            _platformAdminUserManager = platformAdminUserManager;
            _documentStore = documentStoreHolder;
            _logger = logger;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody]ApplicationCreationRequest request)
        {

            try
            {
                // Who's logged in?
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);

                // Which project are we working on?

                var testMode = TestProjectId.IsValidIdentity(request.ProjectId) && !ProjectId.IsValidIdentity(request.ProjectId);

                var project = testMode ? await _projectManager.GetTest((TestProjectId)request.ProjectId, session) : await _projectManager.Get((ProjectId)request.ProjectId, session);

                // Does the user have access to the project?
                if (!project.AdminIds.Contains(user.Id) && project.OwnerAdminId != user.Id)
                {
                    throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
                }

                var registeredApplication = await _applicationHttpClient.CreateApplication(new CreateApplicationModel
                {
                    Name = project.Name,
                    AuthCallbackUrl = request.AuthCallbackUrl ?? "",
                    //EmailVerificationNotificationEndpointUrl = request.EmailVerificationUrl ?? "",
                    DataUpdateCallbackUrl = request.DataUpdateCallbackUrl ?? ""
                });
                if (string.IsNullOrEmpty(registeredApplication.ApplicationId))
                {
                    _logger.LogError("Create application failed. {@request} {@registeredApplication}", request, registeredApplication);
                    throw new ApiException("Creating the application failed.");
                }
                var application = request.CreateApplication(registeredApplication);

                // One application per project, so just replace
                project.Applications = new List<Core.Entities.Application> { application };
                // Save
                project = await _projectManager.Update(project, session);

                return Ok(project);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Unable to create application. {@request}", request);
                // return error message if there was an exception

                // return BadRequest(new { message = ex.Message });
                throw;
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Save([FromBody]UpdateApplicationUrlsRequest request)
        {
            var errors = Util.UriErrors(new Dictionary<string, string> {
                { "auth-callback-url", request.AuthCallbackUrl },
                { "data-update-callback-url", request.DataUpdateCallbackUrl },
                //{ "email-verification-url", request.EmailVerificationUrl },
            }, _logger);
            // if (errors != null && errors.Any())
            // {
            //     return BadRequest(new { message = "All urls have to be valid.", errors = errors });
            // }
            try
            {
                // Who's logged in?
                using var session = _documentStore.OpenAsyncSession();
                var user = await _platformAdminUserManager.GetByUniqueIdentifierAsync(User.Identity.Name, session);

                // Which project are we working on?


                var testMode = TestProjectId.IsValidIdentity(request.ProjectId) && !ProjectId.IsValidIdentity(request.ProjectId);

                var project = testMode ? await _projectManager.GetTest((TestProjectId)request.ProjectId, session) : await _projectManager.Get((ProjectId)request.ProjectId, session);

                // Does the user have access to the project?
                if (!project.AdminIds.Contains(user.Id) && project.OwnerAdminId != user.Id)
                {
                    throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
                }
                var application = project.Applications?.FirstOrDefault();
                bool recreated = false;

                if (application != null)
                {
                    // Check that the application exists through the API
                    try
                    {
                        var apiApplication = await _applicationHttpClient.Get(application.Id);

                    }
                    catch (ApiException ex)
                    {
                        // Unable to get the application
                        if (ex.InnerException is System.Net.Http.HttpRequestException && ex.StatusCode == (int)System.Net.HttpStatusCode.NotFound)
                        {
                            // Not found in API - recreate the application
                            _logger.LogInformation("Application not found in API. Attempting to re-create.");
                            var apiApplication = await _applicationHttpClient.CreateApplication(new CreateApplicationModel
                            {
                                Name = project.Name,
                                AuthCallbackUrl = request.AuthCallbackUrl ?? "",
                                //EmailVerificationNotificationEndpointUrl = request.EmailVerificationUrl ?? "",
                                DataUpdateCallbackUrl = request.DataUpdateCallbackUrl ?? ""
                            });
                            if (apiApplication != null)
                            {
                            _logger.LogInformation("New application created. {id}", apiApplication.ApplicationId);
                                recreated = true;
                                application = new Core.Entities.Application
                                {
                                    Id = apiApplication.ApplicationId,
                                    SecretKey = apiApplication.SecretKey,
                                    AuthCallbackUrl = request.AuthCallbackUrl,
                                    //EmailVerificationUrl = request.EmailVerificationUrl,
                                    DataUpdateCallbackUrl = request.DataUpdateCallbackUrl
                                };
                            }
                        }
                        if (ex.InnerException is System.Net.Http.HttpRequestException && ex.StatusCode == (int)System.Net.HttpStatusCode.RequestTimeout)
                        {
                            throw;
                        }
                        // throw;
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                    if (!recreated)
                    {

                        //await _applicationHttpClient.PatchEmailVerificationUrl(application.Id, request.EmailVerificationUrl);
                        await _applicationHttpClient.PatchApiEndpointAppSetNotificationUrl(application.Id, request.DataUpdateCallbackUrl);
                        await _applicationHttpClient.PatchAuthCallbackUrl(application.Id, request.AuthCallbackUrl);
                        application = new Core.Entities.Application
                        {
                            Id = application.Id,
                            SecretKey = application.SecretKey,
                            AuthCallbackUrl = request.AuthCallbackUrl,
                            //EmailVerificationUrl = request.EmailVerificationUrl,
                            DataUpdateCallbackUrl = request.DataUpdateCallbackUrl
                        };
                    }
                }
                else
                {
                    _logger.LogError("No application found.");
                    return BadRequest(new { message = "No application found." });
                }

                // One application per project, so just replace
                project.Applications = new List<Core.Entities.Application> { application };
                // Save
                project = await _projectManager.Update(project, session);

                return Ok(project);
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}