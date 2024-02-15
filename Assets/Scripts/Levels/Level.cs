using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public int levelID;
    public int levelNum;
    public int numAssignmentsToComplete;
    public int maxTime;
    public string sceneName;
    public Vector2 playerSpawnPoint;
}
