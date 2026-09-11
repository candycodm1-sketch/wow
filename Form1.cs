using System;
using System.Drawing;
using System.Windows.Forms;

namespace VehicleRentalLogin
{
    public class Form1 : Form
    {
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private CheckBox chkRemember = null!;
        private Button btnLogin = null!;
        private LinkLabel lnkForgotPassword = null!;
        private LinkLabel lnkRegister = null!;

        // Placeholder tracking
        private const string EmailPlaceholder = "Enter your Email";
        private const string PasswordPlaceholder = "Enter your Password";

        // Demo constants removed — authentication now runs against the MySQL users table
        // (admin@drivehub.com / admin123 is seeded on first launch by Database.EnsureDatabase).

        public Form1()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Vehicle Rental & Booking System - Login";
            this.Size = new Size(900, 500);
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
                Location = new Point(20, 190)
            };
            leftPanel.Controls.Add(lblBrandTitle);

            Label lblBrandSubtitle = new Label
            {
                Text = "Easy booking, reliable vehicles,\nbetter journey.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(90, 90, 110),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(340, 50),
                Location = new Point(20, 290)
            };
            leftPanel.Controls.Add(lblBrandSubtitle);

            // ---------- RIGHT PANEL (form) ----------
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(60, 40, 60, 40)
            };
            this.Controls.Add(rightPanel);
            rightPanel.BringToFront();

            Label lblWelcome = new Label
            {
                Text = "Welcome Back!",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(60, 40)
            };
            rightPanel.Controls.Add(lblWelcome);

            Label lblSubtitle = new Label
            {
                Text = "Please login to your account",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(60, 75)
            };
            rightPanel.Controls.Add(lblSubtitle);

            // Email
            Label lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(60, 120)
            };
            rightPanel.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Text = EmailPlaceholder,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(60, 145),
                Size = new Size(400, 28),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtEmail.Enter += (s, e) => ClearPlaceholder(txtEmail, EmailPlaceholder);
            txtEmail.Leave += (s, e) => SetPlaceholder(txtEmail, EmailPlaceholder);
            rightPanel.Controls.Add(txtEmail);

            // Password
            Label lblPassword = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(60, 190)
            };
            rightPanel.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Text = PasswordPlaceholder,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(60, 215),
                Size = new Size(400, 28),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtPassword.Enter += (s, e) => ClearPlaceholder(txtPassword, PasswordPlaceholder, isPassword: true);
            txtPassword.Leave += (s, e) => SetPlaceholder(txtPassword, PasswordPlaceholder, isPassword: true);
            rightPanel.Controls.Add(txtPassword);

            // Remember me + Forgot password
            chkRemember = new CheckBox
            {
                Text = "Remember me",
                Font = new Font("Segoe UI", 9F),
                Checked = true,
                AutoSize = true,
                Location = new Point(60, 260)
            };
            rightPanel.Controls.Add(chkRemember);

            lnkForgotPassword = new LinkLabel
            {
                Text = "Forgot Password?",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                LinkColor = Color.FromArgb(99, 60, 220),
                Location = new Point(340, 260)
            };
            lnkForgotPassword.LinkClicked += LnkForgotPassword_LinkClicked;
            rightPanel.Controls.Add(lnkForgotPassword);

            // Login button
            btnLogin = new Button
            {
                Text = "Login",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(99, 60, 220),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(60, 300),
                Size = new Size(400, 40),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            rightPanel.Controls.Add(btnLogin);

            // Register link
            lnkRegister = new LinkLabel
            {
                Text = "Don't have an account? Register here",
                Font = new Font("Segoe UI", 9F),
                AutoSize = true,
                LinkColor = Color.FromArgb(99, 60, 220),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(140, 355)
            };
            lnkRegister.LinkClicked += LnkRegister_LinkClicked;
            rightPanel.Controls.Add(lnkRegister);

            this.AcceptButton = btnLogin;
        }

        private void ClearPlaceholder(TextBox box, string placeholder, bool isPassword = false)
        {
            if (box.Text == placeholder)
            {
                box.Text = "";
                box.ForeColor = Color.Black;
                if (isPassword) box.UseSystemPasswordChar = true;
            }
        }

        private void SetPlaceholder(TextBox box, string placeholder, bool isPassword = false)
        {
            if (string.IsNullOrWhiteSpace(box.Text))
            {
                if (isPassword) box.UseSystemPasswordChar = false;
                box.Text = placeholder;
                box.ForeColor = Color.Gray;
            }
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string email = txtEmail.Text == EmailPlaceholder ? "" : txtEmail.Text.Trim();
            string password = txtPassword.Text == PasswordPlaceholder ? "" : txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authenticate against the users table (admin@drivehub.com / admin123 is seeded).
            (bool ok, string message) = Database.ValidateLogin(email, password);

            if (ok)
            {
                DashboardForm dashboard = new DashboardForm();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(message,
                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LnkRegister_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.FormClosed += (s, args) => this.Show();
            registerForm.Show();
            this.Hide();
        }

        private void LnkForgotPassword_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPasswordForm forgotForm = new ForgotPasswordForm();
            forgotForm.FormClosed += (s, args) => this.Show();
            forgotForm.Show();
            this.Hide();
        }
    }
}
