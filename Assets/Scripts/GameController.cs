using UnityEngine;

namespace Mastermind
{
    public class GameController : MonoBehaviour
    {
        private GameModel model = default;
        private GameView view = default;
        private int currentPegIndex = default;
        private int[] currentGuess = default;
        private bool[] usedColors;
        private bool gameEnded = false;

        private void OnEnable()
        {
            model = new GameModel();
            view = GetComponent<GameView>();
            currentGuess = new int[model.PegCount];
            usedColors = new bool[model.ColorCount + 1];

            // Initialize all pegs as empty
            for (int i = 0; i < currentGuess.Length; i++)
            {
                currentGuess[i] = -1;
            }

            // Debug: Print the secret code to the console for testing purposes
            Debug.Log($"Secret Code: {string.Join(",", model.SecretCode)}");
        }

        private void Update()
        {
            if (gameEnded) return;

            ProcessInput();
            view.DisplayCurrentAttempt(currentGuess, currentPegIndex, model.Attempts.Count); // Display the current guess with a highlighted peg
        }

        private void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentPegIndex = (currentPegIndex + 1) % model.PegCount;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentPegIndex = (currentPegIndex - 1 + model.PegCount) % model.PegCount;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentGuess[currentPegIndex] == -1)
                {
                    for (int color = 0; color < model.ColorCount; color++)
                    {
                        if (!usedColors[color])
                        {
                            currentGuess[currentPegIndex] = color;
                            usedColors[color] = true;
                            usedColors[model.ColorCount] = false; // Ensure empty peg is marked as unused
                            return;
                        }
                    }
                }
                else
                {
                    int originalColor = currentGuess[currentPegIndex];
                    int nextColor = (originalColor + 1) % (model.ColorCount + 1);

                    for (int i = 0; i < model.ColorCount + 1; i++)
                    {
                        if (nextColor == -1 || !usedColors[nextColor])
                        {
                            currentGuess[currentPegIndex] = nextColor;

                            // Update usedColors
                            usedColors[originalColor] = false;
                            if (nextColor != -1)
                            {
                                usedColors[nextColor] = true;
                            }
                            usedColors[model.ColorCount] = false; // Ensure empty peg is marked as unused
                            return;
                        }
                        nextColor = (nextColor + 1) % (model.ColorCount + 1);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentGuess[currentPegIndex] == -1)
                {
                    for (int color = model.ColorCount - 1; color >= 0; color--)
                    {
                        if (!usedColors[color])
                        {
                            currentGuess[currentPegIndex] = color;
                            usedColors[color] = true;
                            usedColors[model.ColorCount] = false; // Ensure empty peg is marked as unused
                            return;
                        }
                    }
                }
                else
                {
                    int originalColor = currentGuess[currentPegIndex];
                    int nextColor = (originalColor - 1 + (model.ColorCount + 1)) % (model.ColorCount + 1);

                    for (int i = 0; i < model.ColorCount + 1; i++)
                    {
                        if (nextColor == -1 || !usedColors[nextColor])
                        {
                            currentGuess[currentPegIndex] = nextColor;

                            // Update usedColors
                            usedColors[originalColor] = false;
                            if (nextColor != -1)
                            {
                                usedColors[nextColor] = true;
                            }
                            usedColors[model.ColorCount] = false; // Ensure empty peg is marked as unused
                            return;
                        }
                        nextColor = (nextColor - 1 + (model.ColorCount + 1)) % (model.ColorCount + 1);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SubmitGuess();
            }
        }

        private void SubmitGuess()
        {
            // Check if the current guess contains any empty pegs (-1)
            if (System.Array.Exists(currentGuess, peg => peg == -1))
            {
                Debug.Log("Cannot submit a guess with empty pegs!");
                return;
            }

            if (model.SubmitAttempt(currentGuess))
            {
                int row = model.Attempts.Count - 1;
                view.DisplayAttempt(currentGuess, row); // Display the final guess in a permanent row

                var feedback = model.Feedback[row];
                view.DisplayFeedback(feedback.blackPegs, feedback.whitePegs, row); // Display the feedback for the guess

                if (model.CheckWin(currentGuess))
                {
                    Debug.Log("You Win!");
                    gameEnded = true;
                    // TODO: Handle win scenario (e.g., display a win message or restart the game)
                }
                else if (model.Attempts.Count >= model.MaxAttempts)
                {
                    Debug.Log("Game Over! You've used all attempts.");
                    gameEnded = true;
                    // TODO: Handle lose scenario (e.g., display lose message or restart the game)
                }

                currentGuess = new int[model.PegCount]; // Reset current guess for the next attempt

                // Reset current guess for the next attempt
                ResetCurrentGuess();
            }
            else
            {
                Debug.Log("Max attempts reached!");
                // TODO: Handle gameover scenario (e.g., display a lose message or restart the game)
            }
        }

        private void ResetCurrentGuess()
        {
            currentGuess = new int[model.PegCount]; // Initialize new guess
            usedColors = new bool[model.ColorCount + 1]; // Reset used colors

            // Initialize all pegs as empty
            for (int i = 0; i < currentGuess.Length; i++)
            {
                currentGuess[i] = -1;
            }
        }
    }
}