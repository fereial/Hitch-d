using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraUpDown : MonoBehaviour {

	[SerializeField]
	private GameObject camereaObj;
	private GetCameraComp cameraComponent;

	
	private void Awake()
	{

		cameraComponent = camereaObj.GetComponentInChildren<GetCameraComp>();
	
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
		cameraComponent.lookingUp();
		}

	}


	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
		cameraComponent.lookingdown();
		}
		
	}



}
