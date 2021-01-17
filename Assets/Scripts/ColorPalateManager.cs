using UnityEngine;
using UnityEngine.UI;

public class ColorPalateManager : MonoBehaviour
{
    public Color[] colors;

    public Material planeMaterial;
    public Image[] planePRogressBgs;

    public static ColorPalateManager Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    public void StartGameplay()
    {
        int planeColorDecision = Random.Range(0, colors.Length);
        planeMaterial.color = colors[planeColorDecision];
        foreach (Image planePRogressBg in planePRogressBgs)
            planePRogressBg.color = colors[planeColorDecision];
        int skyColorDecision;
        do
        {
            skyColorDecision = Random.Range(0, colors.Length);
        } while (planeColorDecision == skyColorDecision);
        Camera.main.backgroundColor = colors[skyColorDecision];
    }
}