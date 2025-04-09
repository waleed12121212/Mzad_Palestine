using System;

namespace Mzad_Palestine_Core.DTO_s.Subscription
{
    public class CreateSubscriptionDto
    {
        public int UserId { get; set; }
        public string Plan { get; set; }
        public int DurationInMonths { get; set; }
    }
}