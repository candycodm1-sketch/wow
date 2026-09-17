using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class AddBookingDialog : Form
    {
        private TextBox txtBookingId = null!;
        private TextBox txtCustomerName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtContactNumber = null!;
        private TextBox txtAddress = null!;
        private TextBox txtVehicleName = null!;
        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;

        public string BookingId => txtBookingId.Text.Trim();
        public string CustomerName => txtCustomerName.Text.Trim();
        public string Email => txtEmail.Text.Trim();
        public string ContactNumber => txtContactNumber.Text.Trim();
        public string Address => txtAddress.Text.Trim();
        public string VehicleName => txtVehicleName.Text.Trim();
        public string FromDate => dtpFrom.Value.ToString("yyyy-MM-dd HH:mm:ss");
        public string ToDate => dtpTo.Value.ToString("yyyy-MM-dd HH:mm:ss");

        public AddBookingDialog()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Add Booking";
            this.Size = new Size(400, 640);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Label lblTitle = new Label
            {
                Text = "New Booking",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, 20)
            };
            this.Controls.Add(lblTitle);

            int y = 65;

            Label lblId = new Label { Text = "Booking ID", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblId);
            txtBookingId = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle, Text = $"BK-{new Random().Next(1000, 9999)}" };
            this.Controls.Add(txtBookingId);
            y += 60;

            Label lblCustomer = new Label { Text = "Customer Name", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblCustomer);
            txtCustomerName = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtCustomerName);
            y += 60;

            Label lblEmail = new Label { Text = "Email", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblEmail);
            txtEmail = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtEmail);
            y += 60;

            Label lblContact = new Label { Text = "Contact Number", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblContact);
            txtContactNumber = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtContactNumber);
            y += 60;

            Label lblAddress = new Label { Text = "Address", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblAddress);
            txtAddress = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtAddress);
            y += 60;

            Label lblVehicle = new Label { Text = "Vehicle", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblVehicle);
            txtVehicleName = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtVehicleName);
            y += 60;

            Label lblFrom = new Label { Text = "From (start of rental)", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblFrom);
            dtpFrom = new DateTimePicker
            {
                Location = new Point(25, y + 22),
                Size = new Size(155, 26),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MMM dd, yyyy  hh:mm tt"
            };
            this.Controls.Add(dtpFrom);

            Label lblTo = new Label { Text = "To (end of rental)", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(200, y) };
            this.Controls.Add(lblTo);
            dtpTo = new DateTimePicker
            {
                Location = new Point(200, y + 22),
                Size = new Size(155, 26),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MMM dd, yyyy  hh:mm tt",
                Value = DateTime.Today.AddDays(1)
            };
            this.Controls.Add(dtpTo);
            y += 65;

            Label lblProcessed = new Label { Text = "Processed By", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblProcessed);
            TextBox txtProcessedBy = new TextBox
            {
                Text = Database.CurrentUserName,
                ReadOnly = true,
                BackColor = Color.FromArgb(245, 245, 248),
                ForeColor = Color.FromArgb(80, 80, 90),
                Location = new Point(25, y + 22),
                Size = new Size(330, 26),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtProcessedBy);
            y += 62;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(150, 36),
                Location = new Point(25, y),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancel);

            Button btnSave = new Button
            {
                Text = "Create Booking",
                Size = new Size(180, 36),
                Location = new Point(185, y),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 60, 200),
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            this.CancelButton = btnCancel;
            this.AcceptButton = btnSave;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBookingId.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtVehicleName.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpTo.Value < dtpFrom.Value)
            {
                MessageBox.Show("The 'To' date/time cannot be before the 'From' date/time.", "Invalid Dates",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
