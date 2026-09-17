using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class ReportsForm : Form
    {
        private ListView transactionList = null!;

        private Label lblIncomeValue = null!;
        private Label lblRentalsValue = null!;
        private Label lblVehiclesValue = null!;

        private ComboBox cmbMonth = null!;
        private ComboBox cmbYear = null!;
        private Button btnPrintReport = null!;
        private Label lblReportCaption = null!;
        private Label lblMonthIncomeValue = null!;
        private Label lblMonthRentalsValue = null!;

        private int printBookingIndex = 0;
        private int printPageNumber = 1;
        private List<BookingRecord> printBookings = new List<BookingRecord>();
        private int printSelectedMonth = 1;
        private int printSelectedYear = 2026;
        private string printSelectedMonthName = "";
        private decimal printIncome = 0;
        private int printRentals = 0;

        private readonly Color PrimaryBlue =
            Color.FromArgb(48, 73, 181);

        private readonly Color BackgroundColor =
            Color.FromArgb(245, 246, 248);

        private readonly Color SidebarActive =
            Color.FromArgb(235, 238, 250);

        public ReportsForm()
        {
            BookingData.Refresh();

            InitializeForm();

            RefreshReports();
            LoadMonthlyReport();
        }

        private void InitializeForm()
        {
            Text = "DriveHub - Reports";
            Size = new Size(1100, 650);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1000, 550);
            BackColor = BackgroundColor;

            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 270,
                BackColor = Color.White
            };

            Controls.Add(sidebar);

            PictureBox logoIcon = CreateAssetPicture(
                "vehicle logo.png",
                new Size(40, 40)
            );

            logoIcon.Location = new Point(13, 21);
            logoIcon.SizeMode = PictureBoxSizeMode.Zoom;
            logoIcon.BackColor = Color.Transparent;

            sidebar.Controls.Add(logoIcon);

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

                using Font font =
                    new Font("Segoe UI", 12F, FontStyle.Bold);

                using SolidBrush driveBrush =
                    new SolidBrush(Color.Black);

                using SolidBrush hubBrush =
                    new SolidBrush(PrimaryBlue);

                string driveText = "Drive";
                string hubText = "Hub";

                SizeF driveSize =
                    e.Graphics.MeasureString(driveText, font);

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

            Label lblLogoSub = new Label
            {
                Text = "Vehicle Rental System",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(63, 48),
                BackColor = Color.Transparent
            };

            sidebar.Controls.Add(lblLogoSub);

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
                false
            );

            AddNavigationItem(
                sidebar,
                "Reports",
                "reports logo for sidebar.png",
                400,
                true
            );

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
                Font = new Font("Segoe UI", 10F),
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

            Panel mainContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor
            };

            Controls.Add(mainContent);
            mainContent.BringToFront();

            Label lblTitle = new Label
            {
                Text = "Reports",
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

            Label lblAdmin = new Label
            {
                Text = "Admin ▾",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(772, 30)
            };

            mainContent.Controls.Add(lblAdmin);

            CreateSummaryCard(
                mainContent,
                "Total Income",
                "₱0",
                "All Bookings",
                "income logo.png",
                79,
                80,
                out lblIncomeValue
            );

            CreateSummaryCard(
                mainContent,
                "Total Rentals",
                "0",
                "All Bookings",
                "total rentals logo.png",
                289,
                80,
                out lblRentalsValue
            );

            CreateSummaryCard(
                mainContent,
                "Available Vehicles",
                "0",
                "Today",
                "available vehicles logo.png",
                499,
                80,
                out lblVehiclesValue
            );

            Panel reportPanel = new Panel
            {
                Location = new Point(79, 225),
                Size = new Size(825, 190),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainContent.Controls.Add(reportPanel);

            Label lblReportTitle = new Label
            {
                Text = "Monthly Report",
                Font = new Font(
                    "Segoe UI",
                    11F,
                    FontStyle.Bold
                ),
                ForeColor = Color.FromArgb(40, 45, 60),
                AutoSize = true,
                Location = new Point(15, 13)
            };

            reportPanel.Controls.Add(lblReportTitle);

            cmbMonth = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(130, 28),
                Location = new Point(430, 10)
            };

            cmbMonth.Items.AddRange(
                new object[]
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December"
                }
            );

            cmbMonth.SelectedIndex =
                DateTime.Now.Month - 1;

            cmbMonth.SelectedIndexChanged +=
                (s, e) => LoadMonthlyReport();

            reportPanel.Controls.Add(cmbMonth);

            cmbYear = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(95, 28),
                Location = new Point(570, 10)
            };

            int nowYear = DateTime.Now.Year;

            object[] yearItems = new object[7];

            for (int i = 0; i < yearItems.Length; i++)
            {
                yearItems[i] = nowYear - 3 + i;
            }

            cmbYear.Items.AddRange(yearItems);

            cmbYear.SelectedItem = nowYear;

            cmbYear.SelectedIndexChanged +=
                (s, e) => LoadMonthlyReport();

            reportPanel.Controls.Add(cmbYear);

            btnPrintReport = new Button
            {
                Text = "🖨️ Print Report",
                Font = new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold
                ),
                ForeColor = Color.White,
                BackColor = PrimaryBlue,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(135, 28),
                Location = new Point(675, 10),
                Cursor = Cursors.Hand
            };

            btnPrintReport.FlatAppearance.BorderSize = 0;
            btnPrintReport.Click += BtnPrintReport_Click;
            reportPanel.Controls.Add(btnPrintReport);

            lblReportCaption = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(15, 55)
            };

            reportPanel.Controls.Add(lblReportCaption);

            lblMonthIncomeValue = new Label
            {
                Text = "",
                Font = new Font(
                    "Segoe UI",
                    20F,
                    FontStyle.Bold
                ),
                ForeColor = Color.FromArgb(40, 170, 90),
                AutoSize = true,
                Location = new Point(15, 90)
            };

            reportPanel.Controls.Add(lblMonthIncomeValue);

            Label lblRentalsCaption = new Label
            {
                Text = "Rentals",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(560, 55)
            };

            reportPanel.Controls.Add(lblRentalsCaption);

            lblMonthRentalsValue = new Label
            {
                Text = "",
                Font = new Font(
                    "Segoe UI",
                    17F,
                    FontStyle.Bold
                ),
                ForeColor = Color.FromArgb(50, 60, 200),
                AutoSize = true,
                Location = new Point(560, 90)
            };

            reportPanel.Controls.Add(lblMonthRentalsValue);

            Panel transactionsPanel = new Panel
            {
                Location = new Point(79, 435),
                Size = new Size(825, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainContent.Controls.Add(transactionsPanel);

            Label lblTransactions = new Label
            {
                Text = "Recent Transactions",
                Font = new Font(
                    "Segoe UI",
                    11F,
                    FontStyle.Bold
                ),
                AutoSize = true,
                Location = new Point(15, 12)
            };

            transactionsPanel.Controls.Add(lblTransactions);

            transactionList = new ListView
            {
                Location = new Point(0, 45),
                Size = new Size(823, 100),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None
            };

            transactionList.Columns.Add(
                "Booking ID",
                135
            );

            transactionList.Columns.Add(
                "Customer",
                140
            );

            transactionList.Columns.Add(
                "Vehicle",
                140
            );

            transactionList.Columns.Add(
                "Date",
                120
            );

            transactionList.Columns.Add(
                "Amount",
                115
            );

            transactionList.Columns.Add(
                "Status",
                135
            );

            transactionsPanel.Controls.Add(transactionList);
        }

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
                    new CustomerManagementForm().Show();
                    Close();
                    break;

                case "Reports":
                    break;
            }
        }

        private void CreateSummaryCard(
            Panel parent,
            string title,
            string value,
            string subtitle,
            string iconFile,
            int x,
            int y,
            out Label valueLabel)
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
                new Size(40, 40)
            );

            icon.Location = new Point(20, 39);
            icon.Size = new Size(40, 40);
            icon.SizeMode = PictureBoxSizeMode.Zoom;
            icon.BackColor = Color.Transparent;

            card.Controls.Add(icon);

            Label lblCardTitle = new Label
            {
                Text = title,
                Font = new Font(
                    "Segoe UI",
                    title == "Available Vehicles"
                        ? 8.5F
                        : 9.5F
                ),
                ForeColor = Color.Black,
                AutoSize = false,
                Size = new Size(100, 20),
                Location = new Point(90, 43),
                TextAlign = ContentAlignment.MiddleLeft
            };

            card.Controls.Add(lblCardTitle);

            valueLabel = new Label
            {
                Text = value,
                Font = new Font(
                    "Segoe UI",
                    17F
                ),
                ForeColor = Color.Black,
                AutoSize = false,
                Size = new Size(100, 27),
                Location = new Point(90, 65),
                TextAlign = ContentAlignment.MiddleLeft
            };

            card.Controls.Add(valueLabel);

            Label lblSubtitle = new Label
            {
                Text = subtitle,
                Font = new Font(
                    "Segoe UI",
                    8.5F
                ),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(100, 18),
                Location = new Point(90, 94),
                TextAlign = ContentAlignment.MiddleLeft
            };

            card.Controls.Add(lblSubtitle);
        }

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
                using Image original = Image.FromFile(path);

                pictureBox.Image = new Bitmap(original);
            }

            return pictureBox;
        }

        private string FindAsset(string fileName)
        {
            string current = Application.StartupPath;

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

        private void RefreshReports()
        {
            BookingData.Refresh();

            decimal income =
                BookingData.Bookings
                    .Where(
                        x => x.Status != "Cancelled"
                    )
                    .Sum(
                        x => x.Amount
                    );

            int rentals =
                BookingData.Bookings.Count(
                    x => x.Status != "Cancelled"
                );

            int availableVehicles =
                Database.CountAvailableVehicles();

            lblIncomeValue.Text =
                "₱" + income.ToString("N0");

            lblRentalsValue.Text =
                rentals.ToString();

            lblVehiclesValue.Text =
                availableVehicles.ToString();

            transactionList.Items.Clear();

            foreach (
                BookingRecord booking
                in BookingData.Bookings
                    .AsEnumerable()
                    .Reverse()
                    .Take(5)
            )
            {
                string amount =
                    "₱" +
                    booking.Amount.ToString("N0");

                AddTransaction(
                    booking.BookingId,
                    booking.CustomerName,
                    booking.VehicleName,
                    booking.FromDate,
                    amount,
                    booking.Status
                );
            }
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
                item.SubItems[5].ForeColor =
                    Color.Red;
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

        private void LoadMonthlyReport()
        {
            if (
                cmbMonth.SelectedIndex < 0 ||
                cmbYear.SelectedItem == null
            )
            {
                return;
            }

            int month =
                cmbMonth.SelectedIndex + 1;

            int year =
                (int)cmbYear.SelectedItem;

            lblReportCaption.Text =
                $"Total Revenue / Income in {cmbMonth.Text} {year}";

            lblMonthIncomeValue.Text =
                "₱" +
                Database
                    .GetMonthlyIncome(
                        year,
                        month
                    )
                    .ToString("N0");

            int rentals =
                Database.GetMonthlyRentals(
                    year,
                    month
                );

            lblMonthRentalsValue.Text =
                rentals +
                " Rental" +
                (rentals == 1 ? "" : "s");
        }

        // =========================
        // PRINT MONTHLY REPORT
        // =========================

        private void BtnPrintReport_Click(object? sender, EventArgs e)
        {
            if (cmbMonth.SelectedIndex < 0 || cmbYear.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a valid month and year.",
                    "Print Report",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            printSelectedMonth = cmbMonth.SelectedIndex + 1;
            printSelectedYear = (int)cmbYear.SelectedItem;
            printSelectedMonthName = cmbMonth.Text;
            printIncome = Database.GetMonthlyIncome(printSelectedYear, printSelectedMonth);
            printRentals = Database.GetMonthlyRentals(printSelectedYear, printSelectedMonth);
            printBookings = Database.GetMonthlyBookings(printSelectedYear, printSelectedMonth);
            printBookingIndex = 0;
            printPageNumber = 1;

            try
            {
                PrintDocument printDoc = new PrintDocument();
                printDoc.DocumentName = $"DriveHub_Monthly_Report_{printSelectedMonthName}_{printSelectedYear}";
                printDoc.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

                printDoc.BeginPrint += (s, ev) =>
                {
                    printBookingIndex = 0;
                    printPageNumber = 1;
                };

                printDoc.PrintPage += PrintDocument_PrintPage;

                using PrintPreviewDialog previewDlg = new PrintPreviewDialog
                {
                    Document = printDoc,
                    Width = 950,
                    Height = 700,
                    StartPosition = FormStartPosition.CenterParent
                };

                previewDlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to open print preview: {ex.Message}",
                    "Print Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void PrintDocument_PrintPage(
            object? sender,
            PrintPageEventArgs e)
        {
            Graphics g = e.Graphics!;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int left = 45;
            int right = e.PageBounds.Width - 45;
            int pageWidth = right - left;
            int y = 45;

            if (printPageNumber == 1)
            {
                y = DrawPrintHeaderAndKPIs(g, left, right, pageWidth, y);
            }
            else
            {
                y = DrawSubsequentPageHeader(g, left, right, y);
            }

            y = DrawPrintTableHeader(g, left, right, pageWidth, y);
            bool hasMore = DrawPrintTableRows(g, left, right, pageWidth, ref y, e.PageBounds.Height - 85);

            if (hasMore)
            {
                e.HasMorePages = true;
                printPageNumber++;
                DrawPrintFooter(g, left, right, e.PageBounds.Height - 40);
                return;
            }

            DrawPrintSummaryTotal(g, left, right, pageWidth, y);
            DrawPrintFooter(g, left, right, e.PageBounds.Height - 40);
            e.HasMorePages = false;
        }

        private int DrawPrintHeaderAndKPIs(
            Graphics g,
            int left,
            int right,
            int pageWidth,
            int y)
        {
            using Font brandFont = new Font("Segoe UI", 18F, FontStyle.Bold);
            using Font brandSubFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            using Font reportTitleFont = new Font("Segoe UI", 13F, FontStyle.Bold);
            using Font smallFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            using Font smallBoldFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);

            SizeF driveSize = g.MeasureString("Drive", brandFont);
            g.DrawString("Drive", brandFont, Brushes.Black, left, y);
            using SolidBrush blueBrush = new SolidBrush(PrimaryBlue);
            g.DrawString("Hub", brandFont, blueBrush, left + driveSize.Width - 6, y);

            g.DrawString("Vehicle Rental System", brandSubFont, Brushes.Gray, left, y + 30);

            string headingText = "MONTHLY PERFORMANCE REPORT";
            SizeF headingSize = g.MeasureString(headingText, reportTitleFont);
            g.DrawString(headingText, reportTitleFont, Brushes.Black, right - headingSize.Width, y);

            string periodText = $"{printSelectedMonthName.ToUpper()} {printSelectedYear}";
            SizeF periodSize = g.MeasureString(periodText, smallBoldFont);
            g.DrawString(periodText, smallBoldFont, blueBrush, right - periodSize.Width, y + 24);

            y += 52;

            using Pen primaryPen = new Pen(PrimaryBlue, 2f);
            g.DrawLine(primaryPen, left, y, right, y);
            y += 12;

            g.DrawString($"Report Period: {printSelectedMonthName} {printSelectedYear}", smallBoldFont, Brushes.Black, left, y);
            string genDate = $"Generated: {DateTime.Now:MMM d, yyyy, h:mm tt}";
            g.DrawString(genDate, smallFont, Brushes.Gray, left + 260, y);
            string printedBy = $"Generated By: {Database.CurrentUserName}";
            SizeF printedBySize = g.MeasureString(printedBy, smallFont);
            g.DrawString(printedBy, smallFont, Brushes.Gray, right - printedBySize.Width, y);

            y += 24;

            int activeCount = printBookings.Count(b => b.Status == "Active");
            int completedCount = printBookings.Count(b => b.Status == "Completed");

            int cardGap = 12;
            int cardWidth = (pageWidth - (3 * cardGap)) / 4;
            int cardHeight = 65;

            DrawPrintCard(g, left, y, cardWidth, cardHeight, "Total Revenue", "₱" + printIncome.ToString("N0"), Color.FromArgb(40, 160, 80));
            DrawPrintCard(g, left + cardWidth + cardGap, y, cardWidth, cardHeight, "Total Rentals", printRentals.ToString() + " Bookings", Color.FromArgb(48, 73, 181));
            DrawPrintCard(g, left + (2 * (cardWidth + cardGap)), y, cardWidth, cardHeight, "Active Rentals", activeCount.ToString(), Color.FromArgb(50, 60, 200));
            DrawPrintCard(g, left + (3 * (cardWidth + cardGap)), y, cardWidth, cardHeight, "Completed", completedCount.ToString(), Color.FromArgb(30, 140, 180));

            y += cardHeight + 20;

            using Font secFont = new Font("Segoe UI", 11F, FontStyle.Bold);
            g.DrawString("Monthly Rental Transactions", secFont, Brushes.Black, left, y);
            y += 22;

            return y;
        }

        private int DrawSubsequentPageHeader(
            Graphics g,
            int left,
            int right,
            int y)
        {
            using Font pageHeaderFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            using Font pageHeaderSub = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            g.DrawString($"DriveHub — Monthly Report ({printSelectedMonthName} {printSelectedYear})", pageHeaderFont, Brushes.Black, left, y);
            string pageCont = $"(Page {printPageNumber})";
            SizeF contSize = g.MeasureString(pageCont, pageHeaderSub);
            g.DrawString(pageCont, pageHeaderSub, Brushes.Gray, right - contSize.Width, y);
            y += 18;
            using Pen linePen = new Pen(Color.LightGray, 1f);
            g.DrawLine(linePen, left, y, right, y);
            y += 15;
            return y;
        }

        private int DrawPrintTableHeader(
            Graphics g,
            int left,
            int right,
            int pageWidth,
            int y)
        {
            int headerHeight = 26;

            using SolidBrush headerBg = new SolidBrush(PrimaryBlue);
            g.FillRectangle(headerBg, left, y, pageWidth, headerHeight);

            using Font thFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            int colBooking = left + 10;
            int colCustomer = left + 115;
            int colVehicle = left + 265;
            int colPeriod = left + 405;
            int colStatus = left + 560;
            int colAmount = right - 10;

            g.DrawString("Booking ID", thFont, Brushes.White, colBooking, y + 5);
            g.DrawString("Customer Name", thFont, Brushes.White, colCustomer, y + 5);
            g.DrawString("Vehicle", thFont, Brushes.White, colVehicle, y + 5);
            g.DrawString("Rental Period", thFont, Brushes.White, colPeriod, y + 5);
            g.DrawString("Status", thFont, Brushes.White, colStatus, y + 5);

            using StringFormat rightAlign = new StringFormat { Alignment = StringAlignment.Far };
            g.DrawString("Amount", thFont, Brushes.White, colAmount, y + 5, rightAlign);

            return y + headerHeight;
        }

        private bool DrawPrintTableRows(
            Graphics g,
            int left,
            int right,
            int pageWidth,
            ref int y,
            int bottomLimit)
        {
            int rowHeight = 24;
            int colBooking = left + 10;
            int colCustomer = left + 115;
            int colVehicle = left + 265;
            int colPeriod = left + 405;
            int colStatus = left + 560;
            int colAmount = right - 10;

            using Font tdFont = new Font("Segoe UI", 8F, FontStyle.Regular);
            using Font tdBoldFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            using Pen rowBorderPen = new Pen(Color.FromArgb(230, 233, 240));
            using SolidBrush altBg = new SolidBrush(Color.FromArgb(248, 249, 252));
            using StringFormat rightAlign = new StringFormat { Alignment = StringAlignment.Far };

            if (printBookings.Count == 0)
            {
                using Font italicFont = new Font("Segoe UI", 8.5F, FontStyle.Italic);
                g.DrawString("No booking transactions recorded for this month.", italicFont, Brushes.Gray, left + 10, y + 10);
                y += 35;
                return false;
            }

            while (printBookingIndex < printBookings.Count)
            {
                BookingRecord b = printBookings[printBookingIndex];

                if (y + rowHeight > bottomLimit)
                {
                    return true;
                }

                if (printBookingIndex % 2 == 1)
                {
                    g.FillRectangle(altBg, left, y, pageWidth, rowHeight);
                }

                g.DrawLine(rowBorderPen, left, y + rowHeight, right, y + rowHeight);

                g.DrawString(b.BookingId, tdBoldFont, Brushes.Black, colBooking, y + 4);
                g.DrawString(TruncateText(b.CustomerName, 140, tdFont, g), tdFont, Brushes.Black, colCustomer, y + 4);
                g.DrawString(TruncateText(b.VehicleName, 130, tdFont, g), tdFont, Brushes.Black, colVehicle, y + 4);

                string periodStr = FormatPeriod(b.FromDate, b.ToDate);
                g.DrawString(TruncateText(periodStr, 150, tdFont, g), tdFont, Brushes.Black, colPeriod, y + 4);

                Color statusColor = b.Status switch
                {
                    "Completed" => Color.FromArgb(40, 160, 80),
                    "Active" => Color.FromArgb(50, 60, 200),
                    "Cancelled" => Color.Red,
                    _ => Color.FromArgb(230, 140, 30)
                };
                using SolidBrush statusBrush = new SolidBrush(statusColor);
                g.DrawString(b.Status, tdBoldFont, statusBrush, colStatus, y + 4);

                string amtText = "₱" + b.Amount.ToString("N0");
                g.DrawString(amtText, tdBoldFont, Brushes.Black, colAmount, y + 4, rightAlign);

                y += rowHeight;
                printBookingIndex++;
            }

            return false;
        }

        private void DrawPrintSummaryTotal(
            Graphics g,
            int left,
            int right,
            int pageWidth,
            int y)
        {
            y += 4;
            using SolidBrush totalBg = new SolidBrush(Color.FromArgb(240, 244, 255));
            g.FillRectangle(totalBg, left, y, pageWidth, 28);
            using Pen totalPen = new Pen(PrimaryBlue, 1.5f);
            g.DrawLine(totalPen, left, y, right, y);
            g.DrawLine(totalPen, left, y + 28, right, y + 28);

            using Font totalFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            g.DrawString($"Total Revenue ({printSelectedMonthName} {printSelectedYear}):", totalFont, Brushes.Black, left + 10, y + 5);

            using SolidBrush greenBrush = new SolidBrush(Color.FromArgb(40, 160, 80));
            using StringFormat rightAlign = new StringFormat { Alignment = StringAlignment.Far };
            g.DrawString("₱" + printIncome.ToString("N0"), totalFont, greenBrush, right - 10, y + 5, rightAlign);
        }

        private void DrawPrintCard(
            Graphics g,
            int x,
            int y,
            int width,
            int height,
            string title,
            string value,
            Color accentColor)
        {
            using SolidBrush bgBrush = new SolidBrush(Color.FromArgb(248, 249, 252));
            g.FillRectangle(bgBrush, x, y, width, height);

            using Pen borderPen = new Pen(Color.FromArgb(225, 228, 235));
            g.DrawRectangle(borderPen, x, y, width, height);

            using SolidBrush accentBrush = new SolidBrush(accentColor);
            g.FillRectangle(accentBrush, x, y, width, 3);

            using Font titleFont = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            g.DrawString(title, titleFont, Brushes.Gray, x + 8, y + 10);

            using Font valFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            using SolidBrush textBrush = new SolidBrush(accentColor);
            g.DrawString(value, valFont, textBrush, x + 8, y + 26);
        }

        private void DrawPrintFooter(
            Graphics g,
            int left,
            int right,
            int y)
        {
            using Pen footerLine = new Pen(Color.LightGray);
            g.DrawLine(footerLine, left, y, right, y);

            using Font footFont = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            g.DrawString("DriveHub Vehicle Rental Management System — Confidential Report", footFont, Brushes.Gray, left, y + 6);

            string pageStr = $"Page {printPageNumber}";
            SizeF pgSize = g.MeasureString(pageStr, footFont);
            g.DrawString(pageStr, footFont, Brushes.Gray, right - pgSize.Width, y + 6);
        }

        private string FormatPeriod(
            string fromDate,
            string toDate)
        {
            if (DateTime.TryParse(fromDate, out DateTime f) && DateTime.TryParse(toDate, out DateTime t))
            {
                if (f.Date == t.Date)
                    return f.ToString("MMM d, yyyy");
                if (f.Year == t.Year)
                    return $"{f:MMM d} - {t:MMM d, yyyy}";
                return $"{f:MMM d, yyyy} - {t:MMM d, yyyy}";
            }

            return fromDate;
        }

        private string TruncateText(
            string text,
            int maxWidth,
            Font font,
            Graphics g)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            if (g.MeasureString(text, font).Width <= maxWidth)
                return text;

            string truncated = text;
            while (truncated.Length > 0 && g.MeasureString(truncated + "...", font).Width > maxWidth)
            {
                truncated = truncated.Substring(0, truncated.Length - 1);
            }

            return truncated + "...";
        }

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

            Close();
        }
    }
}