using UnityEngine;
using System.Collections;

public class PostmanAI : MonoBehaviour {

	private State state;
	private bool wall = false;
	//keep track of ground
	private RaycastHit ground;
	[SerializeField] private float groundOffset = 0.5f;
	[SerializeField] private float speed = 5.0f;				// flight speed
	[SerializeField] private float damageStrength = 15.0f;			// damage to player
	// the space in which it can travel
	[SerializeField] private Vector3 center = Vector3.zero;		// center of the radius
	[SerializeField] private float radius = 3.0f;
	[SerializeField] private float attackProximity = 0.3f;		// when to do attack animation


	[SerializeField] private GameObject player;
	//animators
	private Animator animator;

	private float change = 0.0f;
	private float angle = 0.0f;
	private float current;
	private bool dirChange = false;

	//player states
	public enum State
	{
		Attacking,
		Turning,
		Walking,
		Spawn,
		PlayerChase
	}

	// property for other scripts
	public State PlayerState {
		get{ return state; }
	}

	//catches collisions
	void OnTriggerEnter(Collider target) {
		
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent <Animator>();
		//state = State.Standing;
		state = State.Spawn;
	}

	void CheckForPlayer() {
		if ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
		    + (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= attackProximity) {
			state = State.Attacking;
		}
	}

	void WalkingState() {
		transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);
		//Debug.Log ("walk state");
		if ((transform.position.x - center.x) * (transform.position.x - center.x)
		    + (transform.position.z - center.z) * (transform.position.z - center.z) >= radius * radius && state != State.Turning) {
			state = State.Turning;
		} else {
			CheckForPlayer ();
		}
	}

	void TurningState() {

		if (!dirChange) {
			transform.Translate (-4.0f * transform.forward * Time.deltaTime * speed, Space.World);
			current = transform.rotation.eulerAngles.y;
			if (current > 180.0f) {
				change = Random.Range (-180.0f, -160.0f);
			} else {
				change = Random.Range (160.0f, 180.0f);
			}
			//Debug.Log ("dir true");
			dirChange = true;
		}

		// the rotation is slerped based on turning direction
		if (current > 180.0f && transform.rotation.eulerAngles.y > current + change) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + change, transform.rotation.eulerAngles.z), Time.deltaTime * speed);
		} else if (current < 180.0f && transform.rotation.eulerAngles.y < current + change) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + change, transform.rotation.eulerAngles.z), Time.deltaTime * speed);
		} else {
			dirChange = false;
			//Debug.Log ("dir false");
			transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);
			state = State.Walking;
		}
		//Debug.Log ("turn state");
		CheckForPlayer ();
	}

	void AttackingState(){
		//Debug.Log ("attack state");

		transform.LookAt (player.transform);

		animator.SetBool ("Walk", false);
		animator.SetTrigger("Attack");

		if ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
		    + (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= attackProximity) {
			state = State.Attacking;
		} else {
			state = State.Walking;
		}
	}

	void SpawnState(){

		CheckForPlayer ();
	}

	void PlayerChaseState(){
		Vector3 dir = (new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z) - transform.position).normalized;
		Quaternion rot = Quaternion.LookRotation (dir);
		transform.rotation = Quaternion.Slerp (transform.rotation, rot, Time.deltaTime * speed);
		transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);

		if ((transform.position.x - center.x) * (transform.position.x - center.x)
			+ (transform.position.z - center.z) * (transform.position.z - center.z) >= radius * radius) {
			state = State.Turning;
		} else {
			CheckForPlayer ();
		}
	}

	void FixedUpdate() {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, -Vector3.up, out hit)) {
			ground = hit;
		}
		//don't walk into walls, do walk up to player
		RaycastHit front;
		if (Physics.Raycast (transform.position, transform.forward, out front)) {
			//Debug.Log ("hit something " + front.distance);
			if (front.distance < 0.3f && !hit.collider.name.Equals("player") && !hit.collider.name.Equals("dog") && state != State.Turning) {
				state = State.Turning;
			}
		} 

	}
	
	// Update is called once per frame
	void Update () {
	
		switch (state) {
		case State.Attacking:
			AttackingState ();
			break;
		case State.Turning:
			TurningState ();
			break;
		case State.Walking:
			WalkingState ();
			animator.SetBool ("Walk", true);
			break;
		case State.Spawn:
			SpawnState ();
			break;
		case State.PlayerChase:
			PlayerChaseState ();
			animator.SetBool ("Walk", true);
			break;
		}

		if ((player.transform.position.x - center.x) * (player.transform.position.x - center.x)
		    + (player.transform.position.z - center.z) * (player.transform.position.z - center.z) <= radius * radius) {
			state = State.PlayerChase;
		}

//		if ((transform.position.x - center.x) * (transform.position.x - center.x)
//			+ (transform.position.z - center.z) * (transform.position.z - center.z) >= radius * radius && state != State.Turning) {
//			state = State.Turning;
//		}

		CheckForPlayer ();

		//stay on the ground
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, ground.point.y + groundOffset, transform.position.z), Time.deltaTime * speed);
	}
}
