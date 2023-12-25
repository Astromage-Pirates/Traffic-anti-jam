using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int highestScore;
    public int currScore;
    public int runTimeStage;
    public int totalBudget;
}
