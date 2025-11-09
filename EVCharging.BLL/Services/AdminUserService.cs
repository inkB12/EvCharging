using EVCharging.BLL.AdminDTOs;
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
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _repository;

        public AdminUserService(IAdminUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AdminUserDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(MapToDTO).ToList();
        }

        public async Task<AdminUserDTO?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToDTO(entity);
        }

        public async Task AddAsync(AdminUserDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");

            var entity = MapToEntity(dto);
            entity.Password = BCrypt.Net.BCrypt.HashPassword(dto.Email); // tạm gán hash từ email hoặc random default
            entity.Role ??= "driver";
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(AdminUserDTO dto)
        {
            await _repository.UpdateAsync(MapToEntity(dto));
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<AdminUserDTO>> GetByRoleAsync(string role)
        {
            var list = await _repository.GetUsersByRoleAsync(role);
            return list.Select(MapToDTO).ToList();
        }

        public async Task<bool> UpdateUserRoleAsync(int id, string newRole)
        {
            return await _repository.UpdateUserRoleAsync(id, newRole);
        }

        private AdminUserDTO MapToDTO(User e)
        {
            return new AdminUserDTO
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                Phone = e.Phone,
                Role = e.Role,
                Vehicle = e.Vehicle,
                ServicePlanId = e.ServicePlanId,
                HomeStationId = e.HomeStationId,
                ServicePlanName = e.ServicePlan?.Name,
                HomeStationName = e.HomeStation?.Name
            };
        }

        private User MapToEntity(AdminUserDTO d)
        {
            return new User
            {
                Id = d.Id,
                FullName = d.FullName ?? "",
                Email = d.Email ?? "",
                Phone = d.Phone ?? "",
                Role = d.Role ?? "driver",
                Vehicle = d.Vehicle ?? "",
                // giữ lại nếu DTO có
                ServicePlanId = d.ServicePlanId,
                HomeStationId = d.HomeStationId,
                IsDeleted = false
            };
        }
    }
}
