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

             using var hmac = new HMACSHA512();

                user.Username = registerDTO.Username.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
                user.PasswordSalt = hmac.Key;
          
             _context.Users.Add(user);
            await _context.SaveChangesAsync();

             return new UserDto
             {
                 Username = user.Username,
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

             using var hmac = new HMACSHA512(user.PasswordSalt);

             var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

             for(int i = 0; i < computedHash.Length; i++)
             {
                 if(computedHash[i] !=user.PasswordHash[i]) return Unauthorized("Invalid password");
             }

             return new UserDto
             {
                 Username = user.Username,
                 Token = _tokenService.CreateToken(user),
                 PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                 KnownAs = user.KnownAs,
                 Gender = user.Gender
             };
         }

         private async Task<bool> UserExist(string username)
         {
             return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
         }
    }
}