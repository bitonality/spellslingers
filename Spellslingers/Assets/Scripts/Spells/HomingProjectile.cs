using UnityEngine;
using System.Collections;

public class HomingProjectile : MonoBehaviour {

    private Transform target;
    private float force = 0.1f;


    public void LaunchProjectile(float delay, float interval, Transform target)
    {
        this.target = target;
        InvokeRepeating("ApplyTorque", delay, interval);
    }


    void ApplyTorque () {

        Vector3 targetDelta = target.position - transform.position;

        //get the angle between transform.forward and target delta
        float angleDiff = Vector3.Angle(transform.forward, targetDelta);

        // get its cross product, which is the axis of rotation to get from one vector to another
        Vector3 cross = Vector3.Cross(transform.forward, targetDelta);

        // apply torque along that axis according to the magnitude of the angle.
        this.gameObject.GetComponent<Rigidbody>().AddTorque(cross * angleDiff * force);

    }
}
