using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class RegisterForm : Form
    {
        private TextBox txtFullName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmPassword = null!;
        private Button btnRegister = null!;
        private LinkLabel lnkBackToLogin = null!;

        public RegisterForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Vehicle Rental & Booking System - Register";
            this.Size = new Size(900, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // ---------- LEFT PANEL ----------
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 380,
                BackColor = Color.FromArgb(219, 229, 247)
            };
            this.Controls.Add(leftPanel);

            Label lblBrandTitle = new Label
            {
                Text = "Vehicle Rental\nBooking System",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 60),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(340, 90),
                Location = new Point(20, 220)
            };
            leftPanel.Controls.Add(lblBrandTitle);

            Label lblBrandSubtitle = new Label
            {
                Text = "Create an account to start\nbooking your next ride.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(90, 90, 110),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(340, 50),
                Location = new Point(20, 320)
            };
            leftPanel.Controls.Add(lblBrandSubtitle);

            // ---------- RIGHT PANEL ----------
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(60, 30, 60, 30)
            };
            this.Controls.Add(rightPanel);
            rightPanel.BringToFront();

            Label lblTitle = new Label
            {
                Text = "Create Account",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(60, 30)
            };
            rightPanel.Controls.Add(lblTitle);

            Label lblSubtitle = new Label
            {
                Text = "Sign up to get started",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(60, 65)
            };
            rightPanel.Controls.Add(lblSubtitle);

            int y = 105;

            AddFieldLabel(rightPanel, "Full Name", y);
            txtFullName = AddTextBox(rightPanel, "Enter your full name", y + 25);
            y += 70;

            AddFieldLabel(rightPanel, "Email", y);
            txtEmail = AddTextBox(rightPanel, "Enter your email", y + 25);
            y += 70;

            AddFieldLabel(rightPanel, "Password", y);
            txtPassword = AddTextBox(rightPanel, "Enter your password", y + 25, isPassword: true);
            y += 70;

            AddFieldLabel(rightPanel, "Confirm Password", y);
            txtConfirmPassword = AddTextBox(rightPanel, "Re-enter your password", y + 25, isPassword: true);
            y += 70;

            btnRegister = new Button
            {
                Text = "Register",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(99, 60, 220),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(60, y + 10),
                Size = new Size(400, 40),
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            rightPanel.Controls.Add(btnRegister);
            y += 60;

            lnkBackToLogin = new LinkLabel
            {
                Text = "Already have an account? Login here",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                LinkColor = Color.FromArgb(99, 60, 220),
                Location = new Point(140, y + 10)
            };
            lnkBackToLogin.LinkClicked += LnkBackToLogin_LinkClicked;
            rightPanel.Controls.Add(lnkBackToLogin);

            this.AcceptButton = btnRegister;
        }

        private Label AddFieldLabel(Panel parent, string text, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(60, y)
            };
            parent.Controls.Add(lbl);
            return lbl;
        }

        private TextBox AddTextBox(Panel parent, string placeholder, int y, bool isPassword = false)
        {
            TextBox box = new TextBox
            {
                Text = placeholder,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(60, y),
                Size = new Size(400, 28),
                BorderStyle = BorderStyle.FixedSingle
            };
            box.Enter += (s, e) =>
            {
                if (box.Text == placeholder)
                {
                    box.Text = "";
                    box.ForeColor = Color.Black;
                    if (isPassword) box.UseSystemPasswordChar = true;
                }
            };
            box.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {
                    if (isPassword) box.UseSystemPasswordChar = false;
                    box.Text = placeholder;
                    box.ForeColor = Color.Gray;
                }
            };
            parent.Controls.Add(box);
            return box;
        }

        private bool IsPlaceholder(TextBox box, string placeholder) => box.Text == placeholder;

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            string fullName = IsPlaceholder(txtFullName, "Enter your full name") ? "" : txtFullName.Text.Trim();
            string email = IsPlaceholder(txtEmail, "Enter your email") ? "" : txtEmail.Text.Trim();
            string password = IsPlaceholder(txtPassword, "Enter your password") ? "" : txtPassword.Text;
            string confirmPassword = IsPlaceholder(txtConfirmPassword, "Re-enter your password") ? "" : txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Registration Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!email.Contains('@') || !email.Contains('.'))
            {
                MessageBox.Show("Please enter a valid email address.", "Registration Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Registration Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Registration Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            (bool ok, string message) = Database.RegisterUser(fullName, email, password);

            if (!ok)
            {
                MessageBox.Show(message, "Registration Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show($"Account created for {fullName}!\nYou can now log in.", "Registration Successful",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void LnkBackToLogin_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
