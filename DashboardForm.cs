using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "DriveHub - Dashboard";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(900, 550);
            this.BackColor = Color.FromArgb(245, 246, 248);

            // ---------- SIDEBAR ----------
            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 210,
                BackColor = Color.White
            };
            this.Controls.Add(sidebar);

            Label lblLogo = new Label
            {
                Text = "🚗 DriveHub",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            sidebar.Controls.Add(lblLogo);

            Label lblLogoSub = new Label
            {
                Text = "Vehicle Rental System",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 45)
            };
            sidebar.Controls.Add(lblLogoSub);

            string[] navItems = { "Dashboard", "Vehicles", "Bookings", "Customers", "Reports" };
            int navY = 90;
            foreach (string item in navItems)
            {
                Label navLabel = new Label
                {
                    Text = "  " + item,
                    Font = new Font("Segoe UI", 10F, item == "Dashboard" ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = item == "Dashboard" ? Color.FromArgb(99, 60, 220) : Color.Black,
                    BackColor = item == "Dashboard" ? Color.FromArgb(235, 230, 250) : Color.White,
                    AutoSize = false,
                    Size = new Size(190, 36),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Location = new Point(10, navY),
                    Cursor = Cursors.Hand,
                    Tag = item
                };
                navLabel.Click += NavLabel_Click;
                sidebar.Controls.Add(navLabel);
                navY += 44;
            }

            LinkLabel lnkLogout = new LinkLabel
            {
                Text = "  Logout",
                Font = new Font("Segoe UI", 10F),
                LinkColor = Color.Red,
                ActiveLinkColor = Color.DarkRed,
                LinkBehavior = LinkBehavior.NeverUnderline,
                AutoSize = false,
                Size = new Size(190, 36),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(10, navY + 20)
            };
            lnkLogout.LinkClicked += LnkLogout_LinkClicked;
            sidebar.Controls.Add(lnkLogout);

            // ---------- MAIN CONTENT ----------
            Panel mainContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 246, 248),
                Padding = new Padding(30, 25, 30, 25)
            };
            this.Controls.Add(mainContent);
            mainContent.BringToFront();

            Label lblDashboardTitle = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 25)
            };
            mainContent.Controls.Add(lblDashboardTitle);

            Label lblAdmin = new Label
            {
                Text = "👤 Admin ▾",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(950, 30)
            };
            mainContent.Controls.Add(lblAdmin);

            // Stat cards
            AddStatCard(mainContent, "Total Vehicles", "15", Color.FromArgb(60, 90, 220), 30, 80);
            AddStatCard(mainContent, "Available Vehicles", "10", Color.FromArgb(40, 170, 90), 260, 80);
            AddStatCard(mainContent, "Active Rentals", "5", Color.FromArgb(230, 140, 30), 490, 80);
            AddStatCard(mainContent, "Total Customers", "12", Color.Black, 720, 80);

            // Recent bookings panel
            Panel bookingsPanel = new Panel
            {
                Location = new Point(30, 230),
                Size = new Size(920, 300),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainContent.Controls.Add(bookingsPanel);

            Label lblRecent = new Label
            {
                Text = "Recent Bookings",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 12)
            };
            bookingsPanel.Controls.Add(lblRecent);

            LinkLabel lnkViewAll = new LinkLabel
            {
                Text = "View All",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                LinkColor = Color.FromArgb(99, 60, 220),
                Location = new Point(850, 15)
            };
            bookingsPanel.Controls.Add(lnkViewAll);
            lnkViewAll.LinkClicked += (s, e) =>
            {
                new BookingManagementForm().Show();
                this.Close();
            };

            ListView bookingsList = new ListView
            {
                Location = new Point(0, 45),
                Size = new Size(918, 250),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None
            };
            bookingsList.Columns.Add("Booking ID", 130);
            bookingsList.Columns.Add("Customer", 180);
            bookingsList.Columns.Add("Vehicle", 180);
            bookingsList.Columns.Add("From", 130);
            bookingsList.Columns.Add("To", 130);
            bookingsList.Columns.Add("Status", 150);
            bookingsPanel.Controls.Add(bookingsList);

            // Sample rows — replace with real data from your database.
            bookingsList.Items.Add(new ListViewItem(new[] { "BK-1001", "Juan Dela Cruz", "Toyota Vios", "2026-08-10", "2026-08-12", "Completed" }));
            bookingsList.Items.Add(new ListViewItem(new[] { "BK-1002", "Maria Santos", "Honda CR-V", "2026-08-11", "2026-08-15", "Active" }));
            bookingsList.Items.Add(new ListViewItem(new[] { "BK-1003", "Pedro Reyes", "Ford Ranger", "2026-08-13", "2026-08-14", "Pending" }));
        }

        private void AddStatCard(Panel parent, string title, string value, Color accentColor, int x, int y)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(210, 120),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(card);

            Panel iconCircle = new Panel
            {
                Size = new Size(36, 36),
                Location = new Point(15, 15),
                BackColor = accentColor
            };
            iconCircle.Paint += (s, e) =>
            {
                using var brush = new SolidBrush(accentColor);
                e.Graphics.FillEllipse(brush, 0, 0, iconCircle.Width, iconCircle.Height);
            };
            card.Controls.Add(iconCircle);

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(15, 65)
            };
            card.Controls.Add(lblTitle);

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = accentColor,
                AutoSize = true,
                Location = new Point(15, 85)
            };
            card.Controls.Add(lblValue);
        }

        private void NavLabel_Click(object? sender, EventArgs e)
        {
            if (sender is not Label lbl) return;
            string target = lbl.Tag?.ToString() ?? "";

            switch (target)
            {
                case "Dashboard":
                    // Already here.
                    break;
                case "Vehicles":
                    new VehicleManagementForm().Show();
                    this.Close();
                    break;
                case "Bookings":
                    new BookingManagementForm().Show();
                    this.Close();
                    break;
                case "Customers":
                case "Reports":
                    MessageBox.Show($"{target} page is not built yet.", "Coming Soon",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void LnkLogout_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f is Form1 loginForm)
                    {
                        loginForm.Show();
                        break;
                    }
                }
                this.Close();
            }
        }
    }
}
