using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUIComponents : MonoBehaviour
{
    public List<GameObject> uiComponentsToShow; // List of UI components to show
    private Button button;
    public bool profileOpened = false;

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
        foreach (GameObject uiComponent in uiComponentsToShow)
        {
            profileOpened = true;
            if (uiComponent != null)
            {
                uiComponent.SetActive(true); // Show the UI component
            }
            else
            {
                Debug.LogWarning("One of the UI components in the list is null.");
            }
        }
    }
}