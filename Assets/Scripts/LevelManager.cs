using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Levels[] levels;
    public Transform[] lanes;

    private ObjectPool glassPool;
    private ObjectPool polePool;
    private ObjectPool explosionCubePool;

    public GameObject levelEndPrefab;
    [HideInInspector]
    public GameObject levelEndGO;

    [HideInInspector]
    public int currentLevel;
    [HideInInspector]
    public int target;
    [HideInInspector]
    public int movingGlass;
    [HideInInspector]
    public float glassMovementSpeed;
    [HideInInspector]
    public int staticGlass;
    [HideInInspector]
    public int pole;
    [HideInInspector]
    public float distanceBetweenObstacles;

    private Player player;
    public static LevelManager Instance { get; private set; }


    void Awake()
    {
        Instance = this;
        foreach (ObjectPool objectPool in FindObjectsOfType<ObjectPool>())
        {
            if (objectPool.poolType == PoolType.Glass)
                glassPool = objectPool;
            if (objectPool.poolType == PoolType.Pole)
                polePool = objectPool;
            if (objectPool.poolType == PoolType.ExplosionCube)
                explosionCubePool = objectPool;
        }
        StartGameplay();
    }

    public void StartGameplay()
    {
        if (!player)
            player = FindObjectOfType<Player>();
        setLevel(InGameData.Instance.playerData.levelsUnlocked);
        generateLevel();
        ColorPalateManager.Instance.StartGameplay();
        player.StartGameplay();
        UIManager.Instance.StartGameplay(InGameData.Instance.playerData.levelsUnlocked);
    }

    public void setLevel(int level)
    {
        currentLevel = level;
        target = levels[currentLevel - 1].target;
        movingGlass = levels[currentLevel - 1].movingGlass;
        glassMovementSpeed = levels[currentLevel - 1].glassMovementSpeed;
        staticGlass = levels[currentLevel - 1].staticGlass;
        pole = levels[currentLevel - 1].pole;
        distanceBetweenObstacles = levels[currentLevel - 1].distanceBetweenObstacles;
    }

    void generateLevel()
    {
        if (glassPool) glassPool.disableAll();
        if (polePool) polePool.disableAll();
        if (explosionCubePool) explosionCubePool.disableAll();
        float lastSpawnedZ = player.startingPosition.z + distanceBetweenObstacles * 4;
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

        //Spawn Level exit
        if (!levelEndGO)
            levelEndGO = Instantiate(levelEndPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        levelEndGO.transform.position = new Vector3(0, 1, lastSpawnedZ + distanceBetweenObstacles * 4);
        foreach (Transform child in levelEndGO.transform)
            if (!child.gameObject.activeSelf)
                child.gameObject.SetActive(true);
    }

    public void LevelCleared()
    {
        Debug.Log("currentLevel " + currentLevel);
        Debug.Log("levels.Length " + levels.Length);
        if (levels.Length > currentLevel)
        {
            InGameData.Instance.playerData.levelsUnlocked = currentLevel + 1;
            LocalSave.Instance.SaveData();
        }
    }
}