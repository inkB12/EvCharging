using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class FaultReportService : IFaultReportService
    {
        private readonly IFaultReportRepository _faultReportRepo;
        public FaultReportService(IFaultReportRepository faultReportRepo)
        {
            _faultReportRepo = faultReportRepo;
        }

        public async Task CreateFaultReportAsync(FaultReportDto faultReportDto)
        {
            var faultReport = new FaultReport
            {
                PointId = faultReportDto.PointId,
                UserId = faultReportDto.UserId, // Sẽ gán cứng ở PageModel
                ReportTime = faultReportDto.ReportTime, // Sẽ gán ở PageModel
                Title = faultReportDto.Title,
                Description = faultReportDto.Description,
                Severity = faultReportDto.Severity,
                Status = faultReportDto.Status // Sẽ gán "Pending" ở PageModel
            };
            await _faultReportRepo.CreateFaultReportAsync(faultReport);
        }

        public async Task DeleteFaultReportAsync(int id)
        {
            await _faultReportRepo.DeleteFaultReportAsync(id);
        }

        public async Task<IEnumerable<FaultReportDto>> GetAllFaultReportAsync()
        {
            var report = await _faultReportRepo.GetAllFaultReportAsync();
            return report.Select(fr => new FaultReportDto
            {
                Id = fr.Id,
                PointId = fr.PointId,
                PointName = fr.Point.PortType + "-" + fr.Point.Id,
                UserId = fr.UserId,
                ReportedByUserName = fr.User.FullName,
                ReportTime = fr.ReportTime,
                Title = fr.Title,
                Description = fr.Description,
                Severity = fr.Severity,
                Status = fr.Status,

            });
        }

        public async Task<FaultReportDto?> GetFaultReportByIdAsync(int id)
        {
            var fr = await _faultReportRepo.GetByIdFaultReportAsync(id);
            if (fr == null) return null;

            return new FaultReportDto
            {
                Id = fr.Id,
                PointId = fr.PointId,
                UserId = fr.UserId,
                ReportTime = fr.ReportTime,
                Title = fr.Title,
                Description = fr.Description,
                Severity = fr.Severity,
                Status = fr.Status
            };
        }

        public async Task UpdateFaultReportAsync(FaultReportDto faultReportDto)
        {
            var faultReport = await _faultReportRepo.GetByIdFaultReportAsync(faultReportDto.Id);
            if (faultReport != null)
            {
                // Cập nhật các trường Staff có thể sửa
                faultReport.PointId = faultReportDto.PointId;
                faultReport.Title = faultReportDto.Title;
                faultReport.Description = faultReportDto.Description;
                faultReport.Severity = faultReportDto.Severity;
                faultReport.Status = faultReportDto.Status;

                await _faultReportRepo.UpdateFaultReportAsync(faultReport);
            }
        }
    }
}
