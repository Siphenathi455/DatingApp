using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Data;
using API.DTOs;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using AutoMapper;


namespace API.Controllers
{
   [Authorize]
    public class UsersController : BaseApiController
    {
       private readonly IUserRepository _userRepository; 
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
    
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
            {
                var users = await _userRepository.GetMembersAsync();

               
               
                return  Ok(users);
            }

           
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
            {
               

                return  await _userRepository.GetMemberAsync(username);
               
            }
        
    }
}