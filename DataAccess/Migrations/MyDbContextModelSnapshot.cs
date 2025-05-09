﻿// <auto-generated />
using System;
using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccess.Entity.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CustomerID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"));

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("CustomerId")
                        .HasName("PK__Customer__A4AE64B800BC461F");

                    b.HasIndex("UserId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("DataAccess.Entity.DispatchAssignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AssignmentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssignmentId"));

                    b.Property<int>("AssignedBy")
                        .HasColumnType("int");

                    b.Property<DateOnly>("AssignedDate")
                        .HasColumnType("date");

                    b.Property<int?>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("DeliveryImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Deliverydate")
                        .HasColumnType("datetime");

                    b.Property<decimal?>("DropoffLat")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<decimal?>("DropoffLng")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<string>("DropoffLocation")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FuelType")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Note")
                        .HasMaxLength(225)
                        .HasColumnType("nvarchar(225)");

                    b.Property<decimal?>("ParkingLat")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<decimal?>("ParkingLng")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<decimal?>("PickupLat")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<decimal?>("PickupLng")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<string>("PickupLocation")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("Pickupdate")
                        .HasColumnType("datetime");

                    b.Property<int?>("ProductTypeId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("RequestDate")
                        .HasColumnType("date");

                    b.Property<int>("RequestId")
                        .HasColumnType("int")
                        .HasColumnName("RequestID");

                    b.Property<int?>("ShippingCost")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("TripId")
                        .HasColumnType("int");

                    b.Property<int?>("Weight")
                        .HasColumnType("int");

                    b.HasKey("AssignmentId")
                        .HasName("PK__Dispatch__32499E573A817140");

                    b.HasIndex("AssignedBy");

                    b.HasIndex("RequestId");

                    b.HasIndex("TripId");

                    b.ToTable("DispatchAssignments");
                });

            modelBuilder.Entity("DataAccess.Entity.Dispatcher", b =>
                {
                    b.Property<int>("DispatcherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DispatcherID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DispatcherId"));

                    b.Property<string>("Department")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("DispatcherId")
                        .HasName("PK__Dispatch__EB9ED164EB1A2DFB");

                    b.HasIndex("UserId");

                    b.ToTable("Dispatchers");
                });

            modelBuilder.Entity("DataAccess.Entity.Driver", b =>
                {
                    b.Property<int>("DriverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DriverID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DriverId"));

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("HealthStatus")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Idcard")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("IDCard");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("DriverId")
                        .HasName("PK__Drivers__F1B1CD24C75CABBF");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "Idcard" }, "UQ__Drivers__43A2A4E345F311AB")
                        .IsUnique()
                        .HasFilter("[IDCard] IS NOT NULL");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("DataAccess.Entity.Function", b =>
                {
                    b.Property<int>("FunctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FunctionId"));

                    b.Property<string>("FunctionCode")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FunctionName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("FunctionId");

                    b.ToTable("Function", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entity.ProductType", b =>
                {
                    b.Property<int>("ProductTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductTypeId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ProductTypeId");

                    b.ToTable("ProductType", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entity.RealTimeTracking", b =>
                {
                    b.Property<int>("TrackingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TrackingID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrackingId"));

                    b.Property<decimal>("CurrentLat")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<decimal>("CurrentLng")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.Property<int>("TruckId")
                        .HasColumnType("int")
                        .HasColumnName("TruckID");

                    b.HasKey("TrackingId")
                        .HasName("PK__RealTime__3C19EDD16DFB6755");

                    b.HasIndex("TruckId");

                    b.ToTable("RealTimeTracking", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entity.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("RoleID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleId")
                        .HasName("PK__Roles__8AFACE3A2831BE99");

                    b.HasIndex(new[] { "RoleName" }, "UQ__Roles__8A2B6160AAE25D65")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DataAccess.Entity.Shift", b =>
                {
                    b.Property<int>("ShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ShiftID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShiftId"));

                    b.Property<TimeOnly>("EndTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)");

                    b.Property<string>("ShiftName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<TimeOnly>("StartTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)");

                    b.HasKey("ShiftId");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("DataAccess.Entity.ShippingRequest", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("RequestID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int")
                        .HasColumnName("CustomerID");

                    b.Property<DateTime?>("Deliverydate")
                        .HasColumnType("datetime");

                    b.Property<decimal?>("DropoffLat")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<decimal?>("DropoffLng")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<string>("DropoffLocation")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Note")
                        .HasMaxLength(225)
                        .HasColumnType("nvarchar(225)");

                    b.Property<decimal?>("PickupLat")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<decimal?>("PickupLng")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<string>("PickupLocation")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("Pickupdate")
                        .HasColumnType("datetime");

                    b.Property<int?>("ProductTypeId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("RequestDate")
                        .HasColumnType("date");

                    b.Property<int>("ShippingCost")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("RequestId")
                        .HasName("PK__Shipping__33A8519AA7E85CED");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("ShippingRequests");
                });

            modelBuilder.Entity("DataAccess.Entity.Trip", b =>
                {
                    b.Property<int>("TripId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TripId"));

                    b.Property<DateOnly?>("AssignedDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly?>("EndTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)");

                    b.Property<int?>("ShiftId")
                        .HasColumnType("int")
                        .HasColumnName("ShiftID");

                    b.Property<TimeOnly?>("StartTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("TruckId")
                        .HasColumnType("int")
                        .HasColumnName("TruckID");

                    b.HasKey("TripId");

                    b.HasIndex("ShiftId");

                    b.HasIndex("TruckId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("DataAccess.Entity.Truck", b =>
                {
                    b.Property<int>("TruckId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TruckID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TruckId"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<decimal>("ConsumptionRate")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<int?>("DriverId")
                        .HasColumnType("int")
                        .HasColumnName("DriverID");

                    b.Property<string>("FuelType")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LicensePlate")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<decimal?>("ParkingLat")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<decimal?>("ParkingLng")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<string>("ParkingLocation")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("TruckId")
                        .HasName("PK__Trucks__6632E95B7A4565FD");

                    b.HasIndex("DriverId");

                    b.ToTable("Trucks");
                });

            modelBuilder.Entity("DataAccess.Entity.UserFunction", b =>
                {
                    b.Property<int>("UserFunctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserFunctionId"));

                    b.Property<int?>("FunctionId")
                        .HasColumnType("int");

                    b.Property<int?>("IsCreate")
                        .HasColumnType("int");

                    b.Property<int?>("IsDelete")
                        .HasColumnType("int");

                    b.Property<int?>("IsUpdate")
                        .HasColumnType("int");

                    b.Property<int?>("IsView")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserFunctionId");

                    b.ToTable("UserFunction", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entity.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entity.UserSession", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SessionId"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeviceId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Ip")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("IP");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("SessionId");

                    b.ToTable("User_Session", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entity.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("GoogleUserId")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("IdToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("RandomKey")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("RefreshTokenExprired")
                        .HasColumnType("datetime");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId")
                        .HasName("PK__Users__1788CCAC8353AE3B");

                    b.HasIndex(new[] { "Username" }, "UQ__Users__536C85E42FBDB1A9")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataAccess.Entity.Warehouse", b =>
                {
                    b.Property<int>("WarehouseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("WarehouseID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WarehouseId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<TimeOnly>("ClosingTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int")
                        .HasColumnName("CustomerID");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<TimeOnly>("OpeningTime")
                        .HasPrecision(0)
                        .HasColumnType("time(0)");

                    b.HasKey("WarehouseId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("DataAccess.Entity.Customer", b =>
                {
                    b.HasOne("DataAccess.Entity.Users", "User")
                        .WithMany("Customers")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Customers__UserI__4D94879B");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.DispatchAssignment", b =>
                {
                    b.HasOne("DataAccess.Entity.Dispatcher", "AssignedByNavigation")
                        .WithMany("DispatchAssignments")
                        .HasForeignKey("AssignedBy")
                        .IsRequired()
                        .HasConstraintName("FK_DispatchAssignments_Dispatchers");

                    b.HasOne("DataAccess.Entity.ShippingRequest", "Request")
                        .WithMany("DispatchAssignments")
                        .HasForeignKey("RequestId")
                        .IsRequired()
                        .HasConstraintName("FK_DispatchAssignments_ShippingRequests");

                    b.HasOne("DataAccess.Entity.Trip", "Trip")
                        .WithMany("DispatchAssignments")
                        .HasForeignKey("TripId")
                        .HasConstraintName("FK_DispatchAssignments_Trips");

                    b.Navigation("AssignedByNavigation");

                    b.Navigation("Request");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("DataAccess.Entity.Dispatcher", b =>
                {
                    b.HasOne("DataAccess.Entity.Users", "User")
                        .WithMany("Dispatchers")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Dispatche__UserI__5441852A");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.Driver", b =>
                {
                    b.HasOne("DataAccess.Entity.Users", "User")
                        .WithMany("Drivers")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__Drivers__UserID__5165187F");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.RealTimeTracking", b =>
                {
                    b.HasOne("DataAccess.Entity.Truck", "Truck")
                        .WithMany("RealTimeTrackings")
                        .HasForeignKey("TruckId")
                        .IsRequired()
                        .HasConstraintName("FK__RealTimeT__Truck__619B8048");

                    b.Navigation("Truck");
                });

            modelBuilder.Entity("DataAccess.Entity.ShippingRequest", b =>
                {
                    b.HasOne("DataAccess.Entity.Customer", "Customer")
                        .WithMany("ShippingRequests")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK__ShippingR__Custo__59FA5E80");

                    b.HasOne("DataAccess.Entity.ProductType", "ProductType")
                        .WithMany("ShippingRequests")
                        .HasForeignKey("ProductTypeId")
                        .HasConstraintName("FK_ShippingRequests_ProductType");

                    b.Navigation("Customer");

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("DataAccess.Entity.Trip", b =>
                {
                    b.HasOne("DataAccess.Entity.Shift", "Shift")
                        .WithMany("Trips")
                        .HasForeignKey("ShiftId")
                        .HasConstraintName("FK_Trips_Shifts");

                    b.HasOne("DataAccess.Entity.Truck", "Truck")
                        .WithMany("Trips")
                        .HasForeignKey("TruckId")
                        .HasConstraintName("FK_Trips_Trucks");

                    b.Navigation("Shift");

                    b.Navigation("Truck");
                });

            modelBuilder.Entity("DataAccess.Entity.Truck", b =>
                {
                    b.HasOne("DataAccess.Entity.Driver", "Driver")
                        .WithMany("Trucks")
                        .HasForeignKey("DriverId")
                        .HasConstraintName("FK_Trucks_Drivers");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("DataAccess.Entity.UserRole", b =>
                {
                    b.HasOne("DataAccess.Entity.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_UserRole_Roles");

                    b.HasOne("DataAccess.Entity.Users", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_UserRole_Users");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entity.Warehouse", b =>
                {
                    b.HasOne("DataAccess.Entity.Customer", "Customer")
                        .WithMany("Warehouses")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK_Warehouses_Customers");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DataAccess.Entity.Customer", b =>
                {
                    b.Navigation("ShippingRequests");

                    b.Navigation("Warehouses");
                });

            modelBuilder.Entity("DataAccess.Entity.Dispatcher", b =>
                {
                    b.Navigation("DispatchAssignments");
                });

            modelBuilder.Entity("DataAccess.Entity.Driver", b =>
                {
                    b.Navigation("Trucks");
                });

            modelBuilder.Entity("DataAccess.Entity.ProductType", b =>
                {
                    b.Navigation("ShippingRequests");
                });

            modelBuilder.Entity("DataAccess.Entity.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("DataAccess.Entity.Shift", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("DataAccess.Entity.ShippingRequest", b =>
                {
                    b.Navigation("DispatchAssignments");
                });

            modelBuilder.Entity("DataAccess.Entity.Trip", b =>
                {
                    b.Navigation("DispatchAssignments");
                });

            modelBuilder.Entity("DataAccess.Entity.Truck", b =>
                {
                    b.Navigation("RealTimeTrackings");

                    b.Navigation("Trips");
                });

            modelBuilder.Entity("DataAccess.Entity.Users", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("Dispatchers");

                    b.Navigation("Drivers");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
