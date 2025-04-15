using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int UserId { get; set; }
        public int ListingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(7);
        public InvoiceStatus Status { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; }
        public Listing Listing { get; set; }
    }
}
