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

    /// <summary>
    /// Lightweight accessor for the MySQL-backed booking store (see Database.cs).
    /// The management forms talk to Database directly; this facade keeps a single
    /// in-memory snapshot (Bookings) that the Reports form can bind to.
    /// </summary>
    public static class BookingData
    {
        /// <summary>The latest bookings snapshot loaded from the database.</summary>
        public static List<BookingRecord> Bookings { get; } =
            new List<BookingRecord>();

        /// <summary>Reloads all bookings from the database into <see cref="Bookings"/>.</summary>
        public static void Refresh()
        {
            Bookings.Clear();
            Bookings.AddRange(Database.GetBookings());
        }

        public static (bool Ok, string Message) AddBooking(
            string bookingId,
            string customerName,
            string vehicleName,
            string fromDate,
            string toDate,
            string status = "Pending")
        {
            (bool Ok, string Message) result =
                Database.AddBooking(
                    bookingId,
                    customerName,
                    vehicleName,
                    fromDate,
                    toDate,
                    status);

            if (result.Ok)
                Refresh();

            return result;
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