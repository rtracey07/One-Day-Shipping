﻿using UnityEngine;
using System.Collections;

public class CarPath : MonoBehaviour {

	private Vector3[] waypointArray;								// the path
	[SerializeField] private float rotationOffset = 0.05f;			// how far ahead to look to orient on path
	[SerializeField] private float citySpeed = 50.0f;				// speed to travel
	[SerializeField] private float suburbSpeed = 50.0f;				// speed to travel
	[SerializeField] private float postalSpeed = 50.0f;				// speed to travel
	private float speed;
	float currentLook =0.25f;										// where the car is looking
	float percentsPerSecond = 0.1f; 								// %1 of the path moved per second
	//float CurrentPathPercent = 0.0f; 								//min 0, max 1
	bool carHit = false;											// if the player gets hit by car
	[SerializeField] private GameObject player;						// player to hit
	float tossPlayer = 0.0f;
	[SerializeField] private float tossTime = 1.0f;					// how far the player gets tossed

	[SerializeField] private Vector3 mapSuburbCenter;				//creates radius for car placement
	[SerializeField] private Vector3 mapCityACenter;			 	//creates radius for car placement
	[SerializeField] private Vector3 mapCityBCenter;			 	//creates radius for car placement
	[SerializeField] private Vector3 mapCityCCenter;			 	//creates radius for car placement
	[SerializeField] private Vector3 mapPostalCenter;				//creates radius for car placement
	[SerializeField] private float suburbRadius = 300.0f;
	[SerializeField] private float cityARadius = 100.0f;
	[SerializeField] private float cityBRadius = 100.0f;
	[SerializeField] private float cityCRadius = 100.0f;
	[SerializeField] private float postalRadius = 300.0f;

	[SerializeField] private Pathway pathACity;
	[SerializeField] private Pathway pathBCity;
	[SerializeField] private Pathway pathCCity;
	[SerializeField] private Pathway pathSuburb;
	[SerializeField] private Pathway pathPostal;

	private float bounceOffset = 0.07f;								//for car bouncing


	void Start()
	{
		speed = citySpeed;
		currentLook = CurrentPathPercent + rotationOffset;
		percentsPerSecond = speed * 0.0004f;
		waypointArray = pathACity.pathway.ToArray();


	}
		
	public float CurrentPathPercent {
		get;
		set;
	}

	public GameObject Player {
		get{ return player; }
		set{ player = value; }
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
		//terrain is -140 -59, -140 -169, 309 169, 309 -59
		if ((player.transform.position.x - mapSuburbCenter.x) * (player.transform.position.x - mapSuburbCenter.x)
		    + (player.transform.position.z - mapSuburbCenter.z) * (player.transform.position.z - mapSuburbCenter.z) <= suburbRadius * suburbRadius) {
			//in the suburb area
			waypointArray = pathSuburb.pathway.ToArray();
			speed = suburbSpeed;
			bounceOffset = 10.0f;
		} else if ((player.transform.position.x - mapCityACenter.x) * (player.transform.position.x - mapCityACenter.x)
			+ (player.transform.position.z - mapCityACenter.z) * (player.transform.position.z - mapCityACenter.z) <= cityARadius * cityARadius) {
			//in the city area
			//Debug.Log ("a");
			waypointArray = pathACity.pathway.ToArray();
			speed = citySpeed*10.0f;
			bounceOffset = 0.07f;
		} else if ((player.transform.position.x - mapCityBCenter.x) * (player.transform.position.x - mapCityBCenter.x)
			+ (player.transform.position.z - mapCityBCenter.z) * (player.transform.position.z - mapCityBCenter.z) <= cityBRadius * cityBRadius) {
			//in the city area
			//Debug.Log ("b");
			waypointArray = pathBCity.pathway.ToArray();
			speed = citySpeed*10.0f;
			bounceOffset = 0.07f;
		} else if ((player.transform.position.x - mapCityCCenter.x) * (player.transform.position.x - mapCityCCenter.x)
			+ (player.transform.position.z - mapCityCCenter.z) * (player.transform.position.z - mapCityCCenter.z) <= cityCRadius * cityCRadius) {
			//in the city area
			waypointArray = pathCCity.pathway.ToArray();
			speed = citySpeed*10.0f;
			//Debug.Log ("c");
			bounceOffset = 0.07f;
		} else if ((player.transform.position.x - mapPostalCenter.x) * (player.transform.position.x - mapPostalCenter.x)
		          + (player.transform.position.z - mapPostalCenter.z) * (player.transform.position.z - mapPostalCenter.z) <= postalRadius * postalRadius) {
			//in the postal area
			waypointArray = pathPostal.pathway.ToArray();
			speed = citySpeed*10.0f;
			bounceOffset = 0.07f;
		}

		// if the we're at the end, restart
		if (CurrentPathPercent >= 0.99f) {
			CurrentPathPercent = 0.0f;
			// TODO - this needs some lerping adjustment
			currentLook = rotationOffset;
		}

		// move along the percentage of the path by time
		CurrentPathPercent += percentsPerSecond * Time.deltaTime;
		currentLook += percentsPerSecond * Time.deltaTime;
		Vector3 look = iTween.PointOnPath (waypointArray, currentLook);
		iTween.PutOnPath(gameObject, waypointArray, CurrentPathPercent);
		transform.LookAt (look);

		//iTween.MoveTo (gameObject, iTween.Hash ("path", waypointArray, "time", CurrentPathPercent, "orienttopath", true , "lookahead", 1.0f ,"axis", "y"));
		//iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("spaceshipPath"), "axis", "z", "time", 20, "orienttopath", true)); 

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
	}

	void OnDrawGizmos()
	{
		//Visual. Not used in movement
		iTween.DrawPath(waypointArray);
	}
}
