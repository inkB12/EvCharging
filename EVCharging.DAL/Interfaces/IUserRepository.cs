using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task SaveChangesAsync();
        Task<List<User>> GetAllUsersAsync();

        Task<int> CreateAsync(User entity);
        Task UpdateAsync(User entity);
        Task DeleteAsync(int id);

        // Quản lý theo vai trò
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<int> CountUsersByRoleAsync(string role);
    }
}
