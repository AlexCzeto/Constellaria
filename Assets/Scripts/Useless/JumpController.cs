// from https://unity3d.com/learn/tutorials/topics/2d-game-creation/creating-basic-platformer-game
using UnityEngine;
using System.Collections;

public class JumpController : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D rigidbody2D;
    public Transform groundCheck;
    
	private float moveForce;
	private float maxSpeed;
    
    private float jumpForce;
    private float jumpForceHold;
    private float maxJumpTime;
    private float internalMaxJumpTime;

    [HideInInspector] public bool facingRight = true;
    private bool isGrounded = false;
	private bool lined = false;

    // Use this for initialization
    void Awake () 
	{
		anim = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

    private void Start()
    {
        moveForce = 365f;
        maxSpeed = 5f;
        jumpForce = 500f;
        jumpForceHold = 50f;
        maxJumpTime = 10f;
    }

    // Update is called once per frame
    void Update () 
	{
		isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		lined = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Lines"));


        if (Input.GetButtonDown("Jump") && (isGrounded || lined))
        {
            rigidbody2D.AddForce(new Vector3(0, jumpForce, 0));
            internalMaxJumpTime = maxJumpTime;
        }
        if (Input.GetButton("Jump") && rigidbody2D.velocity.y > 1 && internalMaxJumpTime > 0)
        {
            rigidbody2D.AddForce(new Vector3(0, jumpForceHold, 0));
            internalMaxJumpTime -= 1;
        }

	}

	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

        anim.SetFloat("Speed", Mathf.Abs(h));

		if (h * rigidbody2D.velocity.x < maxSpeed)
            rigidbody2D.AddForce(Vector2.right * h * moveForce);

		if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
            rigidbody2D.velocity = new Vector2(Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
	}


	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1; //instantly filps the png, replace with animation.
		transform.localScale = theScale;
	}
}