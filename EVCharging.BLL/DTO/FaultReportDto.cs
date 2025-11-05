using System.ComponentModel.DataAnnotations;

namespace EVCharging.BLL.DTO
{
    public class FaultReportDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn điểm sạc")]
        public int PointId { get; set; } // Khớp với entity
        public string? PointName { get; set; } // Tên của điểm sạc (từ navigation)

        public int UserId { get; set; } // Khớp với entity (kiểu int)
        public string? ReportedByUserName { get; set; } // Tên của staff (từ navigation)

        public DateTime ReportTime { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [StringLength(255)]
        public string Title { get; set; } = null!; // Khớp với entity

        public string? Description { get; set; } // Khớp với entity (nullable)

        [Range(1, 5, ErrorMessage = "Mức độ nghiêm trọng phải từ 1 đến 5")]
        public byte Severity { get; set; } // Khớp với entity

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public string Status { get; set; } = null!; // Ví dụ: "Pending", "Resolved"
    }
}
