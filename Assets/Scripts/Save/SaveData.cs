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
    public float musicVolume; // volume of music soundtracks in game
    public float sfxVolume; // volume of sfx (sound effects) in game
    public int numCoins; // number of coins in player inventory
}