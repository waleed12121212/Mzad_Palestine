﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Review
{
    public class CreateReviewDto
    {
        public int ReviewerId { get; set; }
        public int ReviewedUserId { get; set; }
        public int? ListingId { get; set; }
        public int? AuctionId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
