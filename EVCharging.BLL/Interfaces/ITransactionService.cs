using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDTO?>? CreateAsync(TransactionDTO transactionDTO);
        Task<TransactionDTO?>? UpdateAsync(TransactionDTO transactionDTO);
        Task<List<TransactionDTO>> GetByBookingAsync(int bookingId);
    }
}
