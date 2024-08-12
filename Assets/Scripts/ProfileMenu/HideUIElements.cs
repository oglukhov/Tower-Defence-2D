using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMapScene : MonoBehaviour
{
    public string mapSceneName = "Map"; // Name of the scene to load

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found on the GameObject.");
        }
    }

    void OnButtonClick()
    {
        SceneManager.LoadScene(mapSceneName);
    }
}