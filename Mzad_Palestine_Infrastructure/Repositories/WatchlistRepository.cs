using Microsoft.EntityFrameworkCore;
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
    public class WatchlistRepository : GenericRepository<Watchlist>, IWatchlistRepository
    {
        public WatchlistRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Watchlist>> GetUserWatchlistAsync(int userId)
        {
            return await _context.Watchlists.Where(w => w.UserId == userId).ToListAsync();
        }

        public override void Update(Watchlist entity)
        {
            base.Update(entity);
        }

        public async Task<Watchlist> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Watchlists cannot be searched by name. Please use watchlist ID or user ID to search.");
        }
    }
}
