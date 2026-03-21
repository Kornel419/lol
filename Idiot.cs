@echo off
title DEBUG MODE - Minecraft Builder
color 0b

echo ============================================
echo   STEP 1: Checking Administrator Rights
echo ============================================
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!!] ERROR: NOT RUNNING AS ADMIN.
    echo Please right-click this file and select 'Run as Administrator'.
    echo.
    pause
    exit
)
echo [OK] Admin rights confirmed.
echo.

echo ============================================
echo   STEP 2: Searching for Idiot.cs
echo ============================================
:: Szukamy tylko w bieżącym folderze i podfolderach, żeby nie trwało to wieki
set "SOURCE="
for /r . %%f in (Idiot.cs) do (
    if exist "%%f" (
        set "SOURCE=%%f"
        goto :found_source
    )
)

:found_source
if not defined SOURCE (
    echo [!!] ERROR: Idiot.cs NOT FOUND in this folder or subfolders!
    echo Current directory is: %cd%
    echo.
    pause
    exit
)
echo [OK] Found Idiot.cs at: %SOURCE%
echo.

echo ============================================
echo   STEP 3: Searching for Compiler (csc.exe)
echo ============================================
set "CSC="
:: Próbujemy najczęstsze lokalizacje
if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not defined CSC if exist "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" set "CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not defined CSC (
    echo [!!] ERROR: Compiler (csc.exe) not found in standard folders.
    echo Searching everywhere... (This may take a minute)
    for /r C:\Windows\Microsoft.NET\Framework %%i in (csc.exe) do (
        if exist "%%i" set "CSC=%%i"
    )
)

if not defined CSC (
    echo [!!] ERROR: STILL NO COMPILER. .NET Framework is missing!
    echo.
    pause
    exit
)
echo [OK] Found Compiler at: %CSC%
echo.

echo ============================================
echo   STEP 4: Compiling Minecraft_Crack.exe
echo ============================================
"%CSC%" /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:"Minecraft_Crack.exe" "%SOURCE%"

if %errorLevel% neq 0 (
    echo.
    echo [!!] COMPILATION FAILED! Look at the errors above.
) else (
    echo.
    echo [SUCCESS] Minecraft_Crack.exe has been created!
)

echo.
echo Press any key to exit...
pause >nul
