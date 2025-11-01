using EVCharging.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> GetSystemReportAsync(DateTime? start = null, DateTime? end = null);
    }
}
