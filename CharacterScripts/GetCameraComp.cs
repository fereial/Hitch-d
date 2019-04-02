using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GetCameraComp : MonoBehaviour {

	internal GameObject cameraUp;
	internal GameObject cameraDown;
	internal GameObject fallingDown;

	private void Awake()
	{
		cameraUp = transform.Find("LookingUpCam").gameObject;
		cameraDown = transform.Find("LookingDownCam").gameObject;
		fallingDown = transform.Find("FallingDownCam").gameObject;
	}

	public void lookingUp()
	{
		cameraDown.SetActive(false);
		cameraUp.SetActive(true);
		fallingDown.SetActive(false);
	}
	
	public void lookingdown()
	{
		cameraDown.SetActive(true);
		cameraUp.SetActive(false);
		fallingDown.SetActive(false);
	}

	public void FallingDown()
	{
		cameraDown.SetActive(false);
		cameraUp.SetActive(false);
		fallingDown.SetActive(true);
	}

}


