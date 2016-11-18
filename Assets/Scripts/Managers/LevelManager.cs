using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public Level levelData;
	public GameObject currentPickup;
	private GameObject player;

	// Use this for initialization
	void Start () {
		currentPickup = levelData.GetPickupLocation ();
		player = GameObject.FindGameObjectWithTag ("Player");
		SpawnCars ();
	}

	private void SpawnCars(){
		for (int i = 0; i < levelData.carPathGroup.numberOfCarsToSpawn; i++) {
			int prefabIndex = Random.Range (0, levelData.carPathGroup.carPrefabs.Count);
			GameObject carPrefab = levelData.carPathGroup.carPrefabs [prefabIndex];
			GameObject car = GameObject.Instantiate (carPrefab);
			CarPath carPath = car.GetComponent<CarPath> ();
			carPath.Player = player;
			carPath.CurrentPathPercent = (float)i / levelData.carPathGroup.numberOfCarsToSpawn;
		}
	}
}
