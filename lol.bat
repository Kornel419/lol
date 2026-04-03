@echo off
cls
title XP CHIPS COMPILER 2026
color 0e

echo [*] SZUKANIE KOMPILATORA NA DYSKU...
dir /s /b %SystemRoot%\Microsoft.NET\Framework\csc.exe > temp_path.txt
set /p CSC_PATH=<temp_path.txt
del temp_path.txt

if "%CSC_PATH%"=="" (
    echo [!] BLAD: Nie znaleziono .NET Framework! 
    echo Musisz zainstalować .NET Framework 2.0 lub 3.5 na tym XP.
    pause
    exit
)

echo [+] ZNALAZIONO: %CSC_PATH%
echo [*] KOMPILACJA CHIPS...

"%CSC_PATH%" /target:winexe /out:ChipsXP.exe /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll Program.cs

if %errorlevel%==0 (
    echo ========================================
    echo   SUKCES! PLIK ChipsXP.exe GOTOWY.
    echo ========================================
) else (
    echo [!] COS POSZLO NIE TAK W KODZIE C#.
)
pause
