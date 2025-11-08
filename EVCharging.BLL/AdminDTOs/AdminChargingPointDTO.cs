using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.AdminDTOs
{
    public class AdminChargingPointDTO
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public string? PortType { get; set; }
        public string? Status { get; set; }
        public decimal PowerLevelKw { get; set; }
        public decimal ChargingSpeedKw { get; set; }
        public decimal Price { get; set; }
    }
}
