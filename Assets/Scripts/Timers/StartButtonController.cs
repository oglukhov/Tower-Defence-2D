using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonController : MonoBehaviour
{
    public Button startButton; // Reference to the Start button
    public List<MonoBehaviour> componentsToStart; // List of components to start when the button is pressed
    public List<TimerController> timersToStart; // List of timers to start
    public EarthquakeTimer earthquakeTimer; // Reference to the earthquake timer
    public EnemySpawner enemySpawner; // Reference to the enemy spawner
    public List<GameObject> uiElementsToHide; // List of UI elements to hide when the button is pressed
    public List<GameObject> uiElementsToShow; // List of UI elements to show when the button is pressed
    public bool gameStarted = false;
    private Canvas canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvas.sortingLayerID = 5;
        // Disable the components initially
        foreach (MonoBehaviour component in componentsToStart)
        {
            component.enabled = false;
        }

        // Hide the specified UI elements initially
        foreach (GameObject uiElement in uiElementsToHide)
        {
            uiElement.SetActive(true);
        }

        // Show the specified UI elements initially
        foreach (GameObject uiElement in uiElementsToShow)
        {
            uiElement.SetActive(false);
        }

        // Add listener to the start button
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    public void OnStartButtonClicked()
    {
        gameStarted = true;
        // Enable the components when the button is clicked
        foreach (MonoBehaviour component in componentsToStart)
        {
            component.enabled = true;
        }

        // Start the timers
        foreach (TimerController timer in timersToStart)
        {
            timer.StartTimer();
        }

        // Start the earthquake timer
        earthquakeTimer.StartTimer();

        // Start the enemy spawner
        enemySpawner.StartSpawning();

        // Hide the specified UI elements
        foreach (GameObject uiElement in uiElementsToHide)
        {
            uiElement.SetActive(false);
        }

        // Show the specified UI elements
        foreach (GameObject uiElement in uiElementsToShow)
        {
            uiElement.SetActive(true);
        }

        // Optionally, disable the start button to prevent multiple starts
        startButton.interactable = false;
    }
}