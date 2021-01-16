using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Glass,
    Pole
}

public class ObstaclePool : MonoBehaviour
{
    public ObstacleType obstacleType;
    public int obstaclesInPool;
    public GameObject obstacleObject;
    public List<GameObject> obstacleGOs = new List<GameObject>();
    //TODO: I am not using them so far
    public List<Glass> glass = new List<Glass>();
    public List<Pole> pole = new List<Pole>();

    private int index = 0;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < obstaclesInPool; i++)
        {
            obstacleGOs.Add(Instantiate(obstacleObject, transform.localPosition, Quaternion.identity) as GameObject);
            obstacleGOs[i].transform.SetParent(transform);
            obstacleGOs[i].SetActive(false);
            switch (obstacleType)
            {
                case ObstacleType.Glass:
                    glass.Add(obstacleGOs[i].GetComponent<Glass>());
                    break;
                case ObstacleType.Pole:
                    pole.Add(obstacleGOs[i].GetComponent<Pole>());
                    break;
                default:
                    break;
            }
        }
    }

    public void spawnObject(Vector3 position, float movementSpeed)
    {
        obstacleGOs[index].transform.position = position;
        if (obstacleType == ObstacleType.Glass)
            glass[index].movementSpeed = movementSpeed;
        obstacleGOs[index].SetActive(true);
        index += 1;
        if (index >= obstaclesInPool)
            index = 0;
    }

    public void disableAll()
    {
        foreach (GameObject obstacleGO in obstacleGOs)
            obstacleGO.gameObject.SetActive(false);
        index = 0;
    }
}