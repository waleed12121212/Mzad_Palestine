﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Payment
{
    public class CreatePaymentDto
    {
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
    }
}
