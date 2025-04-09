namespace Mzad_Palestine_Core.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public virtual User User { get; set; }
    }
}