# Rogue Remake
A very stripped version of Rogue (1980) made for a school project in C# 12.0 for .NET 8.0.

![preview](https://github.com/user-attachments/assets/f6a5b7c8-e9ba-4647-ae4d-6c16589b6f63)

# Features
- Faithful 1:1 room placement inspired by the original Rogue
- Kruskal's Algorithm and A* Pathfinding for hallway placement
- Player line of sight effect using Brasenham's line algorithm.
- Minimalist roguelike gameplay
- Unit tests checking various elements of the game.

# Release
You can download the precompiled executable in Releases, click [here](https://github.com/qer24/rogue-simple-remake/releases/latest) for the latest one. 

# Prerequisites

- C# 12.0
- .NET 8.0 SDK
- Windows, macOS, or Linux with .NET 8.0 runtime support

# Compiling
To build the project, use the following command:
```bash
dotnet build --configuration Release
```

# Usage
Launch the executable located at:

> bin/Release/net8.0/RogueProject.exe

You can play the entire game with just arrow keys to move. 

Collect items and fight enemies and try to get as far as possible.

Cheat codes:

> R - reveal map
> 
> G - generate new floor
> 
> K - deal 999 damage to player

# Contributing
Feel free to fork the repository and submit pull requests with improvements or bug fixes.

# Acknowledgments
Original Rogue (1980) by Michael Toy and Glenn Wichman

[FastConsole.cs](https://github.com/crowfingers/FastConsole/blob/master/FastConsole.cs) by crowfingers

[WindowUtility.cs](https://stackoverflow.com/a/67010648) by Quickz

Proc Gen libraries by BlueRaja

# License 
[MIT](LICENSE)
