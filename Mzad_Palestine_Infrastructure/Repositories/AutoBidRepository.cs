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
    public class AutoBidRepository : GenericRepository<AutoBid>, IAutoBidRepository
    {
        public AutoBidRepository(ApplicationDbContext context) : base(context) { }

        public async Task<AutoBid?> GetUserAutoBidAsync(int userId , int auctionId)
        {
            return await _context.AutoBids.FirstOrDefaultAsync(ab => ab.UserId == userId && ab.AuctionId == auctionId);
        }

        public override void Update(AutoBid entity)
        {
            base.Update(entity);
        }

        public async Task<AutoBid> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Auto bids cannot be searched by name. Please use auction ID or user ID to search.");
        }
    }
}
