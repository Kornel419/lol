@echo off
title Doom Downloader & Runner 2026
cls

echo ===========================================
echo    DOOM AUTOMATYCZNY INSTALATOR (CMD)
echo ===========================================

:: 1. Tworzenie folderu na gre
set "DOOM_DIR=%USERPROFILE%\Desktop\DoomGame"
if not exist "%DOOM_DIR%" mkdir "%DOOM_DIR%"
cd /d "%DOOM_DIR%"

:: 2. Instalacja GZDoom przez Winget (jesli nie ma)
echo [*] Sprawdzanie silnika GZDoom...
winget list ZDoom.GZDoom >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo [*] Instalowanie GZDoom...
    winget install ZDoom.GZDoom --accept-source-agreements --accept-package-agreements
) else (
    echo [OK] GZDoom jest juz zainstalowany.
)

:: 3. Pobieranie pliku WAD (Demo)
echo [*] Pobieranie plikow gry (WAD)...
curl -L -o doom_demo.zip "https://archive.org/download/2020_03_22_DOOM/DOOM%20WADs/Doom%20(v1.9)%20(Demo).zip"

:: 4. Rozpakowanie pliku
echo [*] Rozpakowywanie...
powershell -command "Expand-Archive -Force -Path 'doom_demo.zip' -DestinationPath '.'"

:: 5. Wyciagniecie pliku WAD do glownego folderu
move "DOOM WADs\Doom (v1.9) (Demo)\DOOM1.WAD" . >nul 2>&1

:: 6. Uruchomienie gry
echo ===========================================
echo    GOTOWE! ODPALAM LADNEGO DOOMA...
echo ===========================================
timeout /t 3 >nul

start "" gzdoom.exe -iwad DOOM1.WAD

echo Jesli gra sie nie uruchomila, upewnij sie, ze GZDoom jest w twoim PATH.
pause
