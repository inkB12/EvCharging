using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.DTO
{
    public class ChargingPointDto
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public int? PowerLevelKw { get; set; }
        public string PortType { get; set; } = null!;
        public int? ChargingSpeedKw { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
    }
}
