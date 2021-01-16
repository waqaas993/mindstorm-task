using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LocalSave : MonoBehaviour
{
    public static LocalSave Instance { get; private set; }
    public bool resetData;

    private void Awake()
    {
        Instance = this;
        if (resetData && Application.platform == RuntimePlatform.WindowsEditor)
            ResetData();
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");
        bf.Serialize(file, JsonUtility.ToJson(InGameData.Instance.playerData));
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
            PlayerData PD = new PlayerData();
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), PD);
            InGameData.Instance.readData(PD);
            file.Close();
        }
    }

    void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/MySaveData.dat");
        }
    }
}