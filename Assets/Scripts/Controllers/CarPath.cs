using UnityEngine;
using System.Collections;

public class CarPath : MonoBehaviour {

	private Vector3[] wayPoints;									// the path
	public float rotationOffset = 0.05f;			// how far ahead to look to orient on path
	public float citySpeed = 50.0f;				// speed to travel
	public float suburbSpeed = 50.0f;				// speed to travel
	public float postalSpeed = 50.0f;				// speed to travel
	private float speed;
	float currentLook =0.25f;										// where the car is looking
	float percentsPerSecond = 0.1f; 								// %1 of the path moved per second
	bool carHit = false;											// if the player gets hit by car
	float tossPlayer = 0.0f;
	[SerializeField] private float tossTime = 1.0f;					// how far the player gets tossed

	private CarPathManager m_Manager;

	void Start()
	{
		speed = citySpeed;
		currentLook = CurrentPathPercent + rotationOffset;
		percentsPerSecond = speed * 0.0004f;

		m_Manager = GetComponentInParent<CarPathManager> ();
	}

	public float CurrentPathPercent {
		get;
		set;
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag.Equals("Player")){
			//Debug.Log ("collision");
			carHit = true;
		}
	}

	void Update () 
	{
		if (wayPoints == null) {
			wayPoints = m_Manager.GetAreaPath ();
		}

		if (wayPoints != null) {
			// if the we're at the end, restart
			if (CurrentPathPercent >= 0.99f) {
				CurrentPathPercent = 0.0f;
				// TODO - this needs some lerping adjustment
				currentLook = rotationOffset;
			}

			// move along the percentage of the path by time
			CurrentPathPercent += percentsPerSecond * Time.deltaTime;
			currentLook += percentsPerSecond * Time.deltaTime;
			Vector3 look = iTween.PointOnPath (wayPoints, currentLook);
			iTween.PutOnPath (gameObject, wayPoints, CurrentPathPercent);
			transform.LookAt (look);
		}
	}

	void OnDrawGizmos()
	{
		//Visual. Not used in movement
		iTween.DrawPath(wayPoints);
	}
}
