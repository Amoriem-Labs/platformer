using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class TimeManager : MonoBehaviour
{
    public static Action OnMinuteChanged; 
    public static Action OnHourChanged;
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    // makes it so that 0.5seconds real time corresponds to 1 minute gametime.
    private float minuteToRealTime = 0.5f; 
    private float timer;

    void Start()
    {
        // start at 9am
        Minute = 0;
        Hour = 9; 
        timer = minuteToRealTime; 
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; 

        // adds 1 to Minute every 0.5seconds, once hits 60, adds 1 to Hour instead and resets Minute. 
        if(timer <= 0)
        {
            Minute++; 
            // if OnMinuteChanged does not = null, invoke it
            OnMinuteChanged?.Invoke();
            Debug.Log("Minute changed!"); 
            
            if(Minute >= 60)
            {
                Hour++;
                // if OnHourChanged does not = null, invoke it
                Minute = 0; 
                OnHourChanged?.Invoke();
                Debug.Log("Hour changed!");
            }

            timer = minuteToRealTime; 
        }
    }
}
