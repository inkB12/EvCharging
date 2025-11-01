namespace EVCharging.DAL.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public DateTime Datetime { get; set; }

    public decimal Total { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Booking Booking { get; set; } = null!;
}
