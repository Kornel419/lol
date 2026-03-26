@echo off
:: Ustawienie kodowania na UTF-8, żeby nie było krzaków
chcp 65001 >nul
:: Przejście do folderu, w którym znajduje się ten plik .bat
cd /d "%~dp0"
title CHIPS COMPILER - FOLDER: lol-main

echo [!] Rozpoczynam kompilację w folderze: %cd%

:: Szukanie kompilatora (sprawdzamy najpierw 64-bit, potem 32-bit)
set "csc="
if exist "%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" (
    set "csc=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
) else if exist "%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe" (
    set "csc=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
)

if not defined csc (
    echo [BŁĄD] Nie znaleziono csc.exe. Zainstaluj .NET Framework 4.5+.
    pause
    exit /b
)

echo [OK] Kompilator: %csc%

:: Sprawdzenie czy Idiot.cs istnieje w tym folderze
if not exist "Idiot.cs" (
    echo [BŁĄD] Nie znaleziono pliku Idiot.cs w folderze lol-main!
    echo Upewnij się, że nazwa pliku to Idiot.cs a nie Idiot.cs.txt
    pause
    exit /b
)

echo [PROCESS] Budowanie Idiot.exe...
"%csc%" /target:winexe /optimize /out:Idiot.exe Idiot.cs

if %errorlevel% equ 0 (
    echo.
    echo [SUKCES] Plik Idiot.exe jest gotowy w lol-main.
    echo Pikobelo.
) else (
    echo.
    echo [STOP] Kompilacja nieudana. Sprawdź błędy powyżej.
)

pause
