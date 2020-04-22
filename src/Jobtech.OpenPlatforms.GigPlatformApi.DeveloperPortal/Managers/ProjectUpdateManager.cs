using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers
{
    public class ProjectUpdateManager : IProjectUpdateManager
    {
        private readonly IProjectManager _projectManager;
        private readonly IPlatformAdminHttpClient _platformAdminHttpClient;
        private readonly IApplicationHttpClient _applicationHttpClient;
        private readonly IPlatformAdminUserManager _platformAdminUserManager;
        private readonly IDocumentStore _documentStore;
        private readonly ILogger<ProjectUpdateManager> _logger;

        public ProjectUpdateManager(IProjectManager projectManager,
            IPlatformAdminHttpClient gigDataHttpClient,
            IApplicationHttpClient applicationHttpClient,
            IPlatformAdminUserManager platformAdminUserManager,
            IDocumentStore documentStoreHolder,
            ILogger<ProjectUpdateManager> logger)
        {
            _projectManager = projectManager;
            _platformAdminHttpClient = gigDataHttpClient;
            _applicationHttpClient = applicationHttpClient;
            _platformAdminUserManager = platformAdminUserManager;
            _documentStore = documentStoreHolder;
            _logger = logger;
        }
        public async Task<Project> Update(UpdateProjectRequest request, IAsyncDocumentSession session, PlatformAdminUserId userId)
        {

            _logger.LogInformation("Project update {@projectId}", request.Id);

            var requestName = request.Name.Trim();

            if (string.IsNullOrEmpty(requestName) || string.IsNullOrWhiteSpace(requestName))
            {
                _logger.LogError("Project update - empty name requested");
                throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.BadRequest);
            }

            var testMode = TestProjectId.IsValidIdentity(request.Id) && !ProjectId.IsValidIdentity(request.Id);

            var project = testMode ? await _projectManager.GetTest((TestProjectId)request.Id, session) : await _projectManager.Get((ProjectId)request.Id, session);

            if (!project.AdminIds.Contains(userId.Value) && project.OwnerAdminId != userId.Value)
            {
                _logger.LogError("User {userId} is not part of {ownerId} or {@adminIds}", userId.Value, project.OwnerAdminId, project.AdminIds);
                throw new ApiException("Seems you are not an admin on this project.", (int)System.Net.HttpStatusCode.Unauthorized);
            }

            var updates = new List<string>();
            var application = project.Applications.FirstOrDefault();
            var platform = project.Platforms.FirstOrDefault();

            if (project.Name != requestName)
            {
                if (testMode)
                {
                    _logger.LogInformation("TestProject - skipping name update {@projectId}", request.Id);
                }
                else
                {
                    // TODO: Catch exception thrown on failed update
                    if (application != null)
                        await _applicationHttpClient.SetName(application.Id, requestName);
                    if (platform != null)
                        await _platformAdminHttpClient.SetName(platform.Id.ToString(), requestName);

                    project.Name = requestName;
                    updates.Add(nameof(Project.Name));
                    // _logger.LogInformation("Project update: {@property}", nameof(Project.Name));
                }
            }
            if (project.LogoUrl != request.LogoUrl)
            {
                // TODO: Catch exception thrown on failed update
                if (application != null)
                    await _applicationHttpClient.SetLogoUrl(application.Id, request.LogoUrl);
                if (platform != null)
                    await _platformAdminHttpClient.SetLogoUrl(platform.Id.ToString(), request.LogoUrl);
                project.LogoUrl = request.LogoUrl;
                updates.Add(nameof(Project.LogoUrl));
                // _logger.LogInformation("Project update: {@property}", nameof(Project.LogoUrl));
            }
            if (project.Description != request.Description)
            {
                // TODO: Catch exception thrown on failed update
                if (application != null)
                    await _applicationHttpClient.SetDescription(application.Id, request.Description);
                if (platform != null)
                    await _platformAdminHttpClient.SetDescription(platform.Id.ToString(), request.Description);
                project.Description = request.Description;
                updates.Add(nameof(Project.Description));
                // _logger.LogInformation("Project update: {@property}", nameof(Project.Description));
            }
            if (project.Webpage != request.Webpage)
            {
                // TODO: Catch exception thrown on failed update
                if (application != null)
                    await _applicationHttpClient.SetWebsiteUrl(application.Id, request.Webpage);
                if (platform != null)
                    await _platformAdminHttpClient.SetWebsiteUrl(platform.Id.ToString(), request.Webpage);
                project.Webpage = request.Webpage;
                updates.Add(nameof(Project.Webpage));
                // _logger.LogInformation("Project update: {@property}", nameof(Project.Webpage));
            }

            if (updates.Any())
            {
                _logger.LogInformation("Project update: Preparing to save {@updates}", updates);
                project = await _projectManager.Update(project, session);
            }
            return project;
        }
    }
}