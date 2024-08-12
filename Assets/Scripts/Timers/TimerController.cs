using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float totalTime = 120f; // Total time in seconds (2 minutes)
    public Image timerImage; // Reference to the UI Image
    public Text timerText; // Reference to the UI Text

    private float currentTime;
    private bool isTimerActive = false; // Controls whether the timer is active

    void Start()
    {
        currentTime = totalTime;
        UpdateTimerUI();
    }

    void Update()
    {
        if (isTimerActive && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                currentTime = 0;
                StopTimer();
            }
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerImage.fillAmount = currentTime / totalTime;
    }

    public void StartTimer()
    {
        isTimerActive = true;
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }
}