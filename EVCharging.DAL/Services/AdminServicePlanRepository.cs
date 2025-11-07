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
            return await _context.ServicePlans.ToListAsync();
        }

        public async Task<ServicePlan?> GetByIdAsync(int id)
        {
            return await _context.ServicePlans.FindAsync(id);
        }

        public async Task AddAsync(ServicePlan plan)
        {
            _context.ServicePlans.Add(plan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ServicePlan plan)
        {
            _context.ServicePlans.Update(plan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var plan = await _context.ServicePlans.FindAsync(id);
            if (plan != null)
            {
                _context.ServicePlans.Remove(plan);
                await _context.SaveChangesAsync();
            }
        }
    }
}
