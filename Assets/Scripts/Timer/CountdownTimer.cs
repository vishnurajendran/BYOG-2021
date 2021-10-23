using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TimerTickEvent: UnityEvent<int, int>
{

}

public class TimerEndEvent : UnityEvent
{

}

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text timerText;

    int minutes = 0;
    int seconds = 0;

    Coroutine timerRoutine = null;

    /// <summary>
    /// Invoked every second
    /// </summary>
    public TimerTickEvent OnTimerTick = null;
    /// <summary>
    /// Invoked when timer ends
    /// </summary>
    public TimerEndEvent OnTimerEnd = null;

    private static CountdownTimer instance;
    public static CountdownTimer Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<CountdownTimer>();

            if (instance.OnTimerTick == null)
                instance.OnTimerTick = new TimerTickEvent();

            if (instance.OnTimerEnd == null)
                instance.OnTimerEnd = new TimerEndEvent();

            return instance;
        }
    }

    public int Minutes
    {
        get
        {
            return minutes;
        }
    }

    public int Seconds
    {
        get
        {
            return seconds;
        }
    }

    /// <summary>
    /// Sets Timer
    /// </summary>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    public void SetTimer(int minutes, int seconds)
    {
        if (seconds > 59)
        {
            int additionalMins = seconds / 60;
            seconds = seconds % 60;
            minutes += additionalMins;
        }

        this.minutes = minutes;
        this.seconds = seconds;
    }

    /// <summary>
    /// Starts Timer
    /// </summary>
    public void StartTimer()
    {
        if(timerRoutine == null)
        {
            timerRoutine = StartCoroutine(CountdownTimerRoutine());
        }
    }

    /// <summary>
    /// Stops Timer
    /// </summary>
    public void StopTimer()
    {
        if(timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
        }
    }

    IEnumerator CountdownTimerRoutine()
    {
        while (minutes > 0 || seconds > 0)
        {
            yield return new WaitForSeconds(1);
            seconds -= 1;

            if (seconds < 0 && minutes > 0)
            {
                seconds = 59;
                minutes -= 1;
            }

            OnTimerTick?.Invoke(minutes, Seconds);
            timerText.text = string.Format("{0}:{1}", minutes > 10 ? minutes.ToString() : "0" + minutes, seconds > 10 ? seconds.ToString() : "0" + seconds);
        }

        OnTimerEnd?.Invoke();
        timerRoutine = null;
    }

    private void Start()
    {
        SetTimer(5, 0);
        StartTimer();
    }
}
