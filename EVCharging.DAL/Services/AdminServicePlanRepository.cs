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
    public class AdminServicePlanRepository : IAdminServicePlanRepository
    {
        private readonly EvchargingContext _context;

        public AdminServicePlanRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<List<ServicePlan>> GetAllAsync()
        {
            // Chỉ lấy plan chưa bị xóa mềm
            return await _context.ServicePlans
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<ServicePlan?> GetByIdAsync(int id)
        {
            // Không dùng FindAsync vì nó không filter theo IsDeleted
            return await _context.ServicePlans
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task AddAsync(ServicePlan plan)
        {
            // Đảm bảo luôn là chưa xóa
            plan.IsDeleted = false;
            plan.Name = plan.Name?.Trim() ?? string.Empty;
            plan.Description = plan.Description?.Trim();

            _context.ServicePlans.Add(plan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ServicePlan plan)
        {
            // Load entity đang tồn tại để không đụng vào IsDeleted
            var existing = await _context.ServicePlans
                .FirstOrDefaultAsync(p => p.Id == plan.Id && !p.IsDeleted);

            if (existing == null)
            {
                return; // hoặc throw exception tùy convention của bạn
            }

            existing.Name = plan.Name?.Trim() ?? string.Empty;
            existing.Description = plan.Description?.Trim();
            existing.Price = plan.Price;

            // IsDeleted giữ nguyên
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var plan = await _context.ServicePlans
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (plan != null)
            {
                // ❗ XÓA MỀM: chỉ đánh dấu IsDeleted = true
                plan.IsDeleted = true;
                _context.ServicePlans.Update(plan);
                await _context.SaveChangesAsync();
            }
        }
    }
}
