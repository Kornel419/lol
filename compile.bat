@echo off
title CHIPS SOURCE FIXER !@#$%^&*()_+
color 0e

echo [*] Sprawdzanie pliku source...

:: 1. Naprawa rozszerzenia (jeśli zapisałeś jako .txt)
if exist "source.cs.txt" (
    echo [!] Znaleziono source.cs.txt - poprawiam nazwe...
    ren "source.cs.txt" "source.cs"
)

:: 2. Sprawdzenie czy plik w ogóle istnieje
if not exist "source.cs" (
    echo [!] BLAD: Nadal nie widze pliku source.cs!
    echo [?] Upewnij sie, ze ten skrypt .bat jest w TYM SAMYM folderze co kod.
    dir /b
    pause
    exit
)

echo [OK] Plik source.cs znaleziony. Szukam kompilatora...

:: 3. Szukanie kompilatora w systemie
set "csc="
for /r "C:\Windows\Microsoft.NET\Framework64" %%f in (csc.exe) do (
    if exist "%%f" set "csc=%%f"
)
if not defined csc (
    for /r "C:\Windows\Microsoft.NET\Framework" %%f in (csc.exe) do (
        if exist "%%f" set "csc=%%f"
    )
)

if not defined csc (
    echo [!] Nie znaleziono kompilatora .NET!
    pause
    exit
)

echo [OK] Kompilator: %csc%
echo [*] Budowanie potwora CHIPS_OMEGA...

"%csc%" /target:winexe /out:CHIPS_OMEGA.exe /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll source.cs

if exist "CHIPS_OMEGA.exe" (
    echo.
    echo ========================================
    echo [!!!] SUKCES! Plik CHIPS_OMEGA.exe gotowy.
    echo [!!!] PAMIETAJ: URUCHOM JAKO ADMIN NA VM!
    echo ========================================
) else (
    echo [!] Kompilacja nie powiodla sie. Sprawdz bledy powyzej.
)
pause
