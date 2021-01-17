using UnityEngine;

public class Pole : MonoBehaviour
{
    private Player player;

    public ObstaclePool obstaclePool;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.playAudio(AudioType.bump);
            Handheld.Vibrate();
            gameObject.SetActive(false);
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