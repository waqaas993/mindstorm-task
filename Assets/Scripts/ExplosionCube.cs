using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ExplosionCube : MonoBehaviour
{
    public bool explodeOnEnable;
    public float explosionForce;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private new Renderer renderer;
    public Material[] materials;
    public float decay = 2;
    private Vector3 scale;

    private void Awake()
    {
        scale = transform.localScale;
    }

    private void OnEnable()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!boxCollider) boxCollider = GetComponent<BoxCollider>();
        if (!renderer) renderer = GetComponent<Renderer>();
        transform.localScale = scale;
        CancelInvoke();
        rb.isKinematic = true;
        boxCollider.enabled = false;
        if (explodeOnEnable)
            explodeCube();
        Invoke("DisableMe", decay);
    }

    public void explodeCube()
    {
        rb.isKinematic = false;
        boxCollider.enabled = true;
        Vector3 unitDir = Random.insideUnitSphere;
        rb.AddForce(new Vector3(unitDir.x, 1, unitDir.z) * explosionForce, ForceMode.Impulse);
        float size = Random.Range(0.0f, scale.x);
        transform.DOScale(new Vector3(size, size, size), 1);
    }

    //0 -> player, 1 -> glass, 2 -> pole
    public void setMaterial(int i)
    {
        renderer.material = materials[i];
    }

    void DisableMe()
    {
        rb.isKinematic = false;
        transform.gameObject.SetActive(false);
    }
}