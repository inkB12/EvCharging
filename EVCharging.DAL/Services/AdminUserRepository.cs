using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Services
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly EvchargingContext _context;

        public AdminUserRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.ServicePlan)
                .Include(u => u.HomeStation)
                .Where(u => !u.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.ServicePlan)
                .Include(u => u.HomeStation)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task AddAsync(User user)
        {
            user.IsDeleted = false;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id && !u.IsDeleted);
            if (existing == null) return;

            existing.FullName = user.FullName;
            existing.Phone = user.Phone;
            existing.Vehicle = user.Vehicle;
            existing.Role = user.Role;
            existing.ServicePlanId = user.ServicePlanId;
            existing.HomeStationId = user.HomeStationId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true; // xóa mềm
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            return await _context.Users
                .Where(u => u.Role == role && !u.IsDeleted)
                .Include(u => u.ServicePlan)
                .Include(u => u.HomeStation)
                .ToListAsync();
        }

        public async Task<bool> UpdateUserRoleAsync(int id, string newRole)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (user == null) return false;

            user.Role = newRole;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
