using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float speed = 1;
    public float jumpSpeed = 10.0f;
    public CapsuleCollider cc;
    public LayerMask groundLayers;
    public float gravityScale = 2;


    private Vector3 direction;
	private Rigidbody rb;
	private float movementX;
	private float movementY;
    private bool canJump;
    private int doubleJump;
    //Animator m_Animator;
// handle all player input below //

    void OnMove(InputValue value)
    {
    // handle move events here - use directional but only handle for movement x
        Vector2 v = value.Get<Vector2>();
        if (v.x != 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 180 - 90 * v.x, 0));
            rb.rotation = deltaRotation;
        }
        movementX = v.x;
        movementY = v.y;
    }

    void OnJump()
    {
        if (canJump)
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
    }

    void OnAtk1()
    {

    }

    void OnCrouch()
    {
        //m_Animator.SetTrigger("IsCrouching");
    }
// player input ends //

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
        //m_Animator = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider>();

        canJump = false;
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 movement = new Vector3(movementX, 0.0f, movementY);
		rb.AddForce(movement * speed);
        
        if (transform.position.y < -2)
        {
            rb.position = new Vector3(0, 2, 0);
            rb.velocity = new Vector3(0, 0, 0);
        }


        bool walk = movementX != 0;
        if (!canJump) walk = false;
        //m_Animator.SetBool("IsWalking",walk);
    }

    void FixedUpdate()
    {

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);

        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

    }

    // two methods to check if player can jump

    
    void OnCollisionEnter(Collision collider)
	{
        
		if (collider.gameObject.CompareTag("Floor"))
			canJump = true;
        
    }

	void OnCollisionExit(Collision collider)
	{
        
		if (collider.gameObject.CompareTag("Floor"))
        	canJump = false;
        
  
    }
    

    private bool IsGround()
    {
        return Physics.CheckCapsule(cc.bounds.center, new Vector3(cc.bounds.center.x,
            cc.bounds.min.y, cc.bounds.center.z), cc.radius * .9f, groundLayers);
    }
}
