@echo off
title Free Minecraft Compiler - DBI Edition
color 0b

:: 1. CHECK FOR ADMIN RIGHTS
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!!] ERROR: PLEASE RUN AS ADMINISTRATOR [!!]
    echo Right-click this file and select "Run as administrator".
    pause
    exit
)

:: 2. DETECT COMPILER
echo Searching for C# Compiler...
set "CSC="
if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" (
    set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
) else if exist "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" (
    set "CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
)

if not defined CSC (
    echo [!!] ERROR: C# Compiler not found! 
    echo Please make sure .NET Framework 4.0/4.5 is installed.
    pause
    exit
)

:: 3. CHECK FOR SOURCE FILE
if not exist "Idiot.cs" (
    echo [!!] ERROR: "Idiot.cs" NOT FOUND!
    echo Make sure your C# code is saved as "Idiot.cs" in this folder.
    pause
    exit
)

:: 4. COMPILATION
echo [OK] Compiler found. Building "Free Minecraft.exe"...
echo.

"%CSC%" /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:"Free Minecraft.exe" Idiot.cs

if exist "Free Minecraft.exe" (
    echo.
    echo ==============================================
    echo   SUCCESS! "Free Minecraft.exe" is ready.
    echo ==============================================
) else (
    echo.
    echo [!!] COMPILATION FAILED. Check for errors in your C# code.
)

pause
