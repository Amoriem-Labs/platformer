using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class used to make every child of this object a singleton and persist between scenes... 
//  probably not the cleanest solution, but it works for keeping only one copy of these objects 
//  when switching back and forth between main menu scenes and level scenes.
public class PersistentObject : MonoBehaviour
{
    private static PersistentObject _instance;
	public static PersistentObject Instance { get { return _instance; } }

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);
            gameObject.SetActive(false);
        }
    }

    public void SetActive(){
        gameObject.SetActive(true);
    }
}
