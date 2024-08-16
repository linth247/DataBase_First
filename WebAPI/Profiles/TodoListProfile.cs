using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.Profiles
{
    public class TodoListProfile : Profile
    {
        public TodoListProfile() 
        {
            CreateMap<TodoList, TodoListDto>()
                .ForMember(
                dest=>dest.InsertEmployeeName,
                opt=>opt.MapFrom(src=>src.InsertEmployee.Name+"("+src.InsertEmployeeId+")")
                )
                .ForMember(
                dest => dest.UpdateEmployeeName,
                opt => opt.MapFrom(src => src.UpdateEmployee.Name + "(" + src.UpdateEmployeeId + ")")
                )
                .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name + "(use automaper)")
                );

            CreateMap<TodoListPostDto, TodoList>();

            //37.【6.更新資料PUT與PATCH】ASP.NET Core Web API 入門教學(6_3) - 使用AutoMapper更新資料
            CreateMap<TodoListPutDto, TodoList>()
               .ForMember(
                dest => dest.UpdateTime,
                opt => opt.MapFrom(src => DateTime.Now)
                )
                .ForMember(
                dest => dest.UpdateEmployeeId,
                opt => opt.MapFrom(src => Guid.Parse("63F8FD9D-E045-4C78-A491-96EABE1D2024"))
                );
        }
    }
}
