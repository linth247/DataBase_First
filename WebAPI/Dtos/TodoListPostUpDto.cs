using WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using WebAPI.ValidationAttributes;
using WebAPI.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Dtos
{
    public class TodoListPostUpDto
    {
        [ModelBinder(BinderType = typeof(FormDataJsonBinder))]
        public TodoListPostDto TodoList {  get; set; }
        public IFormFileCollection files { get; set; }
    }
}
