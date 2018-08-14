@ECHO OFF

REM The following directory is for .NET 4
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%

echo Installing EMPower.QnA.BackgroundServices...
echo ---------------------------------------------------
InstallUtil "%~dp0\EMPower.QnA.BackgroundServices.exe"
echo ---------------------------------------------------
echo Finished installing EMPower.QnA.BackgroundServices
echo ---------------------------------------------------
echo Starting service EMPower.QnA.BackgroundServices
sc start EMPower.QnA.BackgroundServices
pause