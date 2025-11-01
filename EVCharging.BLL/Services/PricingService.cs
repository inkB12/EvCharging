using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class PricingService(IChargingSessionRepository chargingSessionRepo) : IPricingService
    {
        private readonly IChargingSessionRepository _chargingSessionRepo = chargingSessionRepo;

        public async Task<decimal> CalculateSessionFee(int sessionId)
        {
            decimal fee = 0;

            var session = await _chargingSessionRepo.GetByIdAsync(sessionId);

            if (session == null) return fee;

            // Base Price
            var chargingPointFee = session.Point.Price;
            var energyConsumed = session.EnergyConsumedKwh;
            fee = (decimal)(chargingPointFee * energyConsumed);

            // Service Plan Discount (if any)
            var servicePlan = session.Booking.User.ServicePlan;
            if (servicePlan != null)
            {
                fee -= (servicePlan.Price / 100) * fee;
                fee = fee < 0 ? 0 : fee;
            }

            return fee;
        }
    }
}
