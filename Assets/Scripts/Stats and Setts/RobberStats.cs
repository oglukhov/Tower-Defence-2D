using UnityEngine;
using UnityEngine.UI;

public class RobberStats : MonoBehaviour
{
    private Text robberQty;

    void Start()
    {
        robberQty = GetComponent<Text>();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (robberQty != null)
        {
            robberQty.text = GameData.robberStatic.ToString();
        }
    }
}