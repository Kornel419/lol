@echo off
title ROFUCKED SYSTEM - FINAL ANIMATION
mode con cols=80 lines=25
color 0a
cls
echo Computer have been BRICKED.
echo Enjoy "Animation"
echo.
echo Preparing Graphics Engine...
timeout /t 3 >nul

:: Wywołanie hybrydowej animacji (PowerShell narysuje rybę)
powershell -NoProfile -ExecutionPolicy Bypass -Command "^
$f = New-Object Windows.Forms.Form; ^
$f.Text = 'ROFUCKED_FISH'; ^
$f.WindowState = 'Maximized'; ^
$f.FormBorderStyle = 'None'; ^
$f.BackColor = 'Black'; ^
$f.TopMost = $true; ^
$pb = New-Object Windows.Forms.PictureBox; ^
$pb.Dock = 'Fill'; ^
$f.Controls.Add($pb); ^
$bmp = New-Object Drawing.Bitmap($f.Width, $f.Height); ^
$g = [Drawing.Graphics]::FromImage($bmp); ^
$timer = New-Object Windows.Forms.Timer; ^
$timer.Interval = 30; ^
$angle = 0; ^
$timer.Add_Tick({ ^
    $g.Clear([Drawing.Color]::Black); ^
    $g.SmoothingMode = 'AntiAlias'; ^
    $g.TranslateTransform($f.Width/2, $f.Height/2); ^
    $angle += 15; ^
    $g.RotateTransform($angle); ^
    $brush = New-Object Drawing.SolidBrush([Drawing.Color]::Gold); ^
    $g.FillEllipse($brush, -150, -75, 300, 150); ^
    $points = @( (New-Object Drawing.Point(100, 0)), (New-Object Drawing.Point(220, -80)), (New-Object Drawing.Point(220, 80)) ); ^
    $g.FillPolygon($brush, $points); ^
    $g.FillEllipse([Drawing.Brushes]::Black, -100, -30, 20, 20); ^
    $pb.Image = $bmp; ^
}); ^
$timer.Start(); ^
$f.Add_KeyDown({ if ($_.KeyCode -eq 'Escape') { $f.Close() } }); ^
$f.ShowDialog();"

:: Jeśli ktoś zamknie animację, wracamy do pętli
goto :loop
