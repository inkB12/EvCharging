namespace EVCharging.BLL.Interfaces
{
    public interface IVNPayService
    {
        String CreateVNPayUrl(int orderId, decimal amount);
    }
}
