using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3Manager : MonoBehaviour
{
    public GameObject zeroStars;
    public GameObject oneStar;
    public GameObject twoStars;
    public GameObject threeStars;
    public GameObject lockObject; // Замок
    public Button level3Button; // Кнопка для запуску рівня 2

    void Start()
    {
        UpdateStars();
    }
    void Update(){
        CheckLevel2Completion();
    }

    public void StartLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }

    private void CheckLevel2Completion()
    {
        // Перевіряємо, чи завершено рівень 1
        if (PlayerPrefs.GetInt("Level2Completed", 0) == 1)
        {
            lockObject.SetActive(false); // Прибираємо замок
            level3Button.interactable = true; // Робимо кнопку клікабельною
            level3Button.image.color = Color.white;
        }
        else
        {
            lockObject.SetActive(true); // Встановлюємо замок
            level3Button.interactable = false; // Робимо кнопку неклікабельною
        }
    }

    private void UpdateStars()
    {
        int collectedDiamonds = PlayerPrefs.GetInt("Level 3CollectedDiamonds", 0);

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
                PlayerPrefs.SetInt("Level3Completed", 1);
                break;
            case 2:
                twoStars.SetActive(true);
                PlayerPrefs.SetInt("Level3Completed", 1);
                break;
            case 3:
                threeStars.SetActive(true);
                PlayerPrefs.SetInt("Level3Completed", 1);
                break;
        }
    }
}