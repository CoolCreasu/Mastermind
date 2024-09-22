# Mastermind

A recreation of the classic Mastermind game in Unity.

## Features
- **5 Peg Slots**: Each guess consists of a sequence of 5 pegs.
- **8 Possible Colors**: Players can choose from 8 distinct colors, resulting in a wide variety of code combinations.

## Game Rules
- **Objective**: Guess the secret code, a sequence of 5 pegs chosen from 8 available colors, with no repeated colors in the code.
- **Feedback**: For each guess, the game provides feedback to guide the player:
  - **Black Pegs**: The number of pegs that are both the correct color and in the correct position.
  - **White Pegs**: The number of pegs that are the correct color but placed in the wrong position.
 
- The game continues until the player correctly guesses the secret code or exhausts the maximum number of attempts.

## How to Play
1. The secret code is randomly generated at the start of the game.
2. Use the **arrow keys** to navigate and adjust your guess:
   - **Left/Right Arrows**: Switch between the peg slots.
   - **Up/Down Arrows**: Change the color of the currently selected peg.
3. Once all 5 pegs are filled, press the **Enter/Return** key to submit your guess.
4. After submitting, you will receive feedback in the form of black and white pegs:
   - **Black Pegs**: Indicate the number of correct pegs in the right position.
   - **White Pegs**: Indicate the number of correct pegs in the wrong position.
5. Use the feedback to refine your next guess and continue until you either crack the code or run out of attempts.

## Configuration
- **Peg Slots**: 5 pegs per guess.
- **Colors**: 8 possible colors.
  
This configuration creates 32,768 possible combinations, offering a challenging puzzle-solving experience for players.
