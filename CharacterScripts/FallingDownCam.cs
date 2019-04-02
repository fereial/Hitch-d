using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDownCam : MonoBehaviour
{

    [SerializeField]
    private GameObject cameraObj;
    private GetCameraComp cameraComponent;

    private void Awake()
    {

        cameraComponent = cameraObj.GetComponentInChildren<GetCameraComp>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraComponent.FallingDown();
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraComponent.lookingdown();
        }

    }



}
