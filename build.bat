@echo off
set CSC="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
echo Kompilacja ROFUCKED Malware...
%CSC% /out:SysHost.exe /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll Program.cs
if exist SysHost.exe (
    echo [SUKCES] Plik SysHost.exe gotowy. Odpalaj tylko na VM ze snapshotem!
) else (
    echo [BLAD] Sprawdz sciezke do kompilatora.
)
pause
