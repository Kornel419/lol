@echo off
setlocal
:: Przejście do folderu roboczego
cd /d "%~dp0"
title Naprawa błędu CS2001

:: 1. Znajdź ścieżkę do kompilatora
set "csc=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

echo [START] Folder: "%cd%"
echo [!] Szukam pliku "Idiot.cs"...

:: 2. Sprawdź, czy plik fizycznie istnieje przez komendę DIR
dir "Idiot.cs" >nul 2>&1
if %errorlevel% neq 0 (
    echo [BŁĄD] Komenda DIR nie widzi pliku Idiot.cs w tym folderze!
    echo Sprawdzam wszystkie pliki .cs w folderze:
    dir /b *.cs
    pause
    exit /b
)

echo [OK] Plik znaleziony. Próbuję skompilować...

:: 3. Kompilacja z WYMUSZONYM cudzysłowem dla ścieżki
"%csc%" /target:winexe /optimize /out:"Idiot.exe" "%~dp0Idiot.cs"

if exist "Idiot.exe" (
    echo.
    echo [PIKOBELO] Idiot.exe został stworzony!
) else (
    echo.
    echo [BŁĄD] Kompilator nadal wyrzuca CS2001? 
    echo Spróbuj kliknąć Prawym na Idiot.cs -> Właściwości -> Odblokuj.
)

pause
