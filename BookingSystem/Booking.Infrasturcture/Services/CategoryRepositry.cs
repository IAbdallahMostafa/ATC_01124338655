using Booking.Core.Enities;
using Booking.Core.Intefaces;
using Booking.Infrasturcture.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Booking.Infrasturcture.Services
{
    public class CategoryRepositry : GenericRepositry<Category>, ICategoryRepositry
    {
        private BookingDbContext _context;
        private DbSet<Category> _dbSet;
        public CategoryRepositry(BookingDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Category>();
        }

        public async Task<bool> IsValid(int id)
        {
            return await _dbSet.AnyAsync(c => c.Id == id);
        }
    }
}
