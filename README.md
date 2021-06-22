# SRT Align
SRT Align is a CLI software that can edit timestamp in an SRT file (SubRip) to align the timing with the video Source. The tool can help for two types of offset:

- **Shift**: help with different duration of files and one source is faster than the other
- **Linear**: when the subtitle is always slightly before or being the video source due to wrong start time

## Installation
### Windows
#### Installer
1. Download the most recent release (srt-align_Setup.exe)
2. Run the installer

#### Manual
1. Download the most recent release (srt-align.exe)
2. Place the file in your folder of choice
3. <Optional> Add the location of srt-align.exe to your environnement variable (PATH) for access from anywhere in cmd/powershell

### Linux
#### Manual
1. Download the most recent release(srt-align)
2. Place the file in your folder of choice
3. <Optional> Add a symbolic link in /usr/local/bin

### Windows
1. Download the most current .exe in the release page
2. Place it in your folder of choice
3. Add the location of srt-align.exe to your system environnement path

### Linux
1. Download to most recent release (file without an extension)
2. Place it in your folder of choice
  - Placing the file in /usr/local/bin would let you use the software as a bash command without specifying the location

## Usage

The structure of the commands for the tool is has followed:

> srt-align {--shift|--linear} [OPTIONS...] \<input> [output]

### Options
| Option         | Description                                                                                                                                              |
|----------------|----------------------------------------------------------------------------------------------------------------------------------------------------------|
| -h --help      | Show the help panel                                                                                                                                      |
| -v --version   | Show the package version                                                                                                                                 |
| -s --shift     | Shifts the timestamp of a file based on a mutiple ratio (floating point value) where 1 means no change.                                                  |
| -l --linear    | Increase or decrease the timestamp value of a file based on a timestamp provided. The timestamp must be in a valid format for srt file ([-]##:##:##,###) |
| -o --overwrite | Tells the program to overwrite the input file that was provided.                                                                                         |

A choice must be made between a shift alignement and a linear alignement. Both cannot be inputed at the same time.

### Input/Output
- **input:** The input srt file that must be provided. This file is mandatory.
- **output:** The ouptut file for the srt file after modification. This item is optional. If this value is ommited, the file will be named after the input name with "-edited" at the end. The file will be placed in the input file directory.

<!--## Contributing-->

## Licence
[GPL3](LICENSE)
