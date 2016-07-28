using UnityEngine;
using System.Collections;

public class HapticFeedBackLoopTesting : MonoBehaviour
{
    SteamVR_Controller.Device device;
    // Use this for initialization
    void Start()
    {
        startCoroutine(LongVibration(1000, 3000));
    }

    // Update is called once per frame
    IEnumerator LongVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }
}