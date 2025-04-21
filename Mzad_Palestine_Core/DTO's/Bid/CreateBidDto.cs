using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTO_s.Bid
{
    public class CreateBidDto
    {
        [Required(ErrorMessage = "معرف المزاد مطلوب")]
        public int AuctionId { get; set; }

        [Required(ErrorMessage = "مبلغ العرض مطلوب")]
        [Range(0.01, double.MaxValue, ErrorMessage = "يجب أن يكون مبلغ العرض أكبر من صفر")]
        public decimal BidAmount { get; set; }
    }
}
