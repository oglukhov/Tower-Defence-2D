using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level6Manager : MonoBehaviour
    {
    public GameObject zeroStars;
    public GameObject oneStar;
    public GameObject twoStars;
    public GameObject threeStars;
    public GameObject lockObject; // Замок
    public Button level6Button; // Кнопка для запуску рівня 2

    void Start()
    {
        UpdateStars();
    }

    void Update(){
        CheckLevel5Completion();
    }
    public void StartLevel6()
    {
        SceneManager.LoadScene("Level 6");
    }

    private void CheckLevel5Completion()
    {
        // Перевіряємо, чи завершено рівень 1
        if (PlayerPrefs.GetInt("Level5Completed", 0) == 1)
        {
            lockObject.SetActive(false); // Прибираємо замок
            level6Button.interactable = true; // Робимо кнопку клікабельною
            level6Button.image.color = Color.white;
        }
        else
        {
            lockObject.SetActive(true); // Встановлюємо замок
            level6Button.interactable = false; // Робимо кнопку неклікабельною
        }
    }

    private void UpdateStars()
    {
        int collectedDiamonds = PlayerPrefs.GetInt("Level 6CollectedDiamonds", 0);

        // Деактивуємо всі спрайти зірок спочатку
        zeroStars.SetActive(false);
        oneStar.SetActive(false);
        twoStars.SetActive(false);
        threeStars.SetActive(false);

        // Активуємо відповідний спрайт зірки на основі зібраних діамантів
        switch (collectedDiamonds)
        {
            case 0:
                zeroStars.SetActive(true);
                break;
            case 1:
                oneStar.SetActive(true);
                PlayerPrefs.SetInt("Level6Completed", 1);
                break;
            case 2:
                twoStars.SetActive(true);
                PlayerPrefs.SetInt("Level6Completed", 1);
                break;
            case 3:
                threeStars.SetActive(true);
                PlayerPrefs.SetInt("Level6Completed", 1);
                break;
        }
    }
}