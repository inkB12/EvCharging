namespace EVCharging.BLL.Interfaces
{
    public interface IPricingService
    {
        Task<decimal> CalculateSessionFee(int sessionId);
    }
}
