using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class CustomerManagementForm : Form
    {
        private TextBox txtSearch = null!;
        private ListView customerList = null!;
        private Label lblPageCurrent = null!;
        private int currentPage = 1;
        private int totalPages = 2;

        private readonly Color PrimaryBlue = Color.FromArgb(48, 73, 181);
        private readonly Color BackgroundColor = Color.FromArgb(245, 246, 248);
        private readonly Color SidebarActive = Color.FromArgb(235, 238, 250);

        public CustomerManagementForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            Text = "DriveHub - Customer Management";
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
                false
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
                true
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
            // TITLE
            // =========================

            Label lblTitle = new Label
            {
                Text = "Customer Management",
                Font = new Font(
                    "Segoe UI",
                    16F,
                    FontStyle.Bold
                ),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(76, 25)
            };

            mainContent.Controls.Add(lblTitle);

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
            // ADD CUSTOMER
            // =========================

            Button btnAddCustomer = new Button
            {
                Text = "+ Add Customer",
                Font = new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                ),
                ForeColor = Color.White,
                BackColor = PrimaryBlue,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 36),
                Location = new Point(780, 65),
                Cursor = Cursors.Hand
            };

            btnAddCustomer.FlatAppearance.BorderSize = 0;
            btnAddCustomer.Click += BtnAddCustomer_Click;

            mainContent.Controls.Add(btnAddCustomer);

            // =========================
            // SEARCH
            // =========================

            txtSearch = new TextBox
            {
                Text = "Search Customer...",
                ForeColor = Color.Gray,
                Font = new Font(
                    "Segoe UI",
                    9.5F
                ),
                Location = new Point(76, 75),
                Size = new Size(300, 28),
                BorderStyle = BorderStyle.FixedSingle
            };

            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == "Search Customer...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Search Customer...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };

            txtSearch.TextChanged += TxtSearch_TextChanged;

            mainContent.Controls.Add(txtSearch);

            // =========================
            // TABLE PANEL
            // =========================

            Panel tablePanel = new Panel
            {
                Location = new Point(76, 125),
                Size = new Size(828, 350),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainContent.Controls.Add(tablePanel);

            Label lblAllCustomers = new Label
            {
                Text = "All Customers",
                Font = new Font(
                    "Segoe UI",
                    11F,
                    FontStyle.Bold
                ),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(15, 12)
            };

            tablePanel.Controls.Add(lblAllCustomers);

            // =========================
            // CUSTOMER LIST
            // =========================

            customerList = new ListView
            {
                Location = new Point(0, 45),
                Size = new Size(826, 300),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None,
                HideSelection = false
            };

            customerList.Columns.Add(
                "Customer ID",
                90
            );

            customerList.Columns.Add(
                "Customer Name",
                145
            );

            customerList.Columns.Add(
                "Email",
                155
            );

            customerList.Columns.Add(
                "Contact Number",
                115
            );

            customerList.Columns.Add(
                "Address",
                160
            );

            customerList.Columns.Add(
                "Actions",
                150
            );

            tablePanel.Controls.Add(customerList);

            customerList.DoubleClick += CustomerList_DoubleClick;
            customerList.MouseClick += CustomerList_MouseClick;
            customerList.MouseMove += CustomerList_MouseMove;

            RefreshCustomers();

            // =========================
            // PAGINATION
            // =========================

            Panel pagination = new Panel
            {
                Location = new Point(390, 495),
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

            btnPrev.FlatAppearance.BorderColor =
                Color.LightGray;

            btnPrev.Click += (s, e) =>
                ChangePage(-1);

            pagination.Controls.Add(btnPrev);

            lblPageCurrent = new Label
            {
                Text = currentPage.ToString(),
                Size = new Size(32, 32),
                Location = new Point(40, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = PrimaryBlue,
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

            lblPage2.Click += (s, e) =>
                GoToPage(2);

            pagination.Controls.Add(lblPage2);

            Button btnNext = new Button
            {
                Text = ">",
                Size = new Size(32, 32),
                Location = new Point(120, 0),
                FlatStyle = FlatStyle.Flat
            };

            btnNext.FlatAppearance.BorderColor =
                Color.LightGray;

            btnNext.Click += (s, e) =>
                ChangePage(1);

            pagination.Controls.Add(btnNext);
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
        // NAVIGATION
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
                    break;

                case "Reports":
                    new ReportsForm().Show();
                    Close();
                    break;
            }
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
        // CUSTOMER DATA
        // =========================

        private void RefreshCustomers()
        {
            customerList.Items.Clear();

            foreach (
                CustomerRecord customer
                in Database.GetCustomers())
            {
                AddCustomerRow(
                    customer.CustomerId,
                    customer.CustomerName,
                    customer.Email,
                    customer.ContactNumber,
                    customer.Address
                );
            }
        }

        private void AddCustomerRow(
            string id,
            string name,
            string email,
            string contact,
            string address)
        {
            ListViewItem item =
                new ListViewItem(
                    new[]
                    {
                        id,
                        name,
                        email,
                        contact,
                        address,
                        "View | Edit | Delete"
                    }
                );

            item.SubItems[5].ForeColor =
                PrimaryBlue;

            customerList.Items.Add(item);
        }

        // =========================
        // CUSTOMER MOUSE MOVE
        // =========================

        private void CustomerList_MouseMove(
            object? sender,
            MouseEventArgs e)
        {
            ListViewHitTestInfo hit =
                customerList.HitTest(e.Location);

            if (
                hit.Item != null &&
                hit.SubItem != null)
            {
                int columnIndex =
                    hit.Item.SubItems.IndexOf(
                        hit.SubItem
                    );

                if (columnIndex == 5)
                {
                    customerList.Cursor =
                        Cursors.Hand;

                    return;
                }
            }

            customerList.Cursor =
                Cursors.Default;
        }

        // =========================
        // CUSTOMER MOUSE CLICK
        // =========================

        private void CustomerList_MouseClick(
            object? sender,
            MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            ListViewHitTestInfo hit =
                customerList.HitTest(e.Location);

            if (
                hit.Item == null ||
                hit.SubItem == null)
            {
                return;
            }

            int columnIndex =
                hit.Item.SubItems.IndexOf(
                    hit.SubItem
                );

            if (columnIndex != 5)
                return;

            ListViewItem row = hit.Item;

            int actionColumnLeft =
                row.SubItems[5].Bounds.Left;

            int relativeX =
                e.X - actionColumnLeft;

            // VIEW
            if (relativeX < 55)
            {
                MessageBox.Show(
                    $"Customer ID: {row.SubItems[0].Text}\n" +
                    $"Name: {row.SubItems[1].Text}\n" +
                    $"Email: {row.SubItems[2].Text}\n" +
                    $"Contact: {row.SubItems[3].Text}\n" +
                    $"Address: {row.SubItems[4].Text}",
                    "Customer Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            // EDIT
            if (relativeX < 118)
            {
                using AddCustomerDialog dialog =
                    new AddCustomerDialog(
                        row.SubItems[0].Text,
                        row.SubItems[1].Text,
                        row.SubItems[2].Text,
                        row.SubItems[3].Text,
                        row.SubItems[4].Text
                    );

                if (
                    dialog.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    (
                        bool ok,
                        string message
                    ) =
                        Database.UpdateCustomer(
                            dialog.CustomerId,
                            dialog.CustomerName,
                            dialog.Email,
                            dialog.ContactNumber,
                            dialog.Address
                        );

                    if (ok)
                    {
                        RefreshCustomers();

                        MessageBox.Show(
                            "Customer updated successfully.",
                            "Customer Updated",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            message,
                            "Update Customer Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                }

                return;
            }

            // DELETE
            DialogResult confirm =
                MessageBox.Show(
                    $"Are you sure you want to delete customer {row.SubItems[1].Text}?",
                    "Delete Customer",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

            if (confirm == DialogResult.Yes)
            {
                (
                    bool ok,
                    string message
                ) =
                    Database.DeleteCustomer(
                        row.SubItems[0].Text
                    );

                if (ok)
                {
                    RefreshCustomers();

                    MessageBox.Show(
                        "Customer deleted successfully.",
                        "Customer Deleted",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        message,
                        "Delete Customer Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
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
                "Search Customer..."
                    ? ""
                    : txtSearch.Text
                        .Trim()
                        .ToLower();

            foreach (
                ListViewItem item
                in customerList.Items)
            {
                bool matches =
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
                    item.SubItems[3].Text
                        .ToLower()
                        .Contains(query) ||
                    item.SubItems[4].Text
                        .ToLower()
                        .Contains(query);

                item.ForeColor =
                    matches
                        ? Color.Black
                        : Color.LightGray;

                item.SubItems[5].ForeColor =
                    matches
                        ? PrimaryBlue
                        : Color.LightGray;
            }
        }

        // =========================
        // ADD CUSTOMER
        // =========================

        private void BtnAddCustomer_Click(
            object? sender,
            EventArgs e)
        {
            using AddCustomerDialog dialog =
                new AddCustomerDialog();

            if (
                dialog.ShowDialog() ==
                DialogResult.OK)
            {
                (
                    bool ok,
                    string message
                ) =
                    Database.AddCustomer(
                        dialog.CustomerId,
                        dialog.CustomerName,
                        dialog.Email,
                        dialog.ContactNumber,
                        dialog.Address
                    );

                if (ok)
                {
                    RefreshCustomers();

                    MessageBox.Show(
                        "Customer added successfully.",
                        "Customer Added",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        message,
                        "Add Customer Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        // =========================
        // DOUBLE CLICK
        // =========================

        private void CustomerList_DoubleClick(
            object? sender,
            EventArgs e)
        {
            if (customerList.SelectedItems.Count > 0)
            {
                ListViewItem row =
                    customerList.SelectedItems[0];

                MessageBox.Show(
                    $"Customer ID: {row.SubItems[0].Text}\n" +
                    $"Name: {row.SubItems[1].Text}\n" +
                    $"Email: {row.SubItems[2].Text}\n" +
                    $"Contact: {row.SubItems[3].Text}\n" +
                    $"Address: {row.SubItems[4].Text}",
                    "Customer Details",
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

            if (
                newPage < 1 ||
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
            if (
                page < 1 ||
                page > totalPages)
            {
                return;
            }

            currentPage = page;

            lblPageCurrent.Text =
                page.ToString();
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