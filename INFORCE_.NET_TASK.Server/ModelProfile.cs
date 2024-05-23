using AutoMapper;
using INFORCE_.NET_TASK.Server.Account;
using INFORCE_.NET_TASK.Server.DbLogic;

namespace INFORCE_.NET_TASK.Server
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<RegistrationModel, User>()
                .ForMember(dest => dest.Salt, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => PasswordHasher.HashPassword(src.Password)));

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<ShortenedUrl, ShortenedUrlDTO>();
            CreateMap<ShortenedUrlDTO, ShortenedUrl>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id));
        }
    }
}
