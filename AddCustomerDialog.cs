using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class AddCustomerDialog : Form
    {
        private bool isEditMode = false;

        private TextBox txtCustomerId = null!;
        private TextBox txtCustomerName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtContactNumber = null!;
        private TextBox txtAddress = null!;

        public string CustomerId => txtCustomerId.Text.Trim();
        public string CustomerName => txtCustomerName.Text.Trim();
        public string Email => txtEmail.Text.Trim();
        public string ContactNumber => txtContactNumber.Text.Trim();
        public string Address => txtAddress.Text.Trim();

        public AddCustomerDialog()
        {
            isEditMode = false;
            InitializeForm();
        }

        public AddCustomerDialog(
            string customerId,
            string customerName,
            string email,
            string contactNumber,
            string address)
        {
            isEditMode = true;
            InitializeForm();

            txtCustomerId.Text = customerId;
            txtCustomerName.Text = customerName;
            txtEmail.Text = email;
            txtContactNumber.Text = contactNumber;
            txtAddress.Text = address;
        }

        private void InitializeForm()
        {
            this.Text = isEditMode ? "Edit Customer" : "Add Customer";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Label lblTitle = new Label
            {
                Text = isEditMode ? "Edit Customer" : "Customer Information",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, 20)
            };
            this.Controls.Add(lblTitle);

            int y = 65;

            Label lblId = new Label
            {
                Text = "Customer ID",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, y)
            };
            this.Controls.Add(lblId);

            txtCustomerId = new TextBox
            {
                Location = new Point(25, y + 22),
                Size = new Size(330, 26),
                BorderStyle = BorderStyle.FixedSingle,
                Text = isEditMode
                    ? ""
                    : $"CUS-{new Random().Next(1000, 9999)}",
                ReadOnly = isEditMode,
                BackColor = isEditMode
                    ? Color.FromArgb(245, 245, 248)
                    : Color.White
            };
            this.Controls.Add(txtCustomerId);

            y += 60;

            Label lblName = new Label
            {
                Text = "Customer Name",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, y)
            };
            this.Controls.Add(lblName);

            txtCustomerName = new TextBox
            {
                Location = new Point(25, y + 22),
                Size = new Size(330, 26),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtCustomerName);

            y += 60;

            Label lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, y)
            };
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Location = new Point(25, y + 22),
                Size = new Size(330, 26),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtEmail);

            y += 60;

            Label lblContact = new Label
            {
                Text = "Contact Number",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, y)
            };
            this.Controls.Add(lblContact);

            txtContactNumber = new TextBox
            {
                Location = new Point(25, y + 22),
                Size = new Size(330, 26),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtContactNumber);

            y += 60;

            Label lblAddress = new Label
            {
                Text = "Address",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, y)
            };
            this.Controls.Add(lblAddress);

            txtAddress = new TextBox
            {
                Location = new Point(25, y + 22),
                Size = new Size(330, 50),
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true
            };
            this.Controls.Add(txtAddress);

            y += 85;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(150, 36),
                Location = new Point(25, y),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };

            btnCancel.FlatAppearance.BorderColor =
                Color.FromArgb(200, 200, 200);

            this.Controls.Add(btnCancel);

            Button btnSave = new Button
            {
                Text = isEditMode ? "Update Customer" : "Save Customer",
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
            if (string.IsNullOrWhiteSpace(txtCustomerId.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtContactNumber.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show(
                    "Please fill in all fields.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show(
                    "Please enter a valid email address.",
                    "Invalid Email",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (txtContactNumber.Text.Length < 10)
            {
                MessageBox.Show(
                    "Please enter a valid contact number.",
                    "Invalid Contact Number",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}