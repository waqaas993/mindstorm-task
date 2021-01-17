using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LevelFeedbackText : MonoBehaviour
{
    private Text feedback;
    private StringBuilder stringBuilder;

    void Start()
    {
        feedback = GetComponent<Text>();
        stringBuilder = new StringBuilder(50);
    }

    void Update()
    {
        stringBuilder.Length = 0;
        stringBuilder.Append(LevelManager.Instance.levels[LevelManager.Instance.currentLevel - 1].target - LevelManager.Instance.target);
        stringBuilder.Append("/");
        stringBuilder.Append(LevelManager.Instance.levels[LevelManager.Instance.currentLevel - 1].target);
        feedback.text = stringBuilder.ToString();
    }
}