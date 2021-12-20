using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerObject
{
    public static System.Action<int> OnTimerChanged = delegate { };
    public int displayTimer;
    private Coroutine timer;

    public void StartTimer(MonoBehaviour mb, float duration)
    {
        if (timer != null)
        {
            Debug.Log("Timer Already Runs");
            return;
        }

        timer = mb.StartCoroutine(TimerRuns(duration));
    }

    public void stopTimer(MonoBehaviour mb)
    {
        if (timer == null)
        {
            Debug.Log("There are no timer currently running");
            return;
        }

        mb.StopCoroutine(timer);
        timer = null;
    }

    IEnumerator TimerRuns(float duration)
    {
        while (duration > 0f)
        {
            OnTimerChanged((int) duration);
            displayTimer = (int)duration;
            duration -= 1f;
            yield return new WaitForSeconds(1f);
        }

        timer = null;
    }
    
}
