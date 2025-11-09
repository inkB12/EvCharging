using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.BLL.Services;
using EVCharging.DAL;
using EVCharging.DAL.Interfaces;
using EVCharging.DAL.Services;
using Microsoft.EntityFrameworkCore;

namespace EVCharging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add context
            builder.Services.AddDbContext<EvchargingContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Tiêm DI
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IChargingStationRepository, ChargingStationRepository>();
            builder.Services.AddScoped<IChargingPointRepository, ChargingPointRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IServicePlanRepository, ServicePlanRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IChargingStationService, ChargingStationService>();
            builder.Services.AddScoped<IChargingPointService, ChargingPointService>();
            builder.Services.AddScoped<IAdminUserService, AdminUserService>();
            builder.Services.AddScoped<IServicePlanService, ServicePlanService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IPricingService, PricingService>();
            builder.Services.AddScoped<IChargingPointRepository, ChargingPointRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IChargingSessionRepository, ChargingSessionRepository>();

            builder.Services.AddScoped<IFaultReportRepository, FaultReportRepository>();
            builder.Services.AddScoped<IFaultReportService, FaultReportService>();

            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IMapSchedulingService, MapSchedulingService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingQueryService, BookingQueryService>();
            builder.Services.AddScoped<IBookingQueryService, BookingQueryService>();
            builder.Services.AddScoped<IBookingCommandService, BookingCommandService>();
            builder.Services.AddScoped<IChargingSessionService, ChargingSessionService>();
            builder.Services.AddScoped<IStaffBookingQueryService, StaffBookingQueryService>();
            builder.Services.AddScoped<IChargeRuntimeService, ChargeRuntimeService>();
            builder.Services.AddScoped<IMomoService, MomoService>();
            builder.Services.AddScoped<IVNPayService, VNPayService>();


            // Admin Repo
            builder.Services.AddScoped<IAdminChargingStationRepository, AdminChargingStationRepository>();
            builder.Services.AddScoped<IAdminChargingPointRepository, AdminChargingPointRepository>();
            builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
            builder.Services.AddScoped<IAdminServicePlanRepository, AdminServicePlanRepository>();
            builder.Services.AddScoped<IAdminFaultReportRepository, AdminFaultReportRepository>();
            builder.Services.AddScoped<IAdminChargingSessionRepository, AdminChargingSessionRepository>();
            builder.Services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();


            // Admin Services
            builder.Services.AddScoped<IAdminChargingStationService, AdminChargingStationService>();
            builder.Services.AddScoped<IAdminChargingPointService, AdminChargingPointService>();
            builder.Services.AddScoped<IAdminUserService, AdminUserService>();
            builder.Services.AddScoped<IAdminServicePlanService, AdminServicePlanService>();
            builder.Services.AddScoped<IAdminFaultReportService, AdminFaultReportService>();
            builder.Services.AddScoped<IAdminChargingSessionService, AdminChargingSessionService>();
            builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();

            // Kết nối Momo 
            builder.Services.Configure<MomoOptionDTO>(builder.Configuration.GetSection("MomoApi"));

            // Lấy thông tin VNPAY
            builder.Services.Configure<VNPayDTO>(builder.Configuration.GetSection("VnPaySettings"));

            // Session
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromDays(7);
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;
            });

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.Run();
        }
    }
}
