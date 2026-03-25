@echo off
color 0b
title MEMZ BUILDER v2.0
net session >nul 2>&1 || (echo PLEASE RUN AS ADMIN! & pause & exit)

echo [SEARCH] Looking for Idiot.cs...
set "FILE="
for %%d in (C D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
    if exist %%d:\ (
        for /r %%d:\ %%f in (Idiot.cs) do if exist "%%f" set "FILE=%%f" & goto :found
    )
)

:found
if not defined FILE (
    echo [ERROR] Idiot.cs not found on any drive!
    pause
    exit
)

echo [OK] Found at: %FILE%
set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

echo [BUILD] Compiling...
"%CSC%" /target:winexe /out:"Final_Payload.exe" "%FILE%"

if exist "Final_Payload.exe" (
    echo.
    echo ========================================
    echo   SUCCESS: Final_Payload.exe created!
    echo ========================================
) else (
    echo [FAIL] Compilation error.
)
pause
