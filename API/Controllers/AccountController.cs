using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using API.Interfaces;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
         public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
         {
            _mapper = mapper;
             _tokenService = tokenService;
             _context = context;
         }
         [HttpPost("register")]
         public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDTO)
         {
            if (await UserExist(registerDTO.Username)) return  BadRequest("Username is taken");
            
            var user = _mapper.Map<AppUser>(registerDTO);

             

                user.UserName = registerDTO.Username.ToLower();
               
          
             _context.Users.Add(user);
            await _context.SaveChangesAsync();

             return new UserDto
             {
                 Username = user.UserName,
                 Token = _tokenService.CreateToken(user),
                 KnownAs = user.KnownAs,
                 Gender = user.Gender
             };
         }

         [HttpPost("login")]
         public async Task<ActionResult<UserDto>> Login( LoginDTO loginDTO)
         {
             var user = await _context.Users
             .Include(p => p.Photos)
             .SingleOrDefaultAsync(x => x.Username == loginDTO.Username);
             if(user == null) return Unauthorized("Invalid username");

            


             return new UserDto
             {
                 Username = user.UserName,
                 Token = _tokenService.CreateToken(user),
                 PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                 KnownAs = user.KnownAs,
                 Gender = user.Gender
             };
         }

         private async Task<bool> UserExist(string username)
         {
             return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
         }
    }
}