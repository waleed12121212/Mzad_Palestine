using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Enums;
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
    public class DisputeRepository : GenericRepository<Dispute>, IDisputeRepository
    {
        public DisputeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Dispute>> GetOpenDisputesAsync( )
        {
            return await _context.Disputes.Where(d => d.Status == DisputeStatus.Open).ToListAsync();
        }

        public override void Update(Dispute entity)
        {
            base.Update(entity);
        }

        public async Task<Dispute> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Disputes cannot be searched by name. Please use dispute ID, auction ID, or user ID to search.");
        }
    }
}
