using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.AdminDTOs
{
    public class AdminChargingStationDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }

        public List<AdminChargingPointDTO>? ChargingPoints { get; set; }

        // 🆕 Thêm trường này để hiển thị số điểm sạc (đã tính trước)
        public int PointCount { get; set; }
    }
}
