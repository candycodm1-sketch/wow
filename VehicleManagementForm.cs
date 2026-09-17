using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class VehicleManagementForm : Form
    {
        private TextBox txtSearch = null!;
        private ListView vehicleList = null!;
        private Label lblPageCurrent = null!;
        private int currentPage = 1;
        private int totalPages = 2;

        private readonly Color PrimaryBlue = Color.FromArgb(48, 73, 181);
        private readonly Color SidebarActive = Color.FromArgb(235, 238, 250);

        public VehicleManagementForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "DriveHub - Vehicle Management";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 550);
            this.BackColor = Color.FromArgb(245, 246, 248);

            // =========================
            // SIDEBAR - SAME AS DASHBOARD
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
            // VEHICLES - ACTIVE
            // =========================

            AddNavigationItem(
                sidebar,
                "Vehicles",
                "vehicles logo for sidebar.png",
                184,
                true
            );

            // =========================
            // BOOKINGS
            // =========================

            AddNavigationItem(
                sidebar,
                "Bookings",
                "bookings logo for sidebar.png",
                256,
                false
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
                Text = "Vehicle Management",
                Font = new Font(
                    "Segoe UI",
                    16F,
                    FontStyle.Bold
                ),
                AutoSize = true,
                Location = new Point(30, 25)
            };

            mainContent.Controls.Add(lblTitle);

            Button btnAddVehicle = new Button
            {
                Text = "+ Add Vehicle",
                Font = new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                ),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 60, 200),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 36),
                Location = new Point(790, 25),
                Cursor = Cursors.Hand
            };

            btnAddVehicle.FlatAppearance.BorderSize = 0;
            btnAddVehicle.Click += BtnAddVehicle_Click;
            mainContent.Controls.Add(btnAddVehicle);

            txtSearch = new TextBox
            {
                Text = "Search Vehicle...",
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
                if (txtSearch.Text == "Search Vehicle...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Search Vehicle...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            txtSearch.TextChanged += TxtSearch_TextChanged;
            mainContent.Controls.Add(txtSearch);

            Panel tablePanel = new Panel
            {
                Location = new Point(30, 130),
                Size = new Size(920, 350),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainContent.Controls.Add(tablePanel);

            Label lblRecent = new Label
            {
                Text = "Recent Vehicles",
                Font = new Font(
                    "Segoe UI",
                    11F,
                    FontStyle.Bold
                ),
                AutoSize = true,
                Location = new Point(15, 12)
            };

            tablePanel.Controls.Add(lblRecent);

            LinkLabel lnkViewAll = new LinkLabel
            {
                Text = "View All",
                Font = new Font(
                    "Segoe UI",
                    9F
                ),
                AutoSize = true,
                LinkColor = PrimaryBlue,
                Location = new Point(850, 15)
            };

            tablePanel.Controls.Add(lnkViewAll);

            vehicleList = new ListView
            {
                Location = new Point(0, 45),
                Size = new Size(918, 300),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None
            };

            vehicleList.Columns.Add("Vehicle ID", 120);
            vehicleList.Columns.Add("Model", 180);
            vehicleList.Columns.Add("Type", 130);
            vehicleList.Columns.Add("Daily Rate", 130);
            vehicleList.Columns.Add("Status", 130);
            vehicleList.Columns.Add("Actions", 150);

            tablePanel.Controls.Add(vehicleList);

            vehicleList.DoubleClick += VehicleList_DoubleClick;
            vehicleList.MouseClick += VehicleList_MouseClick;
            vehicleList.MouseMove += VehicleList_MouseMove;

            RefreshVehicles();

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
                Font = new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold
                )
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

        // =========================
        // SIDEBAR ITEM
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

            itemPanel.Click += NavLabel_Click;
            icon.Click += NavLabel_Click;
            text.Click += NavLabel_Click;

            sidebar.Controls.Add(itemPanel);
        }

        // =========================
        // IMAGE LOADER
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
        // VEHICLE DATA
        // =========================

        private void RefreshVehicles()
        {
            vehicleList.Items.Clear();

            foreach (VehicleRecord vehicle in Database.GetVehicles())
            {
                AddVehicleRow(
                    vehicle.VehicleId,
                    vehicle.Model,
                    vehicle.VehicleType,
                    "₱" + vehicle.DailyRate.ToString("N0") + "/day",
                    vehicle.Status
                );
            }
        }

        private static decimal ParseDailyRate(
            string dailyRate)
        {
            System.Text.StringBuilder sb =
                new System.Text.StringBuilder();

            foreach (char c in dailyRate)
            {
                if (char.IsDigit(c) || c == '.')
                    sb.Append(c);
            }

            decimal.TryParse(
                sb.ToString(),
                out decimal rate
            );

            return rate;
        }

        private void AddVehicleRow(
            string id,
            string model,
            string type,
            string dailyRate,
            string status)
        {
            ListViewItem item =
                new ListViewItem(
                    new[]
                    {
                        id,
                        model,
                        type,
                        dailyRate,
                        status,
                        "Edit | Delete"
                    }
                );

            vehicleList.Items.Add(item);
        }

        // =========================
        // SEARCH
        // =========================

        private void TxtSearch_TextChanged(
            object? sender,
            EventArgs e)
        {
            string query =
                txtSearch.Text ==
                "Search Vehicle..."
                    ? ""
                    : txtSearch.Text
                        .Trim()
                        .ToLower();

            foreach (ListViewItem item
                     in vehicleList.Items)
            {
                bool match =
                    string.IsNullOrEmpty(query) ||
                    item.SubItems[0].Text
                        .ToLower()
                        .Contains(query) ||
                    item.SubItems[1].Text
                        .ToLower()
                        .Contains(query) ||
                    item.SubItems[2].Text
                        .ToLower()
                        .Contains(query);

                item.ForeColor =
                    match
                        ? Color.Black
                        : Color.LightGray;
            }
        }

        // =========================
        // ADD VEHICLE
        // =========================

        private void BtnAddVehicle_Click(
            object? sender,
            EventArgs e)
        {
            using AddVehicleDialog dialog =
                new AddVehicleDialog();

            if (dialog.ShowDialog() ==
                DialogResult.OK)
            {
                (bool ok, string message) =
                    Database.AddVehicle(
                        dialog.VehicleId,
                        dialog.Model,
                        dialog.VehicleType,
                        ParseDailyRate(
                            dialog.DailyRate
                        ),
                        "Available"
                    );

                if (ok)
                {
                    RefreshVehicles();

                    MessageBox.Show(
                        "Vehicle added successfully.",
                        "Vehicle Added",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        message,
                        "Add Vehicle Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        private const int ActionsColumnIndex = 5;

        // =========================
        // ACTION CURSOR
        // =========================

        private void VehicleList_MouseMove(
            object? sender,
            MouseEventArgs e)
        {
            ListViewHitTestInfo hit =
                vehicleList.HitTest(
                    e.Location
                );

            bool overActions =
                hit.Item != null &&
                hit.SubItem != null &&
                hit.Item.SubItems.IndexOf(
                    hit.SubItem
                ) == ActionsColumnIndex;

            vehicleList.Cursor =
                overActions
                    ? Cursors.Hand
                    : Cursors.Default;
        }

        // =========================
        // ACTION CLICK
        // =========================

        private void VehicleList_MouseClick(
            object? sender,
            MouseEventArgs e)
        {
            ListViewHitTestInfo hit =
                vehicleList.HitTest(
                    e.Location
                );

            if (hit.Item == null ||
                hit.SubItem == null)
            {
                return;
            }

            int columnIndex =
                hit.Item.SubItems.IndexOf(
                    hit.SubItem
                );

            if (columnIndex !=
                ActionsColumnIndex)
            {
                return;
            }

            Rectangle bounds =
                hit.SubItem.Bounds;

            int midpoint =
                bounds.Left +
                bounds.Width / 2;

            if (e.X <= midpoint)
            {
                EditVehicle(hit.Item);
            }
            else
            {
                DeleteVehicle(hit.Item);
            }
        }

        // =========================
        // EDIT VEHICLE
        // =========================

        private void EditVehicle(
            ListViewItem item)
        {
            using EditVehicleDialog dialog =
                new EditVehicleDialog(
                    item.SubItems[0].Text,
                    item.SubItems[1].Text,
                    item.SubItems[2].Text,
                    item.SubItems[3].Text,
                    item.SubItems[4].Text
                );

            if (dialog.ShowDialog() ==
                DialogResult.OK)
            {
                (bool ok, string message) =
                    Database.UpdateVehicle(
                        item.SubItems[0].Text,
                        dialog.Model,
                        dialog.VehicleType,
                        ParseDailyRate(
                            dialog.DailyRate
                        ),
                        dialog.Status
                    );

                if (ok)
                {
                    RefreshVehicles();

                    MessageBox.Show(
                        "Vehicle updated successfully.",
                        "Vehicle Updated",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        message,
                        "Update Vehicle Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        // =========================
        // DELETE VEHICLE
        // =========================

        private void DeleteVehicle(
            ListViewItem item)
        {
            string vehicleId =
                item.SubItems[0].Text;

            string vehicleModel =
                item.SubItems[1].Text;

            DialogResult confirm =
                MessageBox.Show(
                    $"Are you sure you want to delete {vehicleModel} ({vehicleId})?",
                    "Delete Vehicle",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

            if (confirm != DialogResult.Yes)
                return;

            (bool ok, string message) =
                Database.DeleteVehicle(
                    vehicleId
                );

            if (ok)
            {
                RefreshVehicles();

                MessageBox.Show(
                    $"{vehicleModel} has been deleted.",
                    "Vehicle Deleted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    message,
                    "Delete Vehicle Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        // =========================
        // DOUBLE CLICK
        // =========================

        private void VehicleList_DoubleClick(
            object? sender,
            EventArgs e)
        {
            if (vehicleList.SelectedItems.Count > 0)
            {
                ListViewItem item =
                    vehicleList.SelectedItems[0];

                MessageBox.Show(
                    $"Vehicle ID: {item.SubItems[0].Text}\n" +
                    $"Model: {item.SubItems[1].Text}\n" +
                    $"Type: {item.SubItems[2].Text}\n" +
                    $"Daily Rate: {item.SubItems[3].Text}\n" +
                    $"Status: {item.SubItems[4].Text}",
                    "Vehicle Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        // =========================
        // PAGINATION
        // =========================

        private void ChangePage(
            int direction)
        {
            int newPage =
                currentPage + direction;

            if (newPage < 1 ||
                newPage > totalPages)
            {
                return;
            }

            currentPage = newPage;

            lblPageCurrent.Text =
                currentPage.ToString();
        }

        private void GoToPage(
            int page)
        {
            if (page < 1 ||
                page > totalPages)
            {
                return;
            }

            currentPage = page;

            lblPageCurrent.Text =
                page.ToString();
        }

        // =========================
        // SIDEBAR NAVIGATION
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
                    break;

                case "Bookings":
                    new BookingManagementForm().Show();
                    this.Close();
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

            if (confirm != DialogResult.Yes)
                return;

            foreach (Form form
                     in Application.OpenForms)
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