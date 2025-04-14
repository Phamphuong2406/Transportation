using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BusinessLogic.DTOs;
using BusinessLogic.Services.Account;
using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogic.Services
{
    public interface ITripService
    {
        List<Trip> GetAllTrip();
        List<TripDTO> GetTripByStatusandStatus(ShippingRequest model);
        Trip GetTripById(int tripId);
        int CreateTrip(Trip model);
        List<TripDTO> GetTripByUserId(int userId);
        int EndTrip(int tripId);
        Task<int> StartShipping(int? tripId);
    }
    public class TripService : ITripService
    {
        private MyDbContext _context;
        private IAccountService _accountService;
        public TripService(MyDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }
        public List<Trip> GetAllTrip()
        {
            return _context.Trips.ToList();
        }
        public List<TripDTO> GetTripByStatusandStatus(ShippingRequest model)
        {
            var availableTrips = _context.Trips.Include(q => q.Truck)
                                .Include(q => q.Shift)
                   .Where(t => t.Status == "Đã lên lịch" || t.Status == "Đã gán xe")
                  .ToList();
            if (availableTrips.Count > 0)
            {
                // lọc theo trọng tải
                var suitableTrips = availableTrips
                .Where(t => _context.DispatchAssignments.Where(x => x.TripId == t.TripId) // đơn điều phối cho chuyến hàng đó
                                                        .Sum(x => x.Weight) + model.Weight <= t.Truck.Capacity)
                .Select(t => new TripDTO
                {
                    TripId = t.TripId,
                    ShiftName = t.Shift.ShiftName,
                    TruckId = t.TruckId,
                    AssignedDate = t.AssignedDate,
                    Status = t.Status
                })
                .ToList();

                return suitableTrips;
            } // đủ tải trọng trống;
            return null;
        }
        public Trip GetTripById(int tripId)
        {
            return _context.Trips.Include(x => x.Truck).FirstOrDefault(x => x.TripId == tripId);
        }
        public int CreateTrip(Trip model)
        {

            model.Status = "Đã lên lịch";
            _context.Trips.Add(model);
            return _context.SaveChanges();
        }
        public List<TripDTO> GetTripByUserId(int userId)
        {
            var driver = _accountService.GetDriverByUserId(userId);
            if (driver == null)
            {
                return null;
            }
            return _context.Trips.Include(x => x.DispatchAssignments)

                .Where(u => u.Truck.DriverId == driver.DriverId).OrderByDescending(x => x.AssignedDate)
                .Select(t => new TripDTO
                {
                    TripId = t.TripId,
                    AssignedDate = t.AssignedDate,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    ShiftId = t.ShiftId,
                    Status = t.Status,
                    TruckId = t.TruckId

                }).ToList();
        }

        public int EndTrip(int tripId)
        {
            var trip = _context.Trips.FirstOrDefault(x => x.TripId == tripId);
            if (trip == null)
            {
                return -1;
            }
            trip.Status = "Hoàn thành";
            _context.Trips.Update(trip);
             _context.SaveChanges();
            return 1;
        }
        public async Task<int> StartShipping(int? tripId) {
            var trip = await _context.Trips.FirstOrDefaultAsync(x => x.TripId == tripId);
            if (trip != null)
            {
                trip.Status = "Đang vận chuyển";
                _context.Trips.Update(trip);
                _context.SaveChanges();
                return 1;
            }
            return -1;
        }
    }
}