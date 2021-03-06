﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour {

    // Helps set gravity and jumpVelocity - note that time is in seconds
    private float moveSpeed = 5;
    private float minJumpHeight = 1;
    private float maxJumpHeight = 5.5f;
    private float timeToJumpApex = 0.65f;
    private float accelerationTimeAirborne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;


    // Equation assuming we're at the top of our jump should help explain gravity - jumpHeight = (gravity * timeToJumpApex**2)/2 so isolate gravity
    // Equation for velocity is jumpVelocity = gravity * timeToJumpApex
    private float gravity;
    private float minJumpVelocity;
    private float maxJumpVelocity;

    private Vector2 directionalInput;
    public Vector3 velocity;
    private float velocityXSmoothing;

    private Controller2D controller;
    private Animator animator;
    private AudioController audioController;

    private GrapplingHook grapple;
	private Rigidbody2D rb2d;
	private CapsuleCollider2D capColl2d;
    private AudioController audioC;

    private bool facingRight;

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;

            // Animation for jumping
            animator.SetTrigger("Jump");

            // Sound for jumping
            audioController.playJump();

            // Let's the touching ground and line sounds play again
            controller.touchGround = 0;
            controller.touchLine = 0;
        }
    }
    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }
	public void OnShiftInputDown(){
		if (grapple.connected) {
			rb2d.gravityScale = 1f;
			rb2d.velocity = velocity;
			capColl2d.isTrigger = false;
			rb2d.mass = 10f;
		}
	}
	public void OnShiftInputUp(){
		if (grapple.connected) {
			rb2d.gravityScale = 0f;
			velocity = rb2d.velocity;
			rb2d.velocity = new Vector2 (0f, 0f);
			rb2d.mass = 1000f;
			capColl2d.isTrigger = true;
		}
	}

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        audioController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>();

        grapple = GetComponent<GrapplingHook>();
		rb2d = GetComponent<Rigidbody2D> ();
		capColl2d = GetComponent<CapsuleCollider2D> ();
        audioC = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>();

        // Note that gravity has to be negative, hence the -1
        gravity = (-1)*(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    // Update is called once per frame
	void FixedUpdate()
	{
		if (grapple.isSwinging)
		{
			CalculateSwingVelocity();
		}
	}


    void Update()
    {
        if (grapple.isSwinging)
        {
            animator.SetBool("isSwinging", true);
            animator.SetTrigger("Swing");
        }
        if (!grapple.isSwinging)
        {
            animator.SetBool("isSwinging", false);
            CalculateVelocity();
            controller.Move(velocity * Time.deltaTime, directionalInput);

        }		

        // This makes our falling make more sense
        // Due to how our gravity is implemented, this makes sure we don't just fall really quickly when we step off a platform
		if (controller.collisions.above || controller.collisions.below  || controller.playerDeath)
		{
			velocity.y = 0;
		}

        // Animations for player character
		if (velocity.x < 1)
        {
            TurnLeft();
            animator.SetFloat("Speed", (velocity.x));
        }
		else if (velocity.x > 1)
        {
            TurnRight();
            animator.SetFloat("Speed", (velocity.x));
        }
    }

	void CalculateSwingVelocity()
	{
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeAirborne);
		this.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * velocity.x*10f);
	}

    void CalculateVelocity()
    {
        // Smoothing movement in x
        float targetVelocityX = directionalInput.x * moveSpeed;

        if (controller.collisions.below)
        {

            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
        }
        else
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeAirborne);
        }

        // Applying gravity to the player's velocity and moves accordingly
        velocity.y += gravity * Time.deltaTime;
    }

    void TurnRight()
    {
        facingRight = true;
        animator.SetBool("FacingRight", facingRight);
        animator.SetTrigger("Turn");
    }
    void TurnLeft()
    {
        facingRight = false;
        animator.SetBool("FacingRight", facingRight);
        animator.SetTrigger("Turn");
    }
}
