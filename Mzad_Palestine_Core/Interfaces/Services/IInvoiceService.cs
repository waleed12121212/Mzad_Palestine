using Mzad_Palestine_Core.DTO_s.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceDto?> GetByIdAsync(int id);
        Task<IEnumerable<InvoiceDto>> GetByUserIdAsync(int userId);
    }
}
