﻿using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Review>> GetReviewsForUserAsync(int userId)
        {
            return await _context.Reviews
                .Where(r => r.ReviewedUserId == userId)
                .ToListAsync();
        }

        public override void Update(Review entity)
        {
            base.Update(entity);
        }

        public async Task<Review> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Reviews cannot be searched by name. Please use review ID, user ID, or listing ID to search.");
        }
    }
}
