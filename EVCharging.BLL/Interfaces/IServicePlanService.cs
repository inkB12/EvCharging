using EVCharging.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IServicePlanService
    {
        Task<List<ServicePlanDto>> GetAllAsync();
        Task<ServicePlanDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(ServicePlanDto dto);
        Task UpdateAsync(ServicePlanDto dto);
        Task DeleteAsync(int id);
    }
}
