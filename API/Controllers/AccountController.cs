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
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
     
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

         public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
         {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
             _tokenService = tokenService;
          
         }
         [HttpPost("register")]
         public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDTO)
         {
            if (await UserExist(registerDTO.Username)) return  BadRequest("Username is taken");
            
            var user = _mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.Username.ToLower();
            
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
        
            if (!result.Succeeded) return BadRequest(result);

             var roleResult = await _userManager.AddToRoleAsync(user, "Member");

             if (!roleResult.Succeeded) return BadRequest(result);

             return new UserDto
             {
                 Username = user.UserName,
                 Token = await _tokenService.CreateToken(user),
                 KnownAs = user.KnownAs,
                 Gender = user.Gender
             };
         }

         [HttpPost("login")]
         public async Task<ActionResult<UserDto>> Login( LoginDTO loginDTO)
         {
             var user = await _userManager.Users
             .Include(p => p.Photos)
             .SingleOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());
             if(user == null) return Unauthorized("Invalid username");

            
            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password); //CheckPasswordSignInAsync

            if (!result) return Unauthorized(result);
             return new UserDto
             {
                 Username = user.UserName,
                 Token = await _tokenService.CreateToken(user),
                 PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                 KnownAs = user.KnownAs,
                 Gender = user.Gender
             };
         }

         private async Task<bool> UserExist(string username)
         {
             return await _signInManager.UserManager.Users.AnyAsync(x => x.UserName == username.ToLower());
         }
    }
}