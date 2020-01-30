using AutoMapper;
using JLSDataModel.Models.User;
using JLSMobileApplication.Resources;

namespace JLSMobileApplication.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationView, User>();// 将UserRegistrationView 映射到user中
        }
    }
}
