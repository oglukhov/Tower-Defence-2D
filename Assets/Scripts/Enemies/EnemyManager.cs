using UnityEngine;
using UnityEngine.UI;

public class EnemyTracker : MonoBehaviour
{
    public Text destroyerKilledPerLevel;
    public Text demomanKilledPerLevel;
    public Text dodgerKilledPerLevel;
    public Text robberKillerPerLevel;

    public int robberKilledTimes;
    public int destroyerKilledTimes;
    public int demomanKilledTimes;
    public int dodgerKilledTimes;
    private bool isSavedToPrefs = false;

    private DiamondManager diamondManager;

    void Start()
    {
        diamondManager = GameObject.FindObjectOfType<DiamondManager>();

        // Initialize kill counts for the current level
        robberKilledTimes = 0;
        destroyerKilledTimes = 0;
        demomanKilledTimes = 0;
        dodgerKilledTimes = 0;
    }

    void Update()
    {
        // Update UI
        robberKillerPerLevel.text = robberKilledTimes.ToString();
        destroyerKilledPerLevel.text = destroyerKilledTimes.ToString();
        demomanKilledPerLevel.text = demomanKilledTimes.ToString();
        dodgerKilledPerLevel.text = dodgerKilledTimes.ToString();

        if (diamondManager.win && !isSavedToPrefs || diamondManager.lose && !isSavedToPrefs)
        {
            SaveToStaticVars();
            isSavedToPrefs = true;
        }
    }

    void SaveToStaticVars()
    {
        // Save the current level stats to static variables
        GameData.robberStatic += robberKilledTimes;
        GameData.destroyerStatic += destroyerKilledTimes;
        GameData.demomanStatic += demomanKilledTimes;
        GameData.dodgerStatic += dodgerKilledTimes;

        isSavedToPrefs = true;
    }
}