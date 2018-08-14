@ECHO OFF

REM The following directory is for .NET 4
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%

echo Uninstalling Xoom.BackgroundService...
echo ---------------------------------------------------
InstallUtil /u "%~dp0\EMPower.QnA.BackgroundServices.exe"
echo ---------------------------------------------------
echo Finished uninstalling Xoom.BackgroundService
echo ---------------------------------------------------

pause