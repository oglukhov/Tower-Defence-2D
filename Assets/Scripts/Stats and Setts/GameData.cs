using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public static int robberStatic;
    public static int destroyerStatic;
    public static int demomanStatic;
    public static int dodgerStatic;

    void Awake()
    {
        LoadData();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update(){
        SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("robber", robberStatic);
        PlayerPrefs.SetInt("destroyer", destroyerStatic);
        PlayerPrefs.SetInt("demoman", demomanStatic);
        PlayerPrefs.SetInt("dodger", dodgerStatic);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        robberStatic = PlayerPrefs.GetInt("robber", 0);
        destroyerStatic = PlayerPrefs.GetInt("destroyer", 0);
        demomanStatic = PlayerPrefs.GetInt("demoman", 0);
        dodgerStatic = PlayerPrefs.GetInt("dodger", 0);
    }
}