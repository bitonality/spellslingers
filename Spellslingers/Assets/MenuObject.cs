using UnityEngine;
using System.Collections;

public class MenuObject : MonoBehaviour {
    void Start (){
       PlayerPrefs.SetInt("difficulty", 2); // Initialize this here so we don't have problems later down the line in case of difficulty returning null. Default difficulty : Medium
    }
    public string ScenetoLoad;
        void OnTriggerEnter(Collider other)
    {

        Application.LoadLevel(ScenetoLoad);
    }
}
