using WebAPI.Models;

namespace WebAPI.Dtos
{
    public class TodoListDto
    {
        public Guid TodoId { get; set; }
        public string Name { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool Enable { get; set; }
        public int Orders { get; set; }

        //public Guid InsertEmployeeId { get; set; }
        //public Guid UpdateEmployeeId { get; set; }

        public string InsertEmployeeName { get; set; }
        public string UpdateEmployeeName { get; set; }
        public ICollection<UploadFileDto> UploadFiles { get; set; }

    }
}
