using UnityEngine;
using System.Collections;

public class MenuObject : MonoBehaviour {
    public string ScenetoLoad;
	
        void OnTriggerEnter(Collider other)
    {
        Application.LoadLevel(ScenetoLoad);
    }
}
