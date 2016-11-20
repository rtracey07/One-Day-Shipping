using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

	[Tooltip("Character Move Speed")]
	public float speed = 6.0f;

	[Tooltip("Modify backup Speed")]
	public float backupSpeedFactor = 2.0f;

	[Tooltip("Character Rotation Speed")]
	public float rotationSpeed = 60.0f;

	[Tooltip("Character Jump Speed")]
	public float jumpSpeed = 8.0f;

	[Tooltip("Character Speed Down Slope")]
	public float slideSpeed = 2.0f;

	[Tooltip("Downward Force")]
	public float gravity = 20.0f;

	[Tooltip("Minimum world y-position before respawning.")]
	public float yBoundary;

	//Cached Variables to reduce re-declaration.
	private CharacterController controller;		
	private Animator animator;
	public Vector3 moveDirection = Vector3.zero;
	private float rotation = 0.0f;
	public float vertical = 0.0f;
	private float horizontal = 0.0f;
	private float time = 0.0f;
	private bool sliding = false;

	public float currSpeed = 0.0f;
	public float acceleration = 5.0f;

	public Transform legL, legR, armL, armR;

	private Collider[] colliders;
	private bool jumpPrimed = false;

	void Start()
	{
		//Cache Controllers for use.
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator> ();
		colliders = GetComponentsInChildren<Collider> ();

	}

	void Update() 
	{
		//Update Time.
		time += Time.deltaTime;

		//Get Values of User Input.
		vertical = Input.GetAxis ("Vertical");
		horizontal = Input.GetAxis ("Horizontal");

		//Move Character.
		Rotate ();
		Move ();

		float speedFraction = Mathf.Abs(currSpeed/speed);
		//Update Animation variable.
		animator.SetFloat ("Speed", speedFraction);
		animator.SetFloat ("Direction", vertical);

		legL.localScale = new Vector3(legL.localScale.x, Mathf.Lerp(1.0f, 1.4f, speedFraction), legL.localScale.z);
		legR.localScale = new Vector3(legR.localScale.x, Mathf.Lerp(1.0f, 1.4f, speedFraction), legR.localScale.z);
		armL.localScale = new Vector3(armL.localScale.x, Mathf.Lerp(1.2f, 1.4f, speedFraction), armL.localScale.z);
		armR.localScale = new Vector3(armR.localScale.x, Mathf.Lerp(1.2f, 1.4f, speedFraction), armR.localScale.z);
}

	//Rotate Player Around y-axis.
	void Rotate()
	{
		//rotate player from horizontal input.
		rotation = horizontal * rotationSpeed * Time.deltaTime;

		if(currSpeed != 0 )
			transform.Rotate(0,rotation,0);

		//Update Rotation Animation variable.
		animator.SetFloat ("TurnSpeed", rotation);
	}

	//Move Player.
	void Move()
	{
		//Store current y-value.
		float fall = moveDirection.y;

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
				moveDirection *= currSpeed * Time.deltaTime;
			} 
			//Character is moving backwards.
			else 
			{
				moveDirection *= currSpeed/backupSpeedFactor * Time.deltaTime;
			}


		}

		//Jumping.
		if (!sliding && Input.GetButton ("Jump") && controller.isGrounded) {
			animator.SetBool ("Jump", true);
			jumpPrimed = true;
		} 
		//Falling.
		else if (!controller.isGrounded || sliding) {
			moveDirection.y = fall - gravity * Time.deltaTime;
		} else {
			animator.SetBool ("Jump", jumpPrimed);
		}

		if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Stop") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Stop Backwards"))
			controller.Move(moveDirection * Time.deltaTime);
	}

	//Use Collision detection to detect if sliding.
	//This stops the character from overcoming large slopes and escaping the level boundary.
	void OnControllerColliderHit(ControllerColliderHit other)
	{
		//Check for "Ground" tag and if the character exceeds the maximum angle.
		if (other.gameObject.tag == "Ground" && Vector3.Angle (other.normal, Vector3.up) > controller.slopeLimit) 
		{
			//Set the current direction to be relative to the collision normal.
			moveDirection = other.normal;

			//Negate the y-value so the player falls.
			moveDirection.y = -other.normal.y;
			moveDirection *= slideSpeed;
			sliding = true;
		} 
		//Not on slope.
		else 
		{
			sliding = false;
		}
	}

	public void PlayerStop()
	{
		currSpeed /= 2;
	}

	public void PlayerJump()
	{
		moveDirection.y = jumpSpeed;
		controller.Move (new Vector3 (0, jumpSpeed* Time.deltaTime, 0));
		jumpPrimed = false;
	}
}
