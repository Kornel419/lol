@echo off
set CSC_PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

if not exist %CSC_PATH% (
    echo [ERROR] Nie znaleziono kompilatora .NET 4.0!
    pause
    exit
)

echo Kompilowanie Idiot.cs...
%CSC_PATH% /target:winexe /out:Idiot.exe /unsafe /r:System.Windows.Forms.dll /r:System.Drawing.dll Idiot.cs

if %errorlevel%==0 (
    echo [SUKCES] Plik Idiot.exe zostal utworzony!
) else (
    echo [BLAD] Kompilacja sie nie udala. Sprawdz bledy powyzej.
)
pause
