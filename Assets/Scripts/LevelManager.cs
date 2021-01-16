using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Levels[] levels;
    public Transform[] lanes;

    private ObstaclePool glassPool;
    private ObstaclePool polePool;

    public int target;
    public int movingGlass;
    public float glassMovementSpeed;
    public int staticGlass;
    public int pole;
    public float distanceBetweenObstacles;

    private Player player;
    public static LevelManager Instance { get; private set; }


    void Awake()
    {
        Instance = this;
        foreach (ObstaclePool obstaclePool in FindObjectsOfType<ObstaclePool>())
        {
            if (obstaclePool.obstacleType == ObstacleType.Glass)
                glassPool = obstaclePool;
            if (obstaclePool.obstacleType == ObstacleType.Pole)
                polePool = obstaclePool;
        }
        setLevel(InGameData.Instance.playerData.levelsUnlocked);
        generateLevel();
    }

    public void setLevel(int level)
    {
        target = levels[level - 1].target;
        movingGlass = levels[level - 1].movingGlass;
        glassMovementSpeed = levels[level - 1].glassMovementSpeed;
        staticGlass = levels[level - 1].staticGlass;
        pole = levels[level - 1].pole;
        distanceBetweenObstacles = levels[level - 1].distanceBetweenObstacles;
    }


    void generateLevel()
    {
        glassPool.disableAll();
        polePool.disableAll();
        if (!player)
            player = FindObjectOfType<Player>();
        float lastSpawnedZ = player.transform.position.z;
        Vector3 positionToSpawnAt;
        do
        {
            int spawnDecision = Random.Range(0,3);
            switch (spawnDecision)
            {
                case 0:
                    if (movingGlass > 0)
                    {
                        lastSpawnedZ += distanceBetweenObstacles;
                        positionToSpawnAt = new Vector3(lanes[Random.Range(0, lanes.Length/2 + 1) *2].position.x ,1, lastSpawnedZ);
                        glassPool.spawnObject(positionToSpawnAt, glassMovementSpeed);
                        movingGlass -= 1;
                    }
                    break;
                case 1:
                    if (staticGlass > 0)
                    {
                        lastSpawnedZ += distanceBetweenObstacles;
                        positionToSpawnAt = new Vector3(lanes[Random.Range(0, lanes.Length/2 + 1) * 2].position.x, 1, lastSpawnedZ);
                        glassPool.spawnObject(positionToSpawnAt, 0);
                        staticGlass -= 1;
                    }
                    break;
                case 2:
                    if (pole > 0)
                    {
                        lastSpawnedZ += distanceBetweenObstacles;
                        positionToSpawnAt = new Vector3(lanes[Random.Range(0, lanes.Length)].position.x, 1, lastSpawnedZ);
                        polePool.spawnObject(positionToSpawnAt, 0);
                        pole -= 1;
                    }
                    break;
                default:
                    break;
            }
        } while (movingGlass > 0 || staticGlass > 0 || pole > 0);
    }
}