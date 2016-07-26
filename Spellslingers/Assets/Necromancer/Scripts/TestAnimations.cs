using UnityEngine;
using System.Collections;

public class TestAnimations : MonoBehaviour {
    public Animation anim;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(kms());
    }
	
	// Update is called once per frame
    IEnumerator kms(){
        anim = GetComponent<Animation>();
        yield return new WaitForSeconds(3);
        anim.CrossFade("Sits");
     }
}
