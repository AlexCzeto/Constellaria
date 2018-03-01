// from https://unity3d.com/learn/tutorials/topics/2d-game-creation/creating-basic-platformer-game
using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour {

	[HideInInspector] public bool facingRight = false;
	[HideInInspector] public bool jump = false;
	[HideInInspector] public bool running = false;
	[HideInInspector] public int idleCount;
	[HideInInspector]private bool grounded = false;
	[HideInInspector]private bool lined = false;
	[HideInInspector]private bool falling = false;
	[HideInInspector]private float timeSinceFall = 0.0f;
	[HideInInspector]private float timeSinceFootStep = 0.0f;
	[Range(0.5f,2.0f)]
	public float linePitch;
	public float footStepDelay = 0.5f;
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public int	idleTimer = 1000;
	public Transform groundCheck;

	public AudioClip walkingOnCave;
	public AudioClip walkingOnLine;
	public AudioClip jumpSound;
	public AudioClip fallSound;
	public AudioClip deathSound;
	public AudioClip lineHumSound;


	private Animator anim;
	private Rigidbody2D rb2d;
	private AudioSource audio;

	// Use this for initialization
	void Awake () 
	{
		
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource> ();
		idleCount = 0;
	}

	// Update is called once per frame
	void Update () 
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		lined = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Lines"));

		if (Input.GetButtonDown("Jump") && (grounded || lined))
		{
			jump = true;
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{ Debug.Log (col.gameObject.layer);
		if (col.gameObject.layer == 10) //layer 10 is enemies
		{
			if (this.gameObject.GetComponent<GrapplingHook> ().currentNode) 
			{
				this.gameObject.transform.position = this.gameObject.GetComponent<GrapplingHook> ().currentNode.transform.position;
				anim.SetTrigger ("Die");
				audio.PlayOneShot(deathSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
			} 
			else 
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}

		//layer 11 is nodes
		//layer 12 is ground
		//layer 13 is lines
		if (col.gameObject.layer == 13) {
			AudioSource lineAudio = col.gameObject.GetComponent<AudioSource> ();
			lineAudio.pitch = linePitch;
			lineAudio.PlayOneShot(lineHumSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
			//lined = true;
		}
	}

	void FixedUpdate()
	{
		//FixedUpdate plays every 0.2 'time'
		//print (Time.time);
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		anim.SetFloat("Speed", Mathf.Abs(h));

		if (h * rb2d.velocity.x < maxSpeed) {
			rb2d.AddForce (Vector2.right * h * moveForce);
		}

		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed) {
			rb2d.velocity = new Vector2 (Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		}

		if (Mathf.Abs (h) > 0) {//if walking
			if(Time.time > timeSinceFootStep){ //this 
				if(grounded){//todo: check the stage we're in
				//SOUND: Walking on Cave Surface
					audio.PlayOneShot(walkingOnCave ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
					timeSinceFootStep = Time.time + footStepDelay;
				}
				if(lined){
				//SOUND: Walking on light bridge
					audio.PlayOneShot(walkingOnLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
					timeSinceFootStep = Time.time + footStepDelay;
				}
			}
		}

		if (h > 0) {
			TurnRight ();
		} else if (h < 0) {
			TurnLeft ();
		}

		if (jump)
		{
			//SOUND: Jumping
			audio.PlayOneShot(jumpSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
			anim.SetTrigger ("Jump");
			rb2d.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}

		if (rb2d.velocity.y < -1 && !grounded && !lined && (Time.time > timeSinceFall)){
			print("Playing Fall Animation");  
			//SOUND: Falling
			audio.PlayOneShot(fallSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
			//audio.Stop;
			anim.SetTrigger ("Fall");
			timeSinceFall = Time.time + 0.5f; //this 
		}

		if (anim.GetCurrentAnimatorStateInfo (1).IsName ("Idle")) {
			idleCount = idleCount + 1;
			if (idleCount > idleTimer) {
				idleCount = 0;
				if (Random.value > 0.5) {
					anim.SetTrigger ("Wonder");
				} 
				else 
				{
					anim.SetTrigger ("Dance");
				}
			}
		}


	}


	void Flip()
	{
		facingRight = !facingRight;
		anim.SetBool("FacingRight", facingRight);
		anim.SetTrigger("Turn");
		//instantly filps the png, replace with animation.
			//Vector3 theScale = transform.localScale;
			//theScale.x *= -1; 
			//transform.localScale = theScale;

	}

	void TurnRight()
	{
		facingRight = true;
		anim.SetBool("FacingRight", facingRight);
		anim.SetTrigger("Turn");
	}
	void TurnLeft()
	{
		facingRight = false;
		anim.SetBool("FacingRight", facingRight);
		anim.SetTrigger("Turn");
	}



}