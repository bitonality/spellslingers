using UnityEngine;
using System.Collections;

public class OptionMenu : MonoBehaviour {
    public int x;
    public int y;
    public int z;
    void OnTriggerEnter(Collider other)
    {
        GameObject.Find("[CameraRig]").GetComponent<Transform>().position = new Vector3(x, y, z);
    }
}
