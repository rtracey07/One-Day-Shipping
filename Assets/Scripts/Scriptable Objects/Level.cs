using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Data/Level/Default")]
public class Level : ScriptableObject{

	[Header("Mission Data")]
	public string levelName;										// name
	public float missionLength;										// how much time
	public float packageCount;										// how many packages to collect
	public AudioClip backgroundMusic;								// background audio file

	[HideInInspector]
	[Serializable]
	public class LocationGroup
	{
		public List<string> locations;								// package locations
	}

	[HideInInspector]
	[Serializable]
	public class CarPathGroup
	{
		public int numberOfCarsToSpawn;								// how many cars on screen at once
		public List<GameObject> carPrefabs;							// cars to show
	}
		
	[Header("Enemy and Prop Spawning")]
	public CarPathGroup carPathGroup;

	[HideInInspector]
	[Serializable]
	public class DogGroup
	{
		public GameObject dog;										// dog object
		public int numDogsToSpawn;									// how many dogs?
	}

	public DogGroup dogGroup;										// the dog group

	[HideInInspector]
	[Serializable]
	public class StatPostmanGroup
	{
		public GameObject stat;										// stationary postman
		public int numStatPostmenToSpawn;							// number of stationary postmen
	}

	public StatPostmanGroup statPostmanGroup;

	[HideInInspector]
	[Serializable]
	public class PostmanPathGroup
	{
		public GameObject postman;									// the postman object
		public int numPostmanToSpawn;								// number
		public bool throwProjectiles;								// does this level include projectiles
	}

	public PostmanPathGroup postmanPathGroup;


	[HideInInspector]
	[Serializable]
	public class FlamingPackageGroup
	{
		public GameObject flamingPackage;							// flaming packages from sky
		public int numFlamingPackagesToSpawn;						// number of packages
		public bool activated;										// in this level?
	}

	[Header("Flaming Package Spawning")]
	public FlamingPackageGroup flamingPackageGroup;

	[Header("Package Locations")]
	public List<LocationGroup> pickupLocations;
	public List<LocationGroup> dropoffLocations;

	[Header("In Game Dialogue Events")]
	public List<InGameEvent> events;

	[HideInInspector]
	public int currIndex = 0;

	/// <summary>
	/// Gets the pickup location.
	/// </summary>
	/// <returns>The pickup location.</returns>
	/// <param name="activeLocations">Active locations.</param>
	public Location GetPickupLocation(ref Location[] activeLocations)
	{
		string current = pickupLocations [currIndex].locations [UnityEngine.Random.Range(0, pickupLocations [currIndex].locations.Count)];        

		for (int i = 0; i < activeLocations.Length; i++) {
			if (activeLocations[i].gameObject.name == current) {
				Location location = activeLocations [i];
				location.SetActive ();
				return location;
			}
		}

		Debug.Log (string.Format ("Pickup Location {0} not found in scene.", current));
		return null;
	}

	/// <summary>
	/// Gets the dropoff location.
	/// </summary>
	/// <returns>The dropoff location.</returns>
	/// <param name="activeLocations">Active locations.</param>
	public Location GetDropoffLocation(ref Location[] activeLocations)
	{        
		string current = dropoffLocations [currIndex].locations [UnityEngine.Random.Range(0, dropoffLocations [currIndex].locations.Count)];

		currIndex++;

		if (currIndex >= pickupLocations.Count)
			currIndex = 0;

		for (int i = 0; i < activeLocations.Length; i++) {
			if (activeLocations[i].gameObject.name == current) {
				Location location = activeLocations [i];
				location.SetActive ();
				return location;
			}
		}

		Debug.Log (string.Format ("Dropoff Location {0} not found in scene.", current));

		return null;
	}

	/// <summary>
	/// Just for Thursday
	/// </summary>
	/// <returns>The mountain dropoff location.</returns>
	/// <param name="activeLocations">Active locations.</param>
	public Location GetMountainDropoffLocation(ref Location[] activeLocations)
	{        
		string current = "Mountain_Dropoff";

		currIndex = 2; //Thursday level only has 3 packages to complete

		for (int i = 0; i < activeLocations.Length; i++) {
			if (activeLocations[i].gameObject.name == current) {
				Location location = activeLocations [i];
				location.SetActive ();
				return location;
			}
		}


		Debug.Log (string.Format ("Dropoff Location {0} not found in scene.", current));

		return null;
	}

	public virtual IEnumerator RunLevel()
	{
		Debug.Log ("Default Level Type. Override to add level structure");
		yield return null;
	}
	/// <summary>
	/// Triggers dialog
	/// </summary>
	/// <returns>The event.</returns>
	/// <param name="index">Index.</param>
	public virtual IEnumerator TriggerEvent( int index)
	{
		//delay between dialog
		yield return new WaitForSeconds (events[index].timeBeforeDisplaying);
		GameClockManager.Instance.freeze = events [index].pauseGame;

		foreach (string dialogue in events[index].dialogue) {
			LevelManager.Instance.RunEvent (events [index], dialogue);

			if (events [index].requiresConfirmation) {
				yield return new WaitUntil (() => GameManager.Instance.continueClicked);
				GameManager.Instance.continueClicked = false;
			} else {
				yield return new WaitForSeconds (events [index].duration);
			}
		}

		LevelManager.Instance.HideTextBox ();
		GameClockManager.Instance.freeze = false;
	}

	public IEnumerator TimeUp(int eventIndex)
	{
		yield return new WaitUntil (() => GameManager.Instance.timeUp);
		yield return LevelManager.Instance.StartCoroutine (TriggerEvent (eventIndex));
	}
}
