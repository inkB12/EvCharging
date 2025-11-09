using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.AdminDTOs
{
    public class TopStationDTO
    {
        public string StationName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }
}
