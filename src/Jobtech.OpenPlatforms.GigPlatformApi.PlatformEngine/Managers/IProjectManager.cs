using AF.GigPlatform.Core.Entities;
using AF.GigPlatform.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AF.GigPlatform.PlatformEngine.Managers
{
    public interface IProjectManager : IStoreManager<Project>
    {
        Task<IEnumerable<Project>> GetAll(PlatformAdminUserId userId);
        Task<Project> Update(Project project);
    }
}
