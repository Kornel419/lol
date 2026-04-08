@echo off
setlocal enabledelayedexpansion

:: Szukanie kompilatora w różnych lokalizacjach
set "csc_path="
for /d %%D in (%SystemRoot%\Microsoft.NET\Framework*) do (
    for /d %%V in (%%D\v4*) do (
        if exist "%%V\csc.exe" set "csc_path=%%V\csc.exe"
    )
)

if not defined csc_path (
    echo [ERROR] Nie znaleziono kompilatora CSC (wymagany .NET 4.0+).
    pause
    exit /b
)

echo [INFO] Uzywam kompilatora: !csc_path!
echo [INFO] Kompilacja projektu ROFUCKED...

"!csc_path!" /out:SysHost.exe /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll Program.cs

if %errorlevel% equ 0 (
    echo [SUCCESS] Plik SysHost.exe gotowy.
) else (
    echo [FAILED] Blad kompilacji. Sprawdz kod Program.cs.
)
pause
