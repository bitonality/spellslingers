using UnityEngine;
using System.Collections;

public class OptionMenu : MonoBehaviour {
    public float x;
    public float y;
    public float z;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("GameController"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = new Vector3(x, y, z);
        }
    }
}
