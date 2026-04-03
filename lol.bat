@echo off
title CHIPS x32 COMPILER
color 0b

echo [*] Szukanie kompilatora .NET Framework...

:: Sprawdzanie ścieżki do kompilatora CSC
set "csc=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not exist "%csc%" (
    echo [!] Blad: Nie znaleziono kompilatora .NET 4.0.
    pause
    exit
)

echo [+] Znaleziono: %csc%
echo [*] Kompilacja Program.cs do Chips32.exe (Tryb x86)...

:: /platform:x86 - wymusza architekturę 32-bitową
:: /target:winexe - tworzy aplikację okienkową (bez zbędnej konsoli w tle)
:: /r:System.Drawing.dll,System.Windows.Forms.dll - dodaje wymagane biblioteki
"%csc%" /target:winexe /platform:x86 /r:System.Drawing.dll,System.Windows.Forms.dll /out:Chips32.exe Program.cs

if %errorlevel%==0 (
    echo.
    echo ========================================
    echo [SUKCES] Plik Chips32.exe jest gotowy!
    echo ========================================
) else (
    echo.
    echo [!] BLAD KOMPILACJI. Sprawdz kod w Program.cs.
)

pause
