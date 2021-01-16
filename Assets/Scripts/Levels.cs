using UnityEngine;

[CreateAssetMenu(fileName = "New Levels", menuName = "Level")]
public class Levels : ScriptableObject
{
    public int target;
    public int movingGlass;
    public float glassMovementSpeed;
    public int staticGlass;
    public int pole;
    public float distanceBetweenObstacles;
}