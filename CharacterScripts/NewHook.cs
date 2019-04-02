using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewHook : MonoBehaviour
{
	#region  Variables

    [SerializeField]
    private Projectile hookPrefab;
    [SerializeField]
    private float hookAimDistance = 1.5f;
    [SerializeField]
    private float detachTime = 5.0f;
	[SerializeField]
	private Projectile currentHook;
	[SerializeField]
	private float maxHookShotDistance;
	[SerializeField]
	private GameObject laser;
	[SerializeField]
	private float timerReset; 	


	[SerializeField] private AudioClip[] throwClips;
	[SerializeField] private AudioClip[] retractClips;
	[SerializeField] private AudioClip[] hookHitClips;
	[SerializeField] private AudioClip[] hookBounceOffClips;

	[SerializeField] private AudioSource throwAudio;
	[SerializeField] private AudioSource retractAudio;
	[SerializeField] private AudioSource hookHitAudio;
	[SerializeField] private AudioSource hookBounceOffAudio;
	
	private GameObject hookGo;
	private PlayerMovement pMovement;
	private Rigidbody player;
	public bool shootAnchor = false;
	internal bool fired = false;
	private RaycastHit laserHit;
	private bool soundPlayed;
	private Vector3 positionOfAnchor;
	private CapsuleCollider myCapsuleCollider;
	private RefScript fishRef;
	private Animator anim;
	private Vector3 hookSpawn;
	private float capsuleHRef;
	private LineRenderer hookLineRenderer;
	private float timer = 0;

	#endregion Variables


	#region  UnityFunctions

    private void Start()
	{
		player = GetComponentInParent<Rigidbody>();
		pMovement = player.GetComponent<PlayerMovement>();
		myCapsuleCollider = player.GetComponentInParent<CapsuleCollider>();
		positionOfAnchor = transform.TransformDirection(Vector3.up);
		positionOfAnchor.y = maxHookShotDistance;
		fishRef = player.GetComponentInChildren<Caca>();
		anim = player.GetComponentInChildren<Animator>();
		capsuleHRef = myCapsuleCollider.height;
		hookLineRenderer = GetComponent<LineRenderer>();

	}

	private void Update()
	{
		if (pMovement.paused == false)
		{
			/*-----------------------------------------------------------------------------
			Get the position of the mouse and assigns it to the position and rotation of the object and 
			flips the player model
			------------------------------------------------------------------------------*/
			transform.localPosition = GetMouseDir() * hookAimDistance;
			transform.rotation = Quaternion.LookRotation(transform.localPosition, Vector3.up);

			fishRef.transform.rotation = new Quaternion (0, transform.localRotation.y > 0 ? 1 : -1 ,0,1);
			hookSpawn = player.transform.position;
			hookSpawn.y += 1.5f;

			/* When the left button is pressed: instantiate the prefab from the anchor */
			timer -= Time.deltaTime;
			if(Input.GetMouseButtonDown(0) && shootAnchor == false)
			{
				if(timer <= 0)
				{
					shootAnchor = true;
					hookGo = Instantiate(hookPrefab.gameObject, player.transform.position, transform.rotation);
					currentHook = hookGo.GetComponent<Projectile>();
					currentHook.SetOwner(transform.parent.gameObject);
					fired |= true;
					timer = timerReset;		

				}
			}

			if(Input.GetMouseButton(1))
			{
				StartCoroutine(DetachHook());
			}

		}
		/*ray cast for the laser indicator  */
		hookLineRenderer.startWidth = 0.05f;
		hookLineRenderer.endWidth = 0.05f;


		hookLineRenderer.SetPosition(0, player.transform.position);

		Ray ray = new Ray(transform.position, transform.localPosition );
	
		if(Physics.Raycast(ray,out laserHit, maxHookShotDistance))
		{
			Debug.DrawRay(player.transform.position,transform.localPosition,Color.red,50);
			hookLineRenderer.SetPosition(1,laserHit.point);
			laser.transform.position = laserHit.point;
		}
		else
		{
			laser.transform.position = transform.localPosition;			
		}
	}

    private void FixedUpdate()
    {
        if (fired == true)
        {
            MoveAndCheckPlayer();
        }
    }

  	#endregion UnityFunctions


    private void MoveAndCheckPlayer()
    {
        if (currentHook.attached == true)
        {
			Vector3 travelVelocity = (hookGo.transform.position - player.transform.position).normalized;
			player.velocity = travelVelocity * 55;
        	myCapsuleCollider.height = 1f;
			anim.SetBool("Atachd",true);

			if (soundPlayed == false)
			{
				PlayHookHitSound();
				soundPlayed = true;
			}
            if (Vector3.Distance(hookGo.transform.position, player.transform.position) > 0 && Vector3.Distance(hookGo.transform.position, player.transform.position) < 2f)
            {
				player.velocity = travelVelocity * 2.5f;
                StartCoroutine(DetachHook());
            }
    	}	
	}

    private Vector2 GetMouseDir()
    {
        Vector2 v2 = new Vector2(
            (2 * Input.mousePosition.x / Screen.width) - 1,
            (2 * Input.mousePosition.y / Screen.height) - 1
        );
        return v2.normalized;
    }
   
	public IEnumerator DetachHook()
	{
		soundPlayed = false;
		currentHook.attached = false;
		fired = false;
		player.useGravity = true;
		currentHook.forDelete = true;
		shootAnchor = false;
		myCapsuleCollider.height = capsuleHRef;
		anim.SetBool("Atachd",false);
		yield return new WaitForSeconds(detachTime);
	}

	public void PlayThrowSound()
	{
		int randSound = Random.Range(0, throwClips.Length);
		throwAudio.clip = throwClips[randSound];

		throwAudio.Play();
	}

	public void PlayHookHitSound()
	{
		int randSound = Random.Range(0, hookHitClips.Length);
		hookHitAudio.clip = hookHitClips[randSound];

			hookHitAudio.Play();
	}

	public void PlayHookBounceOffSound()
	{
		int randSound = Random.Range(0, hookBounceOffClips.Length);
		hookBounceOffAudio.clip = hookBounceOffClips[randSound];

		hookBounceOffAudio.Play();
	}

	public void PlayRetractSound()
	{
		int randSound = Random.Range(0, retractClips.Length);
		retractAudio.clip = retractClips[randSound];

		retractAudio.Play();
	}

}
