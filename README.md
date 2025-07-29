# Clara's retro snake game 🐍
### A redo of the classical snake game.

![a gif of the snake game](/assets/snake_game_gif.gif)

## Installation
1. Find the .rar archive in [Releases](https://github.com/claracoliveiras/snake-game/releases/tag/1.0)
2. Extract it to a folder
3. Play the executable file in the folder, while keeping the file structure intact

## Controls 🎮
- Click the play button to start the game
- Use the **WASD** keys to move your snake up, down, left and right

## Development info 💻
- **Stack used:** Godot with C#
- To modify this, simply clone this repository and open the folder inside of godot.
- This project has a folder-per-scene structure, although the code is currently significantly small. You should be able to find all the code regarding the player movement in the **Main.cs** file, with the subsequent folders being used mainly for connecting signals or handling animations. The only exception to this rule is the **Menu.cs** file, that handles the start menu.
- I made all of the assets myself, utilizing aseprite.
>Currently, theres a slight error regarding the first point the player makes that generates out of range errors in the code while adding the positions. That shouldn't disrupt the functioning of the game.