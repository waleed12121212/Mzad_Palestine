using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Enums;

namespace Mzad_Palestine_Core.DTO_s.Payment
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public string? TransactionId { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
