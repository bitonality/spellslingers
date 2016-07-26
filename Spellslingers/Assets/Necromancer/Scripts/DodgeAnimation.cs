using UnityEngine;
using System.Collections;

public class DodgeAnimation : MonoBehaviour {
   public float rotationspeed = 30.0f;
    public void  Dodge(Vector3 Destination)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Destination, transform.position), rotationspeed);
        gameObject.GetComponent<Animation>().CrossFade("Run");
        // Ask Frankie how AI stops moving and work that into the class
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GameObject.Find("[CameraRig]").transform.position, transform.position), rotationspeed);
        gameObject.GetComponent<Animation>().CrossFade("Idle1");
    }
}
