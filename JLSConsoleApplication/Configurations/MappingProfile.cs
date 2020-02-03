using AutoMapper;
using JLSConsoleApplication.Resources;
using JLSDataModel.Models;
using JLSDataModel.Models.Product;
using JLSDataModel.Models.User;

namespace JLSMobileApplication.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationView, User>();// 将UserRegistrationView 映射到user中
            CreateMap<ProductRegistrationView, Product>();
            CreateMap<ReferenceLabelRegistrationView, ReferenceLabel>();
        }
    }
}
