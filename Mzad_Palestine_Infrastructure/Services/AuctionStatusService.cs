using System;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class AuctionStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuctionStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateAuctionStatusesAsync()
        {
            var currentTime = DateTime.UtcNow;
            
            // تحديث المزادات التي حان وقت بدايتها
            var pendingAuctions = await _unitOfWork.Auctions.FindAsync(a => 
                a.Status == "Pending" && 
                a.StartDate <= currentTime);

            foreach (var auction in pendingAuctions)
            {
                auction.Status = "Open";
                _unitOfWork.Auctions.Update(auction);
            }

            // تحديث المزادات التي انتهت
            var openAuctions = await _unitOfWork.Auctions.FindAsync(a => 
                a.Status == "Open" && 
                a.EndDate <= currentTime);

            foreach (var auction in openAuctions)
            {
                auction.Status = "Closed";
                _unitOfWork.Auctions.Update(auction);
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}