using EVCharging.BLL.AdminDTOs;
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
    public class AdminServicePlanService : IAdminServicePlanService
    {

        private readonly IAdminServicePlanRepository _repository;

        public AdminServicePlanService(IAdminServicePlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AdminServicePlanDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(MapToDTO).ToList();
        }

        public async Task<AdminServicePlanDTO?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToDTO(entity);
        }

        public async Task AddAsync(AdminServicePlanDTO dto)
        {
            await _repository.AddAsync(MapToEntity(dto));
        }

        public async Task UpdateAsync(AdminServicePlanDTO dto)
        {
            await _repository.UpdateAsync(MapToEntity(dto));
        }

        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

        private AdminServicePlanDTO MapToDTO(ServicePlan e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Price = e.Price
        };

        private ServicePlan MapToEntity(AdminServicePlanDTO d) => new()
        {
            Id = d.Id,
            Name = d.Name ?? "",
            Description = d.Description ?? "",
            Price = d.Price
        };
    }
}
