using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminChargingPointPage
{
    public class IndexModel : PageModel
    {
        private readonly IAdminChargingPointService _pointService;
        private readonly IAdminChargingStationService _stationService;

        public List<PointViewModel> Points { get; set; } = new();

        public IndexModel(IAdminChargingPointService pointService, IAdminChargingStationService stationService)
        {
            _pointService = pointService;
            _stationService = stationService;
        }

        public async Task OnGetAsync()
        {
            var points = await _pointService.GetAllAsync();
            var stations = await _stationService.GetAllAsync();

            Points = points.Select(p => new PointViewModel
            {
                Id = p.Id,
                StationId = p.StationId,
                StationName = stations.FirstOrDefault(s => s.Id == p.StationId)?.Name ?? "(Không xác định)",
                PortType = p.PortType,
                PowerLevelKw = p.PowerLevelKw,
                ChargingSpeedKw = p.ChargingSpeedKw,
                Price = p.Price,
                Status = p.Status
            }).ToList();
        }

        public class PointViewModel : AdminChargingPointDTO
        {
            public string? StationName { get; set; }
        }
    }
}
