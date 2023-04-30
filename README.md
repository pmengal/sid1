# SID1

## Creating a video featuring a fictitious artificial intelligence from arbitrary text

This file presents a console application that converts text into an animated video. The goal of this application is to allow users to easily create animated videos on the fly from their textual content, such as blog posts, stories, or scripts.

This project was originally created as part of a university group work for a pedagogy course at the University of Liege.

### Prerequisites

- .NET 7.0 or later
- FFMpeg (for video export)

### Installation

1. Clone this repository  :

```
git clone https://github.com/pmengal/sid1.git
```

2. Access the project file :

```
cd sid1
```

3. Restore the project dependencies using the dotnet command:

```
dotnet restore
```

### Utilisation

Here are the default commands

* -h, --help                                  Prints help information
* -b, --background                            Path to background video
* -i, --input                                 Path to input audio file
* -o, --ouput                                 Path to output video file
* -k, --key                                   Key of Azure Speech Services. Defaults to current directory
* -r, --region        westeurope              Azure region to use. Defaults to westeurope
* -l, --language      fr-FR                   Language to use. Defaults to fr-FR
* -v, --voice         fr-FR-BrigitteNeural    Voice to use. Defaults to fr-FR-BrigitteNeural

### Licence

This application is distributed under the MIT license. For more information, see the [LICENSE](LICENSE) file included in this repository.

### Contribute

Contributions are welcome! If you would like to contribute to this project, please see the file [CONTRIBUTING.md](CONTRIBUTING.md) for instructions on how to submit pull requests and report issues.
