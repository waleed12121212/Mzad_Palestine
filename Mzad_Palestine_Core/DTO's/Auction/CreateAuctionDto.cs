using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Auction
{
    public class CreateAuctionDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ReservePrice { get; set; }
        public decimal BidIncrement { get; set; }
        public int CategoryId { get; set; }
        public List<string> Images { get; set; }
        public int UserId { get; set; }
    }
}
