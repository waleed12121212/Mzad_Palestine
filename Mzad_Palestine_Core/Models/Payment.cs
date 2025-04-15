using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public PaymentMethod Method { get; set; }
        public PaymentEscrowStatus EscrowStatus { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        // الملاحة
        public User User { get; set; }
        public Auction Auction { get; set; }
    }
}
