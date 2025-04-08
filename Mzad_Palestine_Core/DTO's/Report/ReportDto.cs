using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Report
{
    public class ReportDto
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int ReportedListingId { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public int? ResolvedBy { get; set; }
    }
}
