using UnityEngine;
using System.Collections;

public class OptionMenu : MonoBehaviour {
    public float x;
    public float y;
    public float z;
    void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Camera").GetComponent<Transform>().position = new Vector3(x, y, z);
    }
}
