using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public interface IApplicationManager
    {
        Task<IEnumerable<Application>> GetApplicationsAsync();
        Task<IEnumerable<Application>> GetApplicationsAsync(PlatformAdminUserId platformAdminUserId);

        //Task<Application> GetApplicationAsync(ApplicationId applicationId);
        //Task<Application> GetApplicationByTokenAsync(ApplicationToken platformToken);
        Task<Application> RegisterApplicationAsync(ApplicationRegistrationRequest value);
        //Task<Application> UpdateApplicationAsync(Application application);
        //Task<Application> UpdateApplicationAdminAsync(Application application);

        //Task<Application> AddAdminAsync(Application platform, ApplicationAdminUser admin);
        Task<Application> CreateApplicationAsync(Application entity);
        //Task<Application> CreateAdminApplicationAsync(Application platform, ApplicationAdminUser admin);
    }
}