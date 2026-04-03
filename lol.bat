@echo off
title CHIPS x32 XP COMPILER
color 0a

echo [*] Szukanie kompilatora .NET na Windows XP...

:: Sprawdzamy najpierw wersje 3.5, potem 2.0 (najczestsze na XP)
set "csc="
if exist "C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe" set "csc=C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe"
if not defined csc if exist "C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe" set "csc=C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe"
if not defined csc if exist "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" set "csc=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not defined csc (
    echo [!] BLAD: Nie znaleziono zadnego .NET Framework!
    echo Zainstaluj .NET Framework 2.0 lub 3.5 Redistributable.
    pause
    exit
)

echo [+] Znaleziono kompilator: %csc%
echo [*] Kompilacja dla Windows XP (x32)...

:: Na XP komenda jest prostsza, nie potrzebujemy wymuszania platformy x86, bo XP i tak jest x32
"%csc%" /target:winexe /out:ChipsXP.exe Program.cs /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll

if %errorlevel%==0 (
    echo.
    echo ========================================
    echo   [SUKCES] ChipsXP.exe zostal utworzony!
    echo ========================================
) else (
    echo.
    echo [!] BLAD KOMPILACJI! Sprawdz kod w Program.cs.
)

pause
