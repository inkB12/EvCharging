using EVCharging.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Interfaces
{
    public interface IAdminServicePlanRepository
    {
        Task<List<ServicePlan>> GetAllAsync();
        Task<ServicePlan?> GetByIdAsync(int id);
        Task AddAsync(ServicePlan plan);
        Task UpdateAsync(ServicePlan plan);
        Task DeleteAsync(int id);
    }
}
