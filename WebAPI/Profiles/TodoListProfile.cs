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
                );
        }
    }
}
