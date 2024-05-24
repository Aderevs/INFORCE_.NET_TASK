using AutoMapper;
using INFORCE_.NET_TASK.Server.Account;
using INFORCE_.NET_TASK.Server.DbLogic;

namespace INFORCE_.NET_TASK.Server
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<ShortenedUrl, ShortenedUrlDTO>();
            CreateMap<ShortenedUrlDTO, ShortenedUrl>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id));

            CreateMap<UrlModel, ShortenedUrlDTO>();
        }
    }
}
