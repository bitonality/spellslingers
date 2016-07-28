using UnityEngine;
using System.Collections;

public class HapticFeedBackLoopTesting : MonoBehaviour
{
    /*
    // Initialize here to avoid problems with non-static device methods
    void Start()
    {
        StartCoroutine(LongVibration(1, 3999));
    }

	IEnumerator LongVibration(SteamVR_Controller.Device device,	float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }
    */
}