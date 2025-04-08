using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        // الملاحة
        public ICollection<ListingTag> ListingTags { get; set; }
    }
}
