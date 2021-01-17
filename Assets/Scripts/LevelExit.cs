using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private Player player;
    private CameraScript cameraScript;
    public bool firstDoor;
    public int stoppingPower;
    private bool isGlass;

    //Glass first, Black
    public Material[] materials;
    public new Renderer renderer;

    private void Awake()
    {
        cameraScript = FindObjectOfType<CameraScript>();
    }

    private void Update()
    {
        if (firstDoor)
        {
            if (!renderer)
                renderer = GetComponent<Renderer>();
            if (LevelManager.Instance.target <= 0 && !isGlass)
            {
                renderer.material = materials[0];
                isGlass = true;
            }
            else if (LevelManager.Instance.target > 0 && isGlass)
            {
                renderer.material = materials[1];
                isGlass = false;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!player)
                player = other.GetComponent<Player>();
            if (LevelManager.Instance.target <= 0)
            {
                if (UIManager.Instance.currentScreen != GameScreen.End)
                {
                    SoundManager.Instance.playAudio(AudioType.levelCleared);
                    UIManager.Instance.levelEndReason.text = "LEVEL CLEARED!";
                    UIManager.Instance.screenFlyIn(GameScreen.End, 0.25f);
                    LevelManager.Instance.LevelCleared();
                }
                SoundManager.Instance.playAudio(AudioType.glassBreak);
                gameObject.SetActive(false);
                player.currentSpeed -= stoppingPower;
                if (player.currentSpeed < 0)
                {
                    player.currentSpeed = 0;
                    //player.rb.isKinematic = true;
                }
            }
            else
            {
                Handheld.Vibrate();
                SoundManager.Instance.playAudio(AudioType.bump);
                cameraScript.Shake();
                player.killMe();
            }
        }
    }

}