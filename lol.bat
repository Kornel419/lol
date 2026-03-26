@echo off
setlocal enabledelayedexpansion

echo [CHIPS COMPILER] Szukanie csc.exe...

:: Szukanie najnowszej wersji kompilatora .NET
set "csc="
for /d %%D in (%SystemRoot%\Microsoft.NET\Framework\v4*) do (
    if exist "%%D\csc.exe" set "csc=%%D\csc.exe"
)

if not defined csc (
    echo [ERROR] Nie znaleziono kompilatora .NET Framework 4.0+.
    pause
    exit /b
)

echo [OK] Znaleziono: !csc!
echo [PROCESS] Kompilacja Idiot.cs -> Idiot.exe...

:: Kompilacja: /target:winexe (brak konsoli), /out:nazwa pliku
"!csc!" /target:winexe /out:Idiot.exe Idiot.cs

if %errorlevel% equ 0 (
    echo [SUCCESS] Plik Idiot.exe zostal utworzony w tym folderze.
    echo Pikobelo.
) else (
    echo [FAILED] Blad podczas kompilacji. Sprawdz kod Idiot.cs.
)

pause
