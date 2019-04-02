using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour 
{
	[SerializeField] private float camUpwardsDistance = 20;
	[SerializeField] private GameObject mover;

	//private Vector3 initialPos;
	private Vector3 destinationPos;

	RaycastHit hit1, hit2;

	public void SetInitialCameraPos(Vector3 origin)
	{
		Physics.Raycast(origin, Vector3.left, out hit1, 100);
		Physics.Raycast(origin, Vector3.right, out hit2, 100);

		transform.position = new Vector3((hit1.collider.transform.position.x + hit2.collider.transform.position.x)/2, origin.y, origin.z - 30);
	}

	public void MoveCamera()
	{	
		//initialPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		destinationPos = new Vector3(transform.position.x, transform.position.y + camUpwardsDistance, transform.position.z);

		StartCoroutine(LerpCamera(destinationPos));
		// StartCoroutine(Mover(mover));
	}

	IEnumerator LerpCamera(Vector3 fin)
	{
		while (transform.position.y < fin.y)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
			yield return new WaitForEndOfFrame();
		}
	}

	
}
