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

        if (PreviousPosition != this.gameObject.transform.position)
            {
                continueupdate=false;
                GameObject.Find("Canvas").GetComponent<UnityEngine.UI.Text>().text=("Great, now, hold the trigger to open your castagon, these attempt to draw these shapes IN ORDER to create the different types of hexes spellslingers offers.");
            }

        }
    }

