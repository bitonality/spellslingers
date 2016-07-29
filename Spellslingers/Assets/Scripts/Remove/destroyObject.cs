using UnityEngine;
using System.Collections;

public class destroyObject : MonoBehaviour {
    void OnTriggerEnter(Collider col)
    {
		if (col.tag != "Wall"){
        Debug.Log("Collided with " + col.gameObject.ToString());
        Destroy(col.gameObject);
    }
}
}