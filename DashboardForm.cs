using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class DashboardForm : Form
    {
        private Panel lineChartPanel = null!;
        private Panel pieChartPanel = null!;

        private readonly Color PrimaryBlue = Color.FromArgb(48, 73, 181);
        private readonly Color BackgroundColor = Color.FromArgb(245, 246, 248);
        private readonly Color SidebarActive = Color.FromArgb(235, 238, 250);

        public DashboardForm()
        {
            BookingData.Refresh();
            InitializeForm();
        }

        private void InitializeForm()
        {
            Text = "DriveHub - Dashboard";
            Size = new Size(1100, 650);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1000, 550);
            BackColor = BackgroundColor;

            // =========================
            // SIDEBAR
            // =========================

            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 270,
                BackColor = Color.White
            };

            Controls.Add(sidebar);

            // =========================
            // DRIVEHUB LOGO
            // =========================

            PictureBox logoIcon = CreateAssetPicture(
                "vehicle logo.png",
                new Size(40, 40)
            );

            logoIcon.Location = new Point(13, 21);
            logoIcon.SizeMode = PictureBoxSizeMode.Zoom;
            logoIcon.BackColor = Color.Transparent;

            sidebar.Controls.Add(logoIcon);

            // =========================
            // DRIVEHUB TEXT
            // =========================

            Panel driveHubPanel = new Panel
            {
                Location = new Point(63, 20),
                Size = new Size(180, 25),
                BackColor = Color.Transparent
            };

            driveHubPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode =
                    System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using Font font = new Font(
                    "Segoe UI",
                    12F,
                    FontStyle.Bold
                );

                using SolidBrush driveBrush =
                    new SolidBrush(Color.Black);

                using SolidBrush hubBrush =
                    new SolidBrush(PrimaryBlue);

                string driveText = "Drive";
                string hubText = "Hub";

                SizeF driveSize =
                    e.Graphics.MeasureString(
                        driveText,
                        font
                    );

                e.Graphics.DrawString(
                    driveText,
                    font,
                    driveBrush,
                    0,
                    0
                );

                e.Graphics.DrawString(
                    hubText,
                    font,
                    hubBrush,
                    driveSize.Width - 1,
                    0
                );
            };

            sidebar.Controls.Add(driveHubPanel);

            // =========================
            // SUBTITLE
            // =========================

            Label lblLogoSub = new Label
            {
                Text = "Vehicle Rental System",
                Font = new Font(
                    "Segoe UI",
                    8F
                ),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(63, 48),
                BackColor = Color.Transparent
            };

            sidebar.Controls.Add(lblLogoSub);

            // =========================
            // SIDEBAR ITEMS
            // =========================

            AddNavigationItem(
                sidebar,
                "Dashboard",
                "dashboard logo for sidebar.png",
                112,
                true
            );

            AddNavigationItem(
                sidebar,
                "Vehicles",
                "vehicles logo for sidebar.png",
                184,
                false
            );

            AddNavigationItem(
                sidebar,
                "Bookings",
                "bookings logo for sidebar.png",
                256,
                false
            );

            AddNavigationItem(
                sidebar,
                "Customers",
                "customer logo for sidebar.png",
                328,
                false
            );

            AddNavigationItem(
                sidebar,
                "Reports",
                "reports logo for sidebar.png",
                400,
                false
            );

            // =========================
            // LOGOUT
            // =========================

            Panel logoutPanel = new Panel
            {
                Location = new Point(9, 472),
                Size = new Size(233, 45),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = "Logout"
            };

            PictureBox logoutIcon = CreateAssetPicture(
                "logout logo for sidebar.png",
                new Size(32, 32)
            );

            logoutIcon.Location = new Point(15, 6);
            logoutIcon.SizeMode = PictureBoxSizeMode.Zoom;
            logoutIcon.Cursor = Cursors.Hand;
            logoutIcon.Tag = "Logout";

            Label logoutText = new Label
            {
                Text = "Logout",
                Font = new Font(
                    "Segoe UI",
                    10F
                ),
                ForeColor = Color.Red,
                AutoSize = false,
                Size = new Size(140, 45),
                Location = new Point(81, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent,
                Tag = "Logout"
            };

            logoutPanel.Controls.Add(logoutIcon);
            logoutPanel.Controls.Add(logoutText);

            logoutPanel.Click += Logout_Click;
            logoutIcon.Click += Logout_Click;
            logoutText.Click += Logout_Click;

            sidebar.Controls.Add(logoutPanel);

            // =========================
            // MAIN CONTENT
            // =========================

            Panel mainContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor
            };

            Controls.Add(mainContent);
            mainContent.BringToFront();

            // =========================
            // DASHBOARD TITLE
            // =========================

            Label lblDashboardTitle = new Label
            {
                Text = "Dashboard",
                Font = new Font(
                    "Segoe UI",
                    16F,
                    FontStyle.Bold
                ),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(76, 25)
            };

            mainContent.Controls.Add(lblDashboardTitle);

            // =========================
            // ADMIN
            // =========================

            Label lblAdmin = new Label
            {
                Text = "Admin ▾",
                Font = new Font(
                    "Segoe UI",
                    10F
                ),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(772, 30)
            };

            mainContent.Controls.Add(lblAdmin);

            // =========================
            // DATABASE COUNTS
            // =========================

            int totalVehicles =
                Database.CountVehicles();

            int availableVehicles =
                Database.CountAvailableVehicles();

            int activeRentals =
                Database.CountActiveBookings();

            int totalCustomers =
                Database.CountCustomers();

            // =========================
            // STAT CARDS
            // =========================

            AddStatCard(
                mainContent,
                "Total Vehicles",
                totalVehicles.ToString(),
                "vehicle logo.png",
                Color.FromArgb(60, 90, 220),
                79,
                80
            );

            AddStatCard(
                mainContent,
                "Available Vehicles",
                availableVehicles.ToString(),
                "checkmark.png",
                Color.FromArgb(40, 170, 90),
                289,
                80
            );

            AddStatCard(
                mainContent,
                "Active Rentals",
                activeRentals.ToString(),
                "rentals.png",
                Color.FromArgb(230, 140, 30),
                499,
                80
            );

            AddStatCard(
                mainContent,
                "Total Customers",
                totalCustomers.ToString(),
                "customers.png",
                Color.FromArgb(60, 60, 70),
                709,
                80
            );

            // =========================
            // MONTHLY RENTALS
            // =========================

            lineChartPanel = new Panel
            {
                Location = new Point(79, 225),
                Size = new Size(480, 320),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lineChartPanel.Paint += LineChartPanel_Paint;

            mainContent.Controls.Add(lineChartPanel);

            // =========================
            // VEHICLE DISTRIBUTION
            // =========================

            pieChartPanel = new Panel
            {
                Location = new Point(574, 225),
                Size = new Size(330, 320),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pieChartPanel.Paint += PieChartPanel_Paint;

            mainContent.Controls.Add(pieChartPanel);
        }

        // =========================
        // SIDEBAR NAVIGATION
        // =========================

        private void AddNavigationItem(
            Panel sidebar,
            string title,
            string iconFile,
            int y,
            bool active)
        {
            Panel itemPanel = new Panel
            {
                Location = new Point(9, y),
                Size = new Size(233, 45),
                BackColor = active
                    ? SidebarActive
                    : Color.White,
                Cursor = Cursors.Hand,
                Tag = title
            };

            PictureBox icon = CreateAssetPicture(
                iconFile,
                new Size(32, 32)
            );

            icon.Location = new Point(15, 6);
            icon.SizeMode = PictureBoxSizeMode.Zoom;
            icon.BackColor = Color.Transparent;
            icon.Cursor = Cursors.Hand;
            icon.Tag = title;

            Label text = new Label
            {
                Text = title,
                Font = new Font(
                    "Segoe UI",
                    10F,
                    active
                        ? FontStyle.Bold
                        : FontStyle.Regular
                ),
                ForeColor = active
                    ? PrimaryBlue
                    : Color.Black,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(145, 45),
                Location = new Point(81, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                Tag = title
            };

            itemPanel.Controls.Add(icon);
            itemPanel.Controls.Add(text);

            itemPanel.Click += Navigation_Click;
            icon.Click += Navigation_Click;
            text.Click += Navigation_Click;

            sidebar.Controls.Add(itemPanel);
        }

        // =========================
        // NAVIGATION CLICK
        // =========================

        private void Navigation_Click(
            object? sender,
            EventArgs e)
        {
            if (sender is not Control control)
                return;

            string target =
                control.Tag?.ToString() ?? "";

            switch (target)
            {
                case "Dashboard":
                    break;

                case "Vehicles":
                    new VehicleManagementForm().Show();
                    Close();
                    break;

                case "Bookings":
                    new BookingManagementForm().Show();
                    Close();
                    break;

                case "Customers":
                    new CustomerManagementForm().Show();
                    Close();
                    break;

                case "Reports":
                    new ReportsForm().Show();
                    Close();
                    break;
            }
        }

        // =========================
        // STAT CARD
        // =========================

        private void AddStatCard(
            Panel parent,
            string title,
            string value,
            string iconFile,
            Color accentColor,
            int x,
            int y)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(200, 120),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            parent.Controls.Add(card);

            PictureBox icon = CreateAssetPicture(
                iconFile,
                new Size(34, 34)
            );

            icon.Location = new Point(12, 16);
            icon.SizeMode = PictureBoxSizeMode.Zoom;
            icon.BackColor = Color.Transparent;

            card.Controls.Add(icon);

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font(
                    "Segoe UI",
                    8.5F
                ),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(12, 62)
            };

            card.Controls.Add(lblTitle);

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font(
                    "Segoe UI",
                    15F,
                    FontStyle.Bold
                ),
                ForeColor = accentColor,
                AutoSize = true,
                Location = new Point(12, 83)
            };

            card.Controls.Add(lblValue);
        }

        // =========================
        // CREATE ASSET IMAGE
        // =========================

        private PictureBox CreateAssetPicture(
            string fileName,
            Size size)
        {
            PictureBox pictureBox = new PictureBox
            {
                Size = size,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            string path = FindAsset(fileName);

            if (!string.IsNullOrEmpty(path))
            {
                using Image original =
                    Image.FromFile(path);

                pictureBox.Image =
                    new Bitmap(original);
            }

            return pictureBox;
        }

        // =========================
        // FIND ASSET
        // =========================

        private string FindAsset(string fileName)
        {
            string current =
                Application.StartupPath;

            for (int i = 0; i < 10; i++)
            {
                string path =
                    Path.Combine(
                        current,
                        "Assets",
                        fileName
                    );

                if (File.Exists(path))
                {
                    return path;
                }

                DirectoryInfo? parent =
                    Directory.GetParent(current);

                if (parent == null)
                {
                    break;
                }

                current = parent.FullName;
            }

            return "";
        }

        // =========================
        // MONTHLY RENTALS CHART
        // =========================

        private void LineChartPanel_Paint(
            object? sender,
            PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using Font titleFont =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold
                );

            using Font axisFont =
                new Font(
                    "Segoe UI",
                    7.5F
                );

            using Pen gridPen =
                new Pen(
                    Color.FromArgb(
                        225,
                        225,
                        225
                    )
                );

            using Pen linePen =
                new Pen(
                    Color.FromArgb(
                        80,
                        125,
                        240
                    ),
                    2.5F
                );

            g.DrawString(
                "Monthly Rentals",
                titleFont,
                Brushes.Black,
                18,
                12
            );

            int left = 45;
            int top = 35;
            int right =
                lineChartPanel.Width - 28;
            int bottom =
                lineChartPanel.Height - 40;

            int total =
                BookingData.Bookings.Count(
                    x => x.Status != "Cancelled"
                );

            int max =
                Math.Max(
                    5,
                    total + 5
                );

            for (int i = 0; i <= 5; i++)
            {
                int y =
                    top +
                    (
                        (bottom - top)
                        * i
                        / 5
                    );

                g.DrawLine(
                    gridPen,
                    left,
                    y,
                    right,
                    y
                );

                int value =
                    max -
                    (
                        max * i / 5
                    );

                g.DrawString(
                    value.ToString(),
                    axisFont,
                    Brushes.Black,
                    10,
                    y - 7
                );
            }

            string[] months =
            {
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "May",
                "Jun"
            };

            int[] values =
            {
                10,
                18,
                19,
                21,
                20,
                Math.Max(1, total)
            };

            PointF[] points =
                new PointF[values.Length];

            for (
                int i = 0;
                i < values.Length;
                i++)
            {
                float x =
                    left +
                    (
                        (right - left)
                        * i
                        /
                        (values.Length - 1)
                    );

                float y =
                    bottom -
                    (
                        Math.Min(
                            values[i],
                            max
                        )
                        /
                        (float)max
                    )
                    *
                    (bottom - top);

                points[i] =
                    new PointF(x, y);

                g.DrawString(
                    months[i],
                    axisFont,
                    Brushes.Black,
                    x - 10,
                    bottom + 8
                );
            }

            if (points.Length > 1)
            {
                g.DrawLines(
                    linePen,
                    points
                );
            }

            using SolidBrush pointBrush =
                new SolidBrush(
                    Color.FromArgb(
                        80,
                        125,
                        240
                    )
                );

            foreach (PointF point in points)
            {
                g.FillEllipse(
                    pointBrush,
                    point.X - 3,
                    point.Y - 3,
                    6,
                    6
                );
            }
        }

        // =========================
        // PIE CHART
        // =========================

        private void PieChartPanel_Paint(
            object? sender,
            PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using Font titleFont =
                new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Bold
                );

            g.DrawString(
                "Vehicle Rental Distribution",
                titleFont,
                Brushes.Black,
                15,
                10
            );

            Rectangle pieRect =
                new Rectangle(
                    50,
                    55,
                    170,
                    170
                );

            var vehicleGroups =
                BookingData.Bookings
                    .Where(
                        x => x.Status != "Cancelled"
                    )
                    .GroupBy(
                        x => x.VehicleName
                    )
                    .OrderByDescending(
                        x => x.Count()
                    )
                    .Take(5)
                    .ToList();

            if (vehicleGroups.Count == 0)
            {
                return;
            }

            int total =
                vehicleGroups.Sum(
                    x => x.Count()
                );

            Color[] colors =
            {
                Color.FromArgb(
                    80,
                    125,
                    240
                ),

                Color.FromArgb(
                    160,
                    125,
                    230
                ),

                Color.FromArgb(
                    240,
                    175,
                    95
                ),

                Color.FromArgb(
                    250,
                    215,
                    80
                ),

                Color.FromArgb(
                    225,
                    105,
                    115
                )
            };

            float startAngle = 0;

            for (
                int i = 0;
                i < vehicleGroups.Count;
                i++)
            {
                float percentage =
                    vehicleGroups[i].Count()
                    /
                    (float)total;

                float sweepAngle =
                    percentage * 360f;

                using SolidBrush brush =
                    new SolidBrush(
                        colors[
                            i % colors.Length
                        ]
                    );

                g.FillPie(
                    brush,
                    pieRect,
                    startAngle,
                    sweepAngle
                );

                startAngle += sweepAngle;
            }

            using Font legendFont =
                new Font(
                    "Segoe UI",
                    7F
                );

            for (
                int i = 0;
                i < vehicleGroups.Count;
                i++)
            {
                int x = 230;
                int y = 45 + (i * 32);

                using SolidBrush brush =
                    new SolidBrush(
                        colors[
                            i % colors.Length
                        ]
                    );

                g.FillEllipse(
                    brush,
                    x,
                    y,
                    8,
                    8
                );

                string vehicle =
                    vehicleGroups[i].Key;

                int percentage =
                    (int)Math.Round(
                        vehicleGroups[i].Count()
                        /
                        (double)total
                        * 100
                    );

                g.DrawString(
                    vehicle,
                    legendFont,
                    Brushes.Black,
                    x + 13,
                    y - 2
                );

                g.DrawString(
                    percentage + "%",
                    legendFont,
                    Brushes.Black,
                    x + 13,
                    y + 11
                );
            }
        }

        // =========================
        // LOGOUT
        // =========================

        private void Logout_Click(
            object? sender,
            EventArgs e)
        {
            DialogResult confirm =
                MessageBox.Show(
                    "Are you sure you want to logout?",
                    "Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            foreach (Form form in Application.OpenForms)
            {
                if (form is Form1 loginForm)
                {
                    loginForm.Show();
                    break;
                }
            }

            Close();
        }
    }
}