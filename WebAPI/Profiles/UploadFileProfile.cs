using AutoMapper;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.Profiles
{
    public class UploadFileProfile : Profile
    {
        public UploadFileProfile()
        {
            CreateMap<UploadFile, UploadFileDto>();
            CreateMap<UploadFilePostDto, UploadFile>();
        }
    }
}
