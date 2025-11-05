namespace EVCharging.BLL.Interfaces
{
    public interface IChargeRuntimeService
    {
        Task<(bool ok, string msg, ChargeSessionRuntimeDTO? dto)> GetSessionAsync(int sessionId);

        Task<(bool ok, string msg)> StartAsync(int sessionId, DateTime startUtc);
        Task<(bool ok, string msg, decimal energyKwh)> StopAsync(int sessionId, DateTime endUtc);
    }
}
