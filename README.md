# ExeToText

Utilities that changing PE files (exe, dll) to a text file (.bat) and allows one click execution.
This tools help bypass terminals or kiosks security.

/* Tools */ <br />
<b>ExeToBat</b> - Creating the one-click-execution batch file. <br />
<b>Keyboard Typer</b> - Automatic typing of a text file to different window [windows only].

/* Examples */ <br />
<b>ExeToBat</b> - Utility to convert your PE files to a one-click execution text file<br />
ExeToBat.exe [-e][-d] ExeFile [Output File] [Outcome File] <br />
e = File is EXE (Default value)<br />
d = File is DLL (Execute the DllMain function)<br />
Exefile = File to be changed<br />
OutputFile = The new bat file<br />
Outcome = The file that will be created after running the BAT<br />
ExeToBat.exe -d "user32.dll" "mybypass.bat" "soliter.dll"<br />
ExeToBat.exe -e "explorer.exe" // Will make explorer.bat and explorer.exe

<b>KeyboardTyper</b> - Utility that "types" text to a focus window<br />
KeyboardTyper.exe [-p] [-d time] textFilePath<br />
p = Debug Mode<br />
d = Time delay before stop typing (in miliseconds) - Default is 5 seconds<br />
textFilePath = the text file to be typed<br />

Enjoy
