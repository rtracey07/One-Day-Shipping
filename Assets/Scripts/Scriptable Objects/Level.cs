using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Data/Level/Default")]
public class Level : ScriptableObject{

	public string levelName;
	public float missionLength;

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

	public CarPathGroup carPathGroup;

	[HideInInspector]
	[Serializable]
	public class DogGroup
	{
		public GameObject dog;
		public int numDogsToSpawn;
	}

	public DogGroup dogGroup;

	public List<LocationGroup> pickupLocations;
	public List<LocationGroup> dropoffLocations;
	public List<InGameEvent> events;

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
}
