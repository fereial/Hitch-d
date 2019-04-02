using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{

	[SerializeField]
    private float bulletSpeed;
	[SerializeField]
	private float maxHookShotDistance;
	[SerializeField]
	private float travelSpeed;
	[SerializeField]
	private float lineOffsetY = 1;

	private GameObject mOwner;
	internal bool attached = false;
	internal bool forDelete = false;
	internal bool collisionOn = true;
	internal bool returnHook = false ;
	private float traveledDistance;
	private NewHook ownerNewHook;
	private Rigidbody RB;
	private Collider CD;
	private LineRenderer lineRenderer;
	

    private void Start()
	{
		RB = GetComponent<Rigidbody>();
		RB.velocity = transform.forward * bulletSpeed;
		CD = GetComponent<Collider>();
		ownerNewHook = mOwner.GetComponentInChildren<NewHook>();
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		// Debug.Log(mOwner.transform.position);
		traveledDistance = Vector3.Distance(mOwner.transform.position,transform.position);
		if(attached == false)
		{
			RB.useGravity = true;
			RB.isKinematic = false;
		}
		if(collisionOn == false)
		{
			CD.enabled = false;
		}
		else
		{
			CD.enabled = true;
		}	
		if(forDelete == true)
		{
			Destroy(gameObject);
		}
		if(traveledDistance >= maxHookShotDistance)
		{
			returnHook = true;
		}
		lineRenderer.startWidth = 0.1f;
		lineRenderer.endWidth = 0.1f;
		Vector3 ownerOffset = mOwner.transform.position;
		ownerOffset.y += lineOffsetY;
        lineRenderer.SetPosition (0,ownerOffset);
        lineRenderer.SetPosition(1,transform.position);
	}
	
	private void FixedUpdate()
	{
		if(returnHook == true)
		{
			Vector3 anchorVelocity = ((mOwner.transform.position - transform.position)).normalized;
		
			RB.useGravity = false;
			RB.velocity = anchorVelocity* 55;
			collisionOn = false;
			if(transform.position.y <= mOwner.transform.position.y)
			{
				RB.isKinematic = true;
				ownerNewHook.shootAnchor = false;
				ownerNewHook.fired = false;
				forDelete = true;

			}
		}
	}

	private void OnCollisionEnter(Collision other)
	{
	
		if(other.gameObject.tag != "Unhookable")
		{
			ownerNewHook.PlayHookBounceOffSound();
			RB.useGravity = false;
			RB.isKinematic = true;
			attached = true;
		}
		else
		{
			returnHook = true;
		}
		
	}
	
	public void SetOwner(GameObject owner)
	{
		mOwner = owner;
	}

}
