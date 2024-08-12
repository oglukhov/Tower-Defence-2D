using UnityEngine;
using UnityEngine.UI;

public class VibrationToggle : MonoBehaviour
{
    private Image buttonImage; // Reference to the UI Image component on the button
    public Sprite vibrationOnSprite; // The sprite when vibration is on
    public Sprite vibrationOffSprite; // The sprite when vibration is off

    public static bool isVibrationOn; // Static boolean to keep track of vibration state

    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            buttonImage = GetComponent<Image>();
        }

        // Load the saved state from PlayerPrefs
        int savedState = PlayerPrefs.GetInt("vibration", 1); // 1 means vibration is on by default
        isVibrationOn = (savedState == 1);

        // Set the initial sprite based on the saved state
        buttonImage.sprite = isVibrationOn ? vibrationOnSprite : vibrationOffSprite;
    }

    public void OnButtonClick()
    {
        // Toggle the active sprite and the vibration state
        if (isVibrationOn)
        {
            buttonImage.sprite = vibrationOffSprite;
            PlayerPrefs.SetInt("vibration", 0); // Save the state as 0 (off)
        }
        else
        {
            buttonImage.sprite = vibrationOnSprite;
            PlayerPrefs.SetInt("vibration", 1); // Save the state as 1 (on)
        }

        // Save the changes to PlayerPrefs
        PlayerPrefs.Save();

        // Toggle the static boolean value
        isVibrationOn = !isVibrationOn;
    }
}
