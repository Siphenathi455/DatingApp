using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;


namespace API.Controllers
{
   [Authorize]
    public class UsersController : BaseApiController
    {
       private readonly IUserRepository _userRepository; 
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
    
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
            {
                var users = await  _userRepository.GetUsersAsync();
               
                return  Ok(users);
            }

           
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(string username)
            {
                return await _userRepository.GetUserByUsernameAsync(username);
               
            }
        
    }
}