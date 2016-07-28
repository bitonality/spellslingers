using UnityEngine;
using System.Collections;

public class HapticFeedBackLoopTesting : MonoBehaviour
{
    public SteamVR_Controller.Device device;
    // Initialize here to avoid problems with non-static device methods
    void Start()
    {
        StartCoroutine(TriggerVibration(device, 1, 3999));
    }

	IEnumerator TriggerVibration(SteamVR_Controller.Device device,	float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            Debug.Log("Firing feedback");
            device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }
}