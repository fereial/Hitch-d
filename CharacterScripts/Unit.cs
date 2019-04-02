using UnityEngine;


public class Unit : MonoBehaviour
{

    [SerializeField]
    protected float moveSpeed = 8f;

    [SerializeField]
    protected float raycastOffset = 2.5f;
     [SerializeField]
    protected float wallingOffset = 0.001f;
    [SerializeField]
    protected float raycastDistance = 1.2f;

    internal bool isGrounded = false;
    internal bool isWallingLeft = false;
    protected Rigidbody RB;

    protected void Awake()
    {
        RB = GetComponent<Rigidbody>();

    }
    protected void Move(float direction, float moveSpeed)
    {     
       RB.MovePosition(transform.position + new Vector3(direction * moveSpeed * Time.deltaTime, 0));
    }
}
