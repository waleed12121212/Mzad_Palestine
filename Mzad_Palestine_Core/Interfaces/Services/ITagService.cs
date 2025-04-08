using Mzad_Palestine_Core.DTO_s.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllAsync( );
    }
}
