#include <windows.h>
#include <string>
#include <iostream>

#include <fstream>
using namespace std;

VOID writeLetter(HKL layout, CHAR chr);
void argsWorking(int argc, char ** argv);
void finalExit(string error);


BOOL DEBUG = FALSE;
DWORD SLEEP = 5000;
WORD DELAY = 8;

char * fileName;

void main(int argc, char ** argv)
{
	cout << "### Keyboard Writer - Version 2.0 ###" << endl;
	argsWorking(argc, argv);

	ifstream myBatFile;
	myBatFile.open(fileName);

	if (myBatFile.fail() == true) finalExit("File Error");

	HKL kbl = GetKeyboardLayout(0);
	WORD counter = 0;
	Sleep(SLEEP);

	for (std::string line; getline(myBatFile, line); )
	{
		for each (char chr in line)
		{
			if (chr > 0)
			{
				writeLetter(kbl, chr);
				counter++;
				if (counter == DELAY)
				{
					Sleep(10);
					counter = 0;
				}
			}
		}
	}

	myBatFile.close();
}

// Win Api
VOID writeLetter(HKL layout, CHAR chr)
{
	if (DEBUG == TRUE)
	{
		cout << chr;
		return;
	}

	INPUT ip;
	ip.type = INPUT_KEYBOARD;
	ip.ki.wScan = 0;
	ip.ki.time = 0;
	ip.ki.dwExtraInfo = 0;
	ip.ki.wVk = VkKeyScanEx(chr, layout);
	ip.ki.dwFlags = 0;
	SendInput(1, &ip, sizeof(INPUT));
	ip.ki.dwFlags = KEYEVENTF_KEYUP; 
	SendInput(1, &ip, sizeof(INPUT));
}

void argsWorking(int argc, char ** argv)
{
	if (argc == 1)
	{
		finalExit("Missing Text File Argument");
	}

	int i = 1;
	if (argc > 2)
	{
		for (; i < argc - 1; i++)
		{
			if (argv[i][0] == '-')
			{
				switch (argv[i][1])
				{
				case 'p':
					DEBUG = TRUE;
					break;
				case 'd':
					DELAY = stoi(argv[i + 1]);
					i++;
					break;
				default:
					finalExit("Unknown Option");
				}
			}
			else
			{
				finalExit("Arguments Error");
			}
		}
	}
	
	if (i != argc - 1)
		finalExit("Missing Arguments");

	fileName = argv[argc - 1];
}

void finalExit(string error) 
{ 
	cout << "KeyboardWriter.exe [-p] [-d time] textFilePath" << endl; 
	cout << error << endl; 
	exit(0);
}