using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Data/Level")]
public class Level : ScriptableObject{

	public string levelName;

	[HideInInspector]
	[Serializable]
	public class PickupLocationGroup
	{
		public List<string> locations;
	}

	public List<PickupLocationGroup> pickupLocations;

	private int currIndex = 0;

	public PickupLocation GetPickupLocation(ref PickupLocation[] activeLocations)
	{
		string current = pickupLocations [currIndex].locations [UnityEngine.Random.Range(0, pickupLocations [currIndex].locations.Count-1)];


		for (int i = 0; i < activeLocations.Length; i++) {
			if (activeLocations[i].gameObject.name == current) {
				PickupLocation location = activeLocations [i];
				location.SetActive ();
				return location;
			}
		}

		Debug.Log (string.Format ("PickupLocation {0} not found in scene.", current));
		return null;
	}
}
