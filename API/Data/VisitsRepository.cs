using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Extensions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class VisitsRepository : IVisitsRepository
    {

        private readonly DataContext _context;
        public VisitsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserVisit> GetUserVisit(int sourceUserId, int VisitedUserId)
        {
            return await _context.Visits.FindAsync(sourceUserId, VisitedUserId);
        }

        public async Task<PagedList<VisitDto>> GetUserVisits(VisitsParams visitsParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var visits = _context.Visits.AsQueryable();

            if (visitsParams.predicate == "visited")
            {
                visits = visits.Where(visits => visits.SourceUserId == visitsParams.UserId);
                users = visits.Select(visits => visits.VisitedUser); // List of users like by the current user
            }

            if (visitsParams.predicate == "visitedBy")
            {
                visits = visits.Where(visit => visit.VisitedUserId == visitsParams.UserId);
                users = visits.Select(visit => visit.SourceUser); // List of users who have liked the current user
            }

            var VisitedUsers = users.Select(user => new VisitDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });
            return await PagedList<VisitDto>.CreateAsync(VisitedUsers, visitsParams.PageNumber,
             visitsParams.PageSize);
        }

        public async Task<AppUser> GetUserWithVisits(int userId)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}