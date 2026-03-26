@echo off
:: Wymuszenie folderu, w którym jest ten plik .bat
cd /d "%~dp0"
title Kompilator Idiot.cs

:: Sciezka do kompilatora
set "csc=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

echo [INFO] Folder roboczy: %cd%
echo [INFO] Szukam pliku: Idiot.cs

if not exist "Idiot.cs" (
    echo [ERROR] Nie widze pliku Idiot.cs! 
    echo Upewnij sie, ze nazwa to Idiot.cs a nie Idiot.cs.txt
    dir /b
    pause
    exit
)

echo [PROCESS] Kompilacja...
"%csc%" /target:winexe /optimize /out:Idiot.exe Idiot.cs

if exist "Idiot.exe" (
    echo [SUKCES] Pikobelo! Plik Idiot.exe stworzony.
) else (
    echo [BLAD] Kompilacja sie nie udala.
)

pause
