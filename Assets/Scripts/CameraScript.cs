using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance { get; private set; }

    public float rotation;
    public float height;
    public float offsetZ;

    private Player player;
    public bool shake;
    public float shakeAmount;
    public float shakeDuration;
    private Vector3 targetPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!player) player = FindObjectOfType<Player>();
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation, 0f, 0f), Time.deltaTime * 2f);
        targetPosition = new Vector3(0, height, player.transform.position.z - offsetZ); ;
        if (shake)
        {
            Vector3 shakeVector = targetPosition + Random.insideUnitSphere * shakeAmount;
            targetPosition = new Vector3(shakeVector.x, height, shakeVector.z);
        }
        transform.position = targetPosition;
    }

    public void Shake()
    {
        shake = true;
        Invoke("stopShake", shakeDuration);
    }

    void stopShake()
    {
        shake = false;
    }

}