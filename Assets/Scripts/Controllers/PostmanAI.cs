using UnityEngine;
using System.Collections;

public class PostmanAI : MonoBehaviour {

	private State state;
	public AudioClip physicalAttackSound;
	public AudioClip rangedAttackSound;

	//type of movement
	private enum pathEnum{loop, reverse};
	[SerializeField] private pathEnum pathType;
	//set up a series of points to walk along when not chasing player
	//[SerializeField] Vector3[] route;
	private int next;
	private bool fwd;
	private float turnDelay = 1.0f;
	private float speedMod = 0.5f;

	private bool wall = false;
	private Pathway path;									// the path
	public float rotationOffset = 0.05f;			// how far ahead to look to orient on path
	private float currentLook =0.25f;										// where the car is looking
	private float percentsPerSecond = 0.1f; 								// %1 of the path moved per second
	private bool swapPositions = false;
	private PostmanManager m_Manager;
	private Vector3 cameraSpacePos;

	//keep track of ground
	private RaycastHit ground;
	[SerializeField] private float groundOffset = 0.2f;
	[SerializeField] private float speed = 5.0f;				// flight speed
	[SerializeField] private float damageStrength = 10.0f;		// damage to package
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
	private GameObject player;					// player to chase
	private GameObject package;

	public AudioClip packageDamage1, packageDamage2, packageDamage3; 

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

	public float CurrentPathPercent {
		get;
		set;
	}

	// Use this for initialization
	void Start () {
		center = transform.position;
		player = GameObject.FindGameObjectWithTag ("Player");
		currentLook = CurrentPathPercent + rotationOffset;
		percentsPerSecond = 0.02f;//speed * 0.0004f;

		m_Manager = GetComponentInParent<PostmanManager> ();


		animator = GetComponent <Animator>();
		//state = State.Standing;
		state = State.Spawn;


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


		if (path == null || !path.isActive) {
			path = m_Manager.GetAreaPath ();
		}

//		Quaternion lookRotation = Quaternion.LookRotation((route[next] - transform.position).normalized);
//		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, GameClockManager.Instance.time * speed * 3.0f);
//
//		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (route[next].x, ground.point.y + groundOffset, route[next].z), GameClockManager.Instance.time * speed);
//		//when reached, go to next
//		if ((route [next].x - transform.position.x) * (route [next].x - transform.position.x)
//		    + (route [next].z - transform.position.z) * (route [next].z - transform.position.z) <= 0.1f) {
//			turnDelay = 1.0f;
//			if (next == route.Length - 1 && pathType == pathEnum.loop) {
//				next = 0;
//			} else if (pathType == pathEnum.loop) {
//				next ++;
//			} else if (next == route.Length - 1 && pathType == pathEnum.reverse) {
//				fwd = false;
//			} else if (next == 0 && pathType == pathEnum.reverse) {
//				fwd = true;
//			}
//		}


		if (path != null) {
			// if the we're at the end, restart
			if (CurrentPathPercent >= 0.99f) {
				CurrentPathPercent = 0.0f;
				// TODO - this needs some lerping adjustment
				currentLook = rotationOffset;
			}

			// move along the percentage of the path by time
			CurrentPathPercent += percentsPerSecond * speedMod * GameClockManager.Instance.time;
			currentLook += percentsPerSecond * speedMod * GameClockManager.Instance.time;
			Vector3 look = iTween.PointOnPath (path.pathway, currentLook);
			iTween.PutOnPath (gameObject, path.pathway, CurrentPathPercent);
			transform.LookAt (look);
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
			//Debug.Log ("from walking");
		}
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, ground.point.y + groundOffset, transform.position.z), GameClockManager.Instance.time * speed);
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
			if (rangedAttackSound != null)
				AudioManager.Instance.PlaySoundEffect (rangedAttackSound);
			//Debug.Log(projectile_shoot.gameObject.name);
			//send forward
			projectile_shoot.AddForce (new Vector3(transform.forward.x, transform.forward.y - 0.05f, transform.forward.z) * projectileSpeed);
			Destroy (projectile_shoot.gameObject, 2.0f);


			projectileTime = Time.time + projectileInterval;
		}
	}

	/// <summary>
	/// Turning state
	/// not used at the moment, keeping it for now
	/// </summary>
	void TurningState() {

		transform.LookAt (center);
		//			Vector3 dir3 = (new Vector3 (center.x, 0.0f, center.z) - transform.position).normalized;
		//			Quaternion rot3 = Quaternion.LookRotation (dir3);
		//			transform.rotation = Quaternion.Slerp (transform.rotation, rot3, GameClockManager.Instance.time * speed);
		transform.Translate (transform.forward * GameClockManager.Instance.time * speed, Space.World);
		//state = State.Walking;

		if ((transform.position.x - center.x) * (transform.position.x - center.x)
			+ (transform.position.z - center.z) * (transform.position.z - center.z) <= 0.1f) {

			state = State.Walking;
			//Debug.Log ("back to path");
		}
	}

	/// <summary>
	/// Attacking state
	/// when the player is close, stop and do an attack animation
	/// </summary>
	void AttackingState(){
		////Debug.Log ("attack state");
		animator.SetBool ("Walk", false);
		animator.SetTrigger("Attack");

		if ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
		    + (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= attackProximity) {
			state = State.Attacking;

			if (attackTime >= attackDelay) {
				////Debug.Log ("tossing");
				AudioManager.Instance.PlaySoundEffect (physicalAttackSound);
				GameManager.Instance.stats.postmenHit++;
				package = GameObject.FindGameObjectWithTag ("Package");
				if (package != null) {
					PlayPackageDamageSound ();
					package.GetComponent<Package> ().DamagePackage (damageStrength / 2.0f);
				}
				attackTime = 0.0f;
			} else if (attackTime < attackDelay) {
				attackTime += 5.0f * GameClockManager.Instance.time;
			}

		} else {
			state = State.PlayerChase;
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
		////Debug.Log ("chasing");
		float dir_y = -0.1014264f; 



		if ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
		    + (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= attackProximity) {
			state = State.Attacking;
			//Debug.Log ("player attack");
		}


		//go back to walking state
		if ((transform.position.x - center.x) * (transform.position.x - center.x)
			+ (transform.position.z - center.z) * (transform.position.z - center.z) >= radius * radius) {
			//Debug.Log ("postman outside radius");
			state = State.Turning;

		} else {
			//Debug.Log ("player inside radius");
			Vector3 dir = (new Vector3 (player.transform.position.x, player.transform.position.y - 0.4f, player.transform.position.z) - transform.position).normalized;
			Quaternion rot = Quaternion.LookRotation (dir);
			transform.rotation = Quaternion.Slerp (transform.rotation, rot, GameClockManager.Instance.time * speed);
			transform.Translate (transform.forward * GameClockManager.Instance.time * speed, Space.World);
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, ground.point.y + groundOffset, transform.position.z), GameClockManager.Instance.time * speed);
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
			Vector3 dir = (new Vector3 (player.transform.position.x, player.transform.position.y - 0.4f, player.transform.position.z) - transform.position).normalized;
			Quaternion rot = Quaternion.LookRotation (dir);
			transform.rotation = Quaternion.Slerp (transform.rotation, rot, GameClockManager.Instance.time * speed);
			shoot ();

			//transform.Translate (transform.forward * GameClockManager.Instance.time * speed, Space.World);
			if (radius <= projectileRadius && ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
				+ (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= radius * radius)) {
				center = transform.position;
				state = State.PlayerChase;
				//Debug.Log ("if 1");
			} else if ((player.transform.position.x - center.x) * (player.transform.position.x - center.x)
				+ (player.transform.position.z - center.z) * (player.transform.position.z - center.z) >= projectileRadius * projectileRadius) {
				state = State.Walking;
				//Debug.Log ("if 2");
			} else {
				CheckForPlayer ();
			}
		} else {

			if (radius <= projectileRadius && ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
			    + (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= radius * radius)) {
				center = transform.position;
				state = State.PlayerChase;
				//Debug.Log ("if 3");
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

//		if (path == null || !path.isActive) {
//			path = m_Manager.GetAreaPath ();
//		}
//
//		if (path != null) {
//			// if the we're at the end, restart
//			if (CurrentPathPercent >= 0.99f) {
//				CurrentPathPercent = 0.0f;
//				// TODO - this needs some lerping adjustment
//				currentLook = rotationOffset;
//			}
//
//			// move along the percentage of the path by time
//			CurrentPathPercent += percentsPerSecond * speedMod * GameClockManager.Instance.time;
//			currentLook += percentsPerSecond * speedMod * GameClockManager.Instance.time;
//			Vector3 look = iTween.PointOnPath (path.pathway.ToArray(), currentLook);
//			iTween.PutOnPath (gameObject, path.pathway.ToArray(), CurrentPathPercent);
//			transform.LookAt (look);
//		}


		//don't walk into walls, do walk up to player
//		RaycastHit front;
//		if (Physics.Raycast (transform.position, transform.forward, out front)) {
//			////Debug.Log ("hit something " + front.distance);
//			if (hit.point.y > 0.3f && front.distance < 0.3f && !hit.collider.name.Equals("player") && !hit.collider.name.Equals("dog") && state != State.Turning) {
//				//try to avoid walking through walls by going to next step
//				if (state == State.Walking) {
//					if (next == route.Length - 1 && pathType == pathEnum.loop) {
//						next = 0;
//					} else if (pathType == pathEnum.loop) {
//						next++;
//					} else if (next == route.Length - 1 && pathType == pathEnum.reverse) {
//						fwd = false;
//					} else if (next == 0 && pathType == pathEnum.reverse) {
//						fwd = true;
//					}
//
//					if (pathType == pathEnum.reverse && fwd) {
//						next++;
//					} else if (pathType == pathEnum.reverse && !fwd) {
//						next--;
//					}
//				} else {
//					state = State.Walking;
//				}
//			}
//		} 

	}
	
	// Update is called once per frame
	void Update () {
		if (GameClockManager.Instance.freeze)
			animator.speed = 0.0f;
		else
			animator.speed = 1.0f;

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


	void OnDrawGizmos()
	{
		//Visual. Not used in movement
		if(path != null && path.pathway != null)
			iTween.DrawPath(path.pathway);

		if (m_Manager.debug) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (this.transform.position, 3);
		}
	}

	public void PlayPackageDamageSound(){
		int random = Random.Range (1, 4);
		switch (random) {
		case 1:
			AudioManager.Instance.PlaySoundEffect (packageDamage1);
			break;
		case 2:
			AudioManager.Instance.PlaySoundEffect (packageDamage2);
			break;
		case 3:
			AudioManager.Instance.PlaySoundEffect (packageDamage3);
			break;
		}
	}


}
