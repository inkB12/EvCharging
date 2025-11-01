namespace EVCharging.BLL.DTO
{
    public class TransactionDTO
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public int SessionId { get; set; }

        public DateTime Datetime { get; set; }

        public decimal Total { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
