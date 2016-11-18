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

	private Location[] activeLocations;

	void Awake () {
		if (_Instance == null)
			_Instance = this;
		else
			Debug.Log ("Multiple Level Managers in the scene.");
		
		activeLocations = GameObject.FindObjectsOfType<Location> ();

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

}
