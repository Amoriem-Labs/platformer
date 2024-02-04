using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScoreScreen : MonoBehaviour
{
    public GameObject gradeA;
    public GameObject gradeB;
    public GameObject gradeC;
    public GameObject gradeD;
    public GameObject gradeF;

    void Start(){
        string grade = GameManager.Instance.levelScoringManager.GetGrade();
        switch (grade){
            case "A":
                gradeA.SetActive(true);
                break;
            case "B":
                gradeB.SetActive(true);
                break;
            case "C":
                gradeC.SetActive(true);
                break;
            case "D":
                gradeD.SetActive(true);
                break;
            case "F":
                gradeF.SetActive(true);
                break;
        }
    }
}
