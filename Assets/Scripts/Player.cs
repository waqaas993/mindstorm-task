using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Player : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;
    private BoxCollider boxCollider;
    [HideInInspector]
    public Vector3 startingPosition;

    public float minSpeed = 5.0f;
    public float maxSpeed = 6.5f;
    [HideInInspector]
    public float currentSpeed = 5;

    public float sideBound = 1.77f;
    public float axisSensitivity = 1f;
    public float touchMoveSensitivity = 0.01f;

    public GameObject[] subCubes;
    private Vector3[] subCubePositions;
    private Rigidbody[] subCubeRbs;
    private BoxCollider[] subCubeBcs;
    private List<int> shedCubeHistory;
    private TrailRenderer trailRenderer;
    private void Awake()
    {
        startingPosition = transform.position;
        subCubePositions = new Vector3[subCubes.Length];
        subCubeRbs = new Rigidbody[subCubes.Length];
        subCubeBcs = new BoxCollider[subCubes.Length];
        for (int i = 0; i < subCubes.Length; i++)
        {
            subCubePositions[i] = subCubes[i].transform.localPosition;
            subCubeRbs[i] = subCubes[i].AddComponent<Rigidbody>();
            subCubeRbs[i].isKinematic = true;
            subCubeBcs[i] = subCubes[i].AddComponent<BoxCollider>();
            subCubeBcs[i].enabled = false;
        } 
        StartGameplay();
    }

    public void StartGameplay()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!boxCollider) boxCollider = GetComponent<BoxCollider>();
        if (!trailRenderer) trailRenderer = GetComponent<TrailRenderer>();
        currentSpeed = minSpeed;
        trailRenderer.emitting = false;
        Invoke("setTrailRenderer", 1);
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.position = startingPosition;
        transform.rotation = Quaternion.identity;
        for (int i = 0; i < subCubes.Length; i++)
        {
            subCubes[i].transform.SetParent(transform);
            subCubes[i].transform.localPosition = subCubePositions[i];
            subCubes[i].transform.localRotation = Quaternion.identity;
            subCubeRbs[i].isKinematic = true;
            subCubeBcs[i].enabled = false;
        }
        shedCubeHistory = new List<int>();
        
    }

    void setTrailRenderer()
    {
        trailRenderer.emitting = true;
    }

    private void Update()
    {
        if (UIManager.Instance.currentScreen == GameScreen.Start)
            return;
        rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * currentSpeed, Time.deltaTime * 2);
        //Should not increment speed value when the player has completed level
        //for example, when the stopping force has been applied on player cube
        if (currentSpeed > minSpeed)
        {
            //Interpolation of cube speed from minimum speed to maximum speed based on level progress
            float x = 1 - (LevelManager.Instance.levelEndGO.transform.position.z - transform.position.z) / (LevelManager.Instance.levelEndGO.transform.position.z - startingPosition.z);
            x *= (maxSpeed - minSpeed);
            x += minSpeed;
            currentSpeed = x;
        }
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
                Mathf.Clamp(transform.position.x + inputTouch() * touchMoveSensitivity, -sideBound, sideBound),
                transform.position.y,
                transform.position.z);
        }

    }

    public float inputTouch()
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

    void explodeCube(int i)
    {
        subCubes[i].transform.SetParent(null);
        subCubeRbs[i].isKinematic = false;
        subCubeBcs[i].enabled = true;
        Vector3 unitDir = Random.insideUnitSphere;
        subCubeRbs[i].AddForce(new Vector3(unitDir.x, 1, unitDir.z) * 5, ForceMode.Impulse);
    }

    public void shedCube()
    {
        int decision;
        bool found;
        if (shedCubeHistory.Count < subCubes.Length / 2)
        {
            do
            {
                found = true;
                //Topple off the top first
                decision = Random.Range(subCubes.Length / 2, subCubes.Length);
                foreach (int previouslySheddedCube in shedCubeHistory)
                    if (previouslySheddedCube == decision)
                        found = false;
            } while (!found);
            shedCubeHistory.Add(decision);
            explodeCube(decision);
        }
        else
            killMe();
    }

    public void killMe()
    {
        for (int i = 0; i < subCubes.Length; i++)
            if (subCubes[i].transform.parent == transform)
                explodeCube(i);
        if (UIManager.Instance.currentScreen != GameScreen.End)
        {
            SoundManager.Instance.playAudio(AudioType.levelCleared);
            UIManager.Instance.levelEndReason.text = "TRY AGAIN!";
            UIManager.Instance.screenFlyIn(GameScreen.End, 0.25f);
        }
        rb.isKinematic = true;
    }
}