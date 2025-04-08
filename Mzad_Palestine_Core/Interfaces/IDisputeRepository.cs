using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface IDisputeRepository : IGenericRepository<Dispute>
    {
        Task<IEnumerable<Dispute>> GetOpenDisputesAsync( );
    }
}
