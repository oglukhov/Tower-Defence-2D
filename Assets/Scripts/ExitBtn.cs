using UnityEngine;
using UnityEngine.UI;

public class ExitGameOnClick : MonoBehaviour
{
    private void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnExitButtonClick);
        }
        else
        {
            Debug.LogError("ExitGameOnClick: No Button component found on this GameObject.");
        }
    }

    private void OnExitButtonClick()
    {
        Debug.Log("ExitGameOnClick: Exit button clicked. Exiting game...");
        Application.Quit();

        // If running in the Unity Editor, stop playing the scene
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}