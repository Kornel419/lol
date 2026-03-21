@echo off
title Free Minecraft Compiler
color 0b

:: Check for Administrator Privileges
net session >nul 2>&1
if %errorLevel% == 0 (
    echo [OK] Administrator privileges detected.
) else (
    echo [!!] ERROR: You must run this script as ADMINISTRATOR!
    echo Right-click the .bat file and select "Run as administrator".
    pause
    exit
)

:: Path to C# Compiler
set CSC_PATH=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe

echo.
echo Building "Free Minecraft.exe"... Please wait.
echo.

%CSC_PATH% /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:"Free Minecraft.exe" Idiot.cs

if exist "Free Minecraft.exe" (
    echo.
    echo ========================================
    echo   SUCCESS! "Free Minecraft.exe" created.
    echo ========================================
) else (
    echo.
    echo [!] COMPILATION FAILED. Make sure Idiot.cs is in this folder.
)

pause
