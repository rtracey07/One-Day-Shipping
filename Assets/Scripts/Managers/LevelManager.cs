using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public Level levelData;
	public GameObject currentPickup;

	// Use this for initialization
	void Start () {
		currentPickup = levelData.GetPickupLocation ();
	}
}
