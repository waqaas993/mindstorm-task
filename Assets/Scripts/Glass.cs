using UnityEngine;

public class Glass : MonoBehaviour
{
    public float movementSpeed = 1;
    public float sideBound = 1.5f;
    private float randomSeed;

    private Player player;

    public ObstaclePool obstaclePool;

    // Start is called before the first frame update
    void Awake()
    {
        randomSeed = Random.Range(0.0f, sideBound*2.0f);
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
            if (!player)
                player = other.GetComponent<Player>();
            UIManager.Instance.tweenScoreFeedback();
            SoundManager.Instance.playAudio(AudioType.glassBreak);
            LevelManager.Instance.target -= 1;
            gameObject.SetActive(false);
        }
    }
}
