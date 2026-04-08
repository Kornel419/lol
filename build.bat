@echo off
setlocal enabledelayedexpansion

:: Lista potencjalnych sciezek do kompilatora CSC
set "paths[0]=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
set "paths[1]=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
set "paths[2]=C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe"
set "paths[3]=C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe"

set "CSC="

:: Szukanie dzialajacej sciezki
for /l %%i in (0,1,3) do (
    if exist "!paths[%%i]!" (
        set "CSC=!paths[%%i]!"
        goto :found
    )
)

:found
if "%CSC%"=="" (
    echo [BLAD] Nie znaleziono kompilatora csc.exe. 
    echo Upewnij sie, ze .NET Framework jest zainstalowany.
    pause
    exit /b
)

echo Znaleziono kompilator: %CSC%
echo Kompilacja ROFUCKED Malware...

:: Kompilacja
%CSC% /out:SysHost.exe /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll Program.cs

if exist SysHost.exe (
    echo.
    echo [SUKCES] Plik SysHost.exe zostal utworzony.
    echo Odpal jako Administrator na VM!
) else (
    echo.
    echo [BLAD] Kompilacja nie powiodla sie. Sprawdz bledy w kodzie Program.cs.
)

pause
