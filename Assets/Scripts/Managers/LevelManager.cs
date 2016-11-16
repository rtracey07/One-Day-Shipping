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
	public PickupLocation currentPickup;

	private PickupLocation[] activePickupLocations;

	void Start () {
		if (_Instance == null)
			_Instance = this;
		else
			Debug.Log ("Multiple Level Managers in the scene.");
		
		activePickupLocations = GameObject.FindObjectsOfType<PickupLocation> ();
		currentPickup = levelData.GetPickupLocation (ref activePickupLocations);

		StartCoroutine (RunLevel());
	}

	IEnumerator RunLevel()
	{
		yield return new WaitUntil (() => GameManager.Instance.hasPackage);
		Debug.Log ("Has Package");
	}

}
