using System;
using System.Drawing;
using System.IO;
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

        private readonly Color PrimaryBlue = Color.FromArgb(48, 73, 181);
        private readonly Color SidebarActive = Color.FromArgb(235, 238, 250);

        public BookingManagementForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "DriveHub - Booking Management";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 550);
            this.BackColor = Color.FromArgb(245, 246, 248);

            // =========================
            // SIDEBAR
            // =========================

            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 270,
                BackColor = Color.White
            };

            this.Controls.Add(sidebar);

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
            // VEHICLE RENTAL SYSTEM
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
            // DASHBOARD
            // =========================

            AddNavigationItem(
                sidebar,
                "Dashboard",
                "dashboard logo for sidebar.png",
                112,
                false
            );

            // =========================
            // VEHICLES
            // =========================

            AddNavigationItem(
                sidebar,
                "Vehicles",
                "vehicles logo for sidebar.png",
                184,
                false
            );

            // =========================
            // BOOKINGS - ACTIVE
            // =========================

            AddNavigationItem(
                sidebar,
                "Bookings",
                "bookings logo for sidebar.png",
                256,
                true
            );

            // =========================
            // CUSTOMERS
            // =========================

            AddNavigationItem(
                sidebar,
                "Customers",
                "customer logo for sidebar.png",
                328,
                false
            );

            // =========================
            // REPORTS
            // =========================

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
                BackColor = Color.FromArgb(245, 246, 248),
                Padding = new Padding(30, 25, 30, 25)
            };

            this.Controls.Add(mainContent);
            mainContent.BringToFront();

            Label lblTitle = new Label
            {
                Text = "Booking Management",
                Font = new Font(
                    "Segoe UI",
                    16F,
                    FontStyle.Bold
                ),
                AutoSize = true,
                Location = new Point(30, 25)
            };

            mainContent.Controls.Add(lblTitle);

            Button btnAddBooking = new Button
            {
                Text = "+ Add Booking",
                Font = new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                ),
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
                Font = new Font(
                    "Segoe UI",
                    9.5F
                ),
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
                Font = new Font(
                    "Segoe UI",
                    9.5F
                )
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
                (s, e) =>
                    TxtSearch_TextChanged(
                        cmbStatusFilter,
                        EventArgs.Empty
                    );

            mainContent.Controls.Add(cmbStatusFilter);

            // =========================
            // RECENT BOOKINGS
            // =========================

            Panel recentPanel = new Panel
            {
                Location = new Point(30, 130),
                Size = new Size(920, 200),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainContent.Controls.Add(recentPanel);

            Label lblRecent = new Label
            {
                Text = "Recent Bookings",
                Font = new Font(
                    "Segoe UI",
                    11F,
                    FontStyle.Bold
                ),
                AutoSize = true,
                Location = new Point(15, 12)
            };

            recentPanel.Controls.Add(lblRecent);

            ListView recentList = new ListView
            {
                Location = new Point(0, 45),
                Size = new Size(918, 150),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle =
                    ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None
            };

            recentList.Columns.Add(
                "Booking ID",
                120
            );

            recentList.Columns.Add(
                "Customer",
                165
            );

            recentList.Columns.Add(
                "Vehicle",
                165
            );

            recentList.Columns.Add(
                "From",
                118
            );

            recentList.Columns.Add(
                "To",
                118
            );

            recentList.Columns.Add(
                "Status",
                142
            );

            recentPanel.Controls.Add(recentList);

            foreach (
                BookingRecord booking
                in Database.GetBookings(3)
            )
            {
                recentList.Items.Add(
                    new ListViewItem(
                        new[]
                        {
                            booking.BookingId,
                            booking.CustomerName,
                            booking.VehicleName,
                            booking.FromDate,
                            booking.ToDate,
                            booking.Status
                        }
                    )
                );
            }

            // =========================
            // ALL BOOKINGS
            // =========================

            Panel tablePanel = new Panel
            {
                Location = new Point(30, 345),
                Size = new Size(920, 230),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainContent.Controls.Add(tablePanel);

            Label lblAllBookings = new Label
            {
                Text = "All Bookings",
                Font = new Font(
                    "Segoe UI",
                    11F,
                    FontStyle.Bold
                ),
                AutoSize = true,
                Location = new Point(15, 12)
            };

            tablePanel.Controls.Add(lblAllBookings);

            bookingList = new ListView
            {
                Location = new Point(0, 45),
                Size = new Size(918, 180),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle =
                    ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None,
                HideSelection = false
            };

            bookingList.Columns.Add(
                "Booking ID",
                85
            );

            bookingList.Columns.Add(
                "Customer",
                135
            );

            bookingList.Columns.Add(
                "Vehicle",
                125
            );

            bookingList.Columns.Add(
                "From",
                90
            );

            bookingList.Columns.Add(
                "To",
                90
            );

            bookingList.Columns.Add(
                "Status",
                85
            );

            bookingList.Columns.Add(
                "Created At",
                115
            );

            bookingList.Columns.Add(
                "Processed By",
                95
            );

            bookingList.Columns.Add(
                "Actions",
                95
            );

            tablePanel.Controls.Add(bookingList);

            bookingList.DoubleClick +=
                BookingList_DoubleClick;

            bookingList.MouseClick +=
                BookingList_MouseClick;

            bookingList.MouseMove +=
                BookingList_MouseMove;

            this.cmbStatusFilterRef =
                cmbStatusFilter;

            RefreshBookings();

            // =========================
            // PAGINATION
            // =========================

            Panel pagination = new Panel
            {
                Location = new Point(400, 585),
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

            btnPrev.Click +=
                (s, e) => ChangePage(-1);

            pagination.Controls.Add(btnPrev);

            lblPageCurrent = new Label
            {
                Text = currentPage.ToString(),
                Size = new Size(32, 32),
                Location = new Point(40, 0),
                TextAlign =
                    ContentAlignment.MiddleCenter,
                BackColor =
                    Color.FromArgb(50, 60, 200),
                ForeColor = Color.White,
                Font = new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold
                )
            };

            pagination.Controls.Add(
                lblPageCurrent
            );

            Label lblPage2 = new Label
            {
                Text = "2",
                Size = new Size(32, 32),
                Location = new Point(80, 0),
                TextAlign =
                    ContentAlignment.MiddleCenter,
                BorderStyle =
                    BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            lblPage2.Click +=
                (s, e) => GoToPage(2);

            pagination.Controls.Add(lblPage2);

            Button btnNext = new Button
            {
                Text = ">",
                Size = new Size(32, 32),
                Location = new Point(120, 0),
                FlatStyle = FlatStyle.Flat
            };

            btnNext.Click +=
                (s, e) => ChangePage(1);

            pagination.Controls.Add(btnNext);
        }

        // =========================
        // SIDEBAR NAVIGATION ITEM
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

            PictureBox icon =
                CreateAssetPicture(
                    iconFile,
                    new Size(32, 32)
                );

            icon.Location =
                new Point(15, 6);

            icon.SizeMode =
                PictureBoxSizeMode.Zoom;

            icon.BackColor =
                Color.Transparent;

            icon.Cursor =
                Cursors.Hand;

            icon.Tag =
                title;

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
                TextAlign =
                    ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                Tag = title
            };

            itemPanel.Controls.Add(icon);
            itemPanel.Controls.Add(text);

            itemPanel.Click +=
                NavLabel_Click;

            icon.Click +=
                NavLabel_Click;

            text.Click +=
                NavLabel_Click;

            sidebar.Controls.Add(itemPanel);
        }

        // =========================
        // IMAGE LOADER
        // =========================

        private PictureBox CreateAssetPicture(
            string fileName,
            Size size)
        {
            PictureBox pictureBox =
                new PictureBox
                {
                    Size = size,
                    SizeMode =
                        PictureBoxSizeMode.Zoom,
                    BackColor =
                        Color.Transparent
                };

            string path =
                FindAsset(fileName);

            if (!string.IsNullOrEmpty(path))
            {
                using Image original =
                    Image.FromFile(path);

                pictureBox.Image =
                    new Bitmap(original);
            }

            return pictureBox;
        }

        private string FindAsset(
            string fileName)
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
                    Directory.GetParent(
                        current
                    );

                if (parent == null)
                {
                    break;
                }

                current =
                    parent.FullName;
            }

            return "";
        }

        // =========================
        // BOOKING FUNCTIONS
        // =========================

        private void RefreshBookings()
        {
            bookingList.Items.Clear();

            foreach (
                BookingRecord booking
                in Database.GetBookings()
            )
            {
                AddBookingRow(
                    booking.BookingId,
                    booking.CustomerName,
                    booking.VehicleName,
                    booking.FromDate,
                    booking.ToDate,
                    booking.Status,
                    booking.CreatedAt,
                    booking.ProcessedBy
                );
            }
        }

        private void AddBookingRow(
            string id,
            string customer,
            string vehicle,
            string from,
            string to,
            string status,
            string createdAt,
            string processedBy)
        {
            ListViewItem item =
                new ListViewItem(
                    new[]
                    {
                        id,
                        customer,
                        vehicle,
                        from,
                        to,
                        status,
                        createdAt,
                        processedBy,
                        "View | Cancel"
                    }
                );

            item.SubItems[5].ForeColor =
                status switch
                {
                    "Completed" =>
                        Color.FromArgb(
                            40,
                            170,
                            90
                        ),

                    "Active" =>
                        Color.FromArgb(
                            50,
                            60,
                            200
                        ),

                    "Pending" =>
                        Color.FromArgb(
                            230,
                            140,
                            30
                        ),

                    "Cancelled" =>
                        Color.Red,

                    _ =>
                        Color.Black
                };

            item.SubItems[8].ForeColor =
                Color.FromArgb(
                    50,
                    60,
                    200
                );

            bookingList.Items.Add(item);
        }

        private void BookingList_MouseMove(
            object? sender,
            MouseEventArgs e)
        {
            ListViewHitTestInfo hit =
                bookingList.HitTest(
                    e.Location
                );

            if (
                hit.Item != null &&
                hit.SubItem != null
            )
            {
                int columnIndex =
                    hit.Item.SubItems.IndexOf(
                        hit.SubItem
                    );

                if (columnIndex == 8)
                {
                    bookingList.Cursor =
                        Cursors.Hand;

                    return;
                }
            }

            bookingList.Cursor =
                Cursors.Default;
        }

        private void BookingList_MouseClick(
            object? sender,
            MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            ListViewHitTestInfo hit =
                bookingList.HitTest(
                    e.Location
                );

            if (
                hit.Item == null ||
                hit.SubItem == null
            )
                return;

            int columnIndex =
                hit.Item.SubItems.IndexOf(
                    hit.SubItem
                );

            if (columnIndex != 8)
                return;

            ListViewItem row =
                hit.Item;

            int subItemLeft =
                row.SubItems[8].Bounds.Left;

            int relativeX =
                e.X - subItemLeft;

            if (relativeX < 60)
            {
                MessageBox.Show(
                    $"Booking: {row.SubItems[0].Text}\n" +
                    $"Customer: {row.SubItems[1].Text}\n" +
                    $"Vehicle: {row.SubItems[2].Text}\n" +
                    $"From: {row.SubItems[3].Text}\n" +
                    $"To: {row.SubItems[4].Text}\n" +
                    $"Status: {row.SubItems[5].Text}\n" +
                    $"Created At: {row.SubItems[6].Text}\n" +
                    $"Processed By: {row.SubItems[7].Text}",
                    "Booking Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            if (
                row.SubItems[5].Text ==
                "Cancelled"
            )
            {
                MessageBox.Show(
                    "This booking has already been cancelled.",
                    "Booking Cancelled",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            DialogResult result =
                MessageBox.Show(
                    $"Are you sure you want to cancel booking {row.SubItems[0].Text}?",
                    "Cancel Booking",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

            if (result == DialogResult.Yes)
            {
                (
                    bool ok,
                    string message
                ) =
                    Database.CancelBooking(
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
                txtSearch.Text ==
                "Search Booking..."
                    ? ""
                    : txtSearch.Text
                        .Trim()
                        .ToLower();

            string statusFilter =
                cmbStatusFilterRef?
                    .SelectedItem?
                    .ToString()
                ?? "All Statuses";

            foreach (
                ListViewItem item
                in bookingList.Items
            )
            {
                bool matchesText =
                    string.IsNullOrEmpty(query) ||
                    item.SubItems[0].Text
                        .ToLower()
                        .Contains(query) ||
                    item.SubItems[1].Text
                        .ToLower()
                        .Contains(query) ||
                    item.SubItems[2].Text
                        .ToLower()
                        .Contains(query) ||
                    item.SubItems[6].Text
                        .ToLower()
                        .Contains(query) ||
                    item.SubItems[7].Text
                        .ToLower()
                        .Contains(query);

                bool matchesStatus =
                    statusFilter ==
                        "All Statuses" ||
                    item.SubItems[5].Text ==
                        statusFilter;

                bool visible =
                    matchesText &&
                    matchesStatus;

                item.ForeColor =
                    visible
                        ? Color.Black
                        : Color.LightGray;

                item.SubItems[5].ForeColor =
                    item.SubItems[5].Text switch
                    {
                        "Completed" =>
                            Color.FromArgb(
                                40,
                                170,
                                90
                            ),

                        "Active" =>
                            Color.FromArgb(
                                50,
                                60,
                                200
                            ),

                        "Pending" =>
                            Color.FromArgb(
                                230,
                                140,
                                30
                            ),

                        "Cancelled" =>
                            Color.Red,

                        _ =>
                            Color.Black
                    };
            }
        }

        private void BtnAddBooking_Click(
            object? sender,
            EventArgs e)
        {
            using AddBookingDialog dialog =
                new AddBookingDialog();

            if (
                dialog.ShowDialog() ==
                DialogResult.OK
            )
            {
                (
                    bool ok,
                    string message
                ) =
                    Database.AddBooking(
                        dialog.BookingId,
                        dialog.CustomerName,
                        dialog.VehicleName,
                        dialog.FromDate,
                        dialog.ToDate,
                        "Pending",
                        dialog.ContactNumber,
                        dialog.Address,
                        dialog.Email,
                        Database.CurrentUserName
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
            if (
                bookingList.SelectedItems.Count >
                0
            )
            {
                ListViewItem row =
                    bookingList.SelectedItems[0];

                MessageBox.Show(
                    $"Booking: {row.SubItems[0].Text}\n" +
                    $"Customer: {row.SubItems[1].Text}\n" +
                    $"Vehicle: {row.SubItems[2].Text}\n" +
                    $"From: {row.SubItems[3].Text}\n" +
                    $"To: {row.SubItems[4].Text}\n" +
                    $"Status: {row.SubItems[5].Text}\n" +
                    $"Created At: {row.SubItems[6].Text}\n" +
                    $"Processed By: {row.SubItems[7].Text}",
                    "Booking Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void ChangePage(
            int direction)
        {
            int newPage =
                currentPage + direction;

            if (
                newPage < 1 ||
                newPage > totalPages
            )
                return;

            currentPage =
                newPage;

            lblPageCurrent.Text =
                currentPage.ToString();
        }

        private void GoToPage(
            int page)
        {
            if (
                page < 1 ||
                page > totalPages
            )
                return;

            currentPage =
                page;

            lblPageCurrent.Text =
                page.ToString();
        }

        // =========================
        // NAVIGATION
        // =========================

        private void NavLabel_Click(
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

            if (
                confirm !=
                DialogResult.Yes
            )
                return;

            foreach (
                Form form
                in Application.OpenForms
            )
            {
                if (form is Form1 loginForm)
                {
                    loginForm.Show();
                    break;
                }
            }

            this.Close();
        }

        private void LnkLogout_LinkClicked(
            object? sender,
            LinkLabelLinkClickedEventArgs e)
        {
            Logout_Click(sender, e);
        }
    }
}