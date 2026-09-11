using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MySqlConnector;

namespace VehicleRentalLogin
{
    /// <summary>
    /// Bridges the WinForms app to the MySQL / MariaDB server included in XAMPP.
    ///
    /// SETUP:
    ///   1. Open the XAMPP Control Panel and press "Start" next to MySQL.
    ///   2. Run the app. On first launch it automatically creates the
    ///      "vehicle_rental" database, its tables, and sample data.
    ///      (You can also create everything manually by importing install.sql
    ///      through phpMyAdmin.)
    ///
    /// DEFAULT ADMIN LOGIN (seeded automatically):  admin@drivehub.com / admin123
    /// </summary>
    public static class Database
    {
        // ==== EDIT THESE TO MATCH YOUR XAMPP / MYSQL SETUP ====
        public const string DbServer = "localhost";
        public const uint DbPort = 3306;
        public const string DbName = "vehicle_rental";
        public const string DbUser = "root";
        public const string DbPassword = "";
        // ======================================================

        public static bool IsAvailable { get; private set; }
        public static string LastError { get; private set; } = "";
        public static string LastErrorDetail { get; private set; } = "";

        private static string ServerConnectionString =>
            $"Server={DbServer};Port={DbPort};User ID={DbUser};Password={DbPassword};" +
            "CharSet=utf8mb4;ConnectionTimeout=5;";

        private static string ConnectionString =>
            $"{ServerConnectionString}Database={DbName};";

        // ------------------------------------------------------ helpers

        public static string HashPassword(string password)
        {
            using SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        private static string ConnectionFailureMessage() =>
            "Could not connect to the MySQL database.\n\n" +
            "Please make sure that:\n" +
            "   1. XAMPP Control Panel is open.\n" +
            "   2. MySQL is started (green \"Running\").\n" +
            "   3. The settings in Database.cs match your MySQL.\n\n" +
            "Detail: " + LastErrorDetail;
/// <summary>Creates the database, tables, and seed data if they are missing. Safe to call repeatedly.</summary>
        public static (bool Ok, string Message) EnsureDatabase()
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ServerConnectionString);
                conn.Open();

                static void Exec(MySqlConnection c, string sql)
                {
                    using MySqlCommand cmd = new MySqlCommand(sql, c);
                    cmd.ExecuteNonQuery();
                }

                Exec(conn,
                    $"CREATE DATABASE IF NOT EXISTS `{DbName}` " +
                    "CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");
                Exec(conn, $"USE `{DbName}`");

                Exec(conn,
                    "CREATE TABLE IF NOT EXISTS users (" +
                    " id INT AUTO_INCREMENT PRIMARY KEY," +
                    " full_name VARCHAR(100) NOT NULL," +
                    " email VARCHAR(150) NOT NULL UNIQUE," +
                    " password_hash VARCHAR(64) NOT NULL," +
                    " created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP)");

                Exec(conn,
                    "CREATE TABLE IF NOT EXISTS customers (" +
                    " customer_id VARCHAR(20) PRIMARY KEY," +
                    " name VARCHAR(100) NOT NULL," +
                    " contact_number VARCHAR(30) NOT NULL," +
                    " address VARCHAR(255) NOT NULL)");

                Exec(conn,
                    "CREATE TABLE IF NOT EXISTS vehicles (" +
                    " vehicle_id VARCHAR(20) PRIMARY KEY," +
                    " model VARCHAR(100) NOT NULL," +
                    " vehicle_type VARCHAR(50) NOT NULL," +
                    " daily_rate DECIMAL(10,2) NOT NULL," +
                    " status VARCHAR(20) NOT NULL DEFAULT 'Available')");

                Exec(conn,
                    "CREATE TABLE IF NOT EXISTS bookings (" +
                    " booking_id VARCHAR(20) PRIMARY KEY," +
                    " customer_name VARCHAR(100) NOT NULL," +
                    " vehicle_name VARCHAR(100) NOT NULL," +
                    " from_date DATE NOT NULL," +
                    " to_date DATE NOT NULL," +
                    " status VARCHAR(20) NOT NULL DEFAULT 'Pending'," +
                    " amount DECIMAL(10,2) NOT NULL DEFAULT 0)");

                SeedAdmin(conn);
                SeedVehicles(conn);
                SeedCustomers(conn);
                SeedBookings(conn);

                IsAvailable = true;
                LastError = "";
                LastErrorDetail = "";
                return (true, "Database is ready.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }

        /// <summary>Returns true when the database is reachable; tries to set it up first if needed.</summary>
        public static bool EnsureReady()
        {
            if (IsAvailable)
                return true;

            (bool ok, _) = EnsureDatabase();
            return ok;
        }

        public static bool TestConnection()
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                IsAvailable = true;
                LastError = "";
                LastErrorDetail = "";
                return true;
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return false;
            }
        }
// ------------------------------------------------------ seeding

        private static void SeedAdmin(MySqlConnection conn)
        {
            using MySqlCommand check = new MySqlCommand("SELECT COUNT(*) FROM users", conn);
            if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                return;

            using MySqlCommand cmd = new MySqlCommand(
                "INSERT INTO users (full_name, email, password_hash) " +
                "VALUES (@name, @email, @hash)", conn);
            cmd.Parameters.AddWithValue("@name", "Administrator");
            cmd.Parameters.AddWithValue("@email", "admin@drivehub.com");
            cmd.Parameters.AddWithValue("@hash", HashPassword("admin123"));
            cmd.ExecuteNonQuery();
        }

        private static void SeedVehicles(MySqlConnection conn)
        {
            using MySqlCommand check = new MySqlCommand("SELECT COUNT(*) FROM vehicles", conn);
            if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                return;

            (string Id, string Model, string Type, decimal Rate, string Status)[] rows =
            {
                ("VH-001", "Toyota Vios", "Sedan", 1500, "Available"),
                ("VH-002", "Honda CR-V", "SUV", 2500, "Rented"),
                ("VH-003", "Ford Ranger", "Pickup", 3000, "Available"),
                ("VH-004", "Mitsubishi Mirage", "Hatchback", 1200, "Maintenance"),
                ("VH-005", "Hyundai Starex", "Van", 3500, "Available")
            };

            using MySqlCommand cmd = new MySqlCommand(
                "INSERT INTO vehicles (vehicle_id, model, vehicle_type, daily_rate, status) " +
                "VALUES (@id, @model, @type, @rate, @status)", conn);

            foreach (var row in rows)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", row.Id);
                cmd.Parameters.AddWithValue("@model", row.Model);
                cmd.Parameters.AddWithValue("@type", row.Type);
                cmd.Parameters.AddWithValue("@rate", row.Rate);
                cmd.Parameters.AddWithValue("@status", row.Status);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedCustomers(MySqlConnection conn)
        {
            using MySqlCommand check = new MySqlCommand("SELECT COUNT(*) FROM customers", conn);
            if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                return;

            (string Id, string Name, string Contact, string Address)[] rows =
            {
                ("CUS-1001", "Juan Dela Cruz", "09171234567", "Santa Rosa, Laguna"),
                ("CUS-1002", "Maria Santos", "09181234567", "Binan, Laguna"),
                ("CUS-1003", "Pedro Reyes", "09201234567", "Calamba, Laguna"),
                ("CUS-1004", "Ana Lopez", "09221234567", "Cabuyao, Laguna"),
                ("CUS-1005", "Carlos Tan", "09351234567", "Tagaytay, Cavite")
            };

            using MySqlCommand cmd = new MySqlCommand(
                "INSERT INTO customers (customer_id, name, contact_number, address) " +
                "VALUES (@id, @name, @contact, @address)", conn);

            foreach (var row in rows)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", row.Id);
                cmd.Parameters.AddWithValue("@name", row.Name);
                cmd.Parameters.AddWithValue("@contact", row.Contact);
                cmd.Parameters.AddWithValue("@address", row.Address);
                cmd.ExecuteNonQuery();
            }
        }
private static void SeedBookings(MySqlConnection conn)
        {
            using MySqlCommand check = new MySqlCommand("SELECT COUNT(*) FROM bookings", conn);
            if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                return;

            (string Id, string Customer, string Vehicle, string From, string To, string Status, decimal Amount)[] rows =
            {
                ("BK-1001", "Juan Dela Cruz", "Toyota Vios", "2026-08-10", "2026-08-12", "Completed", 3000),
                ("BK-1002", "Maria Santos", "Honda CR-V", "2026-08-11", "2026-08-15", "Active", 10000),
                ("BK-1003", "Pedro Reyes", "Ford Ranger", "2026-08-13", "2026-08-14", "Pending", 3000),
                ("BK-1004", "Ana Lopez", "Mitsubishi Mirage", "2026-08-05", "2026-08-07", "Completed", 2400),
                ("BK-1005", "Carlos Tan", "Hyundai Starex", "2026-08-14", "2026-08-20", "Cancelled", 21000)
            };

            using MySqlCommand cmd = new MySqlCommand(
                "INSERT INTO bookings (booking_id, customer_name, vehicle_name, from_date, to_date, status, amount) " +
                "VALUES (@id, @customer, @vehicle, @from, @to, @status, @amount)", conn);

            foreach (var row in rows)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", row.Id);
                cmd.Parameters.AddWithValue("@customer", row.Customer);
                cmd.Parameters.AddWithValue("@vehicle", row.Vehicle);
                cmd.Parameters.AddWithValue("@from", row.From);
                cmd.Parameters.AddWithValue("@to", row.To);
                cmd.Parameters.AddWithValue("@status", row.Status);
                cmd.Parameters.AddWithValue("@amount", row.Amount);
                cmd.ExecuteNonQuery();
            }
        }

        // ------------------------------------------------------ users

        public static (bool Ok, string Message) ValidateLogin(string email, string password)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "SELECT full_name FROM users " +
                    "WHERE email = @email AND password_hash = @hash LIMIT 1", conn);
                cmd.Parameters.AddWithValue("@email", email.Trim());
                cmd.Parameters.AddWithValue("@hash", HashPassword(password));

                object? result = cmd.ExecuteScalar();
                IsAvailable = true;
                LastError = "";
                LastErrorDetail = "";

                if (result is null || result is DBNull)
                    return (false, "Invalid email or password.");

                return (true, Convert.ToString(result) ?? "");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }

        public static (bool Ok, string Message) RegisterUser(string fullName, string email, string password)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO users (full_name, email, password_hash) " +
                    "VALUES (@name, @email, @hash)", conn);
                cmd.Parameters.AddWithValue("@name", fullName);
                cmd.Parameters.AddWithValue("@email", email.Trim());
                cmd.Parameters.AddWithValue("@hash", HashPassword(password));
                cmd.ExecuteNonQuery();

                IsAvailable = true;
                return (true, "Account created successfully!\nYou can now log in.");
            }
            catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
            {
                return (false, "An account with that email address already exists.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }

        public static (bool Ok, string Message) ResetPassword(string email)
        {
            const string tempPassword = "Reset@123";

            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand check = new MySqlCommand(
                    "SELECT COUNT(*) FROM users WHERE email = @email", conn);
                check.Parameters.AddWithValue("@email", email.Trim());

                if (Convert.ToInt64(check.ExecuteScalar()) == 0)
                    return (false, "No account found with that email address.");

                using MySqlCommand cmd = new MySqlCommand(
                    "UPDATE users SET password_hash = @hash WHERE email = @email", conn);
                cmd.Parameters.AddWithValue("@hash", HashPassword(tempPassword));
                cmd.Parameters.AddWithValue("@email", email.Trim());
                cmd.ExecuteNonQuery();

                IsAvailable = true;
                return (true,
                    $"Password reset complete.\n\n" +
                    $"Your temporary password is: {tempPassword}\n\n" +
                    "Use it to log in with your email.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }
// ------------------------------------------------------ bookings

        /// <summary>Returns bookings from the database, newest first. Pass a limit to cap the row count.</summary>
        public static List<BookingRecord> GetBookings(int limit = 0)
        {
            List<BookingRecord> result = new List<BookingRecord>();

            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string sql =
                    "SELECT booking_id, customer_name, vehicle_name, from_date, to_date, status, amount " +
                    "FROM bookings ORDER BY from_date DESC, booking_id DESC";

                if (limit > 0)
                    sql += $" LIMIT {limit}";

                using MySqlCommand cmd = new MySqlCommand(sql, conn);
                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new BookingRecord
                    {
                        BookingId = reader.GetString(0),
                        CustomerName = reader.GetString(1),
                        VehicleName = reader.GetString(2),
                        FromDate = reader.GetDateTime(3).ToString("yyyy-MM-dd"),
                        ToDate = reader.GetDateTime(4).ToString("yyyy-MM-dd"),
                        Status = reader.GetString(5),
                        Amount = reader.GetDecimal(6)
                    });
                }

                IsAvailable = true;
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
            }

            return result;
        }

        /// <summary>Looks up a vehicle's daily rate by its model name; falls back to 2000 when unknown.</summary>
        public static decimal GetDailyRate(string vehicleName)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "SELECT daily_rate FROM vehicles WHERE model = @model LIMIT 1", conn);
                cmd.Parameters.AddWithValue("@model", vehicleName.Trim());

                object? rate = cmd.ExecuteScalar();
                IsAvailable = true;

                if (rate is null || rate is DBNull)
                    return 2000m;

                return Convert.ToDecimal(rate);
            }
            catch
            {
                return 2000m;
            }
        }

        public static (bool Ok, string Message) AddBooking(
            string bookingId,
            string customerName,
            string vehicleName,
            string fromDate,
            string toDate,
            string status = "Pending")
        {
            try
            {
                if (!DateTime.TryParse(fromDate, out DateTime from))
                    return (false, "The 'From' date is invalid.");

                if (!DateTime.TryParse(toDate, out DateTime to))
                    return (false, "The 'To' date is invalid.");

                int days = Math.Max(1, (to.Date - from.Date).Days);
                decimal amount = days * GetDailyRate(vehicleName);

                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO bookings " +
                    "(booking_id, customer_name, vehicle_name, from_date, to_date, status, amount) " +
                    "VALUES (@id, @customer, @vehicle, @from, @to, @status, @amount)", conn);
                cmd.Parameters.AddWithValue("@id", bookingId);
                cmd.Parameters.AddWithValue("@customer", customerName);
                cmd.Parameters.AddWithValue("@vehicle", vehicleName);
                cmd.Parameters.AddWithValue("@from", from.Date);
                cmd.Parameters.AddWithValue("@to", to.Date);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();

                IsAvailable = true;
                return (true, "Booking added successfully.");
            }
            catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
            {
                return (false, "A booking with that ID already exists.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }

        public static (bool Ok, string Message) CancelBooking(string bookingId)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "UPDATE bookings SET status = 'Cancelled' WHERE booking_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", bookingId);
                cmd.ExecuteNonQuery();

                IsAvailable = true;
                return (true, "Booking cancelled successfully.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }
// ------------------------------------------------------ customers

        public static List<CustomerRecord> GetCustomers()
        {
            List<CustomerRecord> result = new List<CustomerRecord>();

            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "SELECT customer_id, name, contact_number, address " +
                    "FROM customers ORDER BY name", conn);
                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new CustomerRecord
                    {
                        CustomerId = reader.GetString(0),
                        CustomerName = reader.GetString(1),
                        ContactNumber = reader.GetString(2),
                        Address = reader.GetString(3)
                    });
                }

                IsAvailable = true;
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
            }

            return result;
        }

        public static (bool Ok, string Message) AddCustomer(
            string customerId, string name, string contactNumber, string address)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO customers (customer_id, name, contact_number, address) " +
                    "VALUES (@id, @name, @contact, @address)", conn);
                cmd.Parameters.AddWithValue("@id", customerId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@contact", contactNumber);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.ExecuteNonQuery();

                IsAvailable = true;
                return (true, "Customer added successfully.");
            }
            catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
            {
                return (false, "A customer with that ID already exists.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }

        public static (bool Ok, string Message) DeleteCustomer(string customerId)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "DELETE FROM customers WHERE customer_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", customerId);
                cmd.ExecuteNonQuery();

                IsAvailable = true;
                return (true, "Customer deleted successfully.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }
// ------------------------------------------------------ vehicles

        public static List<VehicleRecord> GetVehicles()
        {
            List<VehicleRecord> result = new List<VehicleRecord>();

            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "SELECT vehicle_id, model, vehicle_type, daily_rate, status " +
                    "FROM vehicles ORDER BY model", conn);
                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new VehicleRecord
                    {
                        VehicleId = reader.GetString(0),
                        Model = reader.GetString(1),
                        VehicleType = reader.GetString(2),
                        DailyRate = reader.GetDecimal(3),
                        Status = reader.GetString(4)
                    });
                }

                IsAvailable = true;
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
            }

            return result;
        }

        public static (bool Ok, string Message) AddVehicle(
            string vehicleId, string model, string type, decimal dailyRate, string status)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO vehicles (vehicle_id, model, vehicle_type, daily_rate, status) " +
                    "VALUES (@id, @model, @type, @rate, @status)", conn);
                cmd.Parameters.AddWithValue("@id", vehicleId);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@rate", dailyRate);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();

                IsAvailable = true;
                return (true, "Vehicle added successfully.");
            }
            catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
            {
                return (false, "A vehicle with that ID already exists.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }

        public static (bool Ok, string Message) UpdateVehicle(
            string vehicleId, string model, string type, decimal dailyRate, string status)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "UPDATE vehicles " +
                    "SET model = @model, vehicle_type = @type, daily_rate = @rate, status = @status " +
                    "WHERE vehicle_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", vehicleId);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@rate", dailyRate);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();

                IsAvailable = true;
                return (true, "Vehicle updated successfully.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }

        public static (bool Ok, string Message) DeleteVehicle(string vehicleId)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "DELETE FROM vehicles WHERE vehicle_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", vehicleId);
                cmd.ExecuteNonQuery();

                IsAvailable = true;
                return (true, "Vehicle deleted successfully.");
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return (false, LastError);
            }
        }
// ------------------------------------------------------ dashboard counts

        public static int CountVehicles() => ScalarCount("vehicles", "");
        public static int CountAvailableVehicles() => ScalarCount("vehicles", "status = 'Available'");
        public static int CountActiveBookings() => ScalarCount("bookings", "status = 'Active'");
        public static int CountCustomers() => ScalarCount("customers", "");

        private static int ScalarCount(string table, string where)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string sql = $"SELECT COUNT(*) FROM `{table}`";
                if (!string.IsNullOrEmpty(where))
                    sql += " WHERE " + where;

                using MySqlCommand cmd = new MySqlCommand(sql, conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                IsAvailable = true;
                return count;
            }
            catch (Exception ex)
            {
                IsAvailable = false;
                LastErrorDetail = ex.Message;
                LastError = ConnectionFailureMessage();
                return 0;
            }
        }
    }
}