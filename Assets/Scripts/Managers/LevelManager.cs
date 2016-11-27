using UnityEngine;
using UnityEngine.UI;
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


	//Dialog Box Elements.
	public Image m_DialogueBox;
	public Text m_Dialogue;
	public Text m_ParcelCount;
	public Button m_Confirm;
	public Button m_Skip;
	public Image m_Avatar;

	public Level levelData;
	public Location currentDestination;

	private Location[] activeLocations;

	void Awake () {
		if (_Instance == null)
			_Instance = this;
		else
			Debug.Log ("Multiple Level Managers in the scene.");

		activeLocations = GameObject.FindObjectsOfType<Location> ();

		SpawnCars ();
		SpawnDogs ();

		StartCoroutine (levelData.RunLevel());
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

	public void SetPickup()
	{
		if (currentDestination != null && currentDestination.minimapMarker != null) {
			currentDestination.SetMiniMapMarkerActive (false);
			currentDestination = levelData.GetPickupLocation (ref activeLocations);
			currentDestination.SetMiniMapMarkerActive (true);   
		} else if (currentDestination == null) {
			currentDestination = levelData.GetPickupLocation (ref activeLocations);
			if (currentDestination.minimapMarker != null) {
				currentDestination.SetMiniMapMarkerActive (true);  
			}
		} else {
			Debug.Log ("Current location doesn't exist or has no minimap marker.");
		}
	}

	public void SetDropoff()
	{
		if (currentDestination != null && currentDestination.minimapMarker != null) {
			currentDestination.SetMiniMapMarkerActive (false);
			currentDestination = levelData.GetDropoffLocation (ref activeLocations);
			currentDestination.SetMiniMapMarkerActive (true);
		} else {
		Debug.Log ("Current location doesn't exist or has no minimap marker.");
		}
	}

	public void RunEvent(InGameEvent currEvent, string dialogue)
	{
		if (currEvent != null) {

			if (currEvent.avatar != null)
				m_Avatar.sprite = currEvent.avatar;

			m_Dialogue.text = dialogue;

			m_DialogueBox.gameObject.SetActive (true);
			m_Confirm.gameObject.SetActive(currEvent.requiresConfirmation);
			m_Skip.gameObject.SetActive(currEvent.isSkippable);
		}
	}

	public void HideTextBox()
	{
		m_DialogueBox.gameObject.SetActive (false);
	}

	public void UpdatePackageDeliveredCount()
	{
		m_ParcelCount.text = string.Format ("{0}/{1}", GameManager.Instance.deliveredCount, levelData.packageCount);
	}

	public bool CheckWinState()
	{
		return (GameManager.Instance.deliveredCount == levelData.packageCount);
	}
}