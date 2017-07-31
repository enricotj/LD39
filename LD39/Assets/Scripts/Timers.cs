using UnityEngine;

public static class Timers
{
    public delegate void TimerCallback();

    public static void DecTimer(ref float timer, TimerCallback callback)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                callback();
            }
        }
    }
}
