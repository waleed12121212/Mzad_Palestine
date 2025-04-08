using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class ListingTag
    {
        public int ListingId { get; set; }
        public int TagId { get; set; }

        // الملاحة
        public Listing Listing { get; set; }
        public Tag Tag { get; set; }
    }
}
