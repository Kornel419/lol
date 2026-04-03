@echo off
title CHIPS XP SELF-EXTRACTOR
color 0e

:: --- SZUKANIE KOMPILATORA ---
set "csc="
for /r C:\Windows\Microsoft.NET\Framework %%f in (csc.exe) do set "csc=%%f"

if not defined csc (
    echo [!] BLAD: Brak .NET Framework. CHIPS nie moze wystartowac.
    pause
    exit
)

echo [*] CHIPS IS COMPILING...
echo [!] TARGET: WINDOWS XP x32

:: --- PISANIE KODU C# DO PLIKU TYMCZASOWEGO ---
echo using System; > CHIPS.cs
echo using System.Drawing; >> CHIPS.cs
echo using System.Windows.Forms; >> CHIPS.cs
echo using System.Runtime.InteropServices; >> CHIPS.cs
echo using System.Threading; >> CHIPS.cs
echo namespace Chips { >> CHIPS.cs
echo   class Program { >> CHIPS.cs
echo     [DllImport("user32.dll")] >> CHIPS.cs
echo     static extern IntPtr GetDC(IntPtr h); >> CHIPS.cs
echo     [DllImport("gdi32.dll")] >> CHIPS.cs
echo     static extern bool BitBlt(IntPtr d,int x,int y,int w,int h,IntPtr s,int sx,int sy,uint r); >> CHIPS.cs
echo     [DllImport("user32.dll")] >> CHIPS.cs
echo     static extern int GetSystemMetrics(int n); >> CHIPS.cs
echo     static void Main() { >> CHIPS.cs
echo       MessageBox.Show("XP EATEN BY CHIPS", "CHIPS", MessageBoxButtons.OK, MessageBoxIcon.Warning); >> CHIPS.cs
echo       new Thread(delegate() { >> CHIPS.cs
echo         IntPtr hdc = GetDC(IntPtr.Zero); >> CHIPS.cs
echo         int w = GetSystemMetrics(0), h = GetSystemMetrics(1); >> CHIPS.cs
echo         Random r = new Random(); >> CHIPS.cs
echo         while(true) { >> CHIPS.cs
echo           BitBlt(hdc, r.Next(-10,11), r.Next(-10,11), w, h, hdc, 0, 0, 0x00CC0020); >> CHIPS.cs
echo           Thread.Sleep(10); >> CHIPS.cs
echo         } >> CHIPS.cs
echo       }).Start(); >> CHIPS.cs
echo       Application.Run(new Form { Opacity = 0, ShowInTaskbar = false }); >> CHIPS.cs
echo     } >> CHIPS.cs
echo   } >> CHIPS.cs
echo } >> CHIPS.cs

:: --- KOMPILACJA ---
"%csc%" /target:winexe /out:CHIPS_ENGINE.exe /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll CHIPS.cs > nul

:: --- SPRZATANIE I START ---
if exist CHIPS_ENGINE.exe (
    del CHIPS.cs
    cls
    echo =================================
    echo   CHIPS XP GOTOWY DO PRACY
    echo =================================
    start CHIPS_ENGINE.exe
    echo [!] SKLADNIKI ZOSTALY "SCHRUPANE".
) else (
    echo [!] KOMPILACJA NIEUDANA.
    del CHIPS.cs
)
pause
