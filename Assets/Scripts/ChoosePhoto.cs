using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePhoto : MonoBehaviour
{
    [HideInInspector]
    public string pathPhoto = ""; // шлях до картинки, яку ми обрали

    public Button button1; // Перша кнопка
    public Button button2; // Друга кнопка

    public void LoadImageIntoSprite(string imagePath) // конвертація шляху до картинки в спрайт, для відображення
    {
        StartCoroutine(LoadImage(imagePath));
    }

    IEnumerator LoadImage(string imagePath)
    {
        Texture2D texture = new Texture2D(2, 2);
        byte[] imageBytes;
        try
        {
            imageBytes = System.IO.File.ReadAllBytes(imagePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load image bytes. Error: {e.Message}");
            yield break;
        }

        if (texture.LoadImage(imageBytes))
        {
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f)); // цей спрайт вже готова картинка, яку ми обрали

            // Оновлюємо зображення кнопок
            if (button1 != null && button1.image != null)
            {
                button1.image.sprite = sprite;
            }

            if (button2 != null && button2.image != null)
            {
                button2.image.sprite = sprite;
            }
        }
        else
        {
            Debug.LogError("Failed to load image into texture.");
        }
        yield return null;
    }

    [DllImport("__Internal")]
    private static extern void _OpenPhotoLibrary(string gameObject);

    public void OpenPhotoLibrary() // через цей метод ти викликаєш нативну функцію, яка відкриває галерею
    {
        _OpenPhotoLibrary(gameObject.name);
    }

    void OnPhotoPicked(string path) // цей метод повертає шлях до обраної картикн
    {
        Debug.Log("Selected photo path: " + path);
        pathPhoto = path;
        LoadImageIntoSprite(path); // Оновлюємо зображення кнопок
    }
}