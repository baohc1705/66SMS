using _66SMS.Domain.Entities;
using _66SMS.Persistence.Configurations.Identity;
using AutoMapper;

namespace _66SMS.Persistence.Commons.Mappers
{
    public class ApplicationUserMapperProfile : Profile
    {
        public ApplicationUserMapperProfile()
        {
            CreateMap<ApplicationUser, User>()
                .ConvertUsing(new NullValueIgnoringConverter<ApplicationUser, User>());
            CreateMap<User, ApplicationUser>()
                .ConvertUsing(new NullValueIgnoringConverter<User, ApplicationUser>());
        }
    }
}
