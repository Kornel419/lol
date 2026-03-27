@echo off
:: Przejdz do folderu, w ktorym jest ten BAT
cd /d "%~dp0"

set CSC_PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

if not exist "Idiot.cs" (
    echo [BLAD] Nie widze pliku Idiot.cs w tym folderze!
    echo Obecny folder to: %cd%
    pause
    exit
)

echo Kompilowanie...
%CSC_PATH% /target:winexe /out:Idiot.exe /unsafe /r:System.Windows.Forms.dll /r:System.Drawing.dll Idiot.cs

if %errorlevel%==0 (
    echo [PIKOBELO] Idiot.exe gotowy!
) else (
    echo [ZONK] Cos nie tak w kodzie.
)
pause
