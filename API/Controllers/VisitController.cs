using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   
    public class VisitController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public VisitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Policy ="RequireVIPRole")]
        [HttpPost("{username}")]
        public async Task<ActionResult> AddVisit(string username)
        {
            var sourceUserId = User.GetUserId();
            var visitedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _unitOfWork.VisitsRepository.GetUserWithVisits(sourceUserId);

            if (visitedUser == null) return NotFound();

            if (sourceUser.UserName == username) return NoContent();

            var userVisit = await _unitOfWork.VisitsRepository.GetUserVisit(sourceUserId, visitedUser.Id);

            if (userVisit != null) return NoContent();

            userVisit = new Entities.UserVisit
            {
                SourceUserId = sourceUserId,
                VisitedUserId = visitedUser.Id
            };

            sourceUser.VisitedUsers.Add(userVisit);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to add visit");
        }

        [Authorize(Policy ="RequireVIPRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetUserVisits([FromQuery]VisitsParams visitsParams)
        {
            visitsParams.UserId = User.GetUserId();
            var users = await _unitOfWork.VisitsRepository.GetUserVisits(visitsParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount,
             users.TotalPages);

            return Ok(users);
        }
    }
}
