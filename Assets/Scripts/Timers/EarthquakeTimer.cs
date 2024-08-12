using UnityEngine;
using UnityEngine.UI;

public class EarthquakeTimer : MonoBehaviour
{
    public float totalTime = 60f; // Total time in seconds (1 minute)
    public Image timerImage; // Reference to the UI Image
    public Text timerText; // Reference to the UI Text

    private float currentTime;
    private bool isTimerActive = false; // Controls whether the timer is active

    private Vector3 acceleration;
    private Vector3 lastAcceleration;
    private Vector3 deltaAcceleration;
    private float shakeThreshold = 2.0f; // Adjust this value to set sensitivity for shake detection
    private bool usedEarthquake = false;
    private StartButtonController startButtonController;
    public GameObject bg;


    void Start()
    {
        startButtonController = GameObject.Find("LevelManager").GetComponent<StartButtonController>();
        currentTime = totalTime;
        UpdateTimerUI();
        lastAcceleration = Input.acceleration;
        StartTimer(); // Automatically start the timer
    }

    void Update()
    {
        if (isTimerActive && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                StopTimer();
            }
            UpdateTimerUI();
            
        }
        DetectShake();
        
    }

    void UpdateTimerUI()
    {
        // Update the timer text
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Update the timer image fill amount
        timerImage.fillAmount = currentTime / totalTime;
    }

    public void StartTimer()
    {
        if(startButtonController.gameStarted)
        {
            isTimerActive = true;
        }
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }

    void ApplyDamageToEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemies)
        {
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            ExplodingEnemy explodingEnemy = enemyObject.GetComponent<ExplodingEnemy>();
            FastEnemy fastEnemy = enemyObject.GetComponent<FastEnemy>();
            Destroyer destroyer = enemyObject.GetComponent<Destroyer>();

            if (enemy != null)
            {
                enemy.TakeDamage(4f);
            }
            if (explodingEnemy != null)
            {
                explodingEnemy.TakeDamage(4f);
            }
            if (fastEnemy != null)
            {
                fastEnemy.TakeDamage(4f);
            }
            if (destroyer != null)
            {
                destroyer.TakeDamage(4f);
            }
        }
        
    }

    void DetectShake()
    {
        if (currentTime == 0 && !usedEarthquake) // Only detect shake when timer is 00:00 and earthquake not used
        {
            acceleration = Input.acceleration;
            deltaAcceleration = acceleration - lastAcceleration;

            if (deltaAcceleration.sqrMagnitude >= shakeThreshold * shakeThreshold)
            {
                usedEarthquake = true; // Prevent multiple shakes
                TriggerEarthquake();
            }

            lastAcceleration = acceleration;
        }
    }

    void TriggerEarthquake()
    {
        if(startButtonController.gameStarted)
        {
            
            ApplyDamageToEnemies(); // Apply damage to enemies
            Debug.Log("Earthquake Triggered");
            bg.SetActive(true);
            if(PlayerPrefs.GetInt("vibration", 0) == 1){
            Handheld.Vibrate();}// Vibrate the phone
        }
    }
}