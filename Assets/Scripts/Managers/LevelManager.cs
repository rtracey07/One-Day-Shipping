using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private static LevelManager _Instance;
	public static LevelManager Instance {
		get {
			return _Instance;
		}
	}

	public Level levelData;
	public Location currentDestination;
	private GameObject player;
	private Location[] activeLocations;

	void Awake () {
		if (_Instance == null)
			_Instance = this;
		else
			Debug.Log ("Multiple Level Managers in the scene.");

		activeLocations = GameObject.FindObjectsOfType<Location> ();

		player = GameObject.FindGameObjectWithTag ("Player");
		SpawnCars ();

		StartCoroutine (RunLevel());
	}

	IEnumerator RunLevel()
	{
		for (int i = 0; i < 5; i++) {
			currentDestination = levelData.GetPickupLocation (ref activeLocations);

			yield return new WaitUntil (() => GameManager.Instance.hasPackage);

			currentDestination = levelData.GetDropoffLocation (ref activeLocations);

			yield return new WaitUntil (() => !GameManager.Instance.hasPackage);
		}
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
