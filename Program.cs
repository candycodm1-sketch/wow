using System;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Create the database, tables, and seed data on first launch
            // (safe to call on every start).
            (bool ok, string message) = Database.EnsureDatabase();

            if (!ok)
            {
                MessageBox.Show(
                    message,
                    "Database Unavailable",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            Application.Run(new Form1());
        }
    }
}
