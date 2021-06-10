using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using API.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API.Interfaces;
using System.Collections.Generic;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }


        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotosForModeration()
        {
            return Ok(await _unitOfWork.PhotoRepository.GetUnapprovedPhotos());
        }
        [HttpPut("approve-photo/{id}")]
        public async Task<ActionResult> ApprovePhoto(int id)
        {
            Photo photo = _unitOfWork.PhotoRepository.GetPhotoById(id);
            if (photo.isApproved) return BadRequest("Photo is already approved");

            photo.isApproved = true;

          var  user = _unitOfWork.PhotoRepository.GetUserByPhotoId(id);

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null)
            {
                photo.IsMain = true;
            }
           


            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to approve photo");

        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("reject-photo/{id}")]
        public async Task<ActionResult> RejectPhotho(int id)
        {
            Photo photo = _unitOfWork.PhotoRepository.GetPhotoById(id);
            if (photo == null) return BadRequest("No photho with provided id was found");
            _unitOfWork.PhotoRepository.RemovePhoto(id);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to reject photo");
        }
    }
} 
