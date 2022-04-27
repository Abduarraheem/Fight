using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float speed = 1;
    public float jumpSpeed = 10.0f;

    private Vector3 direction;
	private Rigidbody rb;
	private float movementX;
	private float movementY;
    private bool canJump;
    Animator m_Animator;

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

// player input ends //

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
        canJump = false;
        m_Animator = GetComponent<Animator>();
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
        
        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");
        
        // bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        // bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        // bool isWalking = hasHorizontalInput || hasVerticalInput;
        bool walk = movementX != 0;
        if (!canJump) walk = false;
        m_Animator.SetBool("IsWalking",walk);
        Debug.Log(walk);
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
}
