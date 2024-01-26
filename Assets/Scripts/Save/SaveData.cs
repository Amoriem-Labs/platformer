using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all the info needed in a save file
// This script is a living document and as the game is developed, more fields will be added to this class.
[Serializable]
public class SaveData
{
    public int levelID; // index of which level the player last saved
}