using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level7Manager : MonoBehaviour
    {
    public GameObject zeroStars;
    public GameObject oneStar;
    public GameObject twoStars;
    public GameObject threeStars;
    public GameObject lockObject; // Замок
    public Button level7Button; // Кнопка для запуску рівня 2

    void Start()
    {
        UpdateStars();
    }

    void Update(){
        CheckLevel6Completion();
    }
    public void StartLevel7()
    {
        SceneManager.LoadScene("Level 7");
    }

    private void CheckLevel6Completion()
    {
        // Перевіряємо, чи завершено рівень 1
        if (PlayerPrefs.GetInt("Level6Completed", 0) == 1)
        {
            lockObject.SetActive(false); // Прибираємо замок
            level7Button.interactable = true; // Робимо кнопку клікабельною
            level7Button.image.color = Color.white;
        }
        else
        {
            lockObject.SetActive(true); // Встановлюємо замок
            level7Button.interactable = false; // Робимо кнопку неклікабельною
        }
    }

    private void UpdateStars()
    {
        int collectedDiamonds = PlayerPrefs.GetInt("Level 7CollectedDiamonds", 0);

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
                PlayerPrefs.SetInt("Level7Completed", 1);
                break;
            case 2:
                twoStars.SetActive(true);
                PlayerPrefs.SetInt("Level7Completed", 1);
                break;
            case 3:
                threeStars.SetActive(true);
                PlayerPrefs.SetInt("Level7Completed", 1);
                break;
        }
    }
}
