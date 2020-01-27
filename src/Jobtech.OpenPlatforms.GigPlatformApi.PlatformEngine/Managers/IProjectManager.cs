using System.Collections.Generic;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers
{
    public interface IProjectManager : IStoreManager<Project>
    {
        Task<IEnumerable<Project>> GetAll(PlatformAdminUserId userId);
        Task<Project> Update(Project project);
    }
}
