*In construction*

# SRT Align
SRT Align is a CLI software that can edit timestamp in an SRT file (SubRip) to align the timing with the video Source. The tool can help for two types of offset:

- **Shift**: help with different duration of files and one source is faster than the other
- **Linear**: when the subtitle is always slightly before or being the video source due to wrong start time

## Installation
1. Download the most current .exe in the release page
2. Place it in your folder of choice
3. Add the location of srt-align.exe to your system environnement path

## Usage

### Basic usage
The structure of the commands for the tool is has followed:

> srt-align {--shift|--linear} [OPTIONS...] \<input> [output]

## Contributing

## Licence
[GPL3](LICENSE)
