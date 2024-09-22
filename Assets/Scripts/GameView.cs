using System.Collections.Generic;
using UnityEngine;

namespace Mastermind
{
    public class GameView : MonoBehaviour
    {
        public GameObject pegPrefab;
        public GameObject feedbackPegPrefab; // Prefab for feedback pegs (black and white)
        public Transform attemptsContainer;

        private List<GameObject[]> displayedAttempts = new List<GameObject[]>();  // Keep track of displayed attempts
        private GameObject[] currentGuessPegs = null;  // Temporary row for current guess

        public void DisplayAttempt(int[] attempt, int row)
        {
            GameObject[] rowPegs = new GameObject[attempt.Length];

            for (int i = 0; i < attempt.Length; i++)
            {
                GameObject peg = Instantiate(pegPrefab, attemptsContainer);
                peg.transform.position = new Vector3(i * 1.25f, -(row * 1.25f), 0); // Adjust row position
                peg.GetComponentInChildren<Renderer>().material.color = GetColor(attempt[i]);
                rowPegs[i] = peg;
            }
            displayedAttempts.Add(rowPegs);  // Keep the row for later reference
        }

        // Display feedback for the current attempt: black and white pegs
        public void DisplayFeedback(int blackPegs, int whitePegs, int row)
        {
            int feedbackPegIndex = 0;

            // Display black pegs first
            for (int i = 0; i < blackPegs; i++)
            {
                GameObject feedbackPeg = Instantiate(feedbackPegPrefab, attemptsContainer);
                feedbackPeg.transform.position = new Vector3((-1.25f - feedbackPegIndex) * 1.25f, -(row * 1.25f), 0);
                feedbackPeg.GetComponentInChildren<Renderer>().material.color = Color.black;
                feedbackPegIndex++;
            }

            // Display white pegs next
            for (int i = 0; i < whitePegs; i++)
            {
                GameObject feedbackPeg = Instantiate(feedbackPegPrefab, attemptsContainer);
                feedbackPeg.transform.position = new Vector3((-1.25f - feedbackPegIndex) * 1.25f, -(row * 1.25f), 0);
                feedbackPeg.GetComponentInChildren<Renderer>().material.color = Color.white;
                feedbackPegIndex++;
            }
        }

        // Display the current guess in a temporary row (no attempt has been submitted yet)
        public void DisplayCurrentAttempt(int[] attempt, int currentPegIndex, int attemptsCount)
        {
            if (currentGuessPegs != null)
            {
                // Destroy the previous preview pegs before displaying the new guess
                foreach (var peg in currentGuessPegs)
                {
                    Destroy(peg);
                }
            }

            currentGuessPegs = new GameObject[attempt.Length];

            int currentRow = attemptsCount;

            // Display the current guess, and highlight the currently selected peg
            for (int i = 0; i < attempt.Length; i++)
            {
                GameObject peg = Instantiate(pegPrefab, attemptsContainer);
                peg.transform.position = new Vector3(i * 1.25f, -(currentRow * 1.25f), 0); // Place preview on the first row
                Renderer renderer = peg.GetComponentInChildren<Renderer>();
                renderer.material.color = GetColor(attempt[i]);

                if (i == currentPegIndex)
                {
                    // Highlight the currently selected peg (for example, by adjusting the scale)
                    peg.transform.localScale *= 1.2f;
                }

                currentGuessPegs[i] = peg;
            }
        }

        private Color GetColor(int pegValue)
        {
            return pegValue switch
            {
                -1 => new Color(1.0f, 1.0f, 1.0f, 0.2f), // Empty Peg
                0 => new Color(0.0f, 0.0f, 0.0f),   // Black
                1 => new Color(0.0f, 0.0f, 1.0f),   // Blue
                2 => new Color(0.0f, 1.0f, 0.0f),   // Green
                3 => new Color(1.0f, 0.647f, 0.0f), // Orange
                4 => new Color(1.0f, 0.0f, 1.0f),   // Pink
                5 => new Color(1.0f, 0.0f, 0.0f),   // Red
                6 => new Color(1.0f, 1.0f, 1.0f),   // White
                7 => new Color(1.0f, 1.0f, 0.0f),   // Yellow
                _ => Color.clear
            };
        }
    }
}