@echo off
title Idiot Compiler
set "csc=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not exist "%csc%" (
    echo [!] Nie widze kompilatora w glownym folderze v4.0.
    echo [!] Szukam gdziekolwiek...
    for /r C:\Windows\Microsoft.NET\Framework %%i in (csc.exe) do set "csc=%%i"
)

echo Kompilator: %csc%
echo Plik: Idiot.cs

"%csc%" /target:winexe /optimize /out:Idiot.exe Idiot.cs

echo.
if exist Idiot.exe (
    echo [PIKOBELO] Sukces! Idiot.exe gotowy.
) else (
    echo [BLAD] Cos poszlo nie tak. Przeczytaj bledy powyzej.
)
pause
