using UnityEngine;
using System.Collections;

public class CarPath : MonoBehaviour {

	[SerializeField] private Vector3[] waypointArray;				// the path
	[SerializeField] private float rotationOffset = 0.05f;			// how far ahead to look to orient on path
	[SerializeField] private float speed = 50.0f;					// speed to travel
	float currentLook =0.25f;										// where the car is looking
	float percentsPerSecond = 0.1f; 								// %1 of the path moved per second
	float currentPathPercent = 0.0f; 								//min 0, max 1
	bool carHit = false;											// if the player gets hit by car
	[SerializeField] private GameObject player;						// player to hit
	float tossPlayer = 0.0f;
	[SerializeField] private float tossTime = 1.0f;					// how far the player gets tossed

	void Start()
	{
		currentLook = rotationOffset;
		percentsPerSecond = speed * 0.001f;
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("collision " + other.name);
		if (other.name.Equals("Player")){
			
			carHit = true;
		}

	}


	void Update () 
	{
		// if the we're at the end, restart
		if (currentPathPercent >= 0.99f) {
			currentPathPercent = 0.0f;
			// TODO - this needs some lerping adjustment
			currentLook = rotationOffset;
		}

		// move along the percentage of the path by time
		currentPathPercent += percentsPerSecond * Time.deltaTime;
		currentLook += percentsPerSecond * Time.deltaTime;
		Vector3 look = iTween.PointOnPath (waypointArray, currentLook);
		iTween.PutOnPath(gameObject, waypointArray, currentPathPercent);
		transform.LookAt (look);

		//iTween.MoveTo (gameObject, iTween.Hash ("path", waypointArray, "time", currentPathPercent, "orienttopath", true , "lookahead", 1.0f ,"axis", "y"));
		//iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("spaceshipPath"), "axis", "z", "time", 20, "orienttopath", true)); 

		if (carHit && tossPlayer < tossTime) {
			Debug.Log ("tossing");
			player.transform.Translate (transform.forward * Time.deltaTime * speed, Space.World);
			player.transform.Translate (0.3f * transform.up * Time.deltaTime * speed, Space.World);
			tossPlayer += 3.0f * Time.deltaTime;
		} else if (carHit && tossPlayer >= tossTime) {
			carHit = false;
			Debug.Log ("not tossing");
			tossPlayer = 0.0f;
		}
	}

	void OnDrawGizmos()
	{
		//Visual. Not used in movement
		iTween.DrawPath(waypointArray);
	}
}
