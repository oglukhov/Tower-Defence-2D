using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level5Manager : MonoBehaviour
{
    public GameObject zeroStars;
    public GameObject oneStar;
    public GameObject twoStars;
    public GameObject threeStars;
    public GameObject lockObject; // Замок
    public Button level5Button; // Кнопка для запуску рівня 2

    void Start()
    {
        
        UpdateStars();
    }

    void Update(){
        CheckLevel4Completion();
    }

    public void StartLevel5()
    {
        SceneManager.LoadScene("Level 5");
    }

    private void CheckLevel4Completion()
    {
        // Перевіряємо, чи завершено рівень 1
        if (PlayerPrefs.GetInt("Level4Completed", 0) == 1)
        {
            lockObject.SetActive(false); // Прибираємо замок
            level5Button.interactable = true; // Робимо кнопку клікабельною
            level5Button.image.color = Color.white;
        }
        else
        {
            lockObject.SetActive(true); // Встановлюємо замок
            level5Button.interactable = false; // Робимо кнопку неклікабельною
        }
    }

    private void UpdateStars()
    {
        int collectedDiamonds = PlayerPrefs.GetInt("Level 5CollectedDiamonds", 0);

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
                PlayerPrefs.SetInt("Level5Completed", 1);
                break;
            case 2:
                twoStars.SetActive(true);
                PlayerPrefs.SetInt("Level5Completed", 1);
                break;
            case 3:
                threeStars.SetActive(true);
                PlayerPrefs.SetInt("Level5Completed", 1);
                break;
        }
    }
}