using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<Transaction?> UpdateAsync(Transaction transaction);
        Task<Transaction?> GetByIdAsync(int id);
        Task<List<Transaction>> GetByBookingAsync(int bookingId);
        Task<List<Transaction>> GetByUserAndYearAsync(int userId, int year);
    }
}
