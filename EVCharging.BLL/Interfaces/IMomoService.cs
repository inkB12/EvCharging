using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IMomoService
    {
        Task<MomoResponseDTO> CreatePaymentAsync(int orderId, decimal orderTotal);
    }
}
