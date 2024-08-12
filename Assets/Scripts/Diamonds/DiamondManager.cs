using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DiamondManager : MonoBehaviour
{
    public List<GameObject> diamonds; // List to store all diamonds in the scene
    public GameObject[] uiElements; // A single list of all UI elements

    private int diamondsCollected = 0;

    public bool lose = false;
    public bool win = false;
    public GameObject loseText;
    public GameObject winText;
    public GameObject zeroStars;
    public GameObject oneStar;
    public GameObject twoStars;
    public GameObject threeStars;
    EnemySpawner enemySpawner;
    private Canvas canvas;
    public Text diamondQty;

    // Additional components to link
    Destroyer destroyer;
    FastEnemy fastEnemy;
    ExplodingEnemy explodingEnemy;
    Tower tower;
    EarthquakeTimer earthquakeTimer;
    FrozenTower frozenTower;
    FireTower fireTower;
    LaserTower laserTower;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        enemySpawner = GameObject.Find("LevelManager").GetComponent<EnemySpawner>();
        
        // Initialize the diamond list with all diamonds in the scene
        diamonds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Diamond"));
        Debug.Log("Total Diamonds: " + diamonds.Count);
        UpdateUI();

        // Link additional components
        destroyer = FindObjectOfType<Destroyer>();
        fastEnemy = FindObjectOfType<FastEnemy>();
        explodingEnemy = FindObjectOfType<ExplodingEnemy>();
        tower = FindObjectOfType<Tower>();
        earthquakeTimer = FindObjectOfType<EarthquakeTimer>();
        frozenTower = FindObjectOfType<FrozenTower>();
        fireTower = FindObjectOfType<FireTower>();
        laserTower = FindObjectOfType<LaserTower>();
    }

    void Update()
    {
        // Check win condition in Update
        if (enemySpawner.enemiesAlive <= 0 && !win && !lose)
        {
            win = true;
            SaveDiamonds();
            UpdateUI();
            StopLevel();
        }
        diamondQty.text = diamonds.Count.ToString();
    }

    public void DecreaseDiamondCount(GameObject diamond)
    {
        if (diamonds.Contains(diamond))
        {
            diamonds.Remove(diamond);
            diamondsCollected++;
            Debug.Log("Remaining Diamonds: " + diamonds.Count);
            UpdateUI();

            if (diamonds.Count <= 0 && !lose && !win)
            {
                lose = true;
                SaveDiamonds();
                Debug.Log("All diamonds collected or destroyed!");
                UpdateUI();
                StopLevel();
            }
        }
    }

    private void SaveDiamonds()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string key = sceneName + "CollectedDiamonds";
        int savedDiamonds = PlayerPrefs.GetInt(key, 0);
        if (diamonds.Count > savedDiamonds)
        {
            PlayerPrefs.SetInt(key, diamonds.Count);
        }
    }

    private void UpdateUI()
    {
        // If lose is true, activate all UI elements related to losing
        if (lose)
        {
            loseText.SetActive(true);
            zeroStars.SetActive(true);
            foreach (GameObject uielement in uiElements)
            {
                uielement.SetActive(true);
            }
            canvas.sortingOrder = 5;
        }
        // If win is true, activate all UI elements related to winning
        else if (win)
        {
            winText.SetActive(true);
            if (diamonds.Count == 1)
            {
                oneStar.SetActive(true);
            }
            else if (diamonds.Count == 2)
            {
                twoStars.SetActive(true);
            }
            else if (diamonds.Count == 3)
            {
                threeStars.SetActive(true);
            }
            if (diamonds.Count == 0)
            {
                zeroStars.SetActive(true);
            }
            foreach (GameObject uielement in uiElements)
            {
                uielement.SetActive(true);
            }
            canvas.sortingOrder = 5;
        }
        else
        {
            // Hide all UI elements if neither win nor lose
            loseText.SetActive(false);
            winText.SetActive(false);
            zeroStars.SetActive(false);
            oneStar.SetActive(false);
            twoStars.SetActive(false);
            threeStars.SetActive(false);
            foreach (GameObject uielement in uiElements)
            {
                uielement.SetActive(false);
            }
            canvas.sortingOrder = 0; // Reset canvas order if needed
        }
    }

    private void StopLevel()
    {
        // Stop all relevant game elements such as timers, enemies, towers, etc.
        Time.timeScale = 0; // This will effectively pause the entire game
        enemySpawner.enabled = false; // Disable enemy spawner

        // Disable additional components
        if (destroyer != null) destroyer.enabled = false;
        if (fastEnemy != null) fastEnemy.enabled = false;
        if (explodingEnemy != null) explodingEnemy.enabled = false;
        if (tower != null) tower.enabled = false;
        if (earthquakeTimer != null) earthquakeTimer.enabled = false;
        if (frozenTower != null) frozenTower.enabled = false;
        if (fireTower != null) fireTower.enabled = false;
        if (laserTower != null) laserTower.enabled = false;

    }

    public void GoToMap()
    {
        // Before changing scenes, resume time scale to avoid issues in the new scene
        Time.timeScale = 1;
        SceneManager.LoadScene("Map");
    }
}