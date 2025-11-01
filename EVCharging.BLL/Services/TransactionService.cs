using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class TransactionService(ITransactionRepository transactionRepo, IPricingService pricingService) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo = transactionRepo;
        private readonly IPricingService _pricingSerivice = pricingService;

        public async Task<TransactionDTO?> CreateAsync(TransactionDTO transactionDTO)
        {
            var transaction = new Transaction()
            {
                Datetime = DateTime.UtcNow,
                PaymentMethod = transactionDTO.PaymentMethod,
                Status = "ON_GOING",
                BookingId = transactionDTO.BookingId,
                Total = await _pricingSerivice.CalculateSessionFee(transactionDTO.SessionId)
            };

            var entity = await _transactionRepo.CreateAsync(transaction);

            return MapToDTO(entity);
        }

        public async Task<List<TransactionDTO>> GetByBookingAsync(int bookingId)
        {
            var list = await _transactionRepo.GetByBookingAsync(bookingId);
            return [.. list.Select(MapToDTO)];
        }

        public async Task<TransactionDTO?>? UpdateAsync(TransactionDTO transactionDTO)
        {
            var entity = await _transactionRepo.GetByIdAsync(transactionDTO.Id);
            if (entity == null)
            {
                return null;
            }

            entity.Status = transactionDTO.Status;
            await _transactionRepo.UpdateAsync(entity);

            return MapToDTO(entity);
        }

        private TransactionDTO? MapToDTO(Transaction entity)
        {
            if (entity == null) return null;

            return new TransactionDTO()
            {
                BookingId = entity.BookingId,
                Datetime = entity.Datetime,
                PaymentMethod = entity.PaymentMethod,
                Status = entity.Status,
                Id = entity.Id,
                Total = entity.Total,
            };
        }
    }
}
