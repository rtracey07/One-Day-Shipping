﻿using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {


	class gizmoData{
		public Vector3 collisionPoint;
		public Vector3 collisionNormal;
	};

	[Tooltip("Character Move Speed")]
	public float speed = 6.0f;												// walk speed

	[Tooltip("Modify backup Speed")]
	public float backupSpeedFactor = 2.0f;									// backwards speed

	[Tooltip("Character Rotation Speed")]
	public float rotationSpeed = 60.0f;										// turning around speed

	[Tooltip("Character Jump Speed")]
	public float jumpSpeed = 8.0f;											// jump speed

	[Tooltip("Character Speed Down Slope")]
	public float slideSpeed = 2.0f;											// sliding on slopes

	[Tooltip("Downward Force")]
	public float gravity = 20.0f;

	[Tooltip("Minimum world y-position before respawning.")]
	public float yBoundary;

	//Cached Variables to reduce re-declaration.
	//private CharacterController controller;		
	private Animator animator;
	public Vector3 moveDirection = Vector3.zero;
	private float rotation = 0.0f;
	public float vertical = 0.0f;
	private float horizontal = 0.0f;
	private float time = 0.0f;
	public bool sliding = false;

	public float currSpeed = 0.0f;
	public float acceleration = 5.0f;

	public Transform legL, legR, armL, armR;
	public GameObject package;
	private gizmoData[] gizmos;

	private bool jumpPrimed = false;
	private Rigidbody m_Rigidbody;

	public AudioClip jumpSound;

	Transform start;
	public AudioClip respawnSound;


	void Awake()
	{
		//Cache Controllers for use.
		//controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator> ();
		m_Rigidbody = GetComponent<Rigidbody> ();
		start = GameObject.FindGameObjectWithTag ("SpawnPoint").transform;
	}

	void FixedUpdate() 
	{
		if (transform.position.y < yBoundary)
			transform.position = start.position;
		// on pause
		if (GameClockManager.Instance.freeze) {
			animator.speed = 0;
		} else {
			animator.speed = 1;
			//Update Time.
			time += GameClockManager.Instance.fixedTime;

			//Get Values of User Input.
			vertical = Input.GetAxis ("Vertical");
			horizontal = Input.GetAxis ("Horizontal");

			//Move Character.
			Rotate ();
			Move ();

			float speedFraction = Mathf.Abs (currSpeed / speed);
			//Update Animation variable.
			animator.SetFloat ("Speed", speedFraction);
			animator.SetFloat ("Direction", vertical);

			//lankiness effects
			legL.localScale = new Vector3 (legL.localScale.x, Mathf.Lerp (1.0f, 1.4f, speedFraction), legL.localScale.z);
			legR.localScale = new Vector3 (legR.localScale.x, Mathf.Lerp (1.0f, 1.4f, speedFraction), legR.localScale.z);
			armL.localScale = new Vector3 (armL.localScale.x, Mathf.Lerp (1.2f, 1.4f, speedFraction), armL.localScale.z);
			armR.localScale = new Vector3 (armR.localScale.x, Mathf.Lerp (1.2f, 1.4f, speedFraction), armR.localScale.z);

			GetPackage (GameManager.Instance.hasPackage);
		}
}

	//Rotate Player Around y-axis.
	void Rotate()
	{
		//rotate player from horizontal input.
		rotation = horizontal * rotationSpeed * Time.fixedDeltaTime;

		if(currSpeed != 0 )
			transform.Rotate(0,rotation,0);

		//Update Rotation Animation variable.
		animator.SetFloat ("TurnSpeed", rotation);
	}

	//Move Player.
	void Move()
	{

		currSpeed = Mathf.Clamp ((vertical != 0) ? (currSpeed + acceleration) : (currSpeed - 3*acceleration), 0, speed);

		//Only move player if they aren't on a sloped surface.
		if (!sliding) 
		{
			//Create new direction with transform along the character's local z-position.
			moveDirection = new Vector3 (0, 0, vertical);
			moveDirection = transform.TransformDirection (moveDirection);

			//Character is moving forward.
			if (vertical >= 0) 
			{
				moveDirection *= currSpeed;// * Time.fixedDeltaTime;
			} 
			//Character is moving backwards.
			else 
			{
				moveDirection *= currSpeed/backupSpeedFactor;// * Time.fixedDeltaTime;
			}
		}

		//Jumping.
		if (!sliding && Input.GetButton ("Jump") && m_Rigidbody.velocity.y == 0 && !sliding) {
			animator.SetBool ("Jump", true);
			jumpPrimed = true;
		} 
		//Falling.
		else if (m_Rigidbody.velocity.y != 0 || sliding) {
//			moveDirection.y = fall - gravity * Time.fixedDeltaTime;
		} else {
			animator.SetBool ("Jump", jumpPrimed);
		}

		if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Stop") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Stop Backwards") && !sliding)
//			controller.Move(moveDirection * GameClockManager.Instance.time);
			m_Rigidbody.AddForce(moveDirection);

	}


	//Use Collision detection to detect if sliding.
	//This stops the character from overcoming large slopes and escaping the level boundary.
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Ground" || (other.gameObject.tag == "Building" && animator.GetBool("Jump"))) {
			for (int i = 0; i < other.contacts.Length; i++) {
				if (other.contacts [i].normal.y < 0.7) {
					if (!sliding) {
						sliding = true;
						currSpeed = -3 * acceleration;
						moveDirection = Vector3.zero;
						m_Rigidbody.AddForce (new Vector3 (other.contacts [i].normal.x, -other.contacts [i].normal.y, other.contacts [i].normal.z) * slideSpeed);
					}
				} else {
					sliding = false;
				}
			}
		}

		//hit by a car, move up and away
		if (other.gameObject.tag.Equals("Vehicle")) {
			vertical = 0.05f;
			Vector3 forcedir = transform.position - other.gameObject.transform.position;
			forcedir = new Vector3 (forcedir.x, 0.1f, forcedir.z);
			moveDirection = Vector3.zero;
			m_Rigidbody.AddForce (forcedir * 300.0f);
			moveDirection = new Vector3 (forcedir.x, -0.1f, forcedir.z);



		}

		// debugging gizmos
		gizmos = new gizmoData[other.contacts.Length];

		for (int i = 0; i < gizmos.Length; i++) {
			gizmos [i] = new gizmoData ();
			gizmos [i].collisionPoint = other.contacts [i].point;
			gizmos [i].collisionNormal = other.contacts [i].normal;
		}
	}

	//use trigger detection to detect if player walks into the water
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Water"){
			AudioSource.PlayClipAtPoint (respawnSound, GameManager.Instance.mainCamera.transform.position);
			transform.position = start.position;
			if (package.activeSelf) {
				GetPackage (false);
				GameManager.Instance.hasPackage = false;
				GameManager.Instance.destroyed = true;
			}
		}
	}

	/// <summary>
	/// stop the player
	/// </summary>
	public void PlayerStop()
	{
		currSpeed /= 2;
	}

	/// <summary>
	/// jump and play sound when jump key pressed
	/// </summary>
	public void PlayerJump()
	{
		if (!sliding) {
			moveDirection.y = jumpSpeed;
			m_Rigidbody.AddForce (new Vector3 (0, jumpSpeed, 0));
			AudioManager.Instance.PlaySoundEffect(jumpSound);
		}
		jumpPrimed = false;
	}

	/// <summary>
	/// debugging gizmos
	/// </summary>
	void OnDrawGizmos()
	{
		if (gizmos != null) {
			for (int i = 0; i < gizmos.Length; i++) {
				if (gizmos [i].collisionNormal.y < 0.7) {
					Gizmos.color = Color.green;
					Gizmos.DrawLine (gizmos [i].collisionPoint,  
						gizmos [i].collisionPoint 
						+ new Vector3(gizmos [i].collisionNormal.x, -gizmos [i].collisionNormal.y, gizmos [i].collisionNormal.z));
					Gizmos.color = Color.red;
				}
				else
					Gizmos.color = Color.white;
				
				Gizmos.DrawLine (gizmos [i].collisionPoint,  gizmos [i].collisionPoint + gizmos [i].collisionNormal);
			}
		}
	}

	/// <summary>
	/// for getting the package for other scripts
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void GetPackage(bool state)
	{
		animator.SetBool ("Package", state);
		package.SetActive (state);
	}
}
