using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.AdminDTOs
{
    public class AdminUserDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public string? Vehicle { get; set; }
        public string? ServicePlanName { get; set; }
        public string? HomeStationName { get; set; }
    }
}
