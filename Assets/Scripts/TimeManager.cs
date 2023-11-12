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
    // makes it so that 1seconds real time corresponds to 1 minute game time.
    private float minuteToRealTime = 1f; 
    private float timer;

    void Start()
    {
        // start at 00:00:00
        Minute = 0;
        Hour = 0; 
        timer = minuteToRealTime; 
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; 

        // adds 1 to Minute every 1seconds, once hits 60, adds 1 to Hour instead and resets Minute. 
        if(timer <= 0)
        {
            Minute++; 
            // if OnMinuteChanged does not = null, invoke it
            OnMinuteChanged?.Invoke();
            
            if(Minute >= 60)
            {
                Hour++;
                // if OnHourChanged does not = null, invoke it
                Minute = 0; 
                OnHourChanged?.Invoke();
            }

            timer = minuteToRealTime; 
        }
    }
}
