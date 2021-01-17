using UnityEngine;

public class Pole : MonoBehaviour
{
    private Player player;
    private CameraScript cameraScript;
    private ObjectPool explosionCubePool;
    public int explosionCubeAmount;

    private void Awake()
    {
        cameraScript = FindObjectOfType<CameraScript>();
        foreach (ObjectPool objectPool in FindObjectsOfType<ObjectPool>())
        {
            if (objectPool.poolType == PoolType.ExplosionCube)
            {
                explosionCubePool = objectPool;
                break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.playAudio(AudioType.bump);
            Handheld.Vibrate();
            cameraScript.Shake();
            gameObject.SetActive(false);
            for (int i = 0; i < explosionCubeAmount; i++)
                explosionCubePool.spawnObject(transform.position, 0, 2);
            if (!player)
                player = other.GetComponent<Player>();
            if (player)
            {
                player.shedCube();
                player.rb.AddForce(-transform.forward * player.rb.velocity.z * 2, ForceMode.Impulse);
            }
        }
    }
}