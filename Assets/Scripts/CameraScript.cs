using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance { get; private set; }

    public float rotation;
    public float height;
    public float offsetZ;

    private Player player;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (!player) player = FindObjectOfType<Player>();
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation, 0f, 0f), Time.deltaTime * 2f);
        transform.position = new Vector3(0, height, player.transform.position.z - offsetZ);
    }
}