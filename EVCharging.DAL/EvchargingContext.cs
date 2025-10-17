using System;
using System.Collections.Generic;
using EVCharging.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EVCharging.DAL;

public partial class EvchargingContext : DbContext
{
    public EvchargingContext()
    {
    }

    public EvchargingContext(DbContextOptions<EvchargingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<ChargingPoint> ChargingPoints { get; set; }

    public virtual DbSet<ChargingSession> ChargingSessions { get; set; }

    public virtual DbSet<ChargingStation> ChargingStations { get; set; }

    public virtual DbSet<FaultReport> FaultReports { get; set; }

    public virtual DbSet<ServicePlan> ServicePlans { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=(local); Database=EVCharging; Uid=sa; Pwd=1234567890; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC074D59CDB9");

            entity.ToTable("Booking");

            entity.HasIndex(e => e.UserId, "IX_Booking_User");

            entity.Property(e => e.BookingTime)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.EndTime).HasPrecision(0);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartTime).HasPrecision(0);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("ongoing");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_User");
        });

        modelBuilder.Entity<ChargingPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Charging__3214EC07BB79306F");

            entity.ToTable("ChargingPoint");

            entity.HasIndex(e => e.StationId, "IX_Point_Station");

            entity.Property(e => e.ChargingSpeedKw).HasColumnName("ChargingSpeedKW");
            entity.Property(e => e.PortType)
                .HasMaxLength(20)
                .HasDefaultValue("AC");
            entity.Property(e => e.PowerLevelKw).HasColumnName("PowerLevelKW");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("online");

            entity.HasOne(d => d.Station).WithMany(p => p.ChargingPoints)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Point_Station");
        });

        modelBuilder.Entity<ChargingSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Charging__3214EC07095995C9");

            entity.ToTable("ChargingSession");

            entity.HasIndex(e => e.BookingId, "IX_Session_Booking");

            entity.HasIndex(e => e.PointId, "IX_Session_Point");

            entity.HasIndex(e => e.StartTime, "IX_Session_Start");

            entity.Property(e => e.EndTime).HasPrecision(0);
            entity.Property(e => e.EnergyConsumedKwh)
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("EnergyConsumedKWh");
            entity.Property(e => e.StartTime).HasPrecision(0);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("coming-soon");

            entity.HasOne(d => d.Booking).WithMany(p => p.ChargingSessions)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Booking");

            entity.HasOne(d => d.Point).WithMany(p => p.ChargingSessions)
                .HasForeignKey(d => d.PointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Point");
        });

        modelBuilder.Entity<ChargingStation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Charging__3214EC0704BC8D5F");

            entity.ToTable("ChargingStation");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Station).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("empty");
        });

        modelBuilder.Entity<FaultReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FaultRep__3214EC07AEF36634");

            entity.ToTable("FaultReport");

            entity.HasIndex(e => e.PointId, "IX_Fault_Point");

            entity.HasIndex(e => e.UserId, "IX_Fault_User");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ReportTime)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("open");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Point).WithMany(p => p.FaultReports)
                .HasForeignKey(d => d.PointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fault_Point");

            entity.HasOne(d => d.User).WithMany(p => p.FaultReports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fault_User");
        });

        modelBuilder.Entity<ServicePlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceP__3214EC07233686A0");

            entity.ToTable("ServicePlan");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(120);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07C80BB0F2");

            entity.ToTable("Transaction");

            entity.HasIndex(e => e.BookingId, "IX_Transaction_Booking");

            entity.Property(e => e.Datetime)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("ongoing");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Booking).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Booking");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07550FC740");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UX_User_Email").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Vehicle).HasMaxLength(150);

            entity.HasOne(d => d.HomeStation).WithMany(p => p.Users)
                .HasForeignKey(d => d.HomeStationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_User_HomeStation");

            entity.HasOne(d => d.ServicePlan).WithMany(p => p.Users)
                .HasForeignKey(d => d.ServicePlanId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_User_ServicePlan");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
