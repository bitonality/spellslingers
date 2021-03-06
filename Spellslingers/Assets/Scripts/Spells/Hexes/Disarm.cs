﻿using UnityEngine;
using System.Collections;
using VRTK;

public class Disarm : Hex {

	public float AiDisarmTime = 2.5F;


	public override void playerCollide (GameObject playerCameraRig)
	{			
		//Disarm wand regardless of hand
		VRTK_InteractGrab[] controllers = playerCameraRig.GetComponentsInChildren<VRTK_InteractGrab>();
		foreach (VRTK_InteractGrab controller in controllers) {
			if (controller.GetGrabbedObject () == null)
				continue;
			GameObject wand = controller.GetGrabbedObject ();
			controller.ForceRelease();
			wand.GetComponent<Rigidbody> ().isKinematic = false;
			wand.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 10, 0);
			wand.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (8, 9, 10);
		}
        playerCameraRig.GetComponent<ControlEntity>().ApplyInfluence(influences.DISARM);
        playerCameraRig.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.DISARM, 0F);
    }

	public override void aiCollide(GameObject aiBody) {
        aiBody.GetComponent<ControlEntity>().ApplyInfluence(influences.DISARM);
        aiBody.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.DISARM, AiDisarmTime);
    }
}
