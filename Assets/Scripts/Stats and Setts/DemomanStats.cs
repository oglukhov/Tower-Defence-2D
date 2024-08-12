using UnityEngine;
using UnityEngine.UI;

public class DemomanStats : MonoBehaviour
    {
    private Text demomanQty;

    void Start()
    {
        demomanQty = GetComponent<Text>();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (demomanQty != null)
        {
            demomanQty.text = GameData.demomanStatic.ToString();
        }
    }
}
