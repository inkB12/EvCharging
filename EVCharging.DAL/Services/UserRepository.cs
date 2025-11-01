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

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _db.Users
                 .Where(u => !u.IsDeleted)
                 .ToListAsync();
        }

        public async Task<int> CreateAsync(User entity)
        {
            _db.Users.Add(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(User entity)
        {
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            return await _db.Users
                .Where(u => u.Role == role && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> CountUsersByRoleAsync(string role)
        {
            return await _db.Users
                 .CountAsync(u => u.Role == role && !u.IsDeleted);
        }
    }
}
