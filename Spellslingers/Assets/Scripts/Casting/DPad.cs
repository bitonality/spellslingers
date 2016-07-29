    using UnityEngine;
    using System.Collections;
     //Helper class
    public class DPad {
     
     
     
	public static DPad_Direction? GetButtonPressed(SteamVR_Controller.Device controller)
        {

       	
                if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y > 0.5f)
                {
					return DPad_Direction.UP;
                }
     
                if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y < -0.5f)
                {
					return DPad_Direction.DOWN;
                }
     
                if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x > 0.5f)
                {
					return DPad_Direction.RIGHT;
                }
     
                if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x < -0.5f)
                {
					return DPad_Direction.LEFT;
                }

		return null;

        }



    }
