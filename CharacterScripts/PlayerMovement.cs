using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : Unit
{

    #region Globals

        [SerializeField]
        private float jumpSpeed = 5f;
        [SerializeField] 
        private GameObject UI;
        [SerializeField]
        private AudioClip[] deathSounds;
        [SerializeField]
        private AudioSource deathAudio;

        private Animator anim;
        private float xInput = 0f;
        private bool doJump = false;
        public bool paused = false;
        private Caca playerRef;

        private NewHook newHookRef;


    #endregion Globals


    #region UnityFunctions

    private new void Awake()
        {
            base.Awake();
            anim = GetComponentInChildren<Animator>();
            playerRef = GetComponentInChildren<Caca>();
            newHookRef = GetComponentInChildren<NewHook>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                Pause();
            }

            if (paused == false)
            {
         
                xInput = Input.GetAxis("Horizontal");
                doJump |= Input.GetKeyDown(KeyCode.Space);
                isGrounded = IsGrounded(raycastOffset, raycastDistance) || IsGrounded(-raycastOffset, raycastDistance);

                if(Input.GetKeyDown("r"))
                {
                    SceneManager.LoadScene(1);
                }
                if(Input.GetMouseButtonDown(0))
                {
                    anim.SetTrigger("Hook");
                }
                if(Input.GetKey(KeyCode.A) && isGrounded == true)
                {
                    
                    anim.SetFloat("Speed", 8f);
                }
                else if(Input.GetKey(KeyCode.D) && isGrounded == true)
                {
                
                    anim.SetFloat("Speed", 8f);
                }
                else if(Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.A))
                {
                    anim.SetFloat("Speed", 0f);
                }
                if(Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
                {
                    anim.SetBool("Jump", true);
                }
                else if (isGrounded == false)
                {
                    anim.SetBool("Jump", false );
                }
            } 
        }
        

        private void FixedUpdate()
        {

            Move(xInput, moveSpeed);

            if(isGrounded) 
            {
                Jump(doJump, jumpSpeed);
            }
           

            doJump = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("DamageVolume"))
            {
                transform.position = ReturnToCheckpoint(transform.position);
                DeathAudio();
            }
            if (other.gameObject.CompareTag("Checkpoint"))
            {
                SetCurrentCheckpoint(other.transform.position);
                Destroy(other.gameObject);
            }
            if (other.gameObject.CompareTag("GameOverVolume"))
            {
                SceneManager.LoadScene(2);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("DamageVolume"))
            {
                transform.position = ReturnToCheckpoint(transform.position);
            }    
        }
 


    #endregion UnityFunctions


    #region ClassFunctions

        private void Jump(bool doJump, float jumpSpeed)
        {
            if(doJump)
            {
                RB.AddForce(0, jumpSpeed, 0, ForceMode.Impulse);
                anim.SetTrigger("Jump");
            }
        }
    protected bool IsGrounded(float xOffset, float raycastDistance)
    {
        Vector3 direction = Vector3.down;
 
        Debug.DrawRay(direction, Vector3.down * raycastDistance, Color.red, 0.1f);
        if(Physics.Raycast(transform.position,direction,2f))
        {
            anim.SetBool("Grounded", true);
            return true;
        }
        else
        {
            anim.SetBool("Grounded", false);
            return false;
        }
    }
  

    private Vector3 ReturnToCheckpoint(Vector3 playerPosition)
    {
        playerPosition = GameManager.Instance.currentCheckpoint;
         StartCoroutine(newHookRef.DetachHook());
         RB.velocity = Vector3.zero;
		return playerPosition;
    }

    private void SetCurrentCheckpoint(Vector3 checkpointPosition)
    {
        GameManager.Instance.currentCheckpoint = new Vector3 (checkpointPosition.x, checkpointPosition.y, 0);    
    }

    private void Pause()
    {
        if (Time.timeScale < 1)
        {
            Cursor.visible = true; //set to false if raycast works;
            paused = false;
            Time.timeScale = 1f;
            UI.SetActive(false);
        }

        else
        {
            Cursor.visible = true;
            paused = true;
            Time.timeScale = 0;
            UI.SetActive(true);
        }
    }

    private void DeathAudio()
    {
        StartCoroutine("DeathAudioDelayed");
    }

    IEnumerator DeathAudioDelayed()
    {
        yield return new WaitForSeconds(0.2f);
        int randSound = Random.Range(0, deathSounds.Length);
		deathAudio.clip = deathSounds[randSound];

		deathAudio.Play();
    }

    #endregion ClassFunctions
    
}
