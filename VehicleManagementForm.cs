using System;
using System.Drawing;
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

        public VehicleManagementForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "DriveHub - Vehicle Management";
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
                bool isActive = item == "Vehicles";
                Label navLabel = new Label
                {
                    Text = "  " + item,
                    Font = new Font("Segoe UI", 10F, isActive ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = isActive ? Color.FromArgb(99, 60, 220) : Color.Black,
                    BackColor = isActive ? Color.FromArgb(235, 230, 250) : Color.White,
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

            Label lblTitle = new Label
            {
                Text = "Vehicle Management",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 25)
            };
            mainContent.Controls.Add(lblTitle);

            Button btnAddVehicle = new Button
            {
                Text = "+ Add Vehicle",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
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

            // Search box
            txtSearch = new TextBox
            {
                Text = "Search Vehicle...",
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9.5F),
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

            // Table panel
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
                Text = "Recent Bookings",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 12)
            };
            tablePanel.Controls.Add(lblRecent);

            LinkLabel lnkViewAll = new LinkLabel
            {
                Text = "View All",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                LinkColor = Color.FromArgb(99, 60, 220),
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

            LoadSampleVehicles();

            // ---------- PAGINATION ----------
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
            lblPage2.Click += (s, e) => GoToPage(2, lblPage2);
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

        private void LoadSampleVehicles()
        {
            vehicleList.Items.Clear();
            // Sample data — replace with real records from your database.
            vehicleList.Items.Add(new ListViewItem(new[] { "VH-001", "Toyota Vios", "Sedan", "₱1,500/day", "Available", "Edit | Delete" }));
            vehicleList.Items.Add(new ListViewItem(new[] { "VH-002", "Honda CR-V", "SUV", "₱2,500/day", "Rented", "Edit | Delete" }));
            vehicleList.Items.Add(new ListViewItem(new[] { "VH-003", "Ford Ranger", "Pickup", "₱3,000/day", "Available", "Edit | Delete" }));
            vehicleList.Items.Add(new ListViewItem(new[] { "VH-004", "Mitsubishi Mirage", "Hatchback", "₱1,200/day", "Maintenance", "Edit | Delete" }));
            vehicleList.Items.Add(new ListViewItem(new[] { "VH-005", "Hyundai Starex", "Van", "₱3,500/day", "Available", "Edit | Delete" }));
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            string query = txtSearch.Text == "Search Vehicle..." ? "" : txtSearch.Text.Trim().ToLower();

            foreach (ListViewItem item in vehicleList.Items)
            {
                bool match = string.IsNullOrEmpty(query) ||
                             item.SubItems[1].Text.ToLower().Contains(query) ||
                             item.SubItems[0].Text.ToLower().Contains(query) ||
                             item.SubItems[2].Text.ToLower().Contains(query);
                item.ForeColor = match ? Color.Black : Color.LightGray;
            }
        }

        private void BtnAddVehicle_Click(object? sender, EventArgs e)
        {
            using AddVehicleDialog dialog = new AddVehicleDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                vehicleList.Items.Add(new ListViewItem(new[]
                {
                    dialog.VehicleId, dialog.Model, dialog.VehicleType, dialog.DailyRate, "Available", "Edit | Delete"
                }));
            }
        }

        private const int ActionsColumnIndex = 5;

        private void VehicleList_MouseMove(object? sender, MouseEventArgs e)
        {
            var hit = vehicleList.HitTest(e.Location);
            bool overActions = hit.Item != null && hit.SubItem != null &&
                                hit.Item.SubItems.IndexOf(hit.SubItem) == ActionsColumnIndex;
            vehicleList.Cursor = overActions ? Cursors.Hand : Cursors.Default;
        }

        private void VehicleList_MouseClick(object? sender, MouseEventArgs e)
        {
            var hit = vehicleList.HitTest(e.Location);
            if (hit.Item == null || hit.SubItem == null) return;
            if (hit.Item.SubItems.IndexOf(hit.SubItem) != ActionsColumnIndex) return;

            // "Edit | Delete" text: left half of the cell = Edit, right half = Delete.
            Rectangle bounds = hit.SubItem.Bounds;
            int midpoint = bounds.Left + bounds.Width / 2;

            if (e.X <= midpoint)
            {
                EditVehicle(hit.Item);
            }
            else
            {
                DeleteVehicle(hit.Item);
            }
        }

        private void EditVehicle(ListViewItem item)
        {
            using EditVehicleDialog dialog = new EditVehicleDialog(
                item.SubItems[0].Text,
                item.SubItems[1].Text,
                item.SubItems[2].Text,
                item.SubItems[3].Text,
                item.SubItems[4].Text);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                item.SubItems[1].Text = dialog.Model;
                item.SubItems[2].Text = dialog.VehicleType;
                item.SubItems[3].Text = dialog.DailyRate;
                item.SubItems[4].Text = dialog.Status;

                // TODO: Persist this update to your database.
            }
        }

        private void DeleteVehicle(ListViewItem item)
        {
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete {item.SubItems[1].Text} ({item.SubItems[0].Text})?",
                "Delete Vehicle", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                vehicleList.Items.Remove(item);
                // TODO: Persist this deletion to your database.
            }
        }

        private void VehicleList_DoubleClick(object? sender, EventArgs e)
        {
            if (vehicleList.SelectedItems.Count > 0)
            {
                MessageBox.Show($"Vehicle details for {vehicleList.SelectedItems[0].Text} would open here.",
                    "Vehicle Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ChangePage(int direction)
        {
            int newPage = currentPage + direction;
            if (newPage < 1 || newPage > totalPages) return;
            currentPage = newPage;
            lblPageCurrent.Text = currentPage.ToString();
            // TODO: Load the actual data for this page from your database.
        }

        private void GoToPage(int page, Label clickedLabel)
        {
            currentPage = page;
            lblPageCurrent.Text = page.ToString();
            // TODO: Load the actual data for this page from your database.
        }

        private void NavLabel_Click(object? sender, EventArgs e)
        {
            if (sender is not Label lbl) return;
            string target = lbl.Tag?.ToString() ?? "";

            switch (target)
            {
                case "Dashboard":
                    new DashboardForm().Show();
                    this.Close();
                    break;
                case "Vehicles":
                    // Already here.
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
