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
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Tag>> SearchTagsAsync(string query)
        {
            return await _context.Tags.Where(t => t.Name.Contains(query)).ToListAsync();
        }

        public override void Update(Tag entity)
        {
            base.Update(entity);
        }

        public async Task<Tag> GetByNameAsync(string name)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        }
    }
}
