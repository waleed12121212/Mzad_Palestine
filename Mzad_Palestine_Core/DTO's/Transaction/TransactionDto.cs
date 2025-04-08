using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Transaction
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
