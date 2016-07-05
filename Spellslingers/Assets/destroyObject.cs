using UnityEngine;
using System.Collections;

public class destroyObject : MonoBehaviour {
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Collided with " + col.gameObject.ToString());
        Destroy(col.gameObject);
    }
}
