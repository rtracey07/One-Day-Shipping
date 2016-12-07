using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

	private static GameManager _Instance;
	public static GameManager Instance {  get { return _Instance; } }


	//Each level has a mission & cut scene associated with it.
	[Serializable]
	public class Mission
	{
		public Level level;
		public CutScene cutScene;
	}

	//List of levels & current level index.
	public List<Mission> levels;
	public int currLevelIndex = 0; 

	[HideInInspector]
	public bool hasPackage = false;

	[HideInInspector]
	public bool destroyed = false;

	[HideInInspector]
	public bool continueClicked = false;

	[HideInInspector]
	public bool skipClicked = false;

	[HideInInspector]
	public bool dogAttack = false;

	[HideInInspector]
	public bool postmanAttack = false;

	[HideInInspector]
	public bool timeUp = false;

	public Camera mainCamera;

	public Stats stats;

	private bool hasBeenUpdated = false;

	void Awake()
	{
		if (_Instance == null) {
			_Instance = this;
			DontDestroyOnLoad (this);
			FindCamera ();
		}
		else {
			DestroyImmediate (this.gameObject);
		}
	}

	// Continue button pressed.
	public void Continue()
	{
		continueClicked = true;
	}

	//Skip button pressed.
	public void Skip()
	{
		continueClicked = true;
		skipClicked = true;
	}

	//Get the main camera in the current scene.
	public void FindCamera()
	{
		GameObject cam = GameObject.Find ("Main Camera");

		if (cam != null)
			mainCamera = cam.GetComponent<Camera> ();
	}

	//Return current level info to levelmanager.
	public Level GetLevelInfo()
	{
		return levels [currLevelIndex].level;
	}

	//Return current cutscene infor to levelmanager.
	public CutScene GetCutSceneInfo()
	{
		return levels [currLevelIndex].cutScene;
	}

	//Change level.
	public void UpdateLevelInfo()
	{
		if (!hasBeenUpdated) {
			currLevelIndex++;
			if (currLevelIndex >= levels.Count)
				currLevelIndex = 0;
			hasBeenUpdated = true;
		}
	}

	//Reset Gamemanager's stats.
	public void Reset()
	{
		hasPackage = false;
		destroyed = false;
		continueClicked = false;
		skipClicked = false;
		dogAttack = false;
		postmanAttack = false;
		timeUp = false;
		hasBeenUpdated = false;

		stats.carsHit = 0;
		stats.dogsHit = 0;
		stats.packagesDelivered = 0;
		stats.packagesDestroyed = 0;
		stats.postmenHit = 0;
		stats.score = 0;

		LevelManager.Instance.m_ParcelCount.text = "";
	}
}
