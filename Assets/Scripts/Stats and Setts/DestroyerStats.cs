using UnityEngine;
using UnityEngine.UI;

public class DestroyerStats : MonoBehaviour
{
    private Text destroyerQty;

    void Start()
    {
        destroyerQty = GetComponent<Text>();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (destroyerQty != null)
        {
            destroyerQty.text = GameData.destroyerStatic.ToString();
        }
    }
}