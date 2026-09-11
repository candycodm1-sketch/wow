using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class EditVehicleDialog : Form
    {
        private TextBox txtVehicleId = null!;
        private TextBox txtModel = null!;
        private ComboBox cmbType = null!;
        private TextBox txtDailyRate = null!;
        private ComboBox cmbStatus = null!;

        public string VehicleId => txtVehicleId.Text.Trim();
        public string Model => txtModel.Text.Trim();
        public string VehicleType => cmbType.Text;
        public string DailyRate => $"₱{txtDailyRate.Text.Trim()}/day";
        public string Status => cmbStatus.Text;

        public EditVehicleDialog(string vehicleId, string model, string type, string dailyRate, string status)
        {
            InitializeForm();

            // Pre-fill with the existing vehicle's data.
            txtVehicleId.Text = vehicleId;
            txtModel.Text = model;
            cmbType.Text = type;
            // Strip the peso sign, separators, and "/day" so only the number shows in the box.
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in dailyRate)
            {
                if (char.IsDigit(c) || c == '.')
                    sb.Append(c);
            }
            txtDailyRate.Text = sb.ToString();
            cmbStatus.Text = status;
        }

        private void InitializeForm()
        {
            this.Text = "Edit Vehicle";
            this.Size = new Size(400, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Label lblTitle = new Label
            {
                Text = "Edit Vehicle",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, 20)
            };
            this.Controls.Add(lblTitle);

            int y = 65;

            Label lblId = new Label { Text = "Vehicle ID", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblId);
            txtVehicleId = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle, ReadOnly = true, BackColor = Color.FromArgb(240, 240, 240) };
            this.Controls.Add(txtVehicleId);
            y += 60;

            Label lblModel = new Label { Text = "Model", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblModel);
            txtModel = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtModel);
            y += 60;

            Label lblType = new Label { Text = "Type", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblType);
            cmbType = new ComboBox { Location = new Point(25, y + 22), Size = new Size(155, 26), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbType.Items.AddRange(new object[] { "Sedan", "SUV", "Hatchback", "Pickup", "Van", "Motorcycle" });
            this.Controls.Add(cmbType);

            Label lblStatus = new Label { Text = "Status", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(200, y) };
            this.Controls.Add(lblStatus);
            cmbStatus = new ComboBox { Location = new Point(200, y + 22), Size = new Size(155, 26), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus.Items.AddRange(new object[] { "Available", "Rented", "Maintenance" });
            this.Controls.Add(cmbStatus);
            y += 60;

            Label lblRate = new Label { Text = "Daily Rate (₱)", Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true, Location = new Point(25, y) };
            this.Controls.Add(lblRate);
            txtDailyRate = new TextBox { Location = new Point(25, y + 22), Size = new Size(330, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtDailyRate);
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
                Text = "Save Changes",
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
            if (string.IsNullOrWhiteSpace(txtModel.Text) || string.IsNullOrWhiteSpace(txtDailyRate.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtDailyRate.Text.Trim(), out _))
            {
                MessageBox.Show("Daily rate must be a number.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
