using UnityEngine;

public enum GameScreen
{
    Start,
    Gameplay,
    End
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameScreen currentScreen;
    Player player;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        currentScreen = GameScreen.Start;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            player = FindObjectOfType<Player>();
        switch (currentScreen)
        {
            case GameScreen.Start:
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
                        currentScreen = GameScreen.Gameplay;
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    if (Mathf.Abs(player.touchInput()) > 0)
                        currentScreen = GameScreen.Gameplay;
                }
                break;
            case GameScreen.Gameplay:
                break;
            case GameScreen.End:
                break;
            default:
                break;
        }
    }
}