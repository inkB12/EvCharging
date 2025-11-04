namespace EVCharging.BLL.Interfaces
{
    public interface IBookingCommandService
    {
        Task<(bool ok, string msg)> CancelAsync(int userId, int bookingId);
    }
}
