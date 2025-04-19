using Mzad_Palestine_Core.DTO_s.Subscription;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Subscription> CreateSubscriptionAsync(CreateSubscriptionDto dto)
        {
            var subscription = new Subscription
            {
                UserId = dto.UserId ,
                Plan = dto.Plan ,
                StartDate = DateTime.UtcNow ,
                EndDate = DateTime.UtcNow.AddMonths(dto.DurationInMonths) ,
                Status = "active"
            };

            await _unitOfWork.Subscriptions.AddAsync(subscription);
            await _unitOfWork.CompleteAsync();

            return subscription;
        }

        public async Task<Subscription> GetSubscriptionAsync(int id)
        {
            return await _unitOfWork.Subscriptions.GetByIdAsync(id);
        }

        public async Task<Subscription> GetUserActiveSubscriptionAsync(int userId)
        {
            var subscriptions = await _unitOfWork.Subscriptions.FindAsync(s =>
                s.UserId == userId && s.Status.ToLower() == "active" && s.EndDate > DateTime.UtcNow);
            return subscriptions.FirstOrDefault();
        }

        public async Task UpdateSubscriptionAsync(Subscription subscription)
        {
            _unitOfWork.Subscriptions.Update(subscription);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CancelSubscriptionAsync(int id)
        {
            var subscription = await GetSubscriptionAsync(id);
            if (subscription != null)
            {
                subscription.Status = "canceled";
                subscription.EndDate = DateTime.UtcNow;
                await UpdateSubscriptionAsync(subscription);
            }
        }

        public Task<SubscriptionDto?> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}