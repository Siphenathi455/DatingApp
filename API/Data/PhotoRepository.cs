using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;

        public PhotoRepository(DataContext context)
        {
            _context = context;
        }

        public Photo GetPhotoById(int id)
        {
           return   _context.Photos.Find(id);
               
        }

        public async Task<IEnumerable<Photo>> GetUnapprovedPhotos()
        {
            return await _context.Photos
                .Where(p => p.isApproved == false).IgnoreQueryFilters()
                .ToListAsync();
        }

        public void RemovePhoto(int id)
        {
            Photo photo = _context.Photos.Find(id);

            if(photo != null)
            _context.Photos.Remove(photo);
        }

        public AppUser GetUserByPhotoId( int id)
        {
            Photo photo = GetPhotoById(id);
            IQueryable<AppUser> lst = _context.Users.Where(u => u.Photos.FirstOrDefault(p => p.Id == id) == photo);
            return lst.FirstOrDefault();
        }
    }
}
