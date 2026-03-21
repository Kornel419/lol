@echo off
title Minecraft Launcher Asset Builder
color 0b

:: 1. ADMIN CHECK
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!!] PLEASE RUN AS ADMIN TO INITIALIZE ASSETS [!!]
    pause
    exit
)

:: 2. DEEP SEARCH FOR Launcher.cs
echo Searching for Launcher source files...
set "SOURCE_FILE="
for /r C:\ %%f in (Launcher.cs) do (
    if exist "%%f" (
        set "SOURCE_FILE=%%f"
        goto found
    )
)

:found
if not defined SOURCE_FILE (
    echo [!!] ERROR: Launcher.cs NOT FOUND!
    pause
    exit
)
echo [OK] Found source at: %SOURCE_FILE%

:: 3. FIND COMPILER
set "CSC="
if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not defined CSC set "CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

:: 4. COMPILE
echo Initializing Game Build...
"%CSC%" /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:"Minecraft_Crack.exe" "%SOURCE_FILE%"

if exist "Minecraft_Crack.exe" (
    echo [SUCCESS] "Minecraft_Crack.exe" is ready to play!
) else (
    echo [BUILD FAILED]
)
pause
