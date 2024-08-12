using UnityEngine;
using UnityEngine.UI; // For UI elements (if needed for feedback)

public class DeleteAllPrefs : MonoBehaviour
{
    public Text feedbackText; // Optional: UI Text to show feedback

    void Start()
    {
        // Call the method to delete all PlayerPrefs when the script starts
        DeleteAllPlayerPrefs();
    }

    void DeleteAllPlayerPrefs()
    {
        // Delete all PlayerPrefs
        PlayerPrefs.DeleteAll();

        // Optional: Provide feedback to the user
        if (feedbackText != null)
        {
            feedbackText.text = "All PlayerPrefs have been deleted.";
        }
        else
        {
            Debug.Log("All PlayerPrefs have been deleted.");
        }
    }
}