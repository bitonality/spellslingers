using UnityEngine;
using System.Collections;

public class rockEvent : MonoBehaviour {
    int framesSinceLastRock = 0;
	
	// Update is called once per frame
	void Update () {
        framesSinceLastRock += 1;
	}
}
