using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.AdminDTOs
{
    public class TopUserDTO
    {
        public string UserName { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
    }
}
