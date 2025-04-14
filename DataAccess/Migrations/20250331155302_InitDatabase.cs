using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Function",
                columns: table => new
                {
                    FunctionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FunctionCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function", x => x.FunctionId);
                });

            migrationBuilder.CreateTable(
                name: "ProductType",
                columns: table => new
                {
                    ProductTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType", x => x.ProductTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__8AFACE3A2831BE99", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    ShiftID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.ShiftID);
                });

            migrationBuilder.CreateTable(
                name: "User_Session",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Session", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "UserFunction",
                columns: table => new
                {
                    UserFunctionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    FunctionId = table.Column<int>(type: "int", nullable: true),
                    IsView = table.Column<int>(type: "int", nullable: true),
                    IsUpdate = table.Column<int>(type: "int", nullable: true),
                    IsCreate = table.Column<int>(type: "int", nullable: true),
                    IsDelete = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFunction", x => x.UserFunctionId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RandomKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RefreshTokenExprired = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleUserId = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCAC8353AE3B", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__A4AE64B800BC461F", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK__Customers__UserI__4D94879B",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Dispatchers",
                columns: table => new
                {
                    DispatcherID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Dispatch__EB9ED164EB1A2DFB", x => x.DispatcherID);
                    table.ForeignKey(
                        name: "FK__Dispatche__UserI__5441852A",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    IDCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HealthStatus = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Drivers__F1B1CD24C75CABBF", x => x.DriverID);
                    table.ForeignKey(
                        name: "FK__Drivers__UserID__5165187F",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleID");
                    table.ForeignKey(
                        name: "FK_UserRole_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "ShippingRequests",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PickupLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PickupLat = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    PickupLng = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    DropoffLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DropoffLat = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    DropoffLng = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    ProductTypeId = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    ShippingCost = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true),
                    Pickupdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deliverydate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shipping__33A8519AA7E85CED", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_ShippingRequests_ProductType",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductType",
                        principalColumn: "ProductTypeId");
                    table.ForeignKey(
                        name: "FK__ShippingR__Custo__59FA5E80",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    WarehouseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ClosingTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    OpeningTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehouseID);
                    table.ForeignKey(
                        name: "FK_Warehouses_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TruckID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverID = table.Column<int>(type: "int", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ConsumptionRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ParkingLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ParkingLat = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    ParkingLng = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    LicensePlate = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Trucks__6632E95B7A4565FD", x => x.TruckID);
                    table.ForeignKey(
                        name: "FK_Trucks_Drivers",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "DriverID");
                });

            migrationBuilder.CreateTable(
                name: "RealTimeTracking",
                columns: table => new
                {
                    TrackingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TruckID = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    CurrentLat = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    CurrentLng = table.Column<decimal>(type: "decimal(9,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RealTime__3C19EDD16DFB6755", x => x.TrackingID);
                    table.ForeignKey(
                        name: "FK__RealTimeT__Truck__619B8048",
                        column: x => x.TruckID,
                        principalTable: "Trucks",
                        principalColumn: "TruckID");
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftID = table.Column<int>(type: "int", nullable: true),
                    TruckID = table.Column<int>(type: "int", nullable: true),
                    AssignedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_Trips_Shifts",
                        column: x => x.ShiftID,
                        principalTable: "Shifts",
                        principalColumn: "ShiftID");
                    table.ForeignKey(
                        name: "FK_Trips_Trucks",
                        column: x => x.TruckID,
                        principalTable: "Trucks",
                        principalColumn: "TruckID");
                });

            migrationBuilder.CreateTable(
                name: "DispatchAssignments",
                columns: table => new
                {
                    AssignmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PickupLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PickupLat = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    PickupLng = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    DropoffLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DropoffLat = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    DropoffLng = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: true),
                    ProductTypeId = table.Column<int>(type: "int", nullable: true),
                    ShippingCost = table.Column<int>(type: "int", nullable: true),
                    ParkingLat = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    ParkingLng = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeliveryImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pickupdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deliverydate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TripId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Dispatch__32499E573A817140", x => x.AssignmentID);
                    table.ForeignKey(
                        name: "FK_DispatchAssignments_Dispatchers",
                        column: x => x.AssignedBy,
                        principalTable: "Dispatchers",
                        principalColumn: "DispatcherID");
                    table.ForeignKey(
                        name: "FK_DispatchAssignments_ShippingRequests",
                        column: x => x.RequestID,
                        principalTable: "ShippingRequests",
                        principalColumn: "RequestID");
                    table.ForeignKey(
                        name: "FK_DispatchAssignments_Trips",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "TripId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserID",
                table: "Customers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchAssignments_AssignedBy",
                table: "DispatchAssignments",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchAssignments_RequestID",
                table: "DispatchAssignments",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchAssignments_TripId",
                table: "DispatchAssignments",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatchers_UserID",
                table: "Dispatchers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_UserID",
                table: "Drivers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "UQ__Drivers__43A2A4E345F311AB",
                table: "Drivers",
                column: "IDCard",
                unique: true,
                filter: "[IDCard] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RealTimeTracking_TruckID",
                table: "RealTimeTracking",
                column: "TruckID");

            migrationBuilder.CreateIndex(
                name: "UQ__Roles__8A2B6160AAE25D65",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequests_CustomerID",
                table: "ShippingRequests",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequests_ProductTypeId",
                table: "ShippingRequests",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ShiftID",
                table: "Trips",
                column: "ShiftID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TruckID",
                table: "Trips",
                column: "TruckID");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_DriverID",
                table: "Trucks",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__536C85E42FBDB1A9",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CustomerID",
                table: "Warehouses",
                column: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispatchAssignments");

            migrationBuilder.DropTable(
                name: "Function");

            migrationBuilder.DropTable(
                name: "RealTimeTracking");

            migrationBuilder.DropTable(
                name: "User_Session");

            migrationBuilder.DropTable(
                name: "UserFunction");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Dispatchers");

            migrationBuilder.DropTable(
                name: "ShippingRequests");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ProductType");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
