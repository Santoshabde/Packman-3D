using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    [Header("Count Down Start Time:")]
    [SerializeField] private float minutes;
    [SerializeField] private float seconds;

    [Header("Clock Text")]
    [SerializeField] TextMeshPro timerText;

    public static Action<float,float> OnTimerStopped;
    private void Awake()
    {
        StartTimer(minutes, seconds);
        timerText.enabled = false;
    }

    public void StartTimer(float minutes, float seconds)
    {
        timerText.text = minutes.ToString("00") + ":" + Mathf.Round(seconds).ToString("00");
        StartCoroutine(StartTimer_Coroutine(minutes, seconds));
    }

    private IEnumerator StartTimer_Coroutine(float minutes, float seconds)
    {
        while(!StartTheClock())
        {
            yield return null;
        }

        timerText.enabled = true;

        while (RunTheClock())
        {
            seconds += Time.deltaTime;
            if (seconds >= 60)
            {
                minutes += 1;
                seconds = 0;
            }

            timerText.text = minutes.ToString("00") + ":" + Mathf.Round(seconds).ToString("00");
            yield return null;
        }

        OnTimerStopped?.Invoke(minutes, seconds);
    }

    protected virtual bool StartTheClock() => true;

    protected virtual bool RunTheClock() => true;
}
