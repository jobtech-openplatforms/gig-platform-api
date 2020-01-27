using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers
{
    public interface IAppManager
    {
        CvApp Register(CvApp app);
        CvApp Update(CvApp app);
    }
}