using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class AddBookingDialog : Form
    {
        private TextBox txtBookingId = null!;
        private TextBox txtCustomerName = null!;
        private TextBox txtVehicleName = null!;
        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;

        public string BookingId => txtBookingId.Text.Trim();
        public string CustomerName => txtCustomerName.Text.Trim();
        public string VehicleName => txtVehicleName.Text.Trim();
        public string FromDate => dtpFrom.Value.ToString("yyyy-MM-dd");
        public string ToDate => dtpTo.Value.ToString("yyyy-MM-dd");

        public AddBookingDialog()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Add Booking";
            this.Size = new Size(400, 420);
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

            Label lblVehicle = new Label { Text = "Vehicle", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblVehicle);
            txtVehicleName = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtVehicleName);
            y += 60;

            Label lblFrom = new Label { Text = "From", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblFrom);
            dtpFrom = new DateTimePicker { Location = new Point(25, y + 22), Size = new Size(155, 26), Format = DateTimePickerFormat.Short };
            this.Controls.Add(dtpFrom);

            Label lblTo = new Label { Text = "To", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(200, y) };
            this.Controls.Add(lblTo);
            dtpTo = new DateTimePicker { Location = new Point(200, y + 22), Size = new Size(155, 26), Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(1) };
            this.Controls.Add(dtpTo);
            y += 65;

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

            if (dtpTo.Value.Date < dtpFrom.Value.Date)
            {
                MessageBox.Show("The 'To' date cannot be before the 'From' date.", "Invalid Dates",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
