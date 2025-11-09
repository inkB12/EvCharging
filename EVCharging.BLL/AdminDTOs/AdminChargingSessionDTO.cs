using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.AdminDTOs
{
    public class AdminChargingSessionDTO
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public int PointId { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public decimal? EnergyConsumedKwh { get; set; }

        public string Status { get; set; } = "coming-soon";

        // ===== Thông tin mở rộng cho dashboard =====
        public string? PointPortType { get; set; }
        public string? StationName { get; set; }
        public string? UserEmail { get; set; }
    }
}
