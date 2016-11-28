using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Data/Level/Default")]
public class Level : ScriptableObject{

	[Header("Mission Data")]
	public string levelName;
	public float missionLength;
	public float packageCount;

	[HideInInspector]
	[Serializable]
	public class LocationGroup
	{
		public List<string> locations;
	}

	[HideInInspector]
	[Serializable]
	public class CarPathGroup
	{
		public int numberOfCarsToSpawn;
		public List<GameObject> carPrefabs;
	}
		
	[Header("Enemy and Prop Spawning")]
	public CarPathGroup carPathGroup;

	[HideInInspector]
	[Serializable]
	public class DogGroup
	{
		public GameObject dog;
		public int numDogsToSpawn;
	}

	public DogGroup dogGroup;

	[HideInInspector]
	[Serializable]
	public class PostmanPathGroup
	{
		public GameObject postman;
		public int numPostmanToSpawn;
		public bool throwProjectiles;
	}

	public PostmanPathGroup postmanPathGroup;

	[Header("Package Locations")]
	public List<LocationGroup> pickupLocations;
	public List<LocationGroup> dropoffLocations;

	[Header("In Game Dialogue Events")]
	public List<InGameEvent> events;

	[HideInInspector]
	public int currIndex = 0;

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

	public virtual IEnumerator RunLevel()
	{
		Debug.Log ("Default Level Type. Override to add level structure");
		yield return null;
	}

	public virtual IEnumerator TriggerEvent( int index)
	{
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

		SceneManager.LoadScene ("Results Screen");
	}
}
