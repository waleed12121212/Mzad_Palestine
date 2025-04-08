using Mzad_Palestine_Core.DTO_s.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto?> GetByUserIdAsync(int userId);
    }
}
