-- ============================================================
-- Vehicle Rental & Booking System  -  MySQL setup script
-- ------------------------------------------------------------
-- How to use:
--   Option A (automatic): just start MySQL in XAMPP, then launch
--     the app. It creates this schema + seed data on first run.
--   Option B (manual):     import this file through phpMyAdmin,
--     or run:  mysql -u root < install.sql
--
-- Default admin login created:  admin@drivehub.com / admin123
-- ============================================================

CREATE DATABASE IF NOT EXISTS vehicle_rental
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE vehicle_rental;

-- ---------- USERS (login accounts) ----------
CREATE TABLE IF NOT EXISTS users (
  id            INT AUTO_INCREMENT PRIMARY KEY,
  full_name     VARCHAR(100) NOT NULL,
  email         VARCHAR(150) NOT NULL UNIQUE,
  password_hash VARCHAR(64)  NOT NULL COMMENT 'SHA-256 hex of the password',
  created_at    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- ---------- CUSTOMERS ----------
CREATE TABLE IF NOT EXISTS customers (
  customer_id    VARCHAR(20) PRIMARY KEY,
  name           VARCHAR(100) NOT NULL,
  contact_number VARCHAR(30)  NOT NULL,
  address        VARCHAR(255) NOT NULL
) ENGINE=InnoDB;

-- ---------- VEHICLES ----------
CREATE TABLE IF NOT EXISTS vehicles (
  vehicle_id   VARCHAR(20)   PRIMARY KEY,
  model        VARCHAR(100)  NOT NULL,
  vehicle_type VARCHAR(50)   NOT NULL,
  daily_rate   DECIMAL(10,2) NOT NULL,
  status       VARCHAR(20)   NOT NULL DEFAULT 'Available'
) ENGINE=InnoDB;

-- ---------- BOOKINGS ----------
CREATE TABLE IF NOT EXISTS bookings (
  booking_id    VARCHAR(20)   PRIMARY KEY,
  customer_name VARCHAR(100)  NOT NULL,
  vehicle_name  VARCHAR(100)  NOT NULL,
  from_date     DATE          NOT NULL,
  to_date       DATE          NOT NULL,
  status        VARCHAR(20)   NOT NULL DEFAULT 'Pending',
  amount        DECIMAL(10,2) NOT NULL DEFAULT 0
) ENGINE=InnoDB;

-- ============================================================
-- SEED DATA
-- ============================================================

-- Default admin login: admin@drivehub.com / admin123
-- (the hash below is SHA-256 of "admin123")
INSERT INTO users (full_name, email, password_hash)
SELECT 'Administrator', 'admin@drivehub.com',
       '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9'
WHERE NOT EXISTS (SELECT 1 FROM users WHERE email = 'admin@drivehub.com');

INSERT INTO customers (customer_id, name, contact_number, address) VALUES
  ('CUS-1001', 'Juan Dela Cruz', '09171234567', 'Santa Rosa, Laguna'),
  ('CUS-1002', 'Maria Santos',   '09181234567', 'Binan, Laguna'),
  ('CUS-1003', 'Pedro Reyes',    '09201234567', 'Calamba, Laguna'),
  ('CUS-1004', 'Ana Lopez',      '09221234567', 'Cabuyao, Laguna'),
  ('CUS-1005', 'Carlos Tan',     '09351234567', 'Tagaytay, Cavite');

INSERT INTO vehicles (vehicle_id, model, vehicle_type, daily_rate, status) VALUES
  ('VH-001', 'Toyota Vios',       'Sedan',     1500.00, 'Available'),
  ('VH-002', 'Honda CR-V',        'SUV',       2500.00, 'Rented'),
  ('VH-003', 'Ford Ranger',       'Pickup',    3000.00, 'Available'),
  ('VH-004', 'Mitsubishi Mirage', 'Hatchback', 1200.00, 'Maintenance'),
  ('VH-005', 'Hyundai Starex',    'Van',       3500.00, 'Available');

INSERT INTO bookings (booking_id, customer_name, vehicle_name, from_date, to_date, status, amount) VALUES
  ('BK-1001', 'Juan Dela Cruz', 'Toyota Vios',       '2026-08-10', '2026-08-12', 'Completed', 3000.00),
  ('BK-1002', 'Maria Santos',   'Honda CR-V',        '2026-08-11', '2026-08-15', 'Active',    10000.00),
  ('BK-1003', 'Pedro Reyes',    'Ford Ranger',       '2026-08-13', '2026-08-14', 'Pending',   3000.00),
  ('BK-1004', 'Ana Lopez',      'Mitsubishi Mirage', '2026-08-05', '2026-08-07', 'Completed', 2400.00),
  ('BK-1005', 'Carlos Tan',     'Hyundai Starex',    '2026-08-14', '2026-08-20', 'Cancelled', 21000.00);