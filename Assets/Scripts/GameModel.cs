using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mastermind
{
    public class GameModel
    {
        public List<int[]> Attempts { get; private set; }
        public List<(int blackPegs, int whitePegs)> Feedback { get; private set; } // Track feedback for each attempt
        public int[] SecretCode { get; private set; }
        public int MaxAttempts { get; private set; } = 12;
        public int PegCount { get; private set; } = 5;
        public int ColorCount { get; private set; } = 8;

        public GameModel()
        {
            SecretCode = GenerateSecretCode();
            Attempts = new List<int[]>();
            Feedback = new List<(int blackPegs, int whitePegs)>();
        }

        private int[] GenerateSecretCode()
        {
            // Create a list of available colors
            List<int> availableColors = Enumerable.Range(0, ColorCount).ToList();

            // Shuffle the available colors
            for (int i = 0; i < availableColors.Count; i++)
            {
                int randomIndex = Random.Range(i, availableColors.Count);
                // Swap
                int temp = availableColors[i];
                availableColors[i] = availableColors[randomIndex];
                availableColors[randomIndex] = temp;
            }

            // Take the first PegCount colors to form the secret code
            return availableColors.Take(PegCount).ToArray();

            //return Enumerable.Range(0, PegCount).Select(x => Random.Range(0, ColorCount)).ToArray();
        }

        public bool SubmitAttempt(int[] attempt)
        {
            if (Attempts.Count < MaxAttempts)
            {
                Attempts.Add(attempt);
                var feedback = CalculateFeedback(attempt);
                Feedback.Add(feedback);
                return true;
            }
            return false;
        }

        public bool CheckWin(int[] attempt)
        {
            return attempt.SequenceEqual(SecretCode);
        }

        // Calculate the number of black and white pegs for feedback
        private (int blackPegs, int whitePegs) CalculateFeedback(int[] attempt)
        {
            int blackPegs = 0;
            int whitePegs = 0;

            bool[] secretCodeMatched = new bool[PegCount]; // Track which secret code pegs were already matched
            bool[] attemptMatched = new bool[PegCount]; // Track which attempt pegs were already matched

            // First pass: Count black pegs (correct color and position)
            for (int i = 0; i < PegCount; i++)
            {
                if (attempt[i] == SecretCode[i])
                {
                    blackPegs++;
                    secretCodeMatched[i] = true;
                    attemptMatched[i] = true;
                }
            }

            // Second pass: Count white pegs (correct color, wrong position)
            for (int i = 0; i < PegCount; i++)
            {
                if (!attemptMatched[i]) // Only check unmatched pegs
                {
                    for (int j = 0; j < PegCount; j++)
                    {
                        if (!secretCodeMatched[j] && attempt[i] == SecretCode[j])
                        {
                            whitePegs++;
                            secretCodeMatched[j] = true;  // Mark the peg as matched
                            break;
                        }
                    }
                }
            }

            return (blackPegs, whitePegs);
        }
    }
}