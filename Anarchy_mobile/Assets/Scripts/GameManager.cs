using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            return main;
        }
    }
    private static GameManager main;
    public PlayerData playerData;
    public AudioManager audioManager;
    public int turn_Number = 0;

    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
        Time.timeScale = 1;

        main = this;
    }

    public void SaveDataToJson()
    {
        string jsondata = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.dataPath, "playerData.json");
        File.WriteAllText(path, jsondata);
    }

    public void LoadDataToJson()
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        string jsondata = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(jsondata);
    }

    public void LoadScene(string str)
    {
        audioManager.ButtonClickSound();
        SceneManager.LoadScene(str);
    }
}

[System.Serializable]
public class PlayerData
{
    public int forceNumber = -1;
    public int mapNumber = -1;
    string forceName;

    public void setForceNumber(int num)
    {
        forceNumber = num;
    }

    public int getForceNumber()
    {
        return forceNumber;
    }

    public void setForceName(string name)
    {
        forceName = name;
    }

    public string getForceName()
    {
        return forceName;
    }

    public void setForceInfo(int num, string name)
    {
        forceNumber = num;
        forceName = name;
    }
}