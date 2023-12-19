using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{

    private TextMeshProUGUI assignmentText;

    // Start is called before the first frame update
    void Start()
    {
        assignmentText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateAssignmentText(PlayerInventory playerInventory)
    {
        assignmentText.text = playerInventory.numItems.ToString();
    }


}
