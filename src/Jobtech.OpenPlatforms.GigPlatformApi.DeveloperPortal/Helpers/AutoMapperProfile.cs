using AutoMapper;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models.App.Responses;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
            CreateMap<PlatformAdminUser, PlatformAdminUserModel>();
            CreateMap<PlatformAdminUserModel, PlatformAdminUser>();
            CreateMap<PlatformUpdateRequestModel, PlatformRequest>();
            CreateMap<PlatformRequest, PlatformUpdateRequestModel>();
            CreateMap<EditPlatformResponse, Core.Entities.Platform>();
            CreateMap<Core.Entities.Platform, EditPlatformResponse>();
        }
    }
}