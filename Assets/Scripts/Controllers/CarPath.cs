using UnityEngine;
using System.Collections;

public class CarPath : MonoBehaviour {

	private Pathway path;						// the path
	private Rigidbody m_Rigidbody;
	public float rotationOffset = 0.05f;			// how far ahead to look to orient on path
	public float speed = 50.0f;
	private float speedMod = 1.0f;

	private float currentLook =0.25f;										// where the car is looking
	private float percentsPerSecond = 0.1f; 								// %1 of the path moved per second
	private CarPathManager m_Manager;
	private bool isHit = false;
	private Vector3 cameraSpacePos;

	public AudioClip carCollisionSound;

	void Start()
	{
		currentLook = CurrentPathPercent + rotationOffset;
		percentsPerSecond = 0.02f;//speed * 0.0004f;

		m_Manager = GetComponentInParent<CarPathManager> ();
		m_Rigidbody = GetComponent<Rigidbody> ();
	}

	public float CurrentPathPercent {
		get;
		set;
	}
		

	void Update ()
	{
		if (path == null || !path.isActive) {
			path = m_Manager.GetAreaPath ();
		}

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
		if (other.gameObject.tag == "Player") {
			other.rigidbody.AddForce (-other.contacts [0].normal * Vector3.Dot(-other.contacts [0].normal,transform.forward)*m_Rigidbody.mass);
			if (!isHit) {
				GameManager.Instance.stats.carsHit++;
				isHit = true;
				AudioManager.Instance.PlaySoundEffect (carCollisionSound);
				StartCoroutine (ResetHit (2.0f));
			}
		}
	}

	IEnumerator ResetHit(float time)
	{
		yield return new WaitForSeconds (time);
		isHit = false;
	}
}
