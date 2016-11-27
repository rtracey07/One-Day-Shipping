using UnityEngine;
using System.Collections;
/*
 * 
 * enemies fly back at forth at random within a set radius
 * they inflict damage on intersection
 * lose life points and are destroyed by shooting
 * 
*/
public class DogAI : MonoBehaviour {
	//speed to move
	[SerializeField] private float speed = 5.0f;				// flight speed
	[SerializeField] private float damageStrength = 1.0f;			// damage to player
	// the space in which it can travel
	private Vector3 center = Vector3.zero;		// center of the radius
	[SerializeField] private float radius = 3.0f;
	[SerializeField] private float attackProximity = 0.3f;		// when to do attack animation
	private GameObject package;				// package to damage

	private GameObject player;
	private Collider col;
	private bool wallHit = false;
	private bool playerChase = false;							// if the player is nearby
	private Animator animator;									// for animating
	private bool wall = false;

	//positions
	private float x;
	private float z;
	private float change = 0.0f;
	private float angle = 0.0f;
	private float current;

	private bool attack = false;
	private float attackTime = 3.0f;
	private float attackDelay = 3.0f;

	//keep track of ground
	private RaycastHit ground;
	[SerializeField] private float groundOffset = 0.5f;

	// property to get damage points
	public float DamageStrength {
		get{ return damageStrength; }
	}

	public Vector3 Center {
		set{ 
			center = value;
			transform.position = center;
			Debug.Log ("changing pos to " + transform.position);
		}
		get{ return center; }

	}

	// Use this for initialization
	void Start () {
		animator = GetComponent <Animator>();
		//transform.position = center;
		transform.Rotate(0, Random.Range(-180.0f, 180.0f), 0);
		player = GameObject.Find ("Player");
		package = GameObject.FindGameObjectWithTag ("Package");
		//life = lifePoints;
	}

	/// <summary>
	/// accept damage when hit by glows
	/// </summary>
	/// <param name="col">Col.</param>
	void  OnTriggerEnter (Collider col)
	{
		print ("COLLISION");

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
			if (front.distance < 0.3f && !hit.collider.name.Equals("player")) {
				wall = true;
				Debug.Log ("hit wall " + hit.collider.name);
			}
			else {
				wall = false;
			}
		} else {
			wall = false;
		}
	}


	// Update is called once per frame
	void Update () {
		if (GameClockManager.Instance.freeze) {
			animator.speed = 0;
		}
		else
		{
			animator.speed = 1;
		Debug.Log ("pos " + transform.position);
		//attack or run around
			if ((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)
			   + (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z) <= attackProximity) {
				animator.SetBool ("Attack", true);
				GameManager.Instance.dogAttack = true;
				attack = true;
			} else {
				animator.SetBool ("Attack", false);
		


				if ((player.transform.position.x - center.x) * (player.transform.position.x - center.x)
				   + (player.transform.position.z - center.z) * (player.transform.position.z - center.z) <= radius * radius) {
					// the player is within the radius, attack
					package = GameObject.FindGameObjectWithTag ("Package");
					wallHit = false;
					//animator.SetBool ("Attack", true);
					playerChase = true;
					//Vector3 dir = (new Vector3 (player.transform.position.x, center.y, player.transform.position.z) - transform.position).normalized;
					Vector3 dir = (new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z) - transform.position).normalized;
					Quaternion rot = Quaternion.LookRotation (dir);
					transform.rotation = Quaternion.Slerp (transform.rotation, rot, GameClockManager.Instance.time * speed);
				} else if ((transform.position.x - center.x) * (transform.position.x - center.x)
				         + (transform.position.z - center.z) * (transform.position.z - center.z) >= radius * radius && !wallHit) {
					// hit the circumference, so turn around
					transform.Translate (-2.0f * transform.forward * GameClockManager.Instance.time * speed, Space.World);
					current = transform.rotation.eulerAngles.y;
					if (current > 180.0f) {
						change = Random.Range (-180.0f, -120.0f);
					} else {
						change = Random.Range (120.0f, 180.0f);
					}
					playerChase = false;
					wallHit = true;
				} else if (wall && !wallHit) {
					// hit a wall, so turn around
					transform.Translate (-2.0f * transform.forward * GameClockManager.Instance.time * speed, Space.World);
					current = transform.rotation.eulerAngles.y;
					if (current > 180.0f) {
						change = Random.Range (-180.0f, -120.0f);
					} else {
						change = Random.Range (120.0f, 180.0f);
					}
					playerChase = false;
					wallHit = true;
				}

				// the rotation is slerped based on turning direction
				if (wallHit && current > 180.0f && transform.rotation.eulerAngles.y > current + change) {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + change, transform.rotation.eulerAngles.z), GameClockManager.Instance.time * speed);
				} else if (wallHit && current < 180.0f && transform.rotation.eulerAngles.y < current + change) {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + change, transform.rotation.eulerAngles.z), GameClockManager.Instance.time * speed);
				} else {
					wallHit = false;
					transform.Translate (transform.forward * GameClockManager.Instance.time * speed, Space.World);
				}

				// stay on the ground
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, ground.point.y + groundOffset, transform.position.z), GameClockManager.Instance.time * speed);
			}
		}


		if (attack && attackTime >= attackDelay) {
			//Debug.Log ("tossing");
			package = GameObject.FindGameObjectWithTag ("Package");
			if (package != null) {
				package.GetComponent<Package> ().DamagePackage (damageStrength);
			}
			attackTime = 0.0f;
		} else if (attack && attackTime < attackDelay) {
			attackTime += 3.0f * GameClockManager.Instance.time;
			attack = false;
		}

	}
}
