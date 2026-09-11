using System;

namespace VehicleRentalLogin
{
    public class CustomerRecord
    {
        public string CustomerId { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string Address { get; set; } = "";
    }

    public class VehicleRecord
    {
        public string VehicleId { get; set; } = "";
        public string Model { get; set; } = "";
        public string VehicleType { get; set; } = "";
        public decimal DailyRate { get; set; }
        public string Status { get; set; } = "Available";
    }
}