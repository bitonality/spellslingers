using UnityEngine;
using System.Collections;

public class TestAnimations : MonoBehaviour {
    public Animation anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animation>();
        anim.CrossFade("Death1");
        anim.CrossFade("Walk");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
