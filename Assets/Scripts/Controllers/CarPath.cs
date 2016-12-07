using UnityEngine;
using System.Collections;

public class CarPath : MonoBehaviour {

	private Pathway path;													// the path
	private Package package;
	private Rigidbody m_Rigidbody;
	public float rotationOffset = 0.05f;									// how far ahead to look to orient on path
	public float speed = 50.0f;
	private float speedMod = 0.70f;											// speed around path

	[SerializeField] private float damageStrength = 10.0f;					// for hurting package

	private float currentLook =0.25f;										// where the car is looking
	private float percentsPerSecond = 0.1f; 								// %1 of the path moved per second
	private CarPathManager m_Manager;										// the manager object
	private bool isHit = false;												// checks if car is colliding
	private Vector3 cameraSpacePos;											// keeps track of camera
	private float groundOffset = -0.1f;										// adjusts for floating off ground

	private Vector3[] iTweenPath;											// the path to follow

	public AudioClip carCollisionSound1, carCollisionSound2, carCollisionSound3;

	void Start()
	{
		currentLook = CurrentPathPercent + rotationOffset;
		percentsPerSecond = 0.02f;//speed * 0.0004f;

		m_Manager = GetComponentInParent<CarPathManager> ();
		m_Rigidbody = GetComponent<Rigidbody> ();

		//iTween.PutOnPath (gameObject, path.pathway, CurrentPathPercent);
	}

	public float CurrentPathPercent {
		get;
		set;
	}

	// property to get damage points
	public float DamageStrength {
		get{ return damageStrength; }
	}
		

	//this should be fixed update for smooth colisions
	void FixedUpdate(){
		if (path != null) {
			Vector3 look = iTween.PointOnProcessedPath (path.pathway, currentLook);
			iTween.PutOnProcessedPath (gameObject, path.pathway, CurrentPathPercent);

			transform.LookAt (look);
		}

		if (path == null || !path.isActive) {
			path = m_Manager.GetAreaPath ();
			if (path != null && path.pathway != null)
				iTweenPath = iTween.PathControlPointGenerator (path.pathway);
		}

		if (path != null) {
			// if the we're at the end, restart
			if (CurrentPathPercent >= 0.99f) {
				CurrentPathPercent = 0.0f;
				currentLook = rotationOffset;
			}

			// move along the percentage of the path by time
			CurrentPathPercent += percentsPerSecond * speedMod * GameClockManager.Instance.fixedTime;
			// look ahead on the path
			currentLook += percentsPerSecond * speedMod * GameClockManager.Instance.fixedTime;

		}
	}

	void OnDrawGizmos()
	{
		if (path != null && path.pathway != null) {
			//Visual. Not used in movement
			iTween.DrawPath (path.pathway);
		}

		if (m_Manager.debug) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (this.transform.position, 3);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		// if running into player, do damage
		if (other.gameObject.tag == "Player") {
			other.rigidbody.AddForce (-other.contacts [0].normal * Vector3.Dot(-other.contacts [0].normal,transform.forward)*m_Rigidbody.mass);
			if (!isHit) {
				GameManager.Instance.stats.carsHit++;
				isHit = true;
				PlayCarHornSound ();
				//damage package if it exists:
				GameObject pack_gameobject = GameObject.FindGameObjectWithTag("Package");
				if (pack_gameobject != null) {
					package = pack_gameobject.GetComponent<Package>();
					if (package != null) {
						package.DamagePackage (damageStrength);
					}
				}
				StartCoroutine (ResetHit (2.0f));
			}
		}
	}

	/// <summary>
	/// sound effects for running into player
	/// </summary>
	public void PlayCarHornSound(){
		int random = Random.Range (1, 4);
		switch (random) {
		case 1:
			AudioManager.Instance.PlaySoundEffect (carCollisionSound1);
			break;
		case 2:
			AudioManager.Instance.PlaySoundEffect (carCollisionSound2, 0.05f);
			break;
		case 3:
			AudioManager.Instance.PlaySoundEffect (carCollisionSound3);
			break;
		}
	}

	/// <summary>
	/// delays on hits to avoid pop-in issues
	/// </summary>
	/// <returns>The hit.</returns>
	/// <param name="time">Time.</param>
	IEnumerator ResetHit(float time)
	{
		yield return new WaitForSeconds (time);
		isHit = false;
	}
}
