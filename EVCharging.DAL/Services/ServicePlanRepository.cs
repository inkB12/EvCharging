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
    public class ServicePlanRepository : IServicePlanRepository
    {
        private readonly EvchargingContext _context;

        public ServicePlanRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<List<ServicePlan>> GetAllAsync()
        {
            return await _context.ServicePlans.ToListAsync();
        }

        public async Task<ServicePlan?> GetByIdAsync(int id)
        {
            // Bỏ điều kiện !s.IsDeleted để cho phép xem cả gói đã dừng hoạt động
            return await _context.ServicePlans.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> CreateAsync(ServicePlan entity)
        {
            _context.ServicePlans.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(ServicePlan entity)
        {
            _context.ServicePlans.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var plan = await _context.ServicePlans.FindAsync(id);
            if (plan != null)
            {
                plan.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
