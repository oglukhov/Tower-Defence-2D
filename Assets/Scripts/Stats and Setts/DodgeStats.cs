using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeStats : MonoBehaviour
{
    private Text dodgerQty;

    void Start()
    {
        dodgerQty = GetComponent<Text>();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (dodgerQty != null)
        {
            dodgerQty.text = GameData.dodgerStatic.ToString();
        }
    }
}
