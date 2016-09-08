# ExeToText

Utilities that changing PE files (exe, dll) to text file (.bat) and allow one click execution.
The tools help bypass terminals or kiosks security.

<b>ExeToBat</b> - Creating the batch file. <br />
<b>Keyboard Typer</b> - Automat typing of text file to different windows window.

<b>ExeToBat</b> - Nice utility so your files will become text.<br />
ExeToBat.exe [-d][-e] ExeFile [Output File] [Outcome File]
d = File is DLL (run with DllMain) and not Exe<br />
e = File is EXE (also default value)<br />
Exefile = File to be changed<br />
OutputFile = The new bat file<br />
Outcome = The file that will be created after running the BAT<br />
ExeToBat.exe -d "user32.dll" "mybypass.bat" "soliter.dll"<br />
ExeToBat.exe -e "explorer.exe" // Will make explorer.bat and explorer.exe

<b>KeyboardTyper</b> - Utility that "types" text to the focus window the ultimate terminal bypass<br />
KeyboardTyper.exe [-p] [-d time] textFilePath<br />
p = Debug Mode<br />
d = Time delay before stop typing (in miliseconds) - Default is 5 seconds<br />
textFilePath = the text file (from any kind) to be typed<br />

Enjoy
