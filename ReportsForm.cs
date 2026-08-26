using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class ReportsForm : Form
    {
        private Panel lineChartPanel = null!;
        private Panel pieChartPanel = null!;
        private ListView transactionList = null!;

        private Label lblIncomeValue = null!;
        private Label lblRentalsValue = null!;
        private Label lblVehiclesValue = null!;

        public ReportsForm()
        {
            BookingData.LoadSamples();
            InitializeForm();
            RefreshReports();
        }

        private void InitializeForm()
        {
            Text = "DriveHub - Reports";
            Size = new Size(1100, 650);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 550);
            BackColor = Color.FromArgb(245, 246, 248);

            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 210,
                BackColor = Color.White
            };

            Controls.Add(sidebar);

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

            string[] navItems =
            {
                "Dashboard",
                "Vehicles",
                "Bookings",
                "Customers",
                "Reports"
            };

            int navY = 90;

            foreach (string item in navItems)
            {
                bool active = item == "Reports";

                Label navLabel = new Label
                {
                    Text = "  " + item,
                    Font = new Font(
                        "Segoe UI",
                        10F,
                        active ? FontStyle.Bold : FontStyle.Regular
                    ),
                    ForeColor = active
                        ? Color.FromArgb(99, 60, 220)
                        : Color.Black,
                    BackColor = active
                        ? Color.FromArgb(235, 230, 250)
                        : Color.White,
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

            Panel mainContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 246, 248)
            };

            Controls.Add(mainContent);
            mainContent.BringToFront();

            Label lblTitle = new Label
            {
                Text = "Reports",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(40, 30)
            };

            mainContent.Controls.Add(lblTitle);

            CreateSummaryCard(
                mainContent,
                "Total Income",
                "₱0",
                "All Bookings",
                Color.FromArgb(130, 220, 120),
                40,
                85,
                out lblIncomeValue
            );

            CreateSummaryCard(
                mainContent,
                "Total Rentals",
                "0",
                "All Bookings",
                Color.FromArgb(60, 120, 210),
                370,
                85,
                out lblRentalsValue
            );

            CreateSummaryCard(
                mainContent,
                "Available Vehicles",
                "10",
                "Today",
                Color.FromArgb(255, 125, 55),
                700,
                85,
                out lblVehiclesValue
            );

            lineChartPanel = new Panel
            {
                Location = new Point(48, 280),
                Size = new Size(550, 267),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lineChartPanel.Paint += LineChartPanel_Paint;
            mainContent.Controls.Add(lineChartPanel);

            pieChartPanel = new Panel
            {
                Location = new Point(643, 280),
                Size = new Size(340, 267),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pieChartPanel.Paint += PieChartPanel_Paint;
            mainContent.Controls.Add(pieChartPanel);

            Panel transactionsPanel = new Panel
            {
                Location = new Point(50, 565),
                Size = new Size(933, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainContent.Controls.Add(transactionsPanel);

            Label lblTransactions = new Label
            {
                Text = "Recent Transactions",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 12)
            };

            transactionsPanel.Controls.Add(lblTransactions);

            transactionList = new ListView
            {
                Location = new Point(0, 45),
                Size = new Size(931, 100),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None
            };

            transactionList.Columns.Add("Booking ID", 150);
            transactionList.Columns.Add("Customer", 155);
            transactionList.Columns.Add("Vehicle", 155);
            transactionList.Columns.Add("Date", 130);
            transactionList.Columns.Add("Amount", 130);
            transactionList.Columns.Add("Status", 150);

            transactionsPanel.Controls.Add(transactionList);
        }

        private void CreateSummaryCard(
            Panel parent,
            string title,
            string value,
            string subtitle,
            Color iconColor,
            int x,
            int y,
            out Label valueLabel)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(280, 168),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            parent.Controls.Add(card);

            Panel icon = new Panel
            {
                Location = new Point(35, 58),
                Size = new Size(50, 50),
                BackColor = iconColor
            };

            icon.Paint += (s, e) =>
            {
                using SolidBrush brush =
                    new SolidBrush(iconColor);

                e.Graphics.FillEllipse(
                    brush,
                    0,
                    0,
                    icon.Width - 1,
                    icon.Height - 1
                );
            };

            card.Controls.Add(icon);

            string symbol = title switch
            {
                "Total Income" => "₱",
                "Total Rentals" => "▣",
                "Available Vehicles" => "🚗",
                _ => ""
            };

            Label lblIcon = new Label
            {
                Text = symbol,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(50, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            icon.Controls.Add(lblIcon);

            Label lblCardTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9.5F),
                AutoSize = true,
                Location = new Point(110, 48)
            };

            card.Controls.Add(lblCardTitle);

            valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 18F),
                AutoSize = true,
                Location = new Point(110, 72)
            };

            card.Controls.Add(valueLabel);

            Label lblSubtitle = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(110, 112)
            };

            card.Controls.Add(lblSubtitle);
        }

        private void RefreshReports()
        {
            BookingData.LoadSamples();

            decimal income =
                BookingData.Bookings
                    .Where(x => x.Status != "Cancelled")
                    .Sum(x => x.Amount);

            int rentals =
                BookingData.Bookings.Count(
                    x => x.Status != "Cancelled"
                );

            lblIncomeValue.Text =
                "₱" + income.ToString("N0");

            lblRentalsValue.Text =
                rentals.ToString();

            transactionList.Items.Clear();

            foreach (BookingRecord booking in BookingData.Bookings
                .AsEnumerable()
                .Reverse()
                .Take(5))
            {
                string amount =
                    "₱" + booking.Amount.ToString("N0");

                AddTransaction(
                    booking.BookingId,
                    booking.CustomerName,
                    booking.VehicleName,
                    booking.FromDate,
                    amount,
                    booking.Status
                );
            }

            lineChartPanel.Invalidate();
            pieChartPanel.Invalidate();
        }

        private void AddTransaction(
            string bookingId,
            string customer,
            string vehicle,
            string date,
            string amount,
            string status)
        {
            ListViewItem item =
                new ListViewItem(bookingId);

            item.SubItems.Add(customer);
            item.SubItems.Add(vehicle);
            item.SubItems.Add(date);
            item.SubItems.Add(amount);
            item.SubItems.Add(status);

            if (status == "Cancelled")
            {
                item.SubItems[5].ForeColor = Color.Red;
            }
            else if (status == "Completed")
            {
                item.SubItems[5].ForeColor =
                    Color.FromArgb(40, 170, 90);
            }
            else if (status == "Active")
            {
                item.SubItems[5].ForeColor =
                    Color.FromArgb(50, 60, 200);
            }
            else
            {
                item.SubItems[5].ForeColor =
                    Color.FromArgb(230, 140, 30);
            }

            transactionList.Items.Add(item);
        }

        private void LineChartPanel_Paint(
            object? sender,
            PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using Font titleFont =
                new Font("Segoe UI", 9F, FontStyle.Bold);

            using Font axisFont =
                new Font("Segoe UI", 7.5F);

            using Pen gridPen =
                new Pen(Color.FromArgb(225, 225, 225));

            using Pen linePen =
                new Pen(Color.FromArgb(80, 125, 240), 2.5F);

            g.DrawString(
                "Monthly Rentals",
                titleFont,
                Brushes.Black,
                18,
                12
            );

            int left = 45;
            int top = 35;
            int right = 520;
            int bottom = 225;

            int total =
                BookingData.Bookings.Count(
                    x => x.Status != "Cancelled"
                );

            int max =
                Math.Max(5, total + 5);

            for (int i = 0; i <= 5; i++)
            {
                int y =
                    top +
                    ((bottom - top) * i / 5);

                g.DrawLine(
                    gridPen,
                    left,
                    y,
                    right,
                    y
                );

                int value =
                    max - ((max * i) / 5);

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

            for (int i = 0; i < values.Length; i++)
            {
                float x =
                    left +
                    ((right - left) * i /
                    (values.Length - 1));

                float y =
                    bottom -
                    ((Math.Min(values[i], max) /
                    (float)max) *
                    (bottom - top));

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
                    Color.FromArgb(80, 125, 240)
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

        private void PieChartPanel_Paint(
            object? sender,
            PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using Font titleFont =
                new Font("Segoe UI", 8F, FontStyle.Bold);

            g.DrawString(
                "Vehicle Rental Distribution",
                titleFont,
                Brushes.Black,
                15,
                10
            );

            Rectangle pieRect =
                new Rectangle(50, 55, 170, 170);

            var vehicleGroups =
                BookingData.Bookings
                    .Where(x => x.Status != "Cancelled")
                    .GroupBy(x => x.VehicleName)
                    .OrderByDescending(x => x.Count())
                    .Take(5)
                    .ToList();

            if (vehicleGroups.Count == 0)
                return;

            int total =
                vehicleGroups.Sum(x => x.Count());

            Color[] colors =
            {
                Color.FromArgb(80, 125, 240),
                Color.FromArgb(160, 125, 230),
                Color.FromArgb(240, 175, 95),
                Color.FromArgb(250, 215, 80),
                Color.FromArgb(225, 105, 115)
            };

            float startAngle = 0;

            for (int i = 0; i < vehicleGroups.Count; i++)
            {
                float percentage =
                    vehicleGroups[i].Count() /
                    (float)total;

                float sweepAngle =
                    percentage * 360f;

                using SolidBrush brush =
                    new SolidBrush(
                        colors[i % colors.Length]
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
                new Font("Segoe UI", 7F);

            for (int i = 0; i < vehicleGroups.Count; i++)
            {
                int x = 230;
                int y = 45 + (i * 32);

                using SolidBrush brush =
                    new SolidBrush(
                        colors[i % colors.Length]
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
                        vehicleGroups[i].Count() /
                        (double)total * 100
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

        private void NavLabel_Click(
            object? sender,
            EventArgs e)
        {
            if (sender is not Label lbl)
                return;

            string target =
                lbl.Tag?.ToString() ?? "";

            switch (target)
            {
                case "Dashboard":
                    new DashboardForm().Show();
                    Close();
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
                    break;
            }
        }

        private void LnkLogout_LinkClicked(
            object? sender,
            LinkLabelLinkClickedEventArgs e)
        {
            DialogResult confirm =
                MessageBox.Show(
                    "Are you sure you want to logout?",
                    "Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

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

                Close();
            }
        }
    }
}