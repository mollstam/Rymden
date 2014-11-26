using UnityEngine;
using System.Collections;

public class TimedDisabler : MonoBehaviour
{
    public float Interval;
    public float DisabledTime;
    public float DisabledAt;
    public float DisableAt;

    void Start ()
    {
        DisableAt = Time.time + Interval;
    }

    void Update ()
    {
        if (Time.time > DisableAt)
        {
            renderer.enabled = false;
            DisabledAt = Time.time;
            DisableAt = Time.time + DisabledTime + Interval;
        }
        else if (Time.time > DisabledAt + DisabledTime)
        {
            renderer.enabled = true;
            DisabledAt = float.PositiveInfinity;
        }
    }
}
