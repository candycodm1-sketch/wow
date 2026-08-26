using System;
using System.Collections.Generic;

namespace VehicleRentalLogin
{
    public class BookingRecord
    {
        public string BookingId { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string VehicleName { get; set; } = "";
        public string FromDate { get; set; } = "";
        public string ToDate { get; set; } = "";
        public string Status { get; set; } = "";
        public decimal Amount { get; set; }
    }

    public static class BookingData
    {
        public static List<BookingRecord> Bookings { get; } =
            new List<BookingRecord>();

        public static void LoadSamples()
        {
            if (Bookings.Count > 0)
                return;

            Bookings.Add(new BookingRecord
            {
                BookingId = "BK-1001",
                CustomerName = "Juan Dela Cruz",
                VehicleName = "Toyota Vios",
                FromDate = "2026-08-18",
                ToDate = "2026-08-19",
                Status = "Completed",
                Amount = 2000
            });

            Bookings.Add(new BookingRecord
            {
                BookingId = "BK-1002",
                CustomerName = "Maria Santos",
                VehicleName = "Toyota Fortuner",
                FromDate = "2026-08-18",
                ToDate = "2026-08-20",
                Status = "Completed",
                Amount = 4500
            });

            Bookings.Add(new BookingRecord
            {
                BookingId = "BK-1003",
                CustomerName = "Pedro Reyes",
                VehicleName = "Mitsubishi Xpander",
                FromDate = "2026-08-19",
                ToDate = "2026-08-21",
                Status = "Active",
                Amount = 3200
            });

            Bookings.Add(new BookingRecord
            {
                BookingId = "BK-1004",
                CustomerName = "Ana Lopez",
                VehicleName = "Honda Civic",
                FromDate = "2026-08-19",
                ToDate = "2026-08-20",
                Status = "Completed",
                Amount = 2800
            });

            Bookings.Add(new BookingRecord
            {
                BookingId = "BK-1005",
                CustomerName = "Carlos Tan",
                VehicleName = "Toyota Vios",
                FromDate = "2026-08-20",
                ToDate = "2026-08-21",
                Status = "Pending",
                Amount = 2500
            });
        }

        public static void AddBooking(
            string bookingId,
            string customerName,
            string vehicleName,
            string fromDate,
            string toDate,
            string status)
        {
            decimal amount = CalculateAmount(
                fromDate,
                toDate
            );

            Bookings.Add(new BookingRecord
            {
                BookingId = bookingId,
                CustomerName = customerName,
                VehicleName = vehicleName,
                FromDate = fromDate,
                ToDate = toDate,
                Status = status,
                Amount = amount
            });
        }

        public static decimal CalculateAmount(
            string fromDate,
            string toDate)
        {
            if (!DateTime.TryParse(fromDate, out DateTime from))
                return 2000;

            if (!DateTime.TryParse(toDate, out DateTime to))
                return 2000;

            int days =
                Math.Max(
                    1,
                    (to.Date - from.Date).Days
                );

            return days * 2000;
        }
    }
}