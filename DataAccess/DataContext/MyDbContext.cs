using System;
using System.Collections.Generic;
using DataAccess.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataContext;

public partial class MyDbContext : DbContext
{
    

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DispatchAssignment> DispatchAssignments { get; set; }

    public virtual DbSet<Dispatcher> Dispatchers { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Function> Functions { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<RealTimeTracking> RealTimeTrackings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<ShippingRequest> ShippingRequests { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<Truck> Trucks { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    public virtual DbSet<UserFunction> UserFunctions { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=Mydb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B800BC461F");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Customers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__UserI__4D94879B");
        });

        modelBuilder.Entity<DispatchAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__Dispatch__32499E573A817140");

            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.Deliverydate).HasColumnType("datetime");
            entity.Property(e => e.DropoffLat).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.DropoffLng).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.DropoffLocation).HasMaxLength(255);
            entity.Property(e => e.FuelType).HasMaxLength(20);
            entity.Property(e => e.Note).HasMaxLength(225);
            entity.Property(e => e.ParkingLat).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.ParkingLng).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.PickupLat).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.PickupLng).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.PickupLocation).HasMaxLength(255);
            entity.Property(e => e.Pickupdate).HasColumnType("datetime");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.DispatchAssignments)
                .HasForeignKey(d => d.AssignedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DispatchAssignments_Dispatchers");

            entity.HasOne(d => d.Request).WithMany(p => p.DispatchAssignments)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DispatchAssignments_ShippingRequests");

            entity.HasOne(d => d.Trip).WithMany(p => p.DispatchAssignments)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("FK_DispatchAssignments_Trips");
        });

        modelBuilder.Entity<Dispatcher>(entity =>
        {
            entity.HasKey(e => e.DispatcherId).HasName("PK__Dispatch__EB9ED164EB1A2DFB");

            entity.Property(e => e.DispatcherId).HasColumnName("DispatcherID");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Dispatchers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dispatche__UserI__5441852A");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.DriverId).HasName("PK__Drivers__F1B1CD24C75CABBF");

            entity.HasIndex(e => e.Idcard, "UQ__Drivers__43A2A4E345F311AB").IsUnique();

            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.HealthStatus).HasMaxLength(255);
            entity.Property(e => e.Idcard)
                .HasMaxLength(50)
                .HasColumnName("IDCard");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Drivers__UserID__5165187F");
        });

        modelBuilder.Entity<Function>(entity =>
        {
            entity.ToTable("Function");

            entity.Property(e => e.FunctionCode).HasMaxLength(200);
            entity.Property(e => e.FunctionName).HasMaxLength(200);
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("ProductType");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<RealTimeTracking>(entity =>
        {
            entity.HasKey(e => e.TrackingId).HasName("PK__RealTime__3C19EDD16DFB6755");

            entity.ToTable("RealTimeTracking");

            entity.Property(e => e.TrackingId).HasColumnName("TrackingID");
            entity.Property(e => e.CurrentLat).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.CurrentLng).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.TruckId).HasColumnName("TruckID");

            entity.HasOne(d => d.Truck).WithMany(p => p.RealTimeTrackings)
                .HasForeignKey(d => d.TruckId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RealTimeT__Truck__619B8048");
        });

     

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.EndTime).HasPrecision(0);
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.StartTime).HasPrecision(0);
        });

        modelBuilder.Entity<ShippingRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Shipping__33A8519AA7E85CED");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Deliverydate).HasColumnType("datetime");
            entity.Property(e => e.DropoffLat).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.DropoffLng).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.DropoffLocation).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(225);
            entity.Property(e => e.PickupLat).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.PickupLng).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.PickupLocation).HasMaxLength(255);
            entity.Property(e => e.Pickupdate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.ShippingRequests)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShippingR__Custo__59FA5E80");

            entity.HasOne(d => d.ProductType).WithMany(p => p.ShippingRequests)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("FK_ShippingRequests_ProductType");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.Property(e => e.EndTime).HasPrecision(0);
            entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            entity.Property(e => e.StartTime).HasPrecision(0);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TruckId).HasColumnName("TruckID");

            entity.HasOne(d => d.Shift).WithMany(p => p.Trips)
                .HasForeignKey(d => d.ShiftId)
                .HasConstraintName("FK_Trips_Shifts");

            entity.HasOne(d => d.Truck).WithMany(p => p.Trips)
                .HasForeignKey(d => d.TruckId)
                .HasConstraintName("FK_Trips_Trucks");
        });

        modelBuilder.Entity<Truck>(entity =>
        {
            entity.HasKey(e => e.TruckId).HasName("PK__Trucks__6632E95B7A4565FD");

            entity.Property(e => e.TruckId).HasColumnName("TruckID");
            entity.Property(e => e.ConsumptionRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.FuelType).HasMaxLength(20);
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ParkingLat).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.ParkingLng).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.ParkingLocation).HasMaxLength(255);

            entity.HasOne(d => d.Driver).WithMany(p => p.Trucks)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK_Trucks_Drivers");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC8353AE3B");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E42FBDB1A9").IsUnique();

            entity.Property(e => e.UserId)
                  .HasColumnName("UserID")
                  .IsRequired(); // Không cho phép null

            entity.Property(e => e.Username)
                  .HasMaxLength(50)
                  .IsRequired(); // Không cho phép null

            entity.Property(e => e.Email)
                  .HasMaxLength(100)
                  .IsRequired(false); // Cho phép null

            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true)
                  .IsRequired(false); // Cho phép null

            entity.Property(e => e.PasswordHash)
                  .HasMaxLength(255)
                  .IsRequired(false); // Cho phép null

            entity.Property(e => e.PhoneNumber)
                  .HasMaxLength(20)
                  .IsRequired(false); // Cho phép null

            entity.Property(e => e.RandomKey)
                  .HasMaxLength(50)
                  .IsRequired(false); // Cho phép null

            entity.Property(e => e.RefreshToken)
                  .HasMaxLength(200)
                  .IsRequired(false); // Cho phép null

            entity.Property(e => e.RefreshTokenExprired)
                  .HasColumnType("datetime")
                  .IsRequired(false); // Cho phép null

            entity.Property(e => e.IdToken)
                  .HasColumnType("nvarchar(max)")
                  .IsRequired(false); // Cho phép null

            entity.Property(e => e.GoogleUserId)
                  .HasColumnType("nvarchar(255)")
                  .IsRequired(false); // Cho phép null
        });



        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A2831BE99");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160AAE25D65").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            entity.ToTable("UserRole");

            entity.HasOne(d => d.Role)
        .WithMany(r => r.UserRoles) // ✅ Đã thêm navigation property
        .HasForeignKey(d => d.RoleId)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("FK_UserRole_Roles");

            entity.HasOne(d => d.User)
                .WithMany(u => u.UserRoles) // ✅ Đã thêm navigation property
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Users");

        });
        modelBuilder.Entity<UserFunction>(entity =>
        {
            entity.ToTable("UserFunction");
        });
        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.SessionId);

            entity.ToTable("User_Session");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeviceId).HasMaxLength(100);
            entity.Property(e => e.Ip)
                .HasMaxLength(100)
                .HasColumnName("IP");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ClosingTime).HasPrecision(0);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OpeningTime).HasPrecision(0);

            entity.HasOne(d => d.Customer).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouses_Customers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
