using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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

	[Header("In-Game UI Elements")]
	public Image m_DialogueBox;
	public Text m_Dialogue;
	public Text m_ParcelCount;
	public Button m_Confirm;
	public Button m_Skip;
	public Image m_Avatar;

	[Header("Cutscene UI Elements")]
	public Image m_CutsceneBackground;
	public Image m_CutsceneAvatarL;
	public Image m_CutsceneAvatarR;
	public Text m_CutsceneText;
	public Button m_CutsceneConfirm;

	[Header("Level Data")]
	public Level levelData;
	public CutScene cutSceneData;

	[HideInInspector]
	public Location currentDestination;

	private Location[] activeLocations;

	void Awake () {
		if (_Instance == null) {
			_Instance = this;
			StartCoroutine(Run ());
		}
		else {
			DestroyImmediate (this);
		}
	}

	public void SpawnCars(){
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

	public void SpawnDogs(){
		GameObject dogParent = GameObject.Find ("Dog Pool");

		if(dogParent != null)
		{
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
			if (currEvent is CutSceneEvent) {
				CutSceneEvent currCutScene = currEvent as CutSceneEvent;

				if (currCutScene.avatar != null)
					m_CutsceneAvatarR.sprite = currCutScene.avatar;
				else
					m_CutsceneAvatarR.sprite = null;
				
				if (currCutScene.avatarL != null)
					m_CutsceneAvatarL.sprite = currCutScene.avatarL;
				else
					m_CutsceneAvatarL.sprite = null;

				m_CutsceneText.text = dialogue;

				m_CutsceneBackground.sprite = currCutScene.background;
				m_CutsceneBackground.gameObject.SetActive (true);
				m_CutsceneConfirm.gameObject.SetActive (currEvent.requiresConfirmation);

			} else {
				if (currEvent.avatar != null)
					m_Avatar.sprite = currEvent.avatar;

				m_Dialogue.text = dialogue;

				m_DialogueBox.gameObject.SetActive (true);
				m_Confirm.gameObject.SetActive (currEvent.requiresConfirmation);
				m_Skip.gameObject.SetActive (currEvent.isSkippable);
			}
		}
	}

	public void HideTextBox()
	{
		m_DialogueBox.gameObject.SetActive (false);
	}

	public void UpdatePackageDeliveredCount()
	{
		m_ParcelCount.text = string.Format ("{0}/{1}", GameManager.Instance.stats.packagesDelivered, levelData.packageCount);
	}

	public bool CheckWinState()
	{
		return (GameManager.Instance.stats.packagesDelivered == levelData.packageCount);
	}

	private IEnumerator Run()
	{
		if (SceneManager.GetActiveScene ().name != "InGame") {
			GameManager.Instance.FindCamera ();
			yield return StartCoroutine (cutSceneData.RunCutScene ());
			DisableCutScene ();
			AsyncOperation loadLevel = SceneManager.LoadSceneAsync ("InGame");
			yield return new WaitUntil (() => loadLevel.isDone);
		}

		GameManager.Instance.FindCamera ();
		activeLocations = GameObject.FindObjectsOfType<Location> ();

		SpawnCars ();

		yield return StartCoroutine (levelData.RunLevel());
		SceneManager.LoadScene ("Results Screen");
	}

	private void DisableCutScene()
	{
		m_CutsceneBackground.gameObject.SetActive (false);
	}
}