@echo off
cd /d "%~dp0"
title Idiot Compiler - HARD MODE

:: 1. Kopiujemy kompilator do Twojego folderu lol-main, zeby nie bylo problemow ze sciezkami
copy "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" "csc_local.exe" >nul
copy "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe.config" "csc_local.exe.config" >nul

echo [!] Kompilator skopiowany lokalnie do lol-main.
echo [!] Plik do kompilacji: Idiot.cs

:: 2. Odpalamy kompilacje z lokalnego pliku
csc_local.exe /target:winexe /optimize /out:Idiot.exe Idiot.cs

echo.
if exist Idiot.exe (
    echo [PIKOBELO] Idiot.exe stworzony pomyślnie!
    :: Sprzątamy po sobie
    del csc_local.exe
    del csc_local.exe.config
) else (
    echo [ERROR] Kompilacja nieudana. Sprawdz bledy powyzej.
)

pause
