using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVCharging.DAL.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly EvchargingContext _db;
        public UserRepository(EvchargingContext db) => _db = db;

        public Task<User?> GetByIdAsync(int id)
            => _db.Users.FirstOrDefaultAsync(x => x.Id == id);

        public Task<User?> GetByEmailAsync(string email)
            => _db.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task AddAsync(User user) => await _db.Users.AddAsync(user);

        public Task SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
