using UnityEngine;

public class Pole : MonoBehaviour
{
    private Player player;

    public float bumpForce = 18;

    public ObstaclePool obstaclePool;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            if (!player)
                player = other.GetComponent<Player>();
            if (player)
                player.rb.AddForce(-transform.forward * bumpForce, ForceMode.Impulse);
        }
            
    }
}