@echo off
title COMPILER DEBUG
cd /d "%~dp0"

echo [1] Checking for Idiot.cs...
if not exist "Idiot.cs" echo ERROR: Idiot.cs not found! & pause & exit

echo [2] Searching for Compiler...
set "C=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%C%" set "C=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
if not exist "%C%" echo ERROR: .NET Framework not found! & pause & exit

echo [3] Compiling...
"%C%" /target:winexe /out:"Minecraft_Crack.exe" "Idiot.cs"

echo [4] Check result:
if exist "Minecraft_Crack.exe" (
    echo SUCCESS! Minecraft_Crack.exe is ready.
) else (
    echo FAILED! Check your Idiot.cs code for errors.
)
pause
