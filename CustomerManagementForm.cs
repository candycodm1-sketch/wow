using System;
using System.Drawing;
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

        public CustomerManagementForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "DriveHub - Customer Management";
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

            string[] navItems = { "Dashboard", "Vehicles", "Bookings", "Customers", "Reports" };
            int navY = 90;

            foreach (string item in navItems)
            {
                bool isActive = item == "Customers";

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
                Text = "Customer Management",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 25)
            };

            mainContent.Controls.Add(lblTitle);

            Button btnAddCustomer = new Button
            {
                Text = "+ Add Customer",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 60, 200),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 36),
                Location = new Point(780, 25),
                Cursor = Cursors.Hand
            };

            btnAddCustomer.FlatAppearance.BorderSize = 0;
            btnAddCustomer.Click += BtnAddCustomer_Click;
            mainContent.Controls.Add(btnAddCustomer);

            txtSearch = new TextBox
            {
                Text = "Search Customer...",
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(30, 80),
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

            Panel tablePanel = new Panel
            {
                Location = new Point(30, 130),
                Size = new Size(920, 350),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainContent.Controls.Add(tablePanel);

            Label lblAllCustomers = new Label
            {
                Text = "All Customers",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 12)
            };

            tablePanel.Controls.Add(lblAllCustomers);

            customerList = new ListView
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

            customerList.Columns.Add("Customer ID", 110);
            customerList.Columns.Add("Customer Name", 190);
            customerList.Columns.Add("Contact Number", 160);
            customerList.Columns.Add("Address", 250);
            customerList.Columns.Add("Actions", 150);

            tablePanel.Controls.Add(customerList);

            customerList.DoubleClick += CustomerList_DoubleClick;
            customerList.MouseClick += CustomerList_MouseClick;
            customerList.MouseMove += CustomerList_MouseMove;

            RefreshCustomers();

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

        private void RefreshCustomers()
        {
            customerList.Items.Clear();

            foreach (CustomerRecord customer in Database.GetCustomers())
            {
                AddCustomerRow(
                    customer.CustomerId,
                    customer.CustomerName,
                    customer.ContactNumber,
                    customer.Address
                );
            }
        }

        private void AddCustomerRow(
            string id,
            string name,
            string contact,
            string address)
        {
            ListViewItem item = new ListViewItem(
                new[]
                {
                    id,
                    name,
                    contact,
                    address,
                    "View | Delete"
                }
            );

            item.SubItems[4].ForeColor =
                Color.FromArgb(50, 60, 200);

            customerList.Items.Add(item);
        }

        private void CustomerList_MouseMove(
            object? sender,
            MouseEventArgs e)
        {
            ListViewHitTestInfo hit =
                customerList.HitTest(e.Location);

            if (hit.Item != null &&
                hit.SubItem != null)
            {
                int columnIndex =
                    hit.Item.SubItems.IndexOf(hit.SubItem);

                if (columnIndex == 4)
                {
                    customerList.Cursor =
                        Cursors.Hand;

                    return;
                }
            }

            customerList.Cursor =
                Cursors.Default;
        }

        private void CustomerList_MouseClick(
            object? sender,
            MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            ListViewHitTestInfo hit =
                customerList.HitTest(e.Location);

            if (hit.Item == null ||
                hit.SubItem == null)
            {
                return;
            }

            int columnIndex =
                hit.Item.SubItems.IndexOf(hit.SubItem);

            if (columnIndex != 4)
                return;

            ListViewItem row = hit.Item;

            int actionColumnLeft =
                row.SubItems[4].Bounds.Left;

            int relativeX =
                e.X - actionColumnLeft;

            if (relativeX < 55)
            {
                MessageBox.Show(
                    $"Customer ID: {row.SubItems[0].Text}\n" +
                    $"Name: {row.SubItems[1].Text}\n" +
                    $"Contact: {row.SubItems[2].Text}\n" +
                    $"Address: {row.SubItems[3].Text}",
                    "Customer Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            DialogResult confirm =
                MessageBox.Show(
                    $"Are you sure you want to delete customer {row.SubItems[1].Text}?",
                    "Delete Customer",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

            if (confirm == DialogResult.Yes)
            {
                (bool ok, string message) = Database.DeleteCustomer(
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

        private void TxtSearch_TextChanged(
            object? sender,
            EventArgs e)
        {
            string query =
                txtSearch.Text == "Search Customer..."
                    ? ""
                    : txtSearch.Text.Trim().ToLower();

            foreach (ListViewItem item in customerList.Items)
            {
                bool matches =
                    string.IsNullOrEmpty(query) ||
                    item.SubItems[0].Text.ToLower().Contains(query) ||
                    item.SubItems[1].Text.ToLower().Contains(query) ||
                    item.SubItems[2].Text.ToLower().Contains(query) ||
                    item.SubItems[3].Text.ToLower().Contains(query);

                item.ForeColor =
                    matches
                        ? Color.Black
                        : Color.LightGray;

                item.SubItems[4].ForeColor =
                    matches
                        ? Color.FromArgb(50, 60, 200)
                        : Color.LightGray;
            }
        }

        private void BtnAddCustomer_Click(
            object? sender,
            EventArgs e)
        {
            using AddCustomerDialog dialog =
                new AddCustomerDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                (bool ok, string message) = Database.AddCustomer(
                    dialog.CustomerId,
                    dialog.CustomerName,
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
                    $"Contact: {row.SubItems[2].Text}\n" +
                    $"Address: {row.SubItems[3].Text}",
                    "Customer Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void ChangePage(int direction)
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

        private void GoToPage(int page)
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
                    new BookingManagementForm().Show();
                    this.Close();
                    break;

                case "Customers":
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
            var confirm =
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