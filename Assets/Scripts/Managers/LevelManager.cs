﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private static LevelManager _Instance;
	public static LevelManager Instance { get { return _Instance; } }

    void OnDestroy()
    {
        levelData.currIndex = 0;
    }


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

	public Image m_BlackOut;

	[Header("Level Data")]
	public Level levelData;
	public CutScene cutSceneData;

	[HideInInspector]
	public Location currentDestination;

	private Location[] activeLocations;

	void Awake () {
		if (_Instance == null) {
			_Instance = this;
			levelData = GameManager.Instance.GetLevelInfo ();
			cutSceneData = GameManager.Instance.GetCutSceneInfo ();
			StartCoroutine(Run ());
		}
		else {
			DestroyImmediate (this);
		}
	}

	/// <summary>
	/// places random cars at equal intervals along activated itween path
	/// </summary>
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

	/// <summary>
	/// places set number of postman at equal intervals along itween path
	/// number of postmen are set by the level scriptable object
	/// </summary>
	public void SpawnPostmans(GameObject parent){

		for (int i = 0; i < levelData.postmanPathGroup.numPostmanToSpawn; i++) {
			GameObject postmanPrefab = levelData.postmanPathGroup.postman;
			GameObject postman = GameObject.Instantiate (postmanPrefab);
			postman.transform.parent = parent.transform;
			PostmanAI postmanPath = postman.GetComponent<PostmanAI> ();
			// only throw projectiles in correct levels
			postmanPath.ThrowsProjectiles = levelData.postmanPathGroup.throwProjectiles;
			postmanPath.CurrentPathPercent = (float)i / levelData.postmanPathGroup.numPostmanToSpawn;
		}
	}

	/// <summary>
	/// places set number of dogs based on placement vector3
	/// </summary>
	public void SpawnDogs(){
		GameObject dogParent = GameObject.Find ("Dog Pool");

		if(dogParent != null)
		{
			for (int i = 0; i < levelData.dogGroup.numDogsToSpawn; i++) {
				GameObject dog = GameObject.Instantiate (levelData.dogGroup.dog);
				dog.transform.parent = dogParent.transform;
			}
		}
		else
		{
			Debug.Log("Dog Pool GameObject missing from scene. Nowhere to instantiate cars");
		}
	}

	/// <summary>
	/// places set number of postman at indicated spawn points
	/// these postmen don't walk around
	/// number of postmen are set by the level scriptable object
	/// </summary>
	public void SpawnStatPostman(){
		GameObject statParent = GameObject.Find ("Stationary Postman Pool");

		if(statParent != null)
		{
			for (int i = 0; i < levelData.statPostmanGroup.numStatPostmenToSpawn; i++) {
				GameObject stat = GameObject.Instantiate (levelData.statPostmanGroup.stat);
				stat.transform.parent = statParent.transform;
				PostmanStationaryAI postman = stat.GetComponent<PostmanStationaryAI> ();
				// only throw projectiles in correct levels
				postman.ThrowsProjectiles = levelData.postmanPathGroup.throwProjectiles;
			}
		}
		else
		{
			Debug.Log("Dog Pool GameObject missing from scene. Nowhere to instantiate dogs");
		}
	}

	public void SpawnFlamingPackages(GameObject player, Rigidbody playerBody){

		//Check if flaming packages are activated:
		if (levelData.flamingPackageGroup.activated) {
			//Find spawn area at Vector3(playerposition, playerposition+50, playerposition+30):
			Vector3 playerPosition = player.transform.position;
			Vector3 flamingPackageSpawnArea = playerPosition;

			//Instantiate the given number of flaming packages at the Flaming Packages Pool:
			for (int i = 0; i < levelData.flamingPackageGroup.numFlamingPackagesToSpawn; i++){
				flamingPackageSpawnArea.y += 30;
				float vel = 4.0f * Mathf.Sqrt(playerBody.velocity.x*playerBody.velocity.x + playerBody.velocity.z*playerBody.velocity.z);
				Vector3 front = playerBody.transform.forward * vel + playerBody.transform.position;

				flamingPackageSpawnArea.z = Random.Range(front.z - 1, front.z + 1);
				flamingPackageSpawnArea.x = Random.Range (front.x - 1, front.x + 1);
				GameObject flamingPackage = GameObject.Instantiate (levelData.flamingPackageGroup.flamingPackage);
				flamingPackage.transform.position = flamingPackageSpawnArea;
			}
		}
	}

	public float GetMissionLength()
	{
		return levelData.missionLength;
	}

	public AudioClip GetBackgroundMusic(){
		return levelData.backgroundMusic;
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

	//specifically for Thursday:
	public void SetMountainDropoff(){
		if (currentDestination != null && currentDestination.minimapMarker != null) {
			currentDestination.SetMiniMapMarkerActive (false);
			currentDestination = levelData.GetMountainDropoffLocation (ref activeLocations);
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

				if (currCutScene.background != null) {
					m_CutsceneBackground.sprite = currCutScene.background;
					StartCoroutine (BackgroundFade (true));
				} else {
					StartCoroutine (BackgroundFade (false));
				}
					
				StartCoroutine (DisplayAvatar (currCutScene.avatar, m_CutsceneAvatarR, 1.5f));
				StartCoroutine (DisplayAvatar (currCutScene.avatarL, m_CutsceneAvatarL, 1.5f));
				StartCoroutine (DisplayDialog (dialogue, m_CutsceneText, 0.01f, currEvent.requiresConfirmation));

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
		m_ParcelCount.text = string.Format ("Packages Delivered: {0}/{1}", GameManager.Instance.stats.packagesDelivered, levelData.packageCount);
	}

	public bool CheckWinState()
	{
		if (GameManager.Instance.stats.packagesDelivered >= levelData.packageCount) {
			GameManager.Instance.UpdateLevelInfo ();
			return true;
		}

		return false;
	}

	private IEnumerator Run()
	{
		AsyncOperation loadLevel;
		if (SceneManager.GetActiveScene ().name != "CutScene") {

			loadLevel = SceneManager.LoadSceneAsync ("CutScene");
			yield return new WaitUntil (() => loadLevel.isDone);
		}

		m_CutsceneBackground.gameObject.SetActive (true);
		GameManager.Instance.FindCamera ();

		yield return StartCoroutine (cutSceneData.RunCutScene ());
		DisableCutScene ();

		loadLevel = SceneManager.LoadSceneAsync ("InGame");
		yield return new WaitUntil (() => loadLevel.isDone);


		GameManager.Instance.FindCamera ();
		activeLocations = GameObject.FindObjectsOfType<Location> ();

		Coroutine level = StartCoroutine (levelData.RunLevel());
		yield return StartCoroutine(levelData.TimeUp(levelData.events.Count-1));
		StopCoroutine (level);

		DisableHUD ();
		loadLevel = SceneManager.LoadSceneAsync ("Results");
		m_ParcelCount.text = "";
		yield return new WaitUntil (() => loadLevel.isDone);
	}

	private void DisableCutScene()
	{
		m_CutsceneBackground.gameObject.SetActive (false);
	}

	private void DisableHUD()
	{
		m_DialogueBox.gameObject.SetActive (false);
	}

	private IEnumerator BackgroundFade(bool fadeIn)
	{
		float time = 0.0f;
		Color currColor = m_CutsceneBackground.color;

		if (fadeIn) {
			while (time <= 1.5f) {
				m_CutsceneBackground.color = Color.Lerp (currColor, Color.white, time / 1.5f);
				time += Time.deltaTime;
				yield return null;
			}

			m_CutsceneBackground.color = Color.white;

		} else {
			while (time <= 1.5f) {
				m_CutsceneBackground.color = Color.Lerp (currColor, Color.black, time / 1.5f);
				time += Time.deltaTime;
				yield return null;
			}

			m_CutsceneBackground.color = Color.black;
		}
	}

	public IEnumerator FullScreenFade(bool fadeOut)
	{
		Color bOut = new Color (0, 0, 0, 1);
		Color bIn = new Color (0, 0, 0, 0);
		float time = 0.0f;

		if (fadeOut) {
			m_BlackOut.color = bIn;

			m_BlackOut.gameObject.SetActive (true);
			while (time <= 3.0f) {
				m_BlackOut.color = Color.Lerp (bIn, bOut, time / 3.0f);
				time += Time.deltaTime;
				yield return null;
			}

			m_BlackOut.color = bOut;

		} else {
			m_BlackOut.color = bOut;

			while (time <= 3.0f) {
				m_BlackOut.color = Color.Lerp (bOut, bIn, time / 3.0f);
				time += Time.deltaTime;
				yield return null;
			}
				
			m_BlackOut.gameObject.SetActive (false);
		}
	}

	public IEnumerator DisplayDialog(string dialogue, Text area, float speed, bool confirmButton)
	{
		m_CutsceneConfirm.gameObject.SetActive (false);
		
		for (int i = 1; i <= dialogue.Length; i++) {
			area.text = dialogue.Substring (0, i);
			yield return new WaitForSeconds (speed);
		}

		m_CutsceneConfirm.gameObject.SetActive (confirmButton);
	}

	public IEnumerator DisplayAvatar(Sprite avatar, Image location, float rate)
	{
			Color fadedOut = new Color (1, 1, 1, 0);
			Color fadedIn = new Color (1, 1, 1, 1);
			float time = 0.0f;

		if ((location.sprite == null && avatar != null) || (location.sprite != null && avatar != null && location.sprite != avatar)) {
				location.color = fadedOut;
				location.sprite = avatar;

				while (time <= rate) {
					location.color = Color.Lerp (fadedOut, fadedIn, time / rate);
					time += Time.deltaTime;
					yield return null;
				}

				location.color = fadedIn;
			} else if (location.sprite != null && avatar == null) {
				location.color = fadedIn;

				while (time <= rate) {
					location.color = Color.Lerp (fadedIn, fadedOut, time / rate);
					time += Time.deltaTime;
					yield return null;
				}

				location.color = fadedOut;
				location.sprite = null;
			}
	}

	public void StartNextLevel()
	{
		levelData = GameManager.Instance.GetLevelInfo ();
		cutSceneData = GameManager.Instance.GetCutSceneInfo ();
		StopAllCoroutines ();
		GameManager.Instance.Reset ();
		StartCoroutine (Run ());
	}
}