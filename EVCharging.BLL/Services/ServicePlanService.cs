using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Services
{
    public class ServicePlanService : IServicePlanService
    {
        private readonly IServicePlanRepository _repo;

        public ServicePlanService(IServicePlanRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ServicePlanDto>> GetAllAsync()
        {
            var plans = await _repo.GetAllAsync();
            return plans.Select(p => new ServicePlanDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsDeleted = p.IsDeleted
            }).ToList();
        }

        public async Task<ServicePlanDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            return new ServicePlanDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsDeleted = p.IsDeleted // ✅ Thêm dòng này
            };

        }

        public async Task<int> CreateAsync(ServicePlanDto dto)
        {
            var entity = new ServicePlan
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsDeleted = false
            };
            return await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(ServicePlanDto dto)
        {
            var entity = new ServicePlan
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsDeleted = dto.IsDeleted
            };
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
