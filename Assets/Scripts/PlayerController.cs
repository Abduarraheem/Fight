using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{

    public float speed = 10.0f;
    public float jumpSpeed = 20.0f;
    public float gravityScale = 5.0f;
    public float movementInAir = 3.0f;
    public CapsuleCollider cc;
    public LayerMask groundLayers;
    public HealthBar healthBar;

    private AudioSource walking_audio;
    private AudioSource attacking_audio;
    public ParticleSystem jumpCloud;

    public ParticleSystem rHand;
    public ParticleSystem lHand;
    public ParticleSystem rFoot;
    public ParticleSystem lFoot;


    private float normSpeed;
    private float runSpeed;




    private Vector3 direction;
    private Vector2 horizontalInput;
	private Rigidbody rb;
	private float movementX;
	private float movementY;


    private float attackTime;
    private bool attacking;
    private bool grounded;
    private bool doubleJump;
    private bool crouching;
    private bool standing;
    private bool walking;
    private bool running;
    private float dirFacing;
    private bool inControl;
    private bool forward;
    private bool isUp;
    private bool isDown;

    private List<Collider> BodyParts = new List<Collider>();
    public List<Collider> CollidingBodyParts = new List<Collider>();    // Set to public for debugging.

    public float health = 100.0f;
    private float damage;

    Animator m_Animator;
    private string attackBodyPart;

    // handle all player input below //

    void OnMove(InputValue value)
    {
    // handle move events here - use directional but only handle for movement x

        horizontalInput = value.Get<Vector2>();

    movementX = horizontalInput.x;
    movementY = horizontalInput.y;
    }

    void OnRun()
    {
        if (walking & grounded)
        {
            m_Animator.SetBool("IsRunning", true);
            running = true;
            speed = runSpeed;
        }

    }

    void OnJump()
    {
        if (grounded & (standing | running))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            m_Animator.SetTrigger("IsJumping");
            jumpCloud.Play(false);

        }
        if (!grounded & doubleJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            doubleJump = false;
            m_Animator.SetTrigger("IsJumping");
            jumpCloud.Play(false);

        }




    }

    void OnAtk1()
    {



        if (attacking == false)
        {
            attacking_audio.PlayDelayed(.3f);
        }
        // isUp not currently working :(
        if (isUp & !grounded)
        {
            //ideally up air, both feet
            m_Animator.SetTrigger("IsAttack");
            attacking = true; attackTime = .9f;
            attackBodyPart = "shin.L";
            damage = 10;
        }

        if (running)
        {
            //Charge attack left knee
            m_Animator.SetTrigger("IsAttack");
            attacking = true; attackTime = .9f;
            attackBodyPart = "shin.L";
            damage = 10;
        }


        if (!grounded & forward)
        {
            // forward air right hand
            m_Animator.SetTrigger("IsAttack");
            attacking = true; attackTime = .9f;
            attackBodyPart = "hand.R";
            damage = 5;
            
        }
 
        else if (!grounded & !forward)
        {
            m_Animator.SetTrigger("IsAttack");
            attacking = true; attackTime = .9f;
            attackBodyPart = "shin.L";
            damage = 10;
        }
        else if (crouching)
        {
            //crouch attack right foot
            m_Animator.SetTrigger("IsAttack");
            attacking = true; attackTime = .5f;
            attackBodyPart = "shin.R";
            damage = 10;

        }
        else if (grounded & standing & !running)
        {
            //punch right hand
            m_Animator.SetTrigger("IsAttack");
            attacking = true; attackTime = .5f;
            attackBodyPart = "hand.R";
            damage = 5;
        }
        else if (grounded & walking & !running)
        {
            //round house left foot
            m_Animator.SetTrigger("IsAttack");
            attacking = true; attackTime = .5f;
            attackBodyPart = "shin.L";
            damage = 10;
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

        // PlayerController control = other.transform.root.GetComponent<PlayerController>();
        
        

        
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
        normSpeed = speed;
        runSpeed = (speed * 2.5f);

        AudioSource[] audios = GetComponents<AudioSource>();
        walking_audio = audios[0];
        attacking_audio = audios[1];

        rb = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider>();

        jumpCloud = GetComponent<ParticleSystem>();
        //dont use .emission.enabled, throws error
        rHand.GetComponent<ParticleSystem>().enableEmission = false; 
        lHand.GetComponent<ParticleSystem>().enableEmission = false;
        rFoot.GetComponent<ParticleSystem>().enableEmission = false;
        lFoot.GetComponent<ParticleSystem>().enableEmission = false;









        standing = true;
        crouching = false;
        grounded = false;
        inControl = true;
        walking = false;
        running = false;
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
        healthBar.setHealth(health/100.0f);
        if (transform.position.y < -2)
        {
            rb.position = new Vector3(0, 2, 0);
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (grounded & !crouching & !attacking)
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

        
        if (forward = (movementX * dirFacing > 0))
        {
            m_Animator.SetBool("IsForward", true);
        }
        if (forward = (movementX * dirFacing < 0))
        {
            m_Animator.SetBool("IsForward", false);
        }
       

        int tmpSpeed = (int)speed;
        if ((grounded & !walking) | !grounded)
        {
            m_Animator.SetBool("IsRunning", false);
            running = false;
            speed = normSpeed;
        }

        
        // Changes direction if one group when recieving input
        if (horizontalInput.x != 0 & grounded & !attacking)
        {
            dirFacing = horizontalInput.x;
        }
        

        //attempting to use movement up for upair, but is always false
        if (movementY > 0f)
        {
            isUp = true;
            m_Animator.SetBool("IsUp", true);
        }
        else
        {
            isUp = false;
            m_Animator.SetBool("IsUp", false);
        }
        if (movementY < 0f)
        {
            isDown = true;
            m_Animator.SetBool("IsDown", true);

        }
        else
        {
            isDown = false;
            m_Animator.SetBool("IsDown", false);
        }


        // initializes walking on input
        walking = movementX != 0;
        if (!grounded) walking = false;
        m_Animator.SetBool("IsWalking",walking);

        // activate inAir animation when not grounded
        m_Animator.SetBool("IsInAir", !grounded);

      

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
            jumpCloud.enableEmission = false;

        }
        else
        {
            jumpCloud.enableEmission = true;

            attacking = false;
            rHand.enableEmission = false;
            lHand.enableEmission = false;
            rFoot.enableEmission = false;
            lFoot.enableEmission = false;


        }

        //sound for walking
        if (walking)
        {
            if (!walking_audio.isPlaying)
            {
                walking_audio.Play();
            }
        }
        else
        {
            walking_audio.Stop();
        }


        



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

    /* OnTriggerEnter & OnTriggerExit function for 
    when body parts of another player are touching 
    the current player.
    Used to detect if a attack occurs. */
    private void OnTriggerEnter(Collider other)
    {
        PlayerController control = other.transform.root.GetComponent<PlayerController>();
        if (BodyParts.Contains(other) || control == null || other.gameObject == control.gameObject){
            return;
        }
        if (!CollidingBodyParts.Contains(other)){
            CollidingBodyParts.Add(other);
            
            Debug.Log(attackBodyPart);
            if (control.IsAttacking() && other.GetComponent<Collider>().gameObject.name == control.GetAtckBodyPart())
            {
                health -= control.DamageToGive();
                Debug.Log(health);
                control.rHand.enableEmission = true;
                control.rHand.Play();


            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (CollidingBodyParts.Contains(other)){
            CollidingBodyParts.Remove(other);
        }
        
    }

    public bool IsAttacking()
    {
        return attacking;
    }

    public string GetAtckBodyPart()
    {
        return attackBodyPart;
    }

    public float DamageToGive()
    {
        return damage;
    }
}
