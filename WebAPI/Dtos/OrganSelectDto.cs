using Microsoft.AspNetCore.Identity.Data;
using WebAPI.Controllers;

namespace WebAPI.Dtos
{
    public class OrganSelectDto
    {
        public Guid FatherOrganId { get; set; }

        public Guid OrganId { get; set; }

        public string Name { get; set; }

        public string Src { get; set; }

        public 第二兒子 Two { get; set; }
    }
}
