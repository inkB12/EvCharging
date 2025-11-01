using EVCharging.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Interfaces
{
    public interface IServicePlanRepository
    {
        Task<List<ServicePlan>> GetAllAsync();
        Task<ServicePlan?> GetByIdAsync(int id);
        Task<int> CreateAsync(ServicePlan entity);
        Task UpdateAsync(ServicePlan entity);
        Task DeleteAsync(int id);
    }
}
