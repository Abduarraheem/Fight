using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{

    public float speed = 10.0f;
    public float jumpSpeed = 20.0f;
    public CapsuleCollider cc;
    public LayerMask groundLayers;
    public float gravityScale = 5.0f;
    public float movementInAir = 3.0f;

    private Vector3 direction;
	private Rigidbody rb;
	private float movementX;
	private float movementY;


    private bool grounded;
    private bool doubleJump;
    private bool crouching;
    private bool standing;
    private float dirFacing;
    private bool inContorl;
    private bool forward;

    public List<Collider> BodyParts = new List<Collider>();

    Animator m_Animator;
// handle all player input below //

    void OnMove(InputValue value)
    {
    // handle move events here - use directional but only handle for movement x
        Vector2 v = value.Get<Vector2>();
        if (v.x != 0 & grounded)
        {
          dirFacing = v.x;
        }
            
            
            
        
    movementX = v.x;
    movementY = v.y;
    }

    void OnJump()
    {
        if (grounded & standing)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            m_Animator.SetTrigger("IsJumping");
        }
        if (!grounded & doubleJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            doubleJump = false;
            m_Animator.SetTrigger("IsDoubleJumping");
        }
    }

    void OnAtk1()
    {
        if (!grounded & forward)
        {
            m_Animator.SetTrigger("IsAttack");
        }
        else if (!grounded & !forward)
        {
            m_Animator.SetTrigger("IsAttack");
        }

    }

    void OnCrouch()
    {
        if (grounded & standing)
        {
            m_Animator.SetBool("IsCrouch", true);
            standing = false;
            crouching = true;
        }
        else if (grounded & !standing)
        {
            m_Animator.SetBool("IsCrouch", false);
            standing = true;
            crouching = false;
        }
    }
    // player input ends //

    /* Function that sets body parts to be triggers */
    private void SetBodyParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach(Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                c.isTrigger = true;
                BodyParts.Add(c);
            }
        }
    }

    
    /* Function that unsets body parts to be triggers */
    private void TurnOnBodyParts()
    {
        foreach(Collider c in BodyParts)
        {
            if (c.gameObject != this.gameObject)
            {
                c.isTrigger = false;
                BodyParts.Add(c);
            }
        }
    }

    void Awake()
    {
        SetBodyParts();
    }

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider>();


        standing = true;
        crouching = false;
        grounded = false;
        inContorl = true;
        dirFacing = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 180 - 90 * dirFacing, 0));
        rb.rotation = deltaRotation;
        
    }

    void FixedUpdate()
    {
        
        if (transform.position.y < -2)
        {
            rb.position = new Vector3(0, 2, 0);
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (grounded & !crouching)
        {
            rb.velocity = new Vector3(speed * movementX, rb.velocity.y, 0.0f);
            doubleJump = true;
        }
        else if (!grounded)
        {
            int dir = rb.velocity.x > 0 ? 1 : rb.velocity.x < 0 ? -1 : 0;
            int movementDir = movementX > 0 ? 1 : movementX < 0 ? -1 : 0;
            if (Math.Abs(rb.velocity.x) < speed || dir != movementDir)
                rb.AddForce(new Vector3(movementX, 0, 0) * speed * movementInAir);
        }

        bool walk = movementX != 0;
        if (!grounded) walk = false;
        if (forward = (movementX * dirFacing > 0))
        {
            m_Animator.SetBool("IsForward", true);
        }
        if (forward = (movementX * dirFacing < 0))
        {
            m_Animator.SetBool("IsForward", false);
        }





        m_Animator.SetBool("IsWalking",walk);

        m_Animator.SetBool("IsInAir", !grounded);

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
