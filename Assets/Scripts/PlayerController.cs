using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{

    public float speed = 1;
    public float jumpSpeed = 10.0f;
    public CapsuleCollider cc;
    public LayerMask groundLayers;
    public float gravityScale = 2;
    public float movementInAir = 0.2f;

    private Vector3 direction;
	private Rigidbody rb;
	private float movementX;
	private float movementY;
    private bool grounded;
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
        if (grounded)
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

        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        //m_Animator.SetBool("IsWalking",walk);
    }

    void FixedUpdate()
    {
        
        if (transform.position.y < -2)
        {
            rb.position = new Vector3(0, 2, 0);
            rb.velocity = new Vector3(0, 0, 0);
        }

        float tmpSpeed;
        if (grounded)
        {
            rb.velocity = new Vector3(speed * movementX, rb.velocity.y, 0.0f);
        }
        else
        {
            int dir = rb.velocity.x > 0 ? 1 : rb.velocity.x < 0 ? -1 : 0;
            int movementDir = movementX > 0 ? 1 : movementX < 0 ? -1 : 0;
            if (Math.Abs(rb.velocity.x) < speed || dir != movementDir)
                rb.AddForce(new Vector3(movementX, 0, 0) * speed * movementInAir);
        }

        bool walk = movementX != 0;
        if (!grounded) walk = false;

        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);

    }

    // two methods to check if player can jump

    
    void OnCollisionEnter(Collision collider)
	{
        
		if (collider.gameObject.CompareTag("Floor"))
			grounded = true;
        
    }

	void OnCollisionExit(Collision collider)
	{
        
		if (collider.gameObject.CompareTag("Floor"))
        	grounded = false;
        
  
    }
    

    private bool IsGround()
    {
        return Physics.CheckCapsule(cc.bounds.center, new Vector3(cc.bounds.center.x,
            cc.bounds.min.y, cc.bounds.center.z), cc.radius * .9f, groundLayers);
    }
}
