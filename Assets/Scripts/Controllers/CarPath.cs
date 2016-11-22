using UnityEngine;
using System.Collections;

public class CarPath : MonoBehaviour {

	private Vector3[] wayPoints;								// the path
	[SerializeField] private float rotationOffset = 0.05f;			// how far ahead to look to orient on path
	[SerializeField] private float citySpeed = 50.0f;				// speed to travel
	[SerializeField] private float suburbSpeed = 50.0f;				// speed to travel
	[SerializeField] private float postalSpeed = 50.0f;				// speed to travel
	private float speed;
	float currentLook =0.25f;										// where the car is looking
	float percentsPerSecond = 0.1f; 								// %1 of the path moved per second
	//float CurrentPathPercent = 0.0f; 								//min 0, max 1
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

//	/// <summary>
//	/// FixedUpdate
//	/// using raycasts to detect player
//	/// </summary>
//	void FixedUpdate() {
//		RaycastHit front;
//		if (Physics.Raycast (transform.position, transform.forward, out front)) {
//			//Debug.Log ("hit something " + front.distance);
//			if (front.distance < 0.1f && front.collider.tag.Equals("Player")) {
//				
//			}
//		} 
//
//	}


	void Update () 
	{
		if (m_Manager.pathChange) {
			wayPoints = m_Manager.GetCurrentAreaPath ();
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

/*
		if (carHit && tossPlayer < tossTime) {
			//Debug.Log ("tossing");
			player.transform.Translate (transform.forward * Time.deltaTime * speed * bounceOffset, Space.World);
			player.transform.Translate (0.5f * transform.up * Time.deltaTime * speed * bounceOffset, Space.World);
			tossPlayer += 3.0f * Time.deltaTime;
		} else if (carHit && tossPlayer >= tossTime) {
			carHit = false;
			//Debug.Log ("not tossing");
			tossPlayer = 0.0f;
		}

*/
	}

	void OnDrawGizmos()
	{
		//Visual. Not used in movement
		iTween.DrawPath(wayPoints);
	}
}
