using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Color unitColor;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }




    [System.Serializable]
    public class SaveData 
    {
        public Color teamColor;
    }

    public void saveColorData()
    {
        SaveData data = new SaveData();
        data.teamColor = unitColor;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/SaveFile.json", json);

    }

    public void loadColorData()
    {
        string path = Application.persistentDataPath + "/SaveFile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            unitColor = data.teamColor;
        }
    }

}
