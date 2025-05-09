using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TimerTickEvent : UnityEvent<int, int>
{

}

public class TimerEndEvent : UnityEvent
{

}

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text timerText;
    [SerializeField] AudioSource tickSource;
    [SerializeField] AudioSource tingSource;
    [SerializeField] bool ignoreFirst = true;

    [SerializeField] int testMin = 5;
    [SerializeField] int testSec = 0;
    [SerializeField] bool test = false;

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

    public static CountdownTimer[] Instance
    {
        get
        {
            CountdownTimer[] instances = null;

            if (instances == null)
            {
                instances = FindObjectsOfType<CountdownTimer>();

                foreach (CountdownTimer instance in instances)
                {
                    if (instance.OnTimerTick == null)
                        instance.OnTimerTick = new TimerTickEvent();

                    if (instance.OnTimerEnd == null)
                        instance.OnTimerEnd = new TimerEndEvent();
                }
            }
            return instances;
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
        if (timerRoutine == null)
        {
            timerRoutine = StartCoroutine(CountdownTimerRoutine());
        }
    }

    /// <summary>
    /// Stops Timer
    /// </summary>
    public void StopTimer()
    {
        if (timerRoutine != null)
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

                if (ignoreFirst)
                {
                    ignoreFirst = false;
                }
                else
                    tingSource.Play();
            }

            tickSource.Play();

            OnTimerTick?.Invoke(minutes, Seconds);
            timerText.text = string.Format("{0}:{1}", minutes > 9 ? minutes.ToString() : "0" + minutes, seconds > 9 ? seconds.ToString() : "0" + seconds);
        }

        OnTimerEnd?.Invoke();
        timerRoutine = null;
    }

    public string GetCurrentTimeString()
    {
        return string.Format("{0}{1}", minutes > 9 ? minutes.ToString() : "0" + minutes, seconds > 9 ? seconds.ToString() : "0" + seconds);
    }

    private void Start()
    {
        if (test)
        {
            SetTimer(testMin, testSec);
            StartTimer();
        }
    }
}
