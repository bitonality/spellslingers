using UnityEngine;
using System.Collections;

public class InfluenceValue {
    private bool Status;
    private float TimeEnabled;
    private string FriendlyName;
    public InfluenceValue(bool InitialStatus, float InitialTime, string InitialName)
    {
        Status = InitialStatus;
        TimeEnabled = InitialTime;
        FriendlyName = InitialName;
    }

    public void SetStatus(bool NewStatus)
    {
        Status = NewStatus;
    }

    public void SetTime(float offset)
    {
        TimeEnabled = offset + Time.time;
    }

    public void SetName(string NewName)
    {
        FriendlyName = NewName;
    }

    public bool GetStatus()
    {
        return Status;
    }

    public float GetTime()
    {
        return TimeEnabled;
    }

    public string GetName()
    {
        return FriendlyName;
    }
}
