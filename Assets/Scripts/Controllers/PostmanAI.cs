using UnityEngine;
using System.Collections;

public class PostmanAI : MonoBehaviour {

	private State state;

	//type of movement
	private enum pathEnum{loop, reverse};
	[SerializeField] private pathEnum pathType;
	//set up a series of points to walk along when not chasing player
	[SerializeField] Vector3[] route;
	private int next;
	private bool fwd;
	private float turnDelay = 1.0f;

	private bool wall = false;
	//keep track of ground
	private RaycastHit ground;
	[SerializeField] private float groundOffset = 0.5f;
	[SerializeField] private float speed = 5.0f;				// flight speed
	[SerializeField] private float damageStrength = 5.0f;		// damage to player
	private Vector3 center;										// center of the chase radius
	[SerializeField] private float radius = 3.0f;				// chasing radius
	[SerializeField] private float projectileRadius = 5.0f;		// radius for shooting projectiles
	[SerializeField] private float attackProximity = 0.3f;		// when to do attack animation


	// projectile variables
	[SerializeField] private bool throwsProjectiles = true;		// turn this on to throw packages
	[SerializeField] private float projectileSpeed = 1000.0f;	// speed of projectiles
	[SerializeField] private float projectileInterval = 1.0f;	// wait between projectiles
	private float projectileTime = 0.0f;
	[SerializeField] private Rigidbody projectile;				// package
	[SerializeField] private GameObject player;					// player to chase
	private GameObject package;

	private Animator animator;
	private float attackTime = 1.0f;
	private float attackDelay = 3.0f;

	private float change = 0.0f;
	private float angle = 0.0f;
	private float current;
	private bool dirChange = false;

	//postman states
	public enum State
	{
		Attacking,
		Turning,
		Walking,
		Spawn,
		PlayerChase,
		PlayerShoot
	}

	// property for other scripts
	public State PlayerState {
		get{ return state; }

	}

	// property for global scripts
	public bool ThrowsProjectiles {
		set{ throwsProjectiles = value; }
		get{ return throwsProjectiles; }
	}

	//catches collisions
	void OnTriggerEnter(Collider target) {
		
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent <Animator>();
		//state = State.Standing;
		state = State.Spawn;
		if (route.Length == 1) { 		// he stays in one place for some reason
			next = 0;
		} else if (route.Length < 1) {	// forgot to set his route
			route[0] = transform.position;
			next = 0;
		} else {
			next = 1;
		}
		transform.position = route [0];
		center = transform.position;

		fwd = true;

		package = GameObject.FindGameObjectWithTag ("Package");
	}

	/// <summary>
	/// Checks for player within radius
	/// </summary>
	void CheckForPlayer() {
		if (Mathf.Sqrt( (player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
			+ (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) ) <= attackProximity
			&& Mathf.Abs(player.transform.position.y - transform.position.y) <= attackProximity) {
			state = State.Attacking;
		}
	}

	/// <summary>
	/// Walking state
	/// moves from spot to spot on route
	/// </summary>
	void WalkingState() {
		//create the rotation we need to be in to look at the target
		Quaternion lookRotation = Quaternion.LookRotation((route[next] - transform.position).normalized);
		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * speed * 3.0f);

		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (route[next].x, ground.point.y + groundOffset, route[next].z), Time.deltaTime * speed);
		//when reached, go to next
		if ((route [next].x - transform.position.x) * (route [next].x - transform.position.x)
		    + (route [next].z - transform.position.z) * (route [next].z - transform.position.z) <= 0.1f) {
			turnDelay = 1.0f;
			if (next == route.Length - 1 && pathType == pathEnum.loop) {
				next = 0;
			} else if (pathType == pathEnum.loop) {
				next ++;
			} else if (next == route.Length - 1 && pathType == pathEnum.reverse) {
				fwd = false;
			} else if (next == 0 && pathType == pathEnum.reverse) {
				fwd = true;
			}

			if (pathType == pathEnum.reverse && fwd) {
				next++;
			} else if (pathType == pathEnum.reverse && !fwd) {
				next--;
			}

		}

		if ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
			+ (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= projectileRadius * projectileRadius) {
			center = transform.position;
			state = State.PlayerShoot;
		}

		if ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
			+ (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= radius * radius) {
			center = transform.position;
			state = State.PlayerChase;
		}
	}

	/// <summary>
	/// shoot a projectile that is destroyed after time
	/// </summary>
	void shoot(){

		if (Time.time >= projectileTime) {
			package = GameObject.FindGameObjectWithTag ("Package");
			Rigidbody projectile_shoot = Instantiate (projectile, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), transform.rotation) as Rigidbody;

			projectile_shoot.GetComponent<PostmanProjectile> ().DamageStrength = damageStrength;
			if (package != null) {
				projectile_shoot.GetComponent<PostmanProjectile> ().Pack = package;
			}

			Debug.Log(projectile_shoot.gameObject.name);
			//send forward
			projectile_shoot.AddForce (transform.forward * projectileSpeed);
			Destroy (projectile_shoot.gameObject, 2.0f);


			projectileTime = Time.time + projectileInterval;
		}
	}

	/// <summary>
	/// Turning state
	/// not used at the moment, keeping it for now
	/// </summary>
	void TurningState() {
		if (!dirChange) {
			transform.Translate (-4.0f * transform.forward * Time.deltaTime * speed, Space.World);
			current = transform.rotation.eulerAngles.y;
			if (current > 180.0f) {
				change = Random.Range (-180.0f, -120.0f);
			} else {
				change = Random.Range (120.0f, 180.0f);
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

	/// <summary>
	/// Attacking state
	/// when the player is close, stop and do an attack animation
	/// </summary>
	void AttackingState(){
		//Debug.Log ("attack state");

		transform.LookAt (player.transform);

		animator.SetBool ("Walk", false);
		animator.SetTrigger("Attack");

		if ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
		    + (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= attackProximity) {
			state = State.Attacking;

			if (attackTime >= attackDelay) {
				//Debug.Log ("tossing");
				package = GameObject.FindGameObjectWithTag ("Package");
				if (package != null) {
					package.GetComponent<Package> ().DamagePackage (damageStrength / 2.0f);
				}
				attackTime = 0.0f;
			} else if (attackTime < attackDelay) {
				attackTime += 5.0f * Time.deltaTime;
			}

		} else {
			state = State.Walking;
		}
	}

	/// <summary>
	/// Spawn state
	/// just transitions to walking for now
	/// </summary>
	void SpawnState(){
		state = State.Walking;
		CheckForPlayer ();
	}

	/// <summary>
	/// Player Chase State
	/// If the player is nearby, chase until he gets away, then go back to route
	/// </summary>
	void PlayerChaseState(){
		//Debug.Log ("chasing");
		float dir_y = -0.1014264f; 
		Vector3 dir = (new Vector3 (player.transform.position.x, dir_y, player.transform.position.z) - transform.position).normalized;
		Quaternion rot = Quaternion.LookRotation (dir);
		transform.rotation = Quaternion.Slerp (transform.rotation, rot, Time.deltaTime * speed);
		transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);

		if ((transform.position.x - center.x) * (transform.position.x - center.x)
			+ (transform.position.z - center.z) * (transform.position.z - center.z) >= radius * radius) {
			state = State.Walking;
		} else {
			CheckForPlayer ();
		}
	}

	/// <summary>
	/// Shooting
	/// Throws packages at player
	/// </summary>
	void PlayerShootState(){

		if (throwsProjectiles) {
			animator.SetBool ("Walk", false);
			animator.SetTrigger("Attack");
			Vector3 dir = (new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z) - transform.position).normalized;
			Quaternion rot = Quaternion.LookRotation (dir);
			transform.rotation = Quaternion.Slerp (transform.rotation, rot, Time.deltaTime * speed);
			shoot ();
			//transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);
			if (radius <= projectileRadius && ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
				+ (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= radius * radius)) {
				center = transform.position;
				state = State.PlayerChase;
			} else if ((player.transform.position.x - center.x) * (player.transform.position.x - center.x)
				+ (player.transform.position.z - center.z) * (player.transform.position.z - center.z) >= projectileRadius * projectileRadius) {
				state = State.Walking;
			} else {
				CheckForPlayer ();
			}
		} else {

			if (radius <= projectileRadius && ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
			    + (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= radius * radius)) {
				center = transform.position;
				state = State.PlayerChase;
			} else {
				state = State.Walking;
			}


		}

	}

	/// <summary>
	/// FixedUpdate
	/// using raycasts to keep track of ground and walls
	/// </summary>
	void FixedUpdate() {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, -Vector3.up, out hit)) {
			ground = hit;
		}
		//don't walk into walls, do walk up to player
		RaycastHit front;
		if (Physics.Raycast (transform.position, transform.forward, out front)) {
			//Debug.Log ("hit something " + front.distance);
			if (hit.point.y > 0.3f && front.distance < 0.3f && !hit.collider.name.Equals("player") && !hit.collider.name.Equals("dog") && state != State.Turning) {
				//try to avoid walking through walls by going to next step
				if (state == State.Walking) {
					if (next == route.Length - 1 && pathType == pathEnum.loop) {
						next = 0;
					} else if (pathType == pathEnum.loop) {
						next++;
					} else if (next == route.Length - 1 && pathType == pathEnum.reverse) {
						fwd = false;
					} else if (next == 0 && pathType == pathEnum.reverse) {
						fwd = true;
					}

					if (pathType == pathEnum.reverse && fwd) {
						next++;
					} else if (pathType == pathEnum.reverse && !fwd) {
						next--;
					}
				} else {
					state = State.Walking;
				}
			}
		} 

	}
	
	// Update is called once per frame
	void Update () {
	
		//state machine
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
		case State.PlayerShoot:
			PlayerShootState ();
			break;
		}
		CheckForPlayer ();


	}



}
