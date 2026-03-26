@echo off
setlocal enabledelayedexpansion
title CHIPS COMPILER v2.0

echo [!] Szukanie kompilatora csc.exe w zasobach systemowych...

:: Sprawdzanie najpopularniejszych lokalizacji .NET 4.0 (standard w Win 10/11)
set "csc_path=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not exist "!csc_path!" (
    echo [?] Nie znaleziono w v4.0, szukam w innych wersjach...
    for /r "%SystemRoot%\Microsoft.NET\Framework" %%i in (csc.exe) do (
        set "csc_path=%%i"
    )
)

if not exist "!csc_path!" (
    echo [ERROR] Brak kompilatora C# w systemie. 
    echo Zainstaluj .NET Framework lub sprawdz uprawnienia.
    pause
    exit /b
)

echo [OK] Kompilator znaleziony: "!csc_path!"
echo [PROCESS] Budowanie Idiot.exe...

:: Kompilacja z flagami:
:: /target:winexe - ukrywa okno konsoli po odpaleniu Idiot.exe
:: /optimize - sprawia, ze kod dziala szybciej (wazne przy GDI)
:: /platform:x86 - wymusza 32-bit dla lepszej kompatybilnosci z MBR
"!csc_path!" /target:winexe /optimize /platform:x86 /out:Idiot.exe Idiot.cs

if %errorlevel% equ 0 (
    echo.
    echo [PIKOBELO] Kompilacja zakonczona sukcesem.
    echo Plik: %~dp0Idiot.exe
) else (
    echo.
    echo [BŁĄD] Kompilacja sie wywalila. Sprawdz czy:
    echo 1. Plik Idiot.cs jest w tym samym folderze.
    echo 2. Nie masz bledu w kodzie (literowki).
    echo 3. Antywirus nie zablokowal zapisu EXE.
)

pause
