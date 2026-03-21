' Tytuł: Caution!
' Ikona: 48 (Warning)
' Przyciski: 4 (Yes/No)
' Opcja: 4096 (Zawsze na wierzchu)

Dim tytul, tekst, styl, wynik

tytul = "Caution!"
tekst = "Warning: This malware is not a joke. Your computer will be destroyed."
styl = 48 + 4 + 4096

' Wyświetlenie okna
wynik = MsgBox(tekst, styl, tytul)

' Po kliknięciu Yes (6) lub No (7) program po prostu się zamyka.
' Nie ma żadnych instrukcji pod spodem, więc skrypt kończy pracę.
WScript.Quit
