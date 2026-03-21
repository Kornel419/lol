@echo off
color 0b
echo ========================================
echo   MINECRAFT ASSET BUILDER (DEBUG)
echo ========================================
echo.

:: Sprawdzenie czy plik Idiot.cs w ogole istnieje w tym folderze
if not exist "Idiot.cs" (
    echo [ERROR] I cannot find Idiot.cs in this folder!
    echo Put Idiot.cs and this .bat file in the same folder.
    pause
    exit
)

:: Reczne ustawienie sciezki do kompilatora (Najczestsza w Windows 7/10/11)
set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"

if not exist "%CSC%" (
    echo [RETRY] 64-bit compiler not found, trying 32-bit...
    set "CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
)

if not exist "%CSC%" (
    echo [ERROR] .NET Framework 4.0/4.5 is NOT INSTALLED on this VM.
    echo Please download and install .NET Framework 4.5.
    pause
    exit
)

echo [SYSTEM] Compiling... please wait...
echo.

:: Proba kompilacji
"%CSC%" /target:winexe /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:"Minecraft_Crack.exe" "Idiot.cs"

if %errorlevel% equ 0 (
    echo ========================================
    echo   SUCCESS! Minecraft_Crack.exe created!
    echo ========================================
) else (
    echo.
    echo [ERROR] Something is wrong inside Idiot.cs code.
    echo Check the error messages above.
)

echo.
echo Press any key to close this window...
pause >nul
