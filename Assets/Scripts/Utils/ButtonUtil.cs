using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// This class exists purely to be able to connect Button's onClick events to GameObjects that are located in the DontDestroyOnLoad scene that is generated for singleton managers.
public class ButtonUtil : MonoBehaviour//, IPointerClickHandler
{
    [Tooltip("ID that specifies which method should be called in void Start(). functionID == 0 adds gameManager.DeleteSave(0) to the onClick listener, and functionID == 1 adds gameManager.LoadSave(0) to the onClick listener.")]
    public int functionID; 

    void OnMouseDown(){
            if (functionID == 0){
                // The reason why we need to sandwich the GameManger function call in between setting
                //  persistent objects active and then inactive is because GameManager is a singleton
                //  and will be inactive otherwise when we try to call its functions.
                PersistentObject.Instance.SetActive();
                GameManager.Instance.DeleteSave(0);
                PersistentObject.Instance.SetInactive();
            }
            if (functionID == 1){
                PersistentObject.Instance.SetActive();
                GameManager.Instance.LoadSave(0);
            }
    }
}
