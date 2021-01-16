using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Player : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;
    private BoxCollider boxCollider;
    private Vector3 startingPosition;

    public int speed = 5;
    public float sideBound = 1.77f;
    public float axisSensitivity = 1f;
    public float touchMoveSensitivity = 0.01f;

    private void Awake()
    {
        startingPosition = transform.position;
        StartGameplay();
    }

    public void StartGameplay()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!boxCollider) boxCollider = GetComponent<BoxCollider>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.position = startingPosition;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (UIManager.Instance.currentScreen != GameScreen.Gameplay)
            return;
        rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * speed, Time.deltaTime * 3f);
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x + Input.GetAxis("Horizontal") * axisSensitivity, -sideBound, sideBound),
                transform.position.y,
                transform.position.z);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x + touchInput() * touchMoveSensitivity, -sideBound, sideBound),
                transform.position.y,
                transform.position.z);
        }

    }

    public float touchInput()
    {
        if (Input.touchCount > 0)
        {
            int touchFingerId = Input.touchCount - 1;
            Touch touch = Input.GetTouch(touchFingerId);
            //Get the latest touch finger id
            while (EventSystem.current.IsPointerOverGameObject(touch.fingerId) && touchFingerId > 0)
            {
                touchFingerId -= 1;
                touch = Input.GetTouch(touchFingerId);
            }
            //Cancel, if it's above any UI element with raycast target enabled
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return 0;
            if (touch.phase == TouchPhase.Moved)
                return touch.deltaPosition.x;
        }
        return 0;
    }
}