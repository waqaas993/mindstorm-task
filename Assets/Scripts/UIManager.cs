using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    public Transform[] screens;
    //[Header("Start")]

    [Header("Gameplay")]
    public Image levelProgressBar;
    public Text levelFrom;
    public Text levelTo;
    public Transform scoreTransform;

    [Header("End")]
    public Text levelEndReason;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        currentScreen = GameScreen.Start;
    }

    public void StartGameplay(int level)
    {
        levelProgressBar.fillAmount = 0;
        levelFrom.text = level.ToString();
        levelTo.text = (level+1).ToString();
    }

    public void tweenScoreFeedback()
    {
        scoreTransform.localScale = Vector3.one;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(scoreTransform.DOScale(Vector3.one * 2, 0.25f).SetUpdate(UpdateType.Normal, true));
        mySequence.Append(scoreTransform.DOScale(Vector3.one, 0.25f).SetUpdate(UpdateType.Normal, true));
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
                    if (Mathf.Abs(player.inputTouch()) > 0)
                        currentScreen = GameScreen.Gameplay;
                }
                break;
            case GameScreen.Gameplay:
                levelProgressBar.fillAmount = Mathf.Lerp(
                    levelProgressBar.fillAmount,
                    1 - (LevelManager.Instance.levelEndGO.transform.position.z - player.transform.position.z) / (LevelManager.Instance.levelEndGO.transform.position.z - player.startingPosition.z),
                    Time.deltaTime * 3);
                break;
            case GameScreen.End:
                if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0
                    || Input.touchCount > 0)
                {
                    screens[(int)GameScreen.End].gameObject.SetActive(false);
                    currentScreen = GameScreen.Start;
                    LevelManager.Instance.StartGameplay();
                }
                break;
            default:
                break;
        }
    }

    public void screenFlyIn(GameScreen screen, float tweenTime)
    {
        currentScreen = screen;
        if (!screens[(int)currentScreen].gameObject.activeSelf)
            screens[(int)currentScreen].gameObject.SetActive(true);
        screens[(int)currentScreen].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 2000);
        screens[(int)currentScreen].GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, tweenTime);
    }
}