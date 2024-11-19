# Rogue Remake
A very stripped version of Rogue (1980) made for a school project in C# for .NET 8.0.

# Features
- Faithful 1:1 room placement inspired by the original Rogue
- Kruskal's Algorithm for hallway placement
- A* Pathfinding for connecting rooms
- Minimalist roguelike gameplay

# Prerequisites

- .NET 8.0 SDK
- Windows, macOS, or Linux with .NET 8.0 runtime support

# Compiling
To build the project, use the following command:
```bash
dotnet build --configuration Release
```

# Usage
Launch the executable located at:
```
bin/Release/net8.0/RogueProject.exe
```

You can play the entire game with just arrow keys to move. 

Collect items and fight enemies and try to get as far as possible.

Cheat codes:
```
R - reveal map
G - generate new floor
K - deal 999 damage to player
```

# Contributing
Feel free to fork the repository and submit pull requests with improvements or bug fixes.

# Acknowledgments
Original Rogue (1980) by Michael Toy and Glenn Wichman
