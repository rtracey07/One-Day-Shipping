using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public Level levelData;
	public PickupLocation currentPickup;

	private PickupLocation[] activePickupLocations;

	// Use this for initialization
	void Start () {
		activePickupLocations = GameObject.FindObjectsOfType<PickupLocation> ();

		currentPickup = levelData.GetPickupLocation (ref activePickupLocations);
	}
}
