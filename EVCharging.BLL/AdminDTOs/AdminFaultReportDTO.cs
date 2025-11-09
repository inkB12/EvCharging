using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.AdminDTOs
{
    public class AdminFaultReportDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; } // từ User.FullName
        public string? UserEmail { get; set; } // từ User.Email

        public int PointId { get; set; }
        public string? PointPortType { get; set; }
        public int? StationId { get; set; }
        public string? StationName { get; set; }

        public DateTime ReportTime { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public byte Severity { get; set; } // 1..5
        public string Status { get; set; } = "open"; // open | closed
    }
}
