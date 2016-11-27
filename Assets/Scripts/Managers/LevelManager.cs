using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private static LevelManager _Instance;
	public static LevelManager Instance {
		get {
			return _Instance;
		}
	}

    void OnDestroy()
    {
        levelData.currIndex = 0;
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
		SpawnDogs ();

		StartCoroutine (RunLevel());
	}

	IEnumerator RunLevel()
	{
		for (int i = 0; i < 5; i++) {
			currentDestination = levelData.GetPickupLocation (ref activeLocations);

			currentDestination.SetMiniMapMarkerActive(true);

			yield return new WaitUntil (() => GameManager.Instance.hasPackage);

			currentDestination.SetMiniMapMarkerActive(false);

			currentDestination = levelData.GetDropoffLocation (ref activeLocations);

			currentDestination.SetMiniMapMarkerActive(true);

			yield return new WaitUntil (() => !GameManager.Instance.hasPackage);

			currentDestination.SetMiniMapMarkerActive(false);
		}
	}

	private void SpawnCars(){
		GameObject carParent = GameObject.Find ("Car Pool");

		if(carParent != null)
		{
			for (int i = 0; i < levelData.carPathGroup.numberOfCarsToSpawn; i++) {
				int prefabIndex = Random.Range (0, levelData.carPathGroup.carPrefabs.Count);
				GameObject carPrefab = levelData.carPathGroup.carPrefabs [prefabIndex];
				GameObject car = GameObject.Instantiate (carPrefab);
				car.transform.parent = carParent.transform;
				CarPath carPath = car.GetComponent<CarPath> ();
				carPath.CurrentPathPercent = (float)i / levelData.carPathGroup.numberOfCarsToSpawn;
			}
		}
		else
		{
			Debug.Log("Car Pool GameObject missing from scene. Nowhere to instantiate cars");
		}
	}

	private void SpawnDogs(){
		GameObject dogParent = GameObject.Find ("Dog Pool");

		if(dogParent != null)
		{
			Debug.Log("spawning " + levelData.dogGroup.numDogsToSpawn);
			for (int i = 0; i < levelData.dogGroup.numDogsToSpawn; i++) {
				GameObject dog = GameObject.Instantiate (levelData.dogGroup.dog);
				dog.transform.parent = dogParent.transform;
				//dog.GetComponent<DogAI>().Center = levelData.dogGroup.dogSpawnLocations [i];
			}
		}
		else
		{
			Debug.Log("Dog Pool GameObject missing from scene. Nowhere to instantiate cars");
		}
	}

	public float GetMissionLength()
	{
		return levelData.missionLength;
	}
		

}