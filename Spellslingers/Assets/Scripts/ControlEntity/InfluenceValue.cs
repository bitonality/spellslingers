using UnityEngine;
using System.Collections;

public class InfluenceValue : MonoBehaviour {
    private bool status;
    private float timeEnabled;
    public InfluenceValue(bool s, float t)
    {
        status = s;
        timeEnabled = t;
    }

    public void SetStatus(bool newStatus)
    {
        status = newStatus;
    }

    public void setTime(float offset)
    {
        timeEnabled = offset + Time.time;
    }

    public bool getStatus()
    {
        return status;
    }

    public float getTime()
    {
        return timeEnabled;
    }

}
