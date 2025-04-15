using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface IAuctionRepository : IGenericRepository<Auction>
    {
        Task<Auction> GetAuctionWithBidsAsync(int auctionId);                  // جلب مزاد مع عروضه
        Task<IEnumerable<Auction>> GetOpenAuctionsAsync();                     // المزادات المفتوحة
        Task<IEnumerable<Auction>> GetClosedAuctionsAsync();                   // المزادات المغلقة
        Task<IEnumerable<Auction>> GetActiveAsync();                           // المزادات الفعالة حالياً
        Task<IEnumerable<Auction>> GetByUserIdAsync(int userId);               // مزادات مستخدم معين
        Task<Auction> GetByIdAsync(int auctionId);                             // جلب مزاد بالتفصيل
        Task CloseAuctionAsync(int auctionId);                                 // إغلاق مزاد
        Task UpdateAsync(Auction auction);                                     // تحديث مزاد
        Task DeleteAsync(int id);                                              // حذف مزاد
        Task<bool> ExistsAsync(int auctionId);                                 // هل المزاد موجود؟
        Task<bool> IsAuctionOwnerAsync(int auctionId, int userId);            // هل المستخدم صاحب المزاد؟
        Task<DateTime?> GetEndTimeAsync(int auctionId);                        // وقت انتهاء المزاد
        Task<IEnumerable<Auction>> SearchAsync(AuctionSearchDto searchDto);    // بحث متقدم
    }
}
