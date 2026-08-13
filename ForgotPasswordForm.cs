using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class ForgotPasswordForm : Form
    {
        private TextBox txtEmail = null!;
        private Button btnSendReset;
        private LinkLabel lnkBackToLogin = null!;

        public ForgotPasswordForm()
        {
            btnSendReset = new Button();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Vehicle Rental & Booking System - Forgot Password";
            this.Size = new Size(900, 450);
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
                Location = new Point(20, 170)
            };
            leftPanel.Controls.Add(lblBrandTitle);

            Label lblBrandSubtitle = new Label
            {
                Text = "We'll help you get back into\nyour account.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(90, 90, 110),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(340, 50),
                Location = new Point(20, 270)
            };
            leftPanel.Controls.Add(lblBrandSubtitle);

            // ---------- RIGHT PANEL ----------
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(60, 40, 60, 40)
            };
            this.Controls.Add(rightPanel);
            rightPanel.BringToFront();

            Label lblTitle = new Label
            {
                Text = "Forgot Password?",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(60, 50)
            };
            rightPanel.Controls.Add(lblTitle);

            Label lblSubtitle = new Label
            {
                Text = "Enter your email and we'll send you a link to reset your password.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                MaximumSize = new Size(400, 0),
                Location = new Point(60, 90)
            };
            rightPanel.Controls.Add(lblSubtitle);

            Label lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(60, 150)
            };
            rightPanel.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Text = "Enter your Email",
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(60, 175),
                Size = new Size(400, 28),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtEmail.Enter += (s, e) =>
            {
                if (txtEmail.Text == "Enter your Email")
                {
                    txtEmail.Text = "";
                    txtEmail.ForeColor = Color.Black;
                }
            };
            txtEmail.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    txtEmail.Text = "Enter your Email";
                    txtEmail.ForeColor = Color.Gray;
                }
            };
            rightPanel.Controls.Add(txtEmail);

            btnSendReset = new Button
            {
                Text = "Send Reset Link",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(99, 60, 220),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(60, 225),
                Size = new Size(400, 40),
                Cursor = Cursors.Hand
            };
            btnSendReset.FlatAppearance.BorderSize = 0;
            btnSendReset.Click += BtnSendReset_Click;
            rightPanel.Controls.Add(btnSendReset);

            lnkBackToLogin = new LinkLabel
            {
                Text = "Back to Login",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                LinkColor = Color.FromArgb(99, 60, 220),
                Location = new Point(220, 280)
            };
            lnkBackToLogin.LinkClicked += (s, e) => this.Close();
            rightPanel.Controls.Add(lnkBackToLogin);

            this.AcceptButton = btnSendReset;
        }

        private void BtnSendReset_Click(object? sender, EventArgs e)
        {
            string email = txtEmail.Text == "Enter your Email" ? "" : txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email) || !email.Contains('@') || !email.Contains('.'))
            {
                MessageBox.Show("Please enter a valid email address.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Hook this up to a real email/reset-token service.
            MessageBox.Show($"A password reset link has been sent to {email}.", "Check Your Email",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }
    }
}
