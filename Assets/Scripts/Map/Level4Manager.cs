using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level4Manager : MonoBehaviour
{
    public GameObject zeroStars;
    public GameObject oneStar;
    public GameObject twoStars;
    public GameObject threeStars;
    public GameObject lockObject; // Замок
    public Button level4Button; // Кнопка для запуску рівня 2

    void Start()
    {
        UpdateStars();
    }
    void Update(){
        CheckLevel3Completion();
    }

    public void StartLevel4()
    {
        SceneManager.LoadScene("Level 4");
    }

    private void CheckLevel3Completion()
    {
        // Перевіряємо, чи завершено рівень 1
        if (PlayerPrefs.GetInt("Level3Completed", 0) == 1)
        {
            lockObject.SetActive(false); // Прибираємо замок
            level4Button.interactable = true; // Робимо кнопку клікабельною
            level4Button.image.color = Color.white;
        }
        else
        {
            lockObject.SetActive(true); // Встановлюємо замок
            level4Button.interactable = false; // Робимо кнопку неклікабельною
        }
    }

    private void UpdateStars()
    {
        int collectedDiamonds = PlayerPrefs.GetInt("Level 4CollectedDiamonds", 0);

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
                PlayerPrefs.SetInt("Level4Completed", 1);
                break;
            case 2:
                twoStars.SetActive(true);
                PlayerPrefs.SetInt("Level4Completed", 1);
                break;
            case 3:
                threeStars.SetActive(true);
                PlayerPrefs.SetInt("Level4Completed", 1);
                break;
        }
    }
}