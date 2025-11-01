using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVCharging.DAL.Services
{
    public class TransactionRepository(EvchargingContext context) : ITransactionRepository
    {
        private readonly EvchargingContext _context = context;

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            var entity = await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<List<Transaction>> GetByBookingAsync(int bookingId)
        {
            return await _context.Transactions
                .Where(t => t.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Transaction?> UpdateAsync(Transaction transaction)
        {
            var entity = _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }
    }
}
