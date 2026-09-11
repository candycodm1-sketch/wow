using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class BookingManagementForm : Form
    {
        private TextBox txtSearch = null!;
        private ListView bookingList = null!;
        private Label lblPageCurrent = null!;
        private ComboBox cmbStatusFilterRef = null!;
        private int currentPage = 1;
        private int totalPages = 2;

        public BookingManagementForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "DriveHub - Booking Management";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(900, 550);
            this.BackColor = Color.FromArgb(245, 246, 248);

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
                bool isActive = item == "Bookings";

                Label navLabel = new Label
                {
                    Text = "  " + item,
                    Font = new Font(
                        "Segoe UI",
                        10F,
                        isActive ? FontStyle.Bold : FontStyle.Regular
                    ),
                    ForeColor = isActive
                        ? Color.FromArgb(99, 60, 220)
                        : Color.Black,
                    BackColor = isActive
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
                BackColor = Color.FromArgb(245, 246, 248),
                Padding = new Padding(30, 25, 30, 25)
            };

            this.Controls.Add(mainContent);
            mainContent.BringToFront();

            Label lblTitle = new Label
            {
                Text = "Booking Management",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 25)
            };

            mainContent.Controls.Add(lblTitle);

            Button btnAddBooking = new Button
            {
                Text = "+ Add Booking",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 60, 200),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 36),
                Location = new Point(780, 25),
                Cursor = Cursors.Hand
            };

            btnAddBooking.FlatAppearance.BorderSize = 0;
            btnAddBooking.Click += BtnAddBooking_Click;
            mainContent.Controls.Add(btnAddBooking);

            txtSearch = new TextBox
            {
                Text = "Search Booking...",
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(30, 80),
                Size = new Size(300, 28),
                BorderStyle = BorderStyle.FixedSingle
            };

            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == "Search Booking...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Search Booking...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            txtSearch.TextChanged += TxtSearch_TextChanged;
            mainContent.Controls.Add(txtSearch);

            ComboBox cmbStatusFilter = new ComboBox
            {
                Location = new Point(345, 80),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };

            cmbStatusFilter.Items.AddRange(
                new object[]
                {
                    "All Statuses",
                    "Pending",
                    "Active",
                    "Completed",
                    "Cancelled"
                }
            );

            cmbStatusFilter.SelectedIndex = 0;

            cmbStatusFilter.SelectedIndexChanged +=
                (s, e) => TxtSearch_TextChanged(cmbStatusFilter, EventArgs.Empty);

            mainContent.Controls.Add(cmbStatusFilter);

            Panel tablePanel = new Panel
            {
                Location = new Point(30, 130),
                Size = new Size(920, 350),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainContent.Controls.Add(tablePanel);

            Label lblAllBookings = new Label
            {
                Text = "All Bookings",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 12)
            };

            tablePanel.Controls.Add(lblAllBookings);

            bookingList = new ListView
            {
                Location = new Point(0, 45),
                Size = new Size(918, 300),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None,
                HideSelection = false
            };

            bookingList.Columns.Add("Booking ID", 100);
            bookingList.Columns.Add("Customer", 160);
            bookingList.Columns.Add("Vehicle", 150);
            bookingList.Columns.Add("From", 110);
            bookingList.Columns.Add("To", 110);
            bookingList.Columns.Add("Status", 110);
            bookingList.Columns.Add("Actions", 150);

            tablePanel.Controls.Add(bookingList);

            bookingList.DoubleClick += BookingList_DoubleClick;
            bookingList.MouseClick += BookingList_MouseClick;
            bookingList.MouseMove += BookingList_MouseMove;

            this.cmbStatusFilterRef = cmbStatusFilter;

            RefreshBookings();

            Panel pagination = new Panel
            {
                Location = new Point(400, 500),
                Size = new Size(200, 40),
                BackColor = Color.Transparent
            };

            mainContent.Controls.Add(pagination);

            Button btnPrev = new Button
            {
                Text = "<",
                Size = new Size(32, 32),
                Location = new Point(0, 0),
                FlatStyle = FlatStyle.Flat
            };

            btnPrev.Click += (s, e) => ChangePage(-1);
            pagination.Controls.Add(btnPrev);

            lblPageCurrent = new Label
            {
                Text = currentPage.ToString(),
                Size = new Size(32, 32),
                Location = new Point(40, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(50, 60, 200),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            pagination.Controls.Add(lblPageCurrent);

            Label lblPage2 = new Label
            {
                Text = "2",
                Size = new Size(32, 32),
                Location = new Point(80, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            lblPage2.Click += (s, e) => GoToPage(2);
            pagination.Controls.Add(lblPage2);

            Button btnNext = new Button
            {
                Text = ">",
                Size = new Size(32, 32),
                Location = new Point(120, 0),
                FlatStyle = FlatStyle.Flat
            };

            btnNext.Click += (s, e) => ChangePage(1);
            pagination.Controls.Add(btnNext);
        }

        private void RefreshBookings()
        {
            bookingList.Items.Clear();

            foreach (BookingRecord booking in Database.GetBookings())
            {
                AddBookingRow(
                    booking.BookingId,
                    booking.CustomerName,
                    booking.VehicleName,
                    booking.FromDate,
                    booking.ToDate,
                    booking.Status
                );
            }
        }

        private void AddBookingRow(
            string id,
            string customer,
            string vehicle,
            string from,
            string to,
            string status)
        {
            ListViewItem item = new ListViewItem(
                new[]
                {
                    id,
                    customer,
                    vehicle,
                    from,
                    to,
                    status,
                    "View | Cancel"
                }
            );

            item.SubItems[5].ForeColor = status switch
            {
                "Completed" => Color.FromArgb(40, 170, 90),
                "Active" => Color.FromArgb(50, 60, 200),
                "Pending" => Color.FromArgb(230, 140, 30),
                "Cancelled" => Color.Red,
                _ => Color.Black
            };

            item.SubItems[6].ForeColor = Color.FromArgb(50, 60, 200);

            bookingList.Items.Add(item);
        }

        private void BookingList_MouseMove(
            object? sender,
            MouseEventArgs e)
        {
            ListViewHitTestInfo hit = bookingList.HitTest(e.Location);

            if (hit.Item != null && hit.SubItem != null)
            {
                int columnIndex = hit.Item.SubItems.IndexOf(hit.SubItem);

                if (columnIndex == 6)
                {
                    bookingList.Cursor = Cursors.Hand;
                    return;
                }
            }

            bookingList.Cursor = Cursors.Default;
        }

        private void BookingList_MouseClick(
            object? sender,
            MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            ListViewHitTestInfo hit = bookingList.HitTest(e.Location);

            if (hit.Item == null || hit.SubItem == null)
                return;

            int columnIndex = hit.Item.SubItems.IndexOf(hit.SubItem);

            if (columnIndex != 6)
                return;

            ListViewItem row = hit.Item;

            int subItemLeft = row.SubItems[6].Bounds.Left;
            int relativeX = e.X - subItemLeft;

            if (relativeX < 60)
            {
                MessageBox.Show(
                    $"Booking: {row.SubItems[0].Text}\n" +
                    $"Customer: {row.SubItems[1].Text}\n" +
                    $"Vehicle: {row.SubItems[2].Text}\n" +
                    $"From: {row.SubItems[3].Text}\n" +
                    $"To: {row.SubItems[4].Text}\n" +
                    $"Status: {row.SubItems[5].Text}",
                    "Booking Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            if (row.SubItems[5].Text == "Cancelled")
            {
                MessageBox.Show(
                    "This booking has already been cancelled.",
                    "Booking Cancelled",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to cancel booking {row.SubItems[0].Text}?",
                "Cancel Booking",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                (bool ok, string message) = Database.CancelBooking(
                    row.SubItems[0].Text
                );

                if (ok)
                {
                    RefreshBookings();

                    MessageBox.Show(
                        "Booking has been cancelled successfully.",
                        "Booking Cancelled",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        message,
                        "Cancel Booking Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        private void TxtSearch_TextChanged(
            object? sender,
            EventArgs e)
        {
            string query =
                txtSearch.Text == "Search Booking..."
                    ? ""
                    : txtSearch.Text.Trim().ToLower();

            string statusFilter =
                cmbStatusFilterRef?.SelectedItem?.ToString()
                ?? "All Statuses";

            foreach (ListViewItem item in bookingList.Items)
            {
                bool matchesText =
                    string.IsNullOrEmpty(query) ||
                    item.SubItems[0].Text.ToLower().Contains(query) ||
                    item.SubItems[1].Text.ToLower().Contains(query) ||
                    item.SubItems[2].Text.ToLower().Contains(query);

                bool matchesStatus =
                    statusFilter == "All Statuses" ||
                    item.SubItems[5].Text == statusFilter;

                bool visible =
                    matchesText && matchesStatus;

                item.ForeColor =
                    visible
                        ? Color.Black
                        : Color.LightGray;

                item.SubItems[5].ForeColor =
                    item.SubItems[5].Text switch
                    {
                        "Completed" => Color.FromArgb(40, 170, 90),
                        "Active" => Color.FromArgb(50, 60, 200),
                        "Pending" => Color.FromArgb(230, 140, 30),
                        "Cancelled" => Color.Red,
                        _ => Color.Black
                    };

                item.SubItems[6].ForeColor =
                    visible
                        ? Color.FromArgb(50, 60, 200)
                        : Color.LightGray;
            }
        }

        private void BtnAddBooking_Click(
            object? sender,
            EventArgs e)
        {
            using AddBookingDialog dialog =
                new AddBookingDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                (bool ok, string message) = Database.AddBooking(
                    dialog.BookingId,
                    dialog.CustomerName,
                    dialog.VehicleName,
                    dialog.FromDate,
                    dialog.ToDate,
                    "Pending",
                    dialog.ContactNumber,
                    dialog.Address
                );

                if (ok)
                {
                    RefreshBookings();

                    MessageBox.Show(
                        "Booking added successfully.",
                        "Booking Added",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        message,
                        "Add Booking Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        private void BookingList_DoubleClick(
            object? sender,
            EventArgs e)
        {
            if (bookingList.SelectedItems.Count > 0)
            {
                ListViewItem row =
                    bookingList.SelectedItems[0];

                MessageBox.Show(
                    $"Booking: {row.SubItems[0].Text}\n" +
                    $"Customer: {row.SubItems[1].Text}\n" +
                    $"Vehicle: {row.SubItems[2].Text}\n" +
                    $"From: {row.SubItems[3].Text}\n" +
                    $"To: {row.SubItems[4].Text}\n" +
                    $"Status: {row.SubItems[5].Text}",
                    "Booking Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void ChangePage(int direction)
        {
            int newPage = currentPage + direction;

            if (newPage < 1 || newPage > totalPages)
                return;

            currentPage = newPage;
            lblPageCurrent.Text = currentPage.ToString();
        }

        private void GoToPage(int page)
        {
            if (page < 1 || page > totalPages)
                return;

            currentPage = page;
            lblPageCurrent.Text = page.ToString();
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
                    this.Close();
                    break;

                case "Vehicles":
                    new VehicleManagementForm().Show();
                    this.Close();
                    break;

                case "Bookings":
                    break;

                case "Customers":
                    new CustomerManagementForm().Show();
                    this.Close();
                    break;

                case "Reports":
                    new ReportsForm().Show();
                    this.Close();
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

                this.Close();
            }
        }
    }
}