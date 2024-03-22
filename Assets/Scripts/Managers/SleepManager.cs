using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepManager : MonoBehaviour
{
    private static SleepManager _instance;
	public static SleepManager Instance { get { return _instance; } }
    public delegate void OnSleepTimerUpdate(float timeInTimer, float maxTime);
    public static event OnSleepTimerUpdate onSleepTimerUpdate;

    public delegate void OnChangeSleepValue(float changeAmt);
    public static event OnChangeSleepValue onChangeSleepValue;
    public static void CallChangeSleepValue(float amt) { onChangeSleepValue?.Invoke(amt); }

    public Slider sleepTimer;
    public GameObject vignette;
    public GameObject sleepDemon;
    public float timeLeftToActivateVignette;
    public float timeLeftToActivateSleepDemon;

    public float maxTime; // the original time for the sleep timer for a level
    public float upgradesToMaxTime; // any additions to the sleep timer purchased from the shop
    public float timeInTimer;
    public float timeSpent;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }

    void Start()
    {
        ResetTimer();
        vignette.SetActive(false);
        sleepDemon.SetActive(false);
        onSleepTimerUpdate += UpdateSlider;
        onSleepTimerUpdate += CheckIfCanActivateSleepDebuffs;
        onChangeSleepValue += ChangeSleepValue;
    }
    
    void OnDestroy()
    {
        onSleepTimerUpdate -= UpdateSlider;
        onSleepTimerUpdate -= CheckIfCanActivateSleepDebuffs;
        onChangeSleepValue -= ChangeSleepValue;
    }

    public void ResetTimer(){
        timeInTimer = maxTime + upgradesToMaxTime;
        timeSpent = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isGamePaused){
            timeSpent += Time.deltaTime;
            timeInTimer -= Time.deltaTime;
            onSleepTimerUpdate?.Invoke(timeInTimer, maxTime);
        }
    }

    private void ChangeSleepValue(float amt)
    {
        timeInTimer += amt;
        if (timeInTimer > maxTime)
        {
            timeInTimer = maxTime;
        }
        if (timeInTimer < 0)
        {
            timeInTimer = 0;
        }

        onSleepTimerUpdate.Invoke(timeInTimer, maxTime);
    }

    public void UpdateSlider(float _timeInTimer, float _maxTime)
    {
        sleepTimer.value = _timeInTimer / _maxTime;
    }

    public void CheckIfCanActivateSleepDebuffs(float _timeInTimer, float _maxTime)
    {
        if (_timeInTimer <= timeLeftToActivateVignette)
        {
            if (vignette.activeSelf == false)
            {
                vignette.SetActive(true);
            }
        } else {
            vignette.SetActive(false);
        }
        if (_timeInTimer <= timeLeftToActivateSleepDemon)
        {
            if (sleepDemon.activeSelf == false)
            {
                sleepDemon.SetActive(true);
                Vector3 playerPos = GameManager.Instance.player.transform.position;
                sleepDemon.transform.position = new Vector3(playerPos.x - 15, playerPos.y, playerPos.z);
            }
        } else {
            sleepDemon.SetActive(false);
        }
    }
}
