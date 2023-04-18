using UnityEngine;

[CreateAssetMenu(fileName = "Difficluty", menuName = "Difficluty")]

public class Difficult : ScriptableObject
{
    [SerializeField] private DifficultLevel difficult;
    
    [SerializeField]private int easy = 20;
    [SerializeField]private int medium = 10;
    [SerializeField]private int hard = 5;

    public int DifficultyLevel()
    { 
        if(difficult == DifficultLevel.easy) return easy;
        else if(difficult == DifficultLevel.middle) return medium;
        else return hard;
    }
}
