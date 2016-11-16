using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Data/Level")]
public class Level : ScriptableObject{

	public string name;

	[HideInInspector]
	[Serializable]
	public class PickupLocationGroup
	{
		public List<string> locations;
	}
		
	public List<PickupLocationGroup> pickupLocations;

	private int currIndex = 0;

	public GameObject GetPickupLocation()
	{
		string current = pickupLocations [currIndex].locations [UnityEngine.Random.Range(0, pickupLocations [currIndex].locations.Count-1)];
		GameObject location = GameObject.Find (current);
		return location;
	}
}
