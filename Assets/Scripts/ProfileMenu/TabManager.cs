using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button settingsButton;
    public Button statsButton;
    public Button guideButton;

    public Image settingsButtonImage;
    public Image statsButtonImage;
    public Image guideButtonImage;

    public List<GameObject> settingsList;
    public List<GameObject> statisticsList;
    public List<GameObject> guideList;

    private Color originalColor;
    private Color transparentColor = new Color(1, 1, 1, 0f);
    ShowUIComponents profileButton;

    void Start()
    {
        profileButton = GameObject.Find("Profile").GetComponent<ShowUIComponents>();
        originalColor = settingsButtonImage.color;

        settingsButton.onClick.AddListener(() => OnTabButtonClick(settingsButtonImage, settingsList));
        statsButton.onClick.AddListener(() => OnTabButtonClick(statsButtonImage, statisticsList));
        guideButton.onClick.AddListener(() => OnTabButtonClick(guideButtonImage, guideList));

        // Set the Settings button as active by default
        OnTabButtonClick(settingsButtonImage, settingsList);
    }

    void OnTabButtonClick(Image activeButtonImage, List<GameObject> activeList)
    {
        // Reset all button images to original color
        settingsButtonImage.color = originalColor;
        statsButtonImage.color = originalColor;
        guideButtonImage.color = originalColor;

        // Set the active button image to transparent
        activeButtonImage.color = transparentColor;

        // Deactivate all UI elements
        DeactivateAllUIElements();

        // Activate the UI elements corresponding to the active list
        if(profileButton.profileOpened){
            foreach (GameObject uiElement in activeList)
            {
                uiElement.SetActive(true);
            }
        }
    }

    public void DeactivateAllUIElements()
    {
        // Deactivate all elements in settingsList
        foreach (GameObject uiElement in settingsList)
        {
            uiElement.SetActive(false);
        }

        // Deactivate all elements in statisticsList
        foreach (GameObject uiElement in statisticsList)
        {
            uiElement.SetActive(false);
        }

        // Deactivate all elements in guideList
        foreach (GameObject uiElement in guideList)
        {
            uiElement.SetActive(false);
        }
    }
}