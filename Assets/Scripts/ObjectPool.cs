using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Glass,
    Pole,
    ExplosionCube
}

public class ObjectPool : MonoBehaviour
{
    public PoolType poolType;
    public int objectsInPool;
    public GameObject objectPrefab;
    public List<GameObject> gameObjects = new List<GameObject>();
    //TODO: I am not using them so far
    public List<Glass> glass = new List<Glass>();
    public List<Pole> pole = new List<Pole>();
    public List<ExplosionCube> explosionCube = new List<ExplosionCube>();

    private int index = 0;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < objectsInPool; i++)
        {
            gameObjects.Add(Instantiate(objectPrefab, transform.localPosition, Quaternion.identity) as GameObject);
            gameObjects[i].transform.SetParent(transform);
            gameObjects[i].SetActive(false);
            switch (poolType)
            {
                case PoolType.Glass:
                    glass.Add(gameObjects[i].GetComponent<Glass>());
                    break;
                case PoolType.Pole:
                    pole.Add(gameObjects[i].GetComponent<Pole>());
                    break;
                case PoolType.ExplosionCube:
                    explosionCube.Add(gameObjects[i].GetComponent<ExplosionCube>());
                    break;
                default:
                    break;
            }
        }
    }

    public void spawnObject(Vector3 position, float movementSpeed, int explosionCubeColor = 0)
    {
        gameObjects[index].transform.position = position;
        if (poolType == PoolType.Glass)
            glass[index].movementSpeed = movementSpeed;
        if (poolType == PoolType.ExplosionCube)
            explosionCube[index].setMaterial(explosionCubeColor);
        gameObjects[index].SetActive(true);
        index += 1;
        if (index >= objectsInPool)
            index = 0;
    }

    public void disableAll()
    {
        foreach (GameObject obstacleGO in gameObjects)
            obstacleGO.gameObject.SetActive(false);
        index = 0;
    }
}