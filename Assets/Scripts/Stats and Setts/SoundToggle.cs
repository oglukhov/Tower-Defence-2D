using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToggleButtonImage : MonoBehaviour
{
    private Image buttonImage; // Reference to the UI Image component on the button
    public Sprite sprite1;    // The first sprite
    public Sprite sprite2;    // The second sprite

    private bool isSprite1Active;
    public static bool musicOn;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        Time.timeScale = 1;
        if (buttonImage == null)
        {
            buttonImage = GetComponent<Image>();
        }

        // Load the saved state from PlayerPrefs
        int savedState = PlayerPrefs.GetInt("music", 1);
        isSprite1Active = (savedState == 1);

        // Set the initial sprite and audio state based on the saved state
        buttonImage.sprite = isSprite1Active ? sprite1 : sprite2;
        UpdateAudioState();
    }

    public void OnButtonClick()
    {
        // Toggle the active sprite
        if (isSprite1Active)
        {
            buttonImage.sprite = sprite2;
            PlayerPrefs.SetInt("music", 2); // Save the state as 2
        }
        else
        {
            buttonImage.sprite = sprite1;
            PlayerPrefs.SetInt("music", 1); // Save the state as 1
        }

        // Save the changes to PlayerPrefs
        PlayerPrefs.Save();

        // Toggle the boolean value
        isSprite1Active = !isSprite1Active;

        // Update audio state
        UpdateAudioState();
    }
    private void UpdateAudioState(){
        if(PlayerPrefs.GetInt("music", 0) == 1){
            musicOn = true;
        }
        if(PlayerPrefs.GetInt("music",0) == 2){
            musicOn = false;
        }
    }
}