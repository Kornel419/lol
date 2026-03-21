@echo off
title DBI Malware Builder
color 0c
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!!] PLEASE RUN AS ADMINISTRATOR [!!]
    pause
    exit
)

set "CSC="
if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not defined CSC if exist "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" set "CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

echo Compiling...
"%CSC%" /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:"Free Minecraft.exe" Idiot.cs

if exist "Free Minecraft.exe" (
    echo [OK] Done! "Free Minecraft.exe" is ready for your VM.
) else (
    echo [FAILED] Compilation error. Check Idiot.cs.
)
pause
