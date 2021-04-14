using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
   
    public class UsersController : BaseApiController
    {
       private readonly DataContext _context; 
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
            {
                return await  _context.Users.ToListAsync();
               
            }

         [Authorize]   
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
            {
                return await _context.Users.FindAsync(id);
               
            }
        
    }
}