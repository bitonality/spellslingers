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

            }

        }
    }

