@echo off
title CHIPS COMPILER x64
color 0C
echo [*] Szukanie kompilatora .NET Framework...

:: Lokalizacja csc.exe dla .NET 4.0 (standard w Win 10/11)
set "csc=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"

if not exist "%csc%" (
    echo [!] BLAD: Nie znaleziono kompilatora .NET 4.0 x64!
    pause
    exit
)

echo [*] Kompilacja source.cs -> CHIPS_OMEGA.exe...
"%csc%" /target:winexe /out:CHIPS_OMEGA.exe /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll source.cs

if %errorlevel% neq 0 (
    echo [!] Blad podczas kompilacji! Sprawdz kod zrodlowy.
) else (
    echo [OK] Kompilacja zakonczona sukcesem!
    echo [!] PLIK: CHIPS_OMEGA.exe
)
pause
