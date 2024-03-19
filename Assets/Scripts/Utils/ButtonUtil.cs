using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

// This class exists purely to be able to connect Button's onClick events to GameObjects that are located in the DontDestroyOnLoad scene that is generated for singleton managers.
public class ButtonUtil : MonoBehaviour
{
    [Tooltip("ID that specifies which method should be called in void Start(). functionID == 0 adds gameManager.DeleteSave(0) to the onClick listener, and functionID == 1 adds gameManager.LoadSave(0) to the onClick listener.")]
    public int functionID; 

    void Update(){
        if (Input.GetMouseButtonDown(0)){ // left-click
            if (functionID == 0){
                GameManager.Instance.DeleteSave(0);
            }
            if (functionID == 1){
                PersistentObject.Instance.SetActive();
                GameManager.Instance.LoadSave(0);
            }
        }
    }
}
