# MakeDir

## Why this tool
This tool creates a programmable folder structure on your local computer.

## Installation
The installation of the tool is very simple - just copy the executable to your local computer
in a directory of your choice (perhaps c:\programms\makedir).
After this, start the tool by :
* right-click on the executable on your local machine
* select 'execute as administrator'
* confirm by click with the left mouse button

If this was done, a dialog appears with buttons.
Click on the "install" button - in the log window below, you will see, what happens.
If no error message appears, then all is done...

## Configuration
The configuration of your folder structure is set in the configuration file.
The configuration file (makedir.conf) is located in the same folder as the
executable makedir.exe.

* The configuration file is an ascii coded text file.
* Each line contains only one command or comment.
* Comment lines starts with the character '#'. These lines will not be evaluated
* Directory setup lines starts with the command 'dir='.
* After the command 'dir=' the path must be followed
* Path must ALWAYS start with an backslash character '\\'

Here is an example of a configuration file:
```
#
# This is an example configuration
# 
dir=\documents
dir=\sources
dir=\sources\doc
dir=\sources\dat
dir=\tools
#
```

## Usage
You can use the tool in the windows explorer content menu or by command line.

* Use in windows explorer menu
 1. Open the windows explorer
 2. Select a directory, where you want to create your directory structure
 3. Click at this directory with the right mouse button
 4. In the upcoming menu, select "MakeDir"
 5. That's it!

* Use in command line (1)
 1. Open a console window
 2. Navigate to the directory, where you want to create your directory structure
 3. Type in "makedir ."
 
* Use in command line (2)
 1. Open a console window
 2. Type in "makedir &lt;full qualified folder name&gt;"
