using Mzad_Palestine_Core.DTO_s.Dispute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IDisputeService
    {
        Task<DisputeDto> CreateAsync(CreateDisputeDto dto);
        Task<IEnumerable<DisputeDto>> GetAllAsync( );
    }
}
