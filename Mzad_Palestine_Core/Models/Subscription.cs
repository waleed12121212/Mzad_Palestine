using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public string Plan { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddMonths(1);
        public DateTime RenewalDate { get; set; } = DateTime.UtcNow.AddMonths(1);
        public string Status { get; set; } // مثال: "active", "canceled", "expired"

        // الملاحة
        public User User { get; set; }
    }

}
