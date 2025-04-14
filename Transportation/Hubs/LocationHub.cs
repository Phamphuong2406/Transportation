using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;


namespace Transportation.Hubs
{
    public class LocationHub : Hub
    {
        private readonly MyDbContext _context;

        public LocationHub(MyDbContext context)
        {
            _context = context;
        }

        // Gửi vị trí cho tất cả client khác và lưu DB
        public async Task SendLocationUpdate(int tripId, decimal latitude, decimal longitude)
        {
            try
            {
                Console.WriteLine($"📌 Nhận dữ liệu: TripId={tripId}, Lat={latitude}, Lng={longitude}");
                var trip = await _context.Trips.FirstOrDefaultAsync(x => x.TripId == tripId);
                // 🔹 Gửi vị trí đến tất cả client trước khi lưu DB
                if (trip != null)
                {
                    await Clients.All.SendAsync("ReceiveLocation", trip.TruckId, latitude, longitude);

                    // 🔹 Kiểm tra vị trí trước khi lưu (giảm tải database)
                    var lastLocation = await _context.RealTimeTrackings
                        .Where(l => l.TruckId == trip.TruckId)
                        .OrderByDescending(l => l.Timestamp)
                        .FirstOrDefaultAsync();

                    // 🔹 Chỉ lưu nếu vị trí thay đổi đáng kể (ví dụ: di chuyển trên 10m)
                    if (lastLocation == null || CalculateDistance(lastLocation.CurrentLat, lastLocation.CurrentLng, latitude, longitude) > 10)
                    {
                        var location = new RealTimeTracking
                        {
                            TruckId = (int)trip.TruckId,
                            CurrentLat = latitude,
                            CurrentLng = longitude,
                            Timestamp = DateTime.Now // Nên dùng UTC để tránh lỗi timezone
                        };

                        _context.RealTimeTrackings.Add(location);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"⚠ Lỗi lưu vị trí: {ex.Message}");
            }
        }

        // 🔹 Hàm tính khoảng cách giữa 2 tọa độ (Haversine Formula) - Giữ kiểu decimal
        private double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            double R = 6371000; // Bán kính Trái Đất (mét)
            double dLat = (double)(lat2 - lat1) * Math.PI / 180;
            double dLon = (double)(lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos((double)lat1 * Math.PI / 180) * Math.Cos((double)lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }

}


