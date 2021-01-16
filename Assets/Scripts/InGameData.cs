using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int levelsUnlocked;

    public PlayerData()
    {
        levelsUnlocked = 1;
    }
}
public class InGameData : MonoBehaviour
{
    public static InGameData Instance { get; private set; }
    public PlayerData playerData;
    private void Awake()
    {
        Instance = this;
        playerData = new PlayerData();
        LocalSave.Instance.LoadGame();
    }

    public void readData(PlayerData playerData)
    {
        this.playerData.levelsUnlocked = playerData.levelsUnlocked;
    }
}
