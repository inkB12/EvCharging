using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.AdminDTOs
{
    public class AdminDashboardDTO
    {
        public decimal TotalRevenueThisMonth { get; set; }
        public decimal TotalRevenueThisYear { get; set; }

        public List<MonthlyRevenueDTO> MonthlyRevenues { get; set; } = new();
        public List<YearlyRevenueDTO> YearlyRevenues { get; set; } = new();
        public List<TransactionStatusCountDTO> TransactionStatusCounts { get; set; } = new();
        public List<TopStationDTO> TopStations { get; set; } = new();
        public List<TopUserDTO> TopUsers { get; set; } = new();
    }
}
