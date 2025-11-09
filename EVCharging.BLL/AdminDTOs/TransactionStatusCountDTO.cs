using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.AdminDTOs
{
    public class TransactionStatusCountDTO
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
