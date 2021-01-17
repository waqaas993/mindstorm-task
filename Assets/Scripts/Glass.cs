using UnityEngine;

public class Glass : MonoBehaviour
{
    public float movementSpeed = 1;
    public float sideBound = 1.5f;
    private float randomSeed;

    private Player player;
    private ObjectPool explosionCubePool;
    public int explosionCubeAmount;

    // Start is called before the first frame update
    void Awake()
    {
        randomSeed = Random.Range(0.0f, sideBound*2.0f);
        foreach (ObjectPool objectPool in FindObjectsOfType<ObjectPool>())
        {
            if (objectPool.poolType == PoolType.ExplosionCube)
            {
                explosionCubePool = objectPool;
                break;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movementSpeed > 0)
        {
            transform.position = new Vector3(
                Mathf.PingPong((Time.time - randomSeed) * movementSpeed, sideBound * 2) - sideBound,
                transform.position.y,
                transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < explosionCubeAmount; i++)
                explosionCubePool.spawnObject(transform.position, 0, 1);
            if (!player)
                player = other.GetComponent<Player>();
            UIManager.Instance.tweenScoreFeedback();
            SoundManager.Instance.playAudio(AudioType.glassBreak);
            LevelManager.Instance.target -= 1;
            gameObject.SetActive(false);
        }
    }
}
