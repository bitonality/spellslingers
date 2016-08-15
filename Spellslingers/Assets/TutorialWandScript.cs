using UnityEngine;
using System.Collections;

public class TutorialWandScript : MonoBehaviour {
    Vector3 PreviousPosition;
    bool continueupdate = true;
    void Start() {
        PreviousPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update() {
        while (continueupdate == true){
            if (this.gameObject.transform.position != PreviousPosition)
            {
                continueupdate=false;
                GameObject.Find("StateAI").GetComponent<UnityEngine.UI.Text>().text=("Great, now, hold the trigger to open your castagon, these attempt to draw these shapes IN ORDER to create the different types of hexes spellslingers offers.");
            }

        }
    }
}
