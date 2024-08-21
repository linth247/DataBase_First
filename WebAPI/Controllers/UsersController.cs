using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(WebContext context) : ControllerBase
    {
        [HttpGet]
        //public ActionResult<IEnumerable<Users>> GetUsers()
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            //var users = context.Users.ToList();
            var users = await context.Users.ToListAsync();

            return users;
        }

        [HttpGet("{id:int}")]  // /api/users
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null) return NotFound();

            return user;
        }
    }
}
