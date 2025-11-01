using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.DTO
{
    public class ReportDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalSessions { get; set; }
        public Dictionary<string, int>? UsageByStation { get; set; }
    }
}
