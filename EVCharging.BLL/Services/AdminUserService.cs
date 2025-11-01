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
        private readonly IUserRepository _repo;

        public AdminUserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AdminUserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllUsersAsync();
            return users.Select(u => new AdminUserDto
            {
                Id = u.Id,
                Email = u.Email,
                Phone = u.Phone,
                FullName = u.FullName,
                Role = u.Role,
                Vehicle = u.Vehicle,
                IsDeleted = u.IsDeleted,
                ServicePlanId = u.ServicePlanId,
                HomeStationId = u.HomeStationId
            }).ToList();
        }

        public async Task<AdminUserDto?> GetByIdAsync(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;
            return new AdminUserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role
            };
        }

        public async Task<int> CreateAsync(AdminUserDto dto)
        {
            var entity = new User
            {
                Email = dto.Email,
                Phone = dto.Phone,
                FullName = dto.FullName,
                Role = dto.Role,
                Vehicle = dto.Vehicle,
                IsDeleted = false,
                ServicePlanId = dto.ServicePlanId,
                HomeStationId = dto.HomeStationId
            };
            return await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(AdminUserDto dto)
        {
            var entity = new User
            {
                Id = dto.Id,
                Email = dto.Email,
                Phone = dto.Phone,
                FullName = dto.FullName,
                Role = dto.Role,
                Vehicle = dto.Vehicle,
                ServicePlanId = dto.ServicePlanId,
                HomeStationId = dto.HomeStationId,
                IsDeleted = dto.IsDeleted
            };
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<List<AdminUserDto>> GetByRoleAsync(string role)
        {
            var users = await _repo.GetUsersByRoleAsync(role);
            return users.Select(u => new AdminUserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role
            }).ToList();
        }

        public async Task<int> CountByRoleAsync(string role)
        {
            return await _repo.CountUsersByRoleAsync(role);
        }
    }
}
