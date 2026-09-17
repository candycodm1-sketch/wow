using System;
using System.Drawing;
using System.IO;
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

        private const string EmailPlaceholder = "Enter your Email";
        private const string PasswordPlaceholder = "Enter your Password";

        private readonly Color PrimaryBlue = Color.FromArgb(48, 73, 181);
        private readonly Color DarkText = Color.FromArgb(17, 17, 17);
        private readonly Color SecondaryText = Color.FromArgb(110, 110, 110);
        private readonly Color BorderColor = Color.FromArgb(190, 190, 190);

        public Form1()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            Text = "DriveHub - Vehicle Rental System";
            ClientSize = new Size(1100, 680);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = true;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);

            // =========================
            // LEFT SIDE - FULL IMAGE
            // =========================

            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 520,
                BackColor = Color.FromArgb(241, 246, 255)
            };

            Controls.Add(leftPanel);

            PictureBox vehicleImage = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.FromArgb(241, 246, 255)
            };

            string vehicleArtwork = FindAssetByPrefix(
                "vehicle rental & booking system for"
            );

            if (!string.IsNullOrEmpty(vehicleArtwork))
            {
                using (Image original = Image.FromFile(vehicleArtwork))
                {
                    vehicleImage.Image = new Bitmap(original);
                }
            }

            leftPanel.Controls.Add(vehicleImage);

            // =========================
            // RIGHT SIDE
            // =========================

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            Controls.Add(rightPanel);
            rightPanel.BringToFront();

            Panel loginContainer = new Panel
            {
                Size = new Size(430, 500),
                Location = new Point(75, 80),
                BackColor = Color.White
            };

            rightPanel.Controls.Add(loginContainer);

            // =========================
            // WELCOME
            // =========================

            Label lblWelcome = new Label
            {
                Text = "Welcome Back!",
                Font = new Font("Segoe UI", 25F, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            loginContainer.Controls.Add(lblWelcome);

            Label lblSubtitle = new Label
            {
                Text = "Please login to your account",
                Font = new Font("Segoe UI", 11F),
                ForeColor = SecondaryText,
                AutoSize = true,
                Location = new Point(2, 42)
            };

            loginContainer.Controls.Add(lblSubtitle);

            // =========================
            // EMAIL LABEL
            // =========================

            Label lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = DarkText,
                AutoSize = true,
                Location = new Point(2, 88)
            };

            loginContainer.Controls.Add(lblEmail);

            // =========================
            // EMAIL INPUT
            // =========================

            Panel emailPanel = CreateInputPanel();

            emailPanel.Location = new Point(0, 116);
            emailPanel.Size = new Size(430, 55);

            PictureBox emailIcon = CreateIcon("email.png");

            emailIcon.Location = new Point(12, 15);
            emailIcon.Size = new Size(24, 24);

            emailPanel.Controls.Add(emailIcon);

            txtEmail = new TextBox
            {
                Text = EmailPlaceholder,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.None,
                Location = new Point(48, 15),
                Size = new Size(365, 25)
            };

            txtEmail.Enter += (s, e) =>
            {
                if (txtEmail.Text == EmailPlaceholder)
                {
                    txtEmail.Text = "";
                    txtEmail.ForeColor = Color.Black;
                }
            };

            txtEmail.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    txtEmail.Text = EmailPlaceholder;
                    txtEmail.ForeColor = Color.Gray;
                }
            };

            emailPanel.Controls.Add(txtEmail);
            loginContainer.Controls.Add(emailPanel);

            // =========================
            // PASSWORD LABEL
            // =========================

            Label lblPassword = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = DarkText,
                AutoSize = true,
                Location = new Point(2, 188)
            };

            loginContainer.Controls.Add(lblPassword);

            // =========================
            // PASSWORD INPUT
            // =========================

            Panel passwordPanel = CreateInputPanel();

            passwordPanel.Location = new Point(0, 216);
            passwordPanel.Size = new Size(430, 55);

            PictureBox passwordIcon = CreateIcon("password.png");

            passwordIcon.Location = new Point(12, 15);
            passwordIcon.Size = new Size(24, 24);

            passwordPanel.Controls.Add(passwordIcon);

            txtPassword = new TextBox
            {
                Text = PasswordPlaceholder,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.None,
                Location = new Point(48, 15),
                Size = new Size(320, 25)
            };

            txtPassword.Enter += (s, e) =>
            {
                if (txtPassword.Text == PasswordPlaceholder)
                {
                    txtPassword.Text = "";
                    txtPassword.ForeColor = Color.Black;
                    txtPassword.UseSystemPasswordChar = true;
                }
            };

            txtPassword.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    txtPassword.UseSystemPasswordChar = false;
                    txtPassword.Text = PasswordPlaceholder;
                    txtPassword.ForeColor = Color.Gray;
                }
            };

            passwordPanel.Controls.Add(txtPassword);

            // =========================
            // SHOW PASSWORD
            // =========================

            PictureBox showPasswordIcon = CreateIcon("showpassword.png");

            showPasswordIcon.Location = new Point(390, 15);
            showPasswordIcon.Size = new Size(24, 24);
            showPasswordIcon.Cursor = Cursors.Hand;

            showPasswordIcon.Click += (s, e) =>
            {
                if (txtPassword.Text != PasswordPlaceholder)
                {
                    txtPassword.UseSystemPasswordChar =
                        !txtPassword.UseSystemPasswordChar;
                }
            };

            passwordPanel.Controls.Add(showPasswordIcon);

            loginContainer.Controls.Add(passwordPanel);

            // =========================
            // REMEMBER ME
            // =========================

            chkRemember = new CheckBox
            {
                Text = "Remember me",
                Font = new Font("Segoe UI", 10F),
                ForeColor = SecondaryText,
                AutoSize = true,
                Location = new Point(0, 288),
                Cursor = Cursors.Hand
            };

            loginContainer.Controls.Add(chkRemember);

            // =========================
            // FORGOT PASSWORD
            // =========================

            lnkForgotPassword = new LinkLabel
            {
                Text = "Forgot Password?",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                LinkColor = PrimaryBlue,
                ActiveLinkColor = PrimaryBlue,
                VisitedLinkColor = PrimaryBlue,
                Location = new Point(303, 289),
                Cursor = Cursors.Hand
            };

            lnkForgotPassword.LinkClicked += LnkForgotPassword_LinkClicked;

            loginContainer.Controls.Add(lnkForgotPassword);

            // =========================
            // LOGIN BUTTON
            // =========================

            btnLogin = new Button
            {
                Text = "Login",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = PrimaryBlue,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(430, 53),
                Location = new Point(0, 330),
                Cursor = Cursors.Hand
            };

            btnLogin.FlatAppearance.BorderSize = 0;

            btnLogin.MouseEnter += (s, e) =>
            {
                btnLogin.BackColor = Color.FromArgb(39, 61, 155);
            };

            btnLogin.MouseLeave += (s, e) =>
            {
                btnLogin.BackColor = PrimaryBlue;
            };

            btnLogin.Click += BtnLogin_Click;

            loginContainer.Controls.Add(btnLogin);

            // =========================
            // REGISTER
            // =========================

            lnkRegister = new LinkLabel
            {
                Text = "Don't have an account? Register here",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                LinkColor = PrimaryBlue,
                ActiveLinkColor = PrimaryBlue,
                VisitedLinkColor = PrimaryBlue,
                Location = new Point(108, 405),
                Cursor = Cursors.Hand
            };

            lnkRegister.LinkClicked += LnkRegister_LinkClicked;

            loginContainer.Controls.Add(lnkRegister);

            AcceptButton = btnLogin;
        }

        // =========================
        // INPUT PANEL
        // =========================

        private Panel CreateInputPanel()
        {
            Panel panel = new Panel
            {
                BackColor = Color.White
            };

            panel.Paint += (s, e) =>
            {
                using Pen pen = new Pen(BorderColor, 1);

                e.Graphics.DrawRectangle(
                    pen,
                    0,
                    0,
                    panel.Width - 1,
                    panel.Height - 1
                );
            };

            return panel;
        }

        // =========================
        // LOAD ICON
        // =========================

        private PictureBox CreateIcon(string fileName)
        {
            PictureBox pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            string path = FindAsset(fileName);

            if (!string.IsNullOrEmpty(path))
            {
                using (Image original = Image.FromFile(path))
                {
                    pictureBox.Image = new Bitmap(original);
                }
            }

            return pictureBox;
        }

        // =========================
        // FIND ASSET
        // =========================

        private string FindAsset(string fileName)
        {
            string current = Application.StartupPath;

            for (int i = 0; i < 6; i++)
            {
                string assetsPath = Path.Combine(
                    current,
                    "Assets",
                    fileName
                );

                if (File.Exists(assetsPath))
                    return assetsPath;

                DirectoryInfo? parent = Directory.GetParent(current);

                if (parent == null)
                    break;

                current = parent.FullName;
            }

            return "";
        }

        // =========================
        // FIND LOGIN ARTWORK
        // =========================

        private string FindAssetByPrefix(string prefix)
        {
            string current = Application.StartupPath;

            for (int i = 0; i < 6; i++)
            {
                string assetsFolder = Path.Combine(
                    current,
                    "Assets"
                );

                if (Directory.Exists(assetsFolder))
                {
                    string[] files = Directory.GetFiles(
                        assetsFolder
                    );

                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);

                        if (fileName.StartsWith(
                            prefix,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            return file;
                        }
                    }
                }

                DirectoryInfo? parent = Directory.GetParent(current);

                if (parent == null)
                    break;

                current = parent.FullName;
            }

            return "";
        }

        // =========================
        // LOGIN
        // =========================

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string email =
                txtEmail.Text == EmailPlaceholder
                    ? ""
                    : txtEmail.Text.Trim();

            string password =
                txtPassword.Text == PasswordPlaceholder
                    ? ""
                    : txtPassword.Text;

            if (string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Please enter both email and password.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            (bool ok, string message) =
                Database.ValidateLogin(
                    email,
                    password
                );

            if (ok)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    Database.CurrentUserName = message;
                }

                DashboardForm dashboard =
                    new DashboardForm();

                dashboard.Show();

                Hide();
            }
            else
            {
                MessageBox.Show(
                    message,
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // REGISTER
        // =========================

        private void LnkRegister_LinkClicked(
            object? sender,
            LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm registerForm =
                new RegisterForm();

            registerForm.FormClosed += (s, args) =>
            {
                Show();
            };

            registerForm.Show();

            Hide();
        }

        // =========================
        // FORGOT PASSWORD
        // =========================

        private void LnkForgotPassword_LinkClicked(
            object? sender,
            LinkLabelLinkClickedEventArgs e)
        {
            ForgotPasswordForm forgotForm =
                new ForgotPasswordForm();

            forgotForm.FormClosed += (s, args) =>
            {
                Show();
            };

            forgotForm.Show();

            Hide();
        }
    }
}